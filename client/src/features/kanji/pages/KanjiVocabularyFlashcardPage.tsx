import { useState, useEffect, useCallback, useMemo, useRef } from 'react';
import { useParams, useNavigate, useSearchParams } from 'react-router-dom';
import {
  ArrowLeft,
  Shuffle,
  Check,
  X,
  Volume2,
  RotateCcw,
} from 'lucide-react';
import { KanaInputToggle } from '../../../shared/components/KanaInputToggle';
import {
  convertRomajiToKana,
  normalizeKanaAnswer,
  type KanaInputMode,
} from '../../../shared/utils/kanaInput';
import { kanjiApi } from '../api/kanjiApi';
import type { KanjiVocabulary, KanjiLesson } from '../types/kanji.types';

type PracticeMode = 'flashcard' | 'typing';

type TypingAnswer = {
  itemId: string;
  prompt: string;
  input: string;
  expected: string;
  word: string;
  correct: boolean;
};

const getInitialMode = (value: string | null): PracticeMode => (value === 'typing' ? 'typing' : 'flashcard');

export const KanjiVocabularyFlashcardPage = () => {
  const { level, lessonId } = useParams<{ level: string; lessonId: string }>();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();

  const [vocabs, setVocabs] = useState<KanjiVocabulary[]>([]);
  const [lesson, setLesson] = useState<KanjiLesson | null>(null);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [isFlipped, setIsFlipped] = useState(false);
  const [isShuffleEnabled, setIsShuffleEnabled] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [completed, setCompleted] = useState(false);
  const [stats, setStats] = useState({ known: 0, unknown: 0 });
  const [typingAnswers, setTypingAnswers] = useState<TypingAnswer[]>([]);
  const mode = getInitialMode(searchParams.get('mode'));

  useEffect(() => {
    const fetchData = async () => {
      if (!lessonId) return;
      try {
        const [lessonData, vocabData] = await Promise.all([
          kanjiApi.getLessonById(lessonId),
          kanjiApi.getVocabularyByLesson(lessonId),
        ]);
        setLesson(lessonData);
        setVocabs(vocabData);
      } catch (error) {
        console.error('Failed to fetch data:', error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchData();
  }, [lessonId]);

  const resetSession = useCallback(() => {
    setCurrentIndex(0);
    setIsFlipped(false);
    setCompleted(false);
    setStats({ known: 0, unknown: 0 });
    setTypingAnswers([]);
  }, []);

  const speak = useCallback((vocab: KanjiVocabulary) => {
    if (typeof window === 'undefined' || !('speechSynthesis' in window)) {
      return;
    }

    window.speechSynthesis.cancel();
    const utterance = new SpeechSynthesisUtterance(vocab.word);
    utterance.lang = 'ja-JP';
    utterance.rate = 0.85;
    window.speechSynthesis.speak(utterance);
  }, []);

  const handleNext = useCallback(() => {
    if (currentIndex < vocabs.length - 1) {
      setCurrentIndex(prev => prev + 1);
      setIsFlipped(false);
    } else {
      setCompleted(true);
    }
  }, [currentIndex, vocabs.length]);

  const handlePrev = useCallback(() => {
    if (currentIndex > 0) {
      setCurrentIndex(prev => prev - 1);
      setIsFlipped(false);
    }
  }, [currentIndex]);

  const handleFlip = useCallback(() => {
    setIsFlipped(prev => !prev);
  }, []);

  const handleAnswer = useCallback((known: boolean) => {
    setStats(prev => ({
      known: prev.known + (known ? 1 : 0),
      unknown: prev.unknown + (known ? 0 : 1),
    }));
    handleNext();
  }, [handleNext]);

  const handleTypingAnswer = useCallback((vocab: KanjiVocabulary, input: string, correct: boolean) => {
    setTypingAnswers(prev => [
      ...prev,
      {
        itemId: vocab.id,
        prompt: vocab.word,
        input,
        expected: getTypingAnswer(vocab),
        word: vocab.word,
        correct,
      },
    ]);
    setStats(prev => ({
      known: prev.known + (correct ? 1 : 0),
      unknown: prev.unknown + (correct ? 0 : 1),
    }));
  }, []);

  const handleShuffle = () => {
    setIsShuffleEnabled(prev => !prev);
    const shuffled = [...vocabs].sort(() => Math.random() - 0.5);
    setVocabs(shuffled);
    resetSession();
  };

  useEffect(() => {
    const handleKeyDown = (event: KeyboardEvent) => {
      if (completed || mode !== 'flashcard') return;
      if (event.code === 'Space') {
        event.preventDefault();
        handleFlip();
      } else if (event.code === 'ArrowRight') {
        handleNext();
      } else if (event.code === 'ArrowLeft') {
        handlePrev();
      } else if (event.key.toLowerCase() === 'z') {
        handleAnswer(true);
      } else if (event.key.toLowerCase() === 'x') {
        handleAnswer(false);
      } else if (event.key.toLowerCase() === 'a') {
        const current = vocabs[currentIndex];
        if (current) speak(current);
      }
    };
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [completed, currentIndex, handleAnswer, handleFlip, handleNext, handlePrev, mode, speak, vocabs]);

  if (isLoading) {
    return (
      <div className="flex h-screen items-center justify-center blue-grid">
        <div className="h-10 w-10 animate-spin border-4 border-black border-t-transparent"></div>
      </div>
    );
  }

  if (!lesson || vocabs.length === 0) {
    return <div className="p-8 text-center font-mono">No vocabulary found for this lesson.</div>;
  }

  const currentVocab = vocabs[currentIndex];
  const progress = (currentIndex / vocabs.length) * 100;

  return (
    <div className="min-h-screen blue-grid flex flex-col font-sans">
      <header className="flex h-14 shrink-0 items-center justify-between border-b-2 border-black bg-white px-4 shadow-[2px_0_0_0_#000]">
        <div className="flex items-center gap-3">
          <button
            onClick={() => navigate(`/kanji/${level}/lessons/${lessonId}`)}
            className="flex h-8 w-8 items-center justify-center border border-black hover:bg-black hover:text-white transition-colors"
          >
            <ArrowLeft size={16} />
          </button>
          <div className="flex flex-col">
            <span className="text-[10px] font-black uppercase tracking-widest text-text-tertiary">
              {level} • L{lesson.lessonNumber}
            </span>
            <span className="text-sm font-bold leading-none text-text-primary uppercase">
              Vocab {mode === 'typing' ? 'Typing' : 'Flashcards'}
            </span>
          </div>
        </div>

        <div className="flex items-center gap-2">
          <div className="hidden md:flex flex-col items-end pr-2">
            <span className="text-[10px] font-bold uppercase text-text-tertiary tracking-tighter">Tiến độ</span>
            <span className="font-mono text-xs font-bold">
              {Math.min(currentIndex + 1, vocabs.length)} / {vocabs.length}
            </span>
          </div>
          <button
            onClick={handleShuffle}
            className={`flex h-8 w-8 items-center justify-center border border-black transition-colors ${
              isShuffleEnabled ? 'bg-black text-white' : 'hover:bg-slate-100'
            }`}
            title="Shuffle"
          >
            <Shuffle size={14} />
          </button>
        </div>
      </header>

      <div className="h-1.5 w-full bg-white border-b-2 border-black overflow-hidden">
        <div
          className="h-full bg-accent-primary transition-all duration-500 ease-out border-r-2 border-black"
          style={{ width: `${progress}%` }}
        />
      </div>

      {mode === 'typing' ? (
        <KanjiVocabularyTypingWorkspace
          vocabs={vocabs}
          currentIndex={currentIndex}
          correctCount={stats.known}
          answers={typingAnswers}
          isCompleted={completed}
          onAnswer={handleTypingAnswer}
          onNext={handleNext}
          onRestart={resetSession}
          onClose={() => navigate(`/kanji/${level}/lessons/${lessonId}`)}
        />
      ) : (
        <main className="flex-1 flex flex-col items-center justify-center p-4">
          {completed ? (
            <CompletionPanel
              stats={stats}
              onReset={resetSession}
              onBack={() => navigate(`/kanji/${level}/lessons/${lessonId}`)}
            />
          ) : (
            <FlashcardPanel
              vocab={currentVocab}
              currentIndex={currentIndex}
              total={vocabs.length}
              isFlipped={isFlipped}
              onFlip={handleFlip}
              onSpeak={speak}
              onAnswer={handleAnswer}
            />
          )}
        </main>
      )}
    </div>
  );
};

type FlashcardPanelProps = {
  vocab: KanjiVocabulary;
  currentIndex: number;
  total: number;
  isFlipped: boolean;
  onFlip: () => void;
  onSpeak: (vocab: KanjiVocabulary) => void;
  onAnswer: (known: boolean) => void;
};

const FlashcardPanel = ({
  vocab,
  currentIndex,
  total,
  isFlipped,
  onFlip,
  onSpeak,
  onAnswer,
}: FlashcardPanelProps) => (
  <div className="mx-auto w-full max-w-2xl transform-gpu transition-all duration-500">
    <div className="overflow-hidden rounded-[18px] bg-[#1b2239] shadow-[0_6px_0_rgba(15,23,42,0.92)] perspective-1000">
      <div
        className={`relative min-h-[270px] md:min-h-[360px] transition-all duration-500 transform-style-3d cursor-pointer ${
          isFlipped ? 'rotate-y-180' : ''
        }`}
        onClick={onFlip}
      >
        <div className="absolute inset-0 backface-hidden bg-[#303b5d] flex flex-col items-center justify-center px-10 py-10 text-center">
          <button
            type="button"
            onClick={(event) => {
              event.stopPropagation();
              onSpeak(vocab);
            }}
            className="absolute right-4 top-4 flex h-11 w-11 items-center justify-center rounded-full border border-white/15 bg-white/10 text-white transition-all hover:bg-white/20"
            aria-label="Phát âm từ vựng"
            title="Phát âm"
          >
            <Volume2 size={20} />
          </button>
          <div className="font-jp text-[58px] md:text-[72px] font-bold leading-none tracking-wide text-white drop-shadow-md">
            {vocab.word}
          </div>
          <div className="mt-5 font-jp text-xl font-bold text-slate-300">{vocab.reading}</div>
        </div>

        <div className="absolute inset-0 backface-hidden rotate-y-180 bg-[#303b5d] flex flex-col items-center justify-center px-10 py-10 text-center">
          <button
            type="button"
            onClick={(event) => {
              event.stopPropagation();
              onSpeak(vocab);
            }}
            className="absolute right-4 top-4 flex h-11 w-11 items-center justify-center rounded-full border border-white/15 bg-white/10 text-white transition-all hover:bg-white/20"
            aria-label="Phát âm từ vựng"
            title="Phát âm"
          >
            <Volume2 size={20} />
          </button>
          <div className="text-2xl md:text-3xl font-jp text-slate-300 mb-4">
            {vocab.reading}
          </div>
          <div className="text-lg md:text-xl font-medium text-slate-400 italic">
            {vocab.meaning}
          </div>
        </div>
      </div>

      <div className="flex min-h-10 items-center justify-center bg-[#465174] px-4 text-[11px] font-semibold text-slate-300/75">
        <div className="flex flex-wrap items-center justify-center gap-2">
          <span>Phím tắt:</span>
          <kbd className="rounded bg-slate-500/55 px-2 py-0.5 text-[10px] text-white/80">Space</kbd>
          <span>lật</span>
          <kbd className="rounded bg-slate-500/55 px-2 py-0.5 text-[10px] text-white/80">A</kbd>
          <span>nghe</span>
          <kbd className="rounded bg-slate-500/55 px-2 py-0.5 text-[10px] text-white/80">Z</kbd>
          <span>biết</span>
          <kbd className="rounded bg-slate-500/55 px-2 py-0.5 text-[10px] text-white/80">X</kbd>
          <span>chưa biết</span>
        </div>
      </div>

      <div className="flex min-h-[76px] items-center justify-center gap-8 bg-[#171d33] px-5 py-4 text-white">
        <button
          onClick={() => onAnswer(false)}
          className="flex h-14 w-14 items-center justify-center rounded-full bg-rose-600/35 text-rose-400 transition-all hover:bg-rose-600/50 hover:scale-105 active:scale-95"
          aria-label="Chưa biết"
        >
          <X size={28} strokeWidth={3} />
        </button>

        <div className="min-w-[80px] text-center text-xl font-bold tracking-wider">
          {currentIndex + 1} / {total}
        </div>

        <button
          onClick={() => onAnswer(true)}
          className="flex h-14 w-14 items-center justify-center rounded-full bg-emerald-600/35 text-emerald-400 transition-all hover:bg-emerald-600/50 hover:scale-105 active:scale-95"
          aria-label="Biết"
        >
          <Check size={28} strokeWidth={3} />
        </button>
      </div>
    </div>
  </div>
);

type CompletionPanelProps = {
  stats: { known: number; unknown: number };
  onReset: () => void;
  onBack: () => void;
};

const CompletionPanel = ({ stats, onReset, onBack }: CompletionPanelProps) => (
  <div className="w-full max-w-[600px] animate-fade-in px-4">
    <div className="overflow-hidden rounded-[18px] bg-[#1b2239] shadow-[0_6px_0_rgba(15,23,42,0.92)] text-center text-white p-10">
      <div className="w-24 h-24 bg-emerald-500/20 rounded-full flex items-center justify-center text-emerald-400 mx-auto mb-8 border-4 border-emerald-500/30">
        <Check size={40} strokeWidth={4} />
      </div>
      <h2 className="text-4xl font-black uppercase mb-4 tracking-tight">Hoàn thành!</h2>
      <p className="text-slate-400 mb-10 font-bold uppercase text-xs tracking-[0.2em]">Thành tích ôn tập</p>
      <div className="grid grid-cols-2 gap-6 mb-10">
        <div className="bg-[#303b5d] p-6 rounded-xl">
          <div className="text-4xl font-black text-emerald-400 mb-2">{stats.known}</div>
          <div className="text-[10px] font-black uppercase text-slate-400 tracking-wider">Đã thuộc</div>
        </div>
        <div className="bg-[#303b5d] p-6 rounded-xl">
          <div className="text-4xl font-black text-rose-400 mb-2">{stats.unknown}</div>
          <div className="text-[10px] font-black uppercase text-slate-400 tracking-wider">Cần ôn lại</div>
        </div>
      </div>
      <div className="flex flex-col gap-4">
        <button onClick={onReset} className="w-full py-4 bg-accent-primary text-white font-black uppercase tracking-[0.2em] text-xs hover:bg-blue-600 transition-colors rounded-xl">
          Luyện tập lại
        </button>
        <button onClick={onBack} className="w-full py-4 bg-[#303b5d] text-white font-black uppercase tracking-[0.2em] text-xs hover:bg-[#465174] transition-colors rounded-xl">
          Về trang bài học
        </button>
      </div>
    </div>
  </div>
);

type KanjiVocabularyTypingWorkspaceProps = {
  vocabs: KanjiVocabulary[];
  currentIndex: number;
  correctCount: number;
  answers: TypingAnswer[];
  isCompleted: boolean;
  onAnswer: (vocab: KanjiVocabulary, input: string, correct: boolean) => void;
  onNext: () => void;
  onRestart: () => void;
  onClose: () => void;
};

const KanjiVocabularyTypingWorkspace = ({
  vocabs,
  currentIndex,
  correctCount,
  answers,
  isCompleted,
  onAnswer,
  onNext,
  onRestart,
  onClose,
}: KanjiVocabularyTypingWorkspaceProps) => {
  const rootRef = useRef<HTMLElement>(null);
  const inputRef = useRef<HTMLInputElement>(null);
  const onCloseRef = useRef(onClose);
  const isExitingRef = useRef(false);
  const [inputState, setInputState] = useState({ itemId: '', value: '' });
  const [kanaMode, setKanaMode] = useState<KanaInputMode>('off');
  const [feedback, setFeedback] = useState<TypingAnswer | null>(null);
  const [startedAt, setStartedAt] = useState<number | null>(null);
  const [elapsedMs, setElapsedMs] = useState(0);
  const [completedElapsedMs, setCompletedElapsedMs] = useState<number | null>(null);
  const currentVocab = vocabs[currentIndex];
  const value = currentVocab && inputState.itemId === currentVocab.id ? inputState.value : '';
  const timezone = Intl.DateTimeFormat().resolvedOptions().timeZone;

  useEffect(() => {
    onCloseRef.current = onClose;
  }, [onClose]);

  const doExit = useCallback(() => {
    if (isExitingRef.current) return;
    isExitingRef.current = true;
    if (document.fullscreenElement) {
      document.exitFullscreen?.().catch(() => undefined);
    }
    onCloseRef.current();
  }, []);

  useEffect(() => {
    if (isCompleted) return;

    const root = rootRef.current;
    if (!root) return;

    root.requestFullscreen?.().catch(() => undefined);

    const handleFullscreenChange = () => {
      if (document.fullscreenElement !== root && !isExitingRef.current) {
        doExit();
      }
    };

    const handleVisibilityChange = () => {
      if (document.visibilityState === 'hidden') {
        doExit();
      }
    };

    const handlePageHide = () => {
      doExit();
    };

    document.addEventListener('fullscreenchange', handleFullscreenChange);
    document.addEventListener('visibilitychange', handleVisibilityChange);
    window.addEventListener('pagehide', handlePageHide);
    return () => {
      document.removeEventListener('fullscreenchange', handleFullscreenChange);
      document.removeEventListener('visibilitychange', handleVisibilityChange);
      window.removeEventListener('pagehide', handlePageHide);
    };
  }, [doExit, isCompleted]);

  useEffect(() => {
    const blockKeys = (event: KeyboardEvent) => {
      if (event.key === 'F12') {
        event.preventDefault();
        event.stopPropagation();
        return;
      }
      if (event.ctrlKey && event.shiftKey && ['I', 'J', 'C'].includes(event.key.toUpperCase())) {
        event.preventDefault();
        event.stopPropagation();
        return;
      }
      if (event.ctrlKey && event.key.toUpperCase() === 'U') {
        event.preventDefault();
        event.stopPropagation();
      }
    };

    const blockContextMenu = (event: MouseEvent) => event.preventDefault();

    document.addEventListener('keydown', blockKeys, true);
    document.addEventListener('contextmenu', blockContextMenu, true);
    return () => {
      document.removeEventListener('keydown', blockKeys, true);
      document.removeEventListener('contextmenu', blockContextMenu, true);
    };
  }, []);

  useEffect(() => {
    inputRef.current?.focus();
  }, [currentVocab?.id]);

  useEffect(() => {
    // eslint-disable-next-line react-hooks/set-state-in-effect
    setFeedback(null);
    setInputState({ itemId: '', value: '' });
  }, [currentVocab?.id]);

  useEffect(() => {
    // eslint-disable-next-line react-hooks/set-state-in-effect
    setStartedAt(null);
    setElapsedMs(0);
    setCompletedElapsedMs(null);
    setFeedback(null);
    setInputState({ itemId: '', value: '' });
  }, [vocabs]);

  useEffect(() => {
    if (!startedAt || isCompleted || completedElapsedMs !== null) return;

    const updateElapsed = () => setElapsedMs(Date.now() - startedAt);
    updateElapsed();
    const timer = window.setInterval(updateElapsed, 250);
    return () => window.clearInterval(timer);
  }, [completedElapsedMs, isCompleted, startedAt]);

  const handleInputChange = (rawValue: string) => {
    if (!currentVocab || feedback) return;
    const nextValue = convertRomajiToKana(rawValue, kanaMode);

    if (!startedAt && nextValue.trim()) {
      setStartedAt(Date.now());
      setElapsedMs(0);
    }

    setInputState({ itemId: currentVocab.id, value: nextValue });
  };

  const continueAfterFeedback = () => {
    if (!feedback) return;
    setFeedback(null);
    setInputState({ itemId: '', value: '' });
    onNext();
  };

  const submitAnswer = () => {
    if (!currentVocab) return;

    if (feedback) {
      continueAfterFeedback();
      return;
    }

    const typed = convertRomajiToKana(value, kanaMode, { finalize: true }).trim();
    if (!typed) return;

    const correct = getTypingAcceptedAnswers(currentVocab).some((answer) => (
      normalizeTypingText(answer, kanaMode) === normalizeTypingText(typed, kanaMode)
    ));
    const nextFeedback = {
      itemId: currentVocab.id,
      prompt: currentVocab.word,
      input: typed,
      expected: getTypingAnswer(currentVocab),
      word: currentVocab.word,
      correct,
    };

    setInputState({ itemId: currentVocab.id, value: typed });
    setFeedback(nextFeedback);

    if (currentIndex === vocabs.length - 1 && startedAt) {
      const total = Date.now() - startedAt;
      setCompletedElapsedMs(total);
      setElapsedMs(total);
    }

    onAnswer(currentVocab, typed, correct);
  };

  const answerRows = useMemo(() => answers, [answers]);

  if (isCompleted || !currentVocab) {
    return (
      <main className="fixed inset-0 z-50 overflow-y-auto blue-grid px-4 py-8 text-slate-950 md:px-8">
        <div className="mx-auto w-full max-w-5xl rounded-[18px] border-2 border-black bg-white p-6 shadow-[4px_4px_0_#0F172A] md:p-8">
          <div className="flex flex-col gap-4 border-b-2 border-black pb-5 md:flex-row md:items-end md:justify-between">
            <div>
              <p className="text-xs font-black uppercase tracking-[0.22em] text-emerald-600">Kết quả gõ</p>
              <h2 className="mt-2 text-4xl font-black tracking-tight md:text-5xl">
                {correctCount} / {vocabs.length} câu đúng
              </h2>
              <div className="mt-4 flex flex-wrap gap-3">
                <ResultMetric label="Tổng thời gian" value={formatDuration(completedElapsedMs ?? elapsedMs)} />
                <ResultMetric label="Bắt đầu lúc" value={startedAt ? formatResultTime(new Date(startedAt)) : '--:--:--'} subLabel={timezone} />
              </div>
            </div>
            <div className="flex flex-wrap gap-2">
              <button type="button" onClick={onRestart} className="btn-secondary">
                <RotateCcw size={18} />
                Gõ lại
              </button>
              <button type="button" onClick={doExit} className="btn-primary">
                Quay lại bài học
              </button>
            </div>
          </div>

          <div className="mt-6 grid gap-3 md:grid-cols-2">
            {answerRows.map((answer, index) => (
              <div
                key={`${answer.itemId}-${index}`}
                className={`rounded-2xl border-2 bg-white p-4 text-slate-900 ${
                  answer.correct ? 'border-emerald-400' : 'border-rose-400'
                }`}
              >
                <div className="flex items-start justify-between gap-3">
                  <div>
                    <p className="text-xs font-black uppercase tracking-widest text-slate-500">{answer.prompt}</p>
                    <p className="mt-2 font-jp text-2xl font-black">{answer.expected}</p>
                    {answer.word !== answer.expected && (
                      <p className="mt-1 font-jp text-sm font-bold text-slate-500">{answer.word}</p>
                    )}
                  </div>
                  <span className={`rounded-full px-2.5 py-1 text-xs font-black ${
                    answer.correct ? 'bg-emerald-100 text-emerald-700' : 'bg-rose-100 text-rose-700'
                  }`}>
                    {answer.correct ? 'Đúng' : 'Sai'}
                  </span>
                </div>
                {!answer.correct && (
                  <p className="mt-3 text-sm font-bold text-slate-600">
                    Bạn gõ: <span className="font-jp text-rose-600">{answer.input || 'Trống'}</span>
                  </p>
                )}
              </div>
            ))}
          </div>
        </div>
      </main>
    );
  }

  return (
    <main
      ref={rootRef}
      className="fixed inset-0 z-50 flex min-h-screen flex-col overflow-auto bg-slate-950 px-4 py-5 text-white md:px-8"
    >
      <section className="mx-auto flex w-full max-w-4xl flex-1 flex-col rounded-[18px] border-2 border-white/20 bg-slate-950 p-5 text-white shadow-[4px_4px_0_rgba(255,255,255,0.1)] md:p-7">
        <header className="flex items-center justify-between gap-4 border-b border-white/10 pb-4">
          <div>
            <p className="text-xs font-black uppercase tracking-[0.22em] text-cyan-300">Kanji → Hiragana</p>
            <h2 className="text-2xl font-black">{currentIndex + 1} / {vocabs.length}</h2>
          </div>
          <div className="hidden text-center sm:block">
            <p className="text-[10px] font-black uppercase tracking-[0.22em] text-slate-500">Thời gian</p>
            <p className="mt-1 font-mono text-xl font-black text-white">
              {startedAt ? formatDuration(completedElapsedMs ?? elapsedMs) : '00:00'}
            </p>
          </div>
          <button
            type="button"
            onClick={doExit}
            className="inline-flex h-10 items-center gap-2 rounded-xl border-2 border-white/20 bg-white/10 px-3 text-sm font-black text-white transition-all hover:bg-white/15"
          >
            <X size={16} />
            Thoát
          </button>
        </header>

        <div className="flex flex-1 flex-col items-center justify-center gap-8 py-8">
          <div className="text-center">
            <p className="text-sm font-black uppercase tracking-[0.2em] text-slate-400">Kanji</p>
            <p className="mt-4 font-jp text-5xl font-black leading-tight md:text-7xl">{currentVocab.word}</p>
            {currentVocab.meaning && (
              <p className="mt-4 text-lg font-bold text-slate-300">{currentVocab.meaning}</p>
            )}
          </div>

          <form
            className="w-full max-w-2xl"
            onSubmit={(event) => {
              event.preventDefault();
              submitAnswer();
            }}
          >
            <div className="mb-3 flex flex-col gap-2 rounded-2xl border border-white/10 bg-white/5 p-2 sm:flex-row sm:items-center sm:justify-between">
              <div className="flex items-center justify-between gap-3 sm:justify-start">
                <span className="text-[11px] font-black uppercase tracking-[0.22em] text-slate-400">
                  Bộ gõ trong app
                </span>
                <KanaInputToggle
                  mode={kanaMode}
                  onModeChange={setKanaMode}
                  className="border-white/10 bg-slate-900"
                />
              </div>
              <div className="text-right sm:hidden">
                <p className="text-[10px] font-black uppercase tracking-[0.22em] text-slate-500">Thời gian</p>
                <p className="font-mono text-lg font-black text-white">
                  {startedAt ? formatDuration(completedElapsedMs ?? elapsedMs) : '00:00'}
                </p>
              </div>
            </div>
            <input
              ref={inputRef}
              value={value}
              onChange={(event) => handleInputChange(event.target.value)}
              readOnly={Boolean(feedback)}
              className="h-20 w-full rounded-2xl border-2 border-white/20 bg-white px-5 text-center font-jp text-3xl font-black text-slate-950 outline-none transition-all focus:border-cyan-300 focus:ring-4 focus:ring-cyan-300/20 md:h-24 md:text-5xl"
              placeholder="ひらがな"
              lang="ja"
              autoCapitalize="none"
              autoComplete="off"
              autoCorrect="off"
              spellCheck={false}
            />
            {feedback ? (
              <div
                className={`mt-4 rounded-2xl border-2 p-4 text-left ${
                  feedback.correct
                    ? 'border-emerald-300 bg-emerald-300/10'
                    : 'border-rose-300 bg-rose-300/10'
                }`}
                aria-live="polite"
              >
                <div className="flex flex-wrap items-start justify-between gap-3">
                  <div>
                    <p className={`text-xs font-black uppercase tracking-[0.22em] ${
                      feedback.correct ? 'text-emerald-300' : 'text-rose-300'
                    }`}>
                      {feedback.correct ? 'Đúng' : 'Sai'}
                    </p>
                    <p className="mt-2 text-sm font-bold text-slate-300">Đáp án</p>
                    <p className="font-jp text-3xl font-black text-white">{feedback.expected}</p>
                    {feedback.word !== feedback.expected ? (
                      <p className="mt-1 font-jp text-sm font-bold text-slate-400">{feedback.word}</p>
                    ) : null}
                  </div>
                  {!feedback.correct ? (
                    <div className="text-right">
                      <p className="text-sm font-bold text-slate-400">Bạn gõ</p>
                      <p className="font-jp text-2xl font-black text-rose-200">{feedback.input}</p>
                    </div>
                  ) : null}
                </div>
              </div>
            ) : null}
            <button
              type="submit"
              disabled={!value.trim()}
              className="mt-4 flex h-12 w-full items-center justify-center rounded-xl border-2 border-cyan-300 bg-cyan-300 text-sm font-black text-slate-950 transition-all hover:-translate-y-0.5 disabled:cursor-not-allowed disabled:border-slate-500 disabled:bg-slate-700 disabled:text-slate-300 disabled:hover:translate-y-0"
            >
              {feedback ? 'Tiếp tục' : 'Đã gõ xong từ'}
            </button>
            <p className="mt-3 text-center text-sm font-bold text-slate-400">
              {feedback ? 'Nhấn Enter để sang từ tiếp theo.' : 'Nhấn Enter để kiểm tra từ đã gõ.'}
            </p>
          </form>
        </div>
      </section>
    </main>
  );
};

type ResultMetricProps = {
  label: string;
  value: string;
  subLabel?: string;
};

const ResultMetric = ({ label, value, subLabel }: ResultMetricProps) => (
  <div className="inline-flex flex-col rounded-2xl border border-slate-200 bg-slate-50 px-4 py-3 text-left shadow-[0_3px_0_0_rgba(15,23,42,0.16)]">
    <span className="text-[10px] font-black uppercase tracking-[0.22em] text-slate-500">{label}</span>
    <span className="mt-1 font-mono text-2xl font-black text-slate-950 md:text-3xl">{value}</span>
    {subLabel ? <span className="mt-1 text-xs font-bold text-slate-500">{subLabel}</span> : null}
  </div>
);

const getTypingAnswer = (vocab: KanjiVocabulary) => (
  vocab.reading || vocab.word
);

const getTypingAcceptedAnswers = (vocab: KanjiVocabulary) => (
  Array.from(new Set([vocab.reading].filter(Boolean)))
);

const normalizeTypingText = (value: string, mode: KanaInputMode = 'off') => (
  normalizeKanaAnswer(value, mode).replace(/\s+/g, '')
);

const formatDuration = (durationMs: number) => {
  const totalSeconds = Math.max(0, Math.floor(durationMs / 1000));
  const minutes = Math.floor(totalSeconds / 60);
  const seconds = totalSeconds % 60;

  return `${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`;
};

const formatResultTime = (value: Date) => (
  new Intl.DateTimeFormat('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit',
  }).format(value)
);
