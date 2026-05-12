import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft, Brain, CheckCircle2, CreditCard, ListChecks, Loader2, RotateCcw, X } from 'lucide-react';
import { FlashcardMode } from '../../review/components/FlashcardMode';
import { MultipleChoiceMode } from '../../review/components/MultipleChoiceMode';
import type { ReviewCard } from '../../review/types';
import { staticVocabularyApi } from '../api/vocabularyApi';
import type { StaticVocabularyLessonDetail, VocabularyPracticeCard } from '../types/vocabulary.types';

type PracticeMode = 'flashcard' | 'multichoice';
type PracticeDirection = 'jp_to_vi' | 'vi_to_jp';
type AnswerState = 'correct' | 'wrong' | null;

export const VocabularyLessonPage = () => {
  const { courseCode, lessonId } = useParams<{ courseCode: string; lessonId: string }>();
  const navigate = useNavigate();
  const [detail, setDetail] = useState<StaticVocabularyLessonDetail | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [addingToMemoryIds, setAddingToMemoryIds] = useState<Set<string>>(new Set());
  const [memoryStatusByItemId, setMemoryStatusByItemId] = useState<Record<string, boolean>>({});
  const [memoryError, setMemoryError] = useState('');
  const [practiceMode, setPracticeMode] = useState<PracticeMode | null>(null);
  const [practiceCards, setPracticeCards] = useState<VocabularyPracticeCard[]>([]);
  const [isPracticeLoading, setIsPracticeLoading] = useState(false);
  const [practiceError, setPracticeError] = useState('');
  const [practiceIndex, setPracticeIndex] = useState(0);
  const [isBackVisible, setIsBackVisible] = useState(false);
  const [practiceDirection, setPracticeDirection] = useState<PracticeDirection>('jp_to_vi');
  const [isShuffleEnabled, setIsShuffleEnabled] = useState(false);
  const [answerState, setAnswerState] = useState<AnswerState>(null);
  const [correctCount, setCorrectCount] = useState(0);

  useEffect(() => {
    const fetchLessonDetail = async () => {
      if (!lessonId) return;
      try {
        setIsLoading(true);
        setMemoryError('');
        const data = await staticVocabularyApi.getLessonById(lessonId);
        const statusResults = await Promise.allSettled(
          data.items.map(async (item) => {
            const status = await staticVocabularyApi.getMemoryStatus(item.id);
            return [item.id, status.isInMemory && status.isActive] as const;
          }),
        );
        const statusEntries = statusResults
          .filter((result): result is PromiseFulfilledResult<readonly [string, boolean]> => result.status === 'fulfilled')
          .map((result) => result.value);

        setDetail(data);
        setMemoryStatusByItemId(Object.fromEntries(statusEntries));
        if (statusEntries.length !== data.items.length) {
          setMemoryError('Một số trạng thái Memory chưa tải được. Hãy refresh lại bài học.');
        }
      } catch (error) {
        console.error('Failed to fetch lesson detail:', error);
        setMemoryError('Không tải được bài học từ vựng.');
      } finally {
        setIsLoading(false);
      }
    };
    fetchLessonDetail();
  }, [lessonId]);

  const addToMemory = async (itemId: string) => {
    try {
      setMemoryError('');
      setAddingToMemoryIds((prev) => new Set(prev).add(itemId));
      await staticVocabularyApi.addToMemory(itemId);
      const wasInMemory = memoryStatusByItemId[itemId] === true;
      setMemoryStatusByItemId((prev) => ({ ...prev, [itemId]: true }));
      setDetail((prev) => {
        if (!prev) return prev;

        return {
          ...prev,
          items: prev.items.map((item) => (
            item.id === itemId ? { ...item, isLearned: true } : item
          )),
          lesson: {
            ...prev.lesson,
            learnedCount: wasInMemory
              ? prev.lesson.learnedCount
              : Math.min(prev.lesson.wordCount, prev.lesson.learnedCount + 1),
          },
        };
      });
    } catch (error) {
      console.error('Failed to add vocabulary item to memory:', error);
      setMemoryError('Không thể thêm từ vào Ghi nhớ. Vui lòng thử lại.');
    } finally {
      setAddingToMemoryIds((prev) => {
        const next = new Set(prev);
        next.delete(itemId);
        return next;
      });
    }
  };

  const startPractice = async (mode: PracticeMode) => {
    if (!lessonId) return;

    try {
      setIsPracticeLoading(true);
      setPracticeError('');
      setPracticeMode(mode);
      setPracticeIndex(0);
      setIsBackVisible(false);
      setPracticeDirection('jp_to_vi');
      setIsShuffleEnabled(false);
      setAnswerState(null);
      setCorrectCount(0);
      const data = await staticVocabularyApi.getPracticeCards(lessonId, mode);
      setPracticeCards(data.cards);
    } catch (error) {
      console.error('Failed to load vocabulary practice cards:', error);
      setPracticeError('Không tải được thẻ học từ vựng. Vui lòng thử lại.');
    } finally {
      setIsPracticeLoading(false);
    }
  };

  const closePractice = () => {
    setPracticeMode(null);
    setPracticeCards([]);
    setPracticeIndex(0);
    setIsBackVisible(false);
    setPracticeDirection('jp_to_vi');
    setIsShuffleEnabled(false);
    setAnswerState(null);
    setCorrectCount(0);
    setPracticeError('');
  };

  const recordPractice = async (itemId: string, mode: PracticeMode) => {
    if (mode === 'flashcard') {
      await staticVocabularyApi.recordFlashcardPractice(itemId);
      return;
    }

    await staticVocabularyApi.recordMultipleChoicePractice(itemId);
  };

  const goToNextPracticeCard = () => {
    setPracticeIndex((prev) => prev + 1);
    setIsBackVisible(false);
    setAnswerState(null);
  };

  const shuffleRemainingCards = () => {
    setIsShuffleEnabled((prev) => !prev);
    setPracticeCards((prev) => {
      const locked = prev.slice(0, practiceIndex + 1);
      const remaining = [...prev.slice(practiceIndex + 1)];

      for (let index = remaining.length - 1; index > 0; index -= 1) {
        const swapIndex = Math.floor(Math.random() * (index + 1));
        [remaining[index], remaining[swapIndex]] = [remaining[swapIndex], remaining[index]];
      }

      return [...locked, ...remaining];
    });
  };

  const answerFlashcard = async (_quality: number, correct: boolean) => {
    const card = practiceCards[practiceIndex];
    if (!card || !practiceMode || answerState) return;

    if (correct) {
      setCorrectCount((prev) => prev + 1);
    }

    setAnswerState(correct ? 'correct' : 'wrong');

    try {
      setPracticeError('');
      await recordPractice(card.itemId, practiceMode);
    } catch (error) {
      console.error('Failed to record flashcard practice:', error);
      setPracticeError('Không lưu được lượt học flashcard.');
    }
  };

  const answerMultipleChoice = async (correct: boolean) => {
    const card = practiceCards[practiceIndex];
    if (!card || !practiceMode || answerState) return;

    if (correct) {
      setCorrectCount((prev) => prev + 1);
    }

    setAnswerState(correct ? 'correct' : 'wrong');

    try {
      setPracticeError('');
      await recordPractice(card.itemId, practiceMode);
    } catch (error) {
      console.error('Failed to record multiple choice practice:', error);
      setPracticeError('Không lưu được lượt học multichoice.');
    }
  };

  if (isLoading) {
    return (
      <div className="flex h-64 items-center justify-center">
        <div className="h-10 w-10 animate-spin rounded-full border-4 border-accent-primary border-t-transparent"></div>
      </div>
    );
  }

  if (!detail) {
    return (
      <div className="text-center font-bold text-text-secondary mt-10">
        Lesson not found.
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-5xl space-y-6 px-4 pb-14 animate-fade-in md:px-6">
      <header className="space-y-4">
        <button
          onClick={() => navigate(`/vocabulary/${courseCode}`)}
          className="group inline-flex items-center gap-2 text-sm font-black text-accent-primary transition-colors hover:text-accent-hover"
        >
          <ArrowLeft size={16} className="transition-transform group-hover:-translate-x-1" />
          Back to Course {courseCode}
        </button>

        <div className="flex flex-col gap-4 py-2 md:flex-row md:items-end md:justify-between">
          <div>
            <span className="text-xs font-black uppercase text-text-muted">
              Lesson {detail.lesson.lessonNumber}
            </span>
            <h1 className="text-[32px] font-black leading-none tracking-tight text-text-primary">
              {detail.lesson.title}
            </h1>
          </div>
          <div className="flex flex-wrap gap-2">
            <button
              type="button"
              onClick={() => startPractice('flashcard')}
              className="inline-flex h-11 items-center gap-2 rounded-xl border-2 border-slate-800 bg-white px-4 text-sm font-black text-slate-900 shadow-pop transition-all hover:-translate-y-0.5"
            >
              <CreditCard size={18} />
              Flashcard
            </button>
            <button
              type="button"
              onClick={() => startPractice('multichoice')}
              className="inline-flex h-11 items-center gap-2 rounded-xl border-2 border-slate-800 bg-accent-cta px-4 text-sm font-black text-white shadow-pop transition-all hover:-translate-y-0.5"
            >
              <ListChecks size={18} />
              Multichoice
            </button>
          </div>
        </div>
      </header>

      {(memoryError || practiceError) && (
        <div className="rounded-xl border-2 border-accent-danger/20 bg-accent-danger/10 px-4 py-3 text-sm font-bold text-accent-danger">
          {memoryError || practiceError}
        </div>
      )}

      {practiceMode ? (
        <PracticeWorkspace
          mode={practiceMode}
          cards={practiceCards}
          currentIndex={practiceIndex}
          isLoading={isPracticeLoading}
          isBackVisible={isBackVisible}
          answerState={answerState}
          correctCount={correctCount}
          direction={practiceDirection}
          isShuffleEnabled={isShuffleEnabled}
          onFlip={() => setIsBackVisible((prev) => !prev)}
          onAnswerFlashcard={answerFlashcard}
          onAnswerMultipleChoice={answerMultipleChoice}
          onNext={goToNextPracticeCard}
          onRestart={() => startPractice(practiceMode)}
          onClose={closePractice}
          onToggleDirection={() => setPracticeDirection((prev) => prev === 'jp_to_vi' ? 'vi_to_jp' : 'jp_to_vi')}
          onToggleShuffle={shuffleRemainingCards}
        />
      ) : (
      <div className="mt-8 rounded-[24px] border-2 border-slate-800 bg-white overflow-hidden shadow-[0_4px_0_0_#1e293b] md:shadow-[0_8px_0_0_#1e293b]">
        {/* Header Row */}
        <div className="flex items-center border-b-2 border-slate-800 px-6 py-4 text-[10px] font-black uppercase tracking-widest text-slate-500">
          <div className="w-[8%] shrink-0">No.</div>
          <div className="w-[40%] shrink-0">Từ vựng</div>
          <div className="w-[34%] shrink-0">Nghĩa</div>
          <div className="w-[18%] shrink-0 text-right">Memory</div>
        </div>

        {/* List of items */}
        <div className="flex flex-col">
          {detail.items.map((item, index) => {
            const isInMemory = memoryStatusByItemId[item.id] === true;

            return (
              <div
                key={item.id}
                className={`flex items-start px-6 py-6 transition-colors hover:bg-slate-50 ${
                  index !== detail.items.length - 1 ? 'border-b border-slate-800' : ''
                }`}
              >
              {/* Col 1: Number */}
              <div className="w-[8%] shrink-0 pt-2 text-[13px] font-black tracking-widest text-slate-500">
                {index + 1}.
              </div>

              {/* Col 2: Word & Reading & Tag */}
              <div className="w-[40%] shrink-0 pr-4">
                <div className="flex items-baseline gap-3">
                  <span className="font-jp text-[32px] font-black leading-none text-slate-900">{item.word}</span>
                  <span className="font-jp text-[15px] font-bold text-slate-600">{item.reading}</span>
                </div>
                {item.wordType && (
                  <div className="mt-3 inline-flex items-center rounded-lg bg-[#f0f4f8] px-2.5 py-1 text-[10px] font-bold text-[#475569]">
                    {item.wordType}
                  </div>
                )}
              </div>

              {/* Col 3: Meaning */}
              <div className="w-[34%] shrink-0 pt-2">
                <span className="text-[16px] font-bold text-slate-800">{item.meaning}</span>
              </div>

              {/* Col 4: Memory */}
              <div className="flex w-[18%] shrink-0 justify-end pt-1">
                <button
                  type="button"
                  onClick={() => addToMemory(item.id)}
                  disabled={isInMemory || addingToMemoryIds.has(item.id)}
                  className={`inline-flex h-10 min-w-[112px] items-center justify-center gap-2 rounded-xl border-2 px-3 text-xs font-black transition-all ${
                    isInMemory
                      ? 'border-accent-success/20 bg-accent-success/10 text-accent-success'
                      : 'border-slate-800 bg-accent-cta text-white shadow-pop hover:-translate-y-0.5 disabled:cursor-wait disabled:opacity-70'
                  }`}
                >
                  {addingToMemoryIds.has(item.id) ? (
                    <Loader2 size={15} className="animate-spin" />
                  ) : isInMemory ? (
                    <CheckCircle2 size={15} />
                  ) : (
                    <Brain size={15} />
                  )}
                  <span>{isInMemory ? 'Đã lưu' : 'Ghi nhớ'}</span>
                </button>
              </div>
            </div>
            );
          })}
        </div>
      </div>
      )}
    </div>
  );
};

type PracticeWorkspaceProps = {
  mode: PracticeMode;
  cards: VocabularyPracticeCard[];
  currentIndex: number;
  isLoading: boolean;
  isBackVisible: boolean;
  answerState: AnswerState;
  correctCount: number;
  direction: PracticeDirection;
  isShuffleEnabled: boolean;
  onFlip: () => void;
  onAnswerFlashcard: (quality: number, correct: boolean) => void;
  onAnswerMultipleChoice: (correct: boolean) => void;
  onNext: () => void;
  onRestart: () => void;
  onClose: () => void;
  onToggleDirection: () => void;
  onToggleShuffle: () => void;
};

const PracticeWorkspace = ({
  mode,
  cards,
  currentIndex,
  isLoading,
  isBackVisible,
  answerState,
  correctCount,
  direction,
  isShuffleEnabled,
  onFlip,
  onAnswerFlashcard,
  onAnswerMultipleChoice,
  onNext,
  onRestart,
  onClose,
  onToggleDirection,
  onToggleShuffle,
}: PracticeWorkspaceProps) => {
  const currentCard = cards[currentIndex];
  const isCompleted = !isLoading && cards.length > 0 && currentIndex >= cards.length;
  const title = mode === 'flashcard' ? 'Flashcard' : 'Multichoice';
  const reviewCard = currentCard ? toReviewCard(currentCard) : null;
  const options = currentCard ? buildOptions(currentCard, cards, direction) : [];

  if (isLoading) {
    return (
      <div className="flex min-h-[360px] flex-col items-center justify-center rounded-[24px] border-2 border-slate-800 bg-white shadow-[0_8px_0_0_#1e293b]">
        <Loader2 size={36} className="mb-4 animate-spin text-accent-primary" />
        <p className="font-black text-text-secondary">Đang tải {title.toLowerCase()}...</p>
      </div>
    );
  }

  if (cards.length === 0) {
    return (
      <div className="rounded-[24px] border-2 border-slate-800 bg-white p-8 text-center shadow-[0_8px_0_0_#1e293b]">
        <h2 className="text-2xl font-black text-text-primary">Chưa có thẻ học</h2>
        <p className="mt-2 font-bold text-text-secondary">Bài này chưa có dữ liệu từ vựng để luyện tập.</p>
        <button type="button" onClick={onClose} className="btn-secondary mt-5">
          <ArrowLeft size={18} />
          Quay lại danh sách
        </button>
      </div>
    );
  }

  if (isCompleted || !currentCard) {
    return (
      <div className="rounded-[24px] border-2 border-slate-800 bg-white p-8 text-center shadow-[0_8px_0_0_#1e293b]">
        <CheckCircle2 size={52} className="mx-auto text-accent-success" />
        <h2 className="mt-4 text-3xl font-black text-text-primary">Hoàn thành {title}</h2>
        <p className="mt-2 font-bold text-text-secondary">
          {mode === 'multichoice'
            ? `Bạn trả lời đúng ${correctCount}/${cards.length} câu.`
            : `Bạn đã học ${cards.length} flashcard trong bài này.`}
        </p>
        <div className="mt-6 flex flex-wrap justify-center gap-3">
          <button type="button" onClick={onRestart} className="btn-secondary">
            <RotateCcw size={18} />
            Học lại
          </button>
          <button type="button" onClick={onClose} className="btn-primary">
            Quay lại danh sách
          </button>
        </div>
      </div>
    );
  }

  return (
    <section className="space-y-4 rounded-[24px] border-2 border-slate-800 bg-white p-4 shadow-[0_8px_0_0_#1e293b] md:p-6">
      <div className="flex flex-wrap items-center justify-between gap-3 border-b-2 border-slate-100 pb-4">
        <div>
          <p className="text-xs font-black uppercase tracking-[0.2em] text-text-muted">{title}</p>
          <h2 className="text-2xl font-black text-text-primary">
            {currentIndex + 1} / {cards.length}
          </h2>
        </div>
        <div className="flex flex-wrap items-center gap-2">
          <button
            type="button"
            onClick={onClose}
            className="inline-flex h-10 items-center gap-2 rounded-xl border-2 border-slate-800 bg-white px-3 text-sm font-black shadow-pop transition-all hover:-translate-y-0.5"
          >
            <X size={16} />
            Thoát
          </button>
        </div>
      </div>

      {mode === 'flashcard' && reviewCard ? (
        <FlashcardMode
          card={reviewCard}
          direction={direction}
          currentIndex={currentIndex}
          totalCards={cards.length}
          isShuffleEnabled={isShuffleEnabled}
          revealBack={isBackVisible}
          answered={answerState !== null}
          onReveal={() => {
            if (!isBackVisible) onFlip();
          }}
          onAnswer={(quality, correct) => onAnswerFlashcard(quality, correct)}
          onNext={onNext}
          onToggleDirection={onToggleDirection}
          onToggleShuffle={onToggleShuffle}
        />
      ) : null}

      {mode === 'multichoice' && reviewCard ? (
        <MultipleChoiceMode
          card={reviewCard}
          options={options}
          direction={direction}
          answered={answerState !== null}
          feedback={answerState ? {
            correct: answerState === 'correct',
            message: answerState === 'correct'
              ? 'Chính xác'
              : `Sai rồi. Đáp án: ${direction === 'jp_to_vi' ? currentCard.correctAnswer : `${currentCard.word} (${currentCard.reading})`}`,
          } : undefined}
          onAnswer={(_, correct) => onAnswerMultipleChoice(correct)}
          onNext={onNext}
          onToggleDirection={onToggleDirection}
        />
      ) : null}
    </section>
  );
};

const toReviewCard = (card: VocabularyPracticeCard): ReviewCard => ({
  id: card.itemId,
  itemId: card.itemId,
  word: card.word,
  reading: card.reading,
  wordType: '',
  meaning: card.meaning,
  exampleSentence: card.exampleJapanese,
  exampleMeaning: card.exampleMeaning,
  level: 0,
  status: 'new',
  nextReviewAt: new Date().toISOString(),
  intervalDays: 0,
  repetitions: 0,
  lapseCount: 0,
  learningStepIndex: 0,
});

const buildOptions = (
  card: VocabularyPracticeCard,
  cards: VocabularyPracticeCard[],
  direction: PracticeDirection,
) => {
  const correctLabel = direction === 'jp_to_vi'
    ? card.meaning
    : `${card.word} (${card.reading})`;

  const distractors = cards
    .filter((candidate) => candidate.itemId !== card.itemId)
    .map((candidate) => (
      direction === 'jp_to_vi'
        ? candidate.meaning
        : `${candidate.word} (${candidate.reading})`
    ))
    .filter((label, index, arr) => label && arr.indexOf(label) === index)
    .slice(0, 3);

  return shuffleOptions([
    { label: correctLabel, isCorrect: true },
    ...distractors.map((label) => ({ label, isCorrect: false })),
  ]);
};

const shuffleOptions = <T,>(items: T[]) => {
  const next = [...items];
  for (let index = next.length - 1; index > 0; index -= 1) {
    const swapIndex = Math.floor(Math.random() * (index + 1));
    [next[index], next[swapIndex]] = [next[swapIndex], next[index]];
  }

  return next;
};
