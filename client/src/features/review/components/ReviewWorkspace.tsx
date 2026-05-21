import { useEffect, useMemo, useRef, useState } from 'react';
import {
  ArrowLeft,
  CheckCircle2,
  Clock3,
  Layers3,
  Loader2,
  RefreshCw,
  RotateCcw,
  ShieldAlert,
  Sparkles,
  CreditCard,
  ListTodo,
  Keyboard,
  BadgeCheck,
  BookMarked,
  BrainCircuit,
  Gauge,
  Target,
  ClipboardCheck,
  Rocket,
  MousePointerClick,
  X,
} from 'lucide-react';
import type { LucideIcon } from 'lucide-react';
import { vocabularyApi } from '../../active-vocabulary/api/vocabularyApi';
import { reviewApi } from '../api/reviewApi';
import type {
  ReviewAnswerResult,
  ReviewCard,
  ReviewMode,
  ReviewScope,
  ReviewSessionPayload,
} from '../types';
import { FlashcardMode } from './FlashcardMode';
import { MultipleChoiceMode } from './MultipleChoiceMode';
import { TypingMode } from './TypingMode';

type StudyDirection = 'jp_to_vi' | 'vi_to_jp';

type VocabularyListDetail = {
  id: string;
  name: string;
  description?: string;
  wordCount: number;
  createdAt: string;
  items: Array<{
    id: string;
    word: string;
    reading: string;
    wordType: string;
    meaning: string;
    exampleSentence?: string;
    exampleMeaning?: string;
    orderIndex: number;
    level: number;
    status: string;
  }>;
};

type SessionState = {
  queue: ReviewCard[];
  cardBank: ReviewCard[];
  currentIndex: number;
  totalCards: number;
  correctCount: number;
  wrongCount: number;
  startedAt: number;
  completedDurationSeconds?: number;
  answered: boolean;
  feedback?: {
    correct: boolean;
    message: string;
    answerResult?: ReviewAnswerResult;
  };
  revealBack: boolean;
};

type ReviewWorkspaceProps = {
  listId: string;
  onClose: () => void;
  initialMode?: ReviewMode;
  initialScope?: ReviewScope;
  inline?: boolean;
};

const levelScopes: Array<{ value: ReviewScope; level: number; label: string; description: string; icon: LucideIcon }> = [
  { value: 'level-0', level: 0, label: 'Level 0', description: 'New words', icon: Sparkles },
  { value: 'level-1', level: 1, label: 'Level 1', description: 'Early learning', icon: BrainCircuit },
  { value: 'level-2', level: 2, label: 'Level 2', description: 'Review stage', icon: Gauge },
];

const scopeOptions: Array<{ value: ReviewScope; label: string; description: string; icon: LucideIcon }> = [
  { value: 'due', label: 'Due', description: 'Scheduled for review', icon: Target },
  ...levelScopes,
  { value: 'mastered', label: 'Mastered', description: 'Mastered only', icon: BadgeCheck },
  { value: 'reviewed', label: 'Reviewed', description: 'Reviewed items', icon: ClipboardCheck },
  { value: 'all', label: 'All', description: 'All words in list', icon: BookMarked },
];

const getLevelForScope = (scope: ReviewScope) => {
  return levelScopes.find((option) => option.value === scope)?.level;
};

const getAvailableScope = (
  list: VocabularyListDetail,
  dueCount: number,
  preferredScope: ReviewScope,
): ReviewScope => {
  const levelSummary = list.items.reduce<Record<number, number>>((acc, item) => {
    acc[item.level] = (acc[item.level] || 0) + 1;
    return acc;
  }, {});

  const getCount = (scope: ReviewScope) => {
    const selectedLevel = getLevelForScope(scope);
    if (selectedLevel !== undefined) {
      return levelSummary[selectedLevel] || 0;
    }

    switch (scope) {
      case 'due': return dueCount;
      case 'mastered': return levelSummary[3] || 0;
      case 'reviewed': return (levelSummary[2] || 0) + (levelSummary[3] || 0);
      case 'all': return list.wordCount;
      default: return 0;
    }
  };

  if (preferredScope === 'all' || getCount(preferredScope) > 0) {
    return preferredScope;
  }

  return levelScopes.find((scope) => getCount(scope.value) > 0)?.value || 'all';
};

const modeOptions: Array<{ value: ReviewMode; label: string; description: string; icon: LucideIcon; color: string }> = [
  {
    value: 'flashcard',
    label: 'Flashcards',
    description: 'Manual recall with SRS',
    icon: CreditCard,
    color: 'blue'
  },
  {
    value: 'multichoice',
    label: 'Multiple Choice',
    description: 'Select the correct meaning',
    icon: ListTodo,
    color: 'sky'
  },
  {
    value: 'typing',
    label: 'Typing',
    description: 'Verify your retention',
    icon: Keyboard,
    color: 'indigo'
  },
];

const levelPillStyles = [
  'bg-blue-100 text-blue-700',
  'bg-sky-100 text-sky-700',
  'bg-cyan-100 text-cyan-700',
  'bg-indigo-100 text-indigo-700',
];

const getStableHash = (value: string) => {
  let hash = 0;
  for (let index = 0; index < value.length; index += 1) {
    hash = Math.imul(31, hash) + value.charCodeAt(index);
  }

  return hash >>> 0;
};

const stableShuffle = <T,>(items: T[], seed: string, getKey: (item: T) => string) => {
  return [...items].sort((left, right) => (
    getStableHash(`${seed}:${getKey(left)}`) - getStableHash(`${seed}:${getKey(right)}`)
  ));
};

export const ReviewWorkspace = ({
  listId,
  onClose,
  initialMode = 'flashcard',
  initialScope = 'due',
  inline = true,
}: ReviewWorkspaceProps) => {
  const [list, setList] = useState<VocabularyListDetail | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isStarting, setIsStarting] = useState(false);
  const [error, setError] = useState('');
  const [session, setSession] = useState<SessionState | null>(null);
  const [isSavingSession, setIsSavingSession] = useState(false);
  const [savedSessionId, setSavedSessionId] = useState<string | null>(null);
  const [selectedMode, setSelectedMode] = useState<ReviewMode>(initialMode);
  const [selectedScope, setSelectedScope] = useState<ReviewScope>(initialScope);
  const [selectedDirection, setSelectedDirection] = useState<StudyDirection>('jp_to_vi');
  const [isShuffleEnabled, setIsShuffleEnabled] = useState(false);
  const [isReviewNotesOpen, setIsReviewNotesOpen] = useState(false);
  const [dueCount, setDueCount] = useState<number | null>(null);
  const sessionSavedRef = useRef(false);

  const currentCard = session?.queue[session.currentIndex] ?? null;

  const sessionType = useMemo(() => {
    if (selectedScope === 'due') return 'due';
    if (selectedScope === 'mastered' || selectedScope === 'reviewed') return 'check_learned';
    return 'relearn';
  }, [selectedScope]);

  useEffect(() => {
    const loadList = async () => {
      try {
        setIsLoading(true);
        setError('');
        const [detail, dueResponse] = await Promise.all([
          vocabularyApi.getListById(listId),
          reviewApi.getDueCards(listId)
        ]);
        setList(detail);
        setDueCount(dueResponse.dueCount);
        setSelectedScope((currentScope) => getAvailableScope(detail, dueResponse.dueCount, currentScope));
      } catch (loadError) {
        console.error(loadError);
        setError('Could not load this vocabulary list.');
      } finally {
        setIsLoading(false);
      }
    };

    void loadList();
  }, [listId]);

  useEffect(() => {
    if (!session || session.currentIndex < session.totalCards || sessionSavedRef.current) {
      return;
    }

    const saveSession = async () => {
      try {
        sessionSavedRef.current = true;
        setIsSavingSession(true);
        const payload: ReviewSessionPayload = {
          listId,
          mode: selectedMode,
          sessionType,
          totalCards: session.totalCards,
          correctCount: session.correctCount,
          wrongCount: session.wrongCount,
          durationSeconds: session.completedDurationSeconds
            ?? Math.max(1, Math.round((Date.now() - session.startedAt) / 1000)),
        };
        const result = await reviewApi.saveSession(payload);
        setSavedSessionId(result.sessionId);
      } catch (saveError) {
        console.error(saveError);
      } finally {
        setIsSavingSession(false);
      }
    };

    void saveSession();
  }, [listId, selectedMode, session, sessionType]);

  const shuffleCards = <T,>(items: T[]) => {
    const next = [...items];
    for (let index = next.length - 1; index > 0; index -= 1) {
      const swapIndex = Math.floor(Math.random() * (index + 1));
      [next[index], next[swapIndex]] = [next[swapIndex], next[index]];
    }
    return next;
  };

  const fetchCardsForScope = async (scope: ReviewScope) => {
    const selectedLevel = getLevelForScope(scope);
    if (selectedLevel !== undefined) {
      const cards = await reviewApi.getCardsByLevel(listId, selectedLevel, selectedLevel);
      return cards.filter((card) => card.level === selectedLevel);
    }

    switch (scope) {
      case 'due': {
        const due = await reviewApi.getDueCards(listId);
        return due.cards;
      }
      case 'mastered':
      case 'reviewed':
        return reviewApi.getLearnedCards(listId, scope);
      case 'all':
        return reviewApi.getAllCards(listId);
      default:
        return reviewApi.getAllCards(listId);
    }
  };

  const startSession = async () => {
    try {
      setIsStarting(true);
      setError('');
      sessionSavedRef.current = false;
      setSavedSessionId(null);

      const [cards, allCards] = await Promise.all([
        fetchCardsForScope(selectedScope),
        reviewApi.getAllCards(listId),
      ]);

      if (selectedMode === 'multichoice' && allCards.length < 4) {
        setError('Multiple choice needs at least 4 words in the current list.');
        return;
      }

      if (cards.length === 0) {
        setError('No cards matched this review scope in the current list.');
        return;
      }

      const preparedCards = selectedMode === 'flashcard' && isShuffleEnabled ? shuffleCards(cards) : cards;

      setSession({
        queue: [...preparedCards],
        cardBank: selectedMode === 'multichoice' ? allCards : cards,
        currentIndex: 0,
        totalCards: preparedCards.length,
        correctCount: 0,
        wrongCount: 0,
        startedAt: Date.now(),
        answered: false,
        feedback: undefined,
        revealBack: false,
      });
    } catch (startError) {
      console.error(startError);
      setError('Could not start the review session.');
    } finally {
      setIsStarting(false);
    }
  };

  const refreshListDetail = async () => {
    const [detail, dueResponse] = await Promise.all([
      vocabularyApi.getListById(listId),
      reviewApi.getDueCards(listId)
    ]);
    setList(detail);
    setDueCount(dueResponse.dueCount);
  };

  const finalizeAnswer = async (quality: number, correct: boolean, message: string) => {
    if (!currentCard || !session) return;

    try {
      const answerResult = await reviewApi.submitAnswer({
        itemId: currentCard.itemId,
        quality,
        mode: selectedMode,
        sessionType,
      });

      const nextQueue = [...session.queue];
      nextQueue[session.currentIndex] = {
        ...currentCard,
        level: answerResult.newLevel,
        status: answerResult.newStatus,
        nextReviewAt: answerResult.nextReviewAt,
        intervalDays: answerResult.intervalDays,
        repetitions: answerResult.repetitions,
        lapseCount: answerResult.lapseCount,
        learningStepIndex: answerResult.learningStepIndex,
      };

      if (answerResult.requeueInSession) {
        nextQueue.push(nextQueue[session.currentIndex]);
      }

      setSession({
        ...session,
        queue: nextQueue,
        correctCount: session.correctCount + (correct ? 1 : 0),
        wrongCount: session.wrongCount + (correct ? 0 : 1),
        answered: true,
        revealBack: true,
        feedback: {
          correct,
          message,
          answerResult,
        },
      });

      await refreshListDetail();
    } catch (submitError) {
      console.error(submitError);
      setError('Could not submit your answer.');
    }
  };

  const nextCard = () => {
    if (!session) return;
    const nextIndex = session.currentIndex + 1;

    setSession({
      ...session,
      currentIndex: nextIndex,
      completedDurationSeconds: nextIndex >= session.queue.length
        ? Math.max(1, Math.round((Date.now() - session.startedAt) / 1000))
        : session.completedDurationSeconds,
      answered: false,
      feedback: undefined,
      revealBack: false,
    });
  };

  const prevCard = () => {
    if (!session || session.currentIndex === 0) return;
    const prevIndex = session.currentIndex - 1;

    setSession({
      ...session,
      currentIndex: prevIndex,
      answered: false,
      feedback: undefined,
      revealBack: false,
    });
  };

  const exitSessionToModeSelection = async () => {
    setSavedSessionId(null);
    setError('');
    setSession(null);
    sessionSavedRef.current = false;
    await refreshListDetail();
  };

  const resetProgress = async () => {
    try {
      setError('');
      const result = await reviewApi.resetProgress(listId, {
        resetType: 'all',
        hardReset: false,
      });
      await refreshListDetail();
      setSession(null);
      setError(`Reset ${result.affectedCount} words in this list back to level 0.`);
    } catch (resetError) {
      console.error(resetError);
      setError('Could not reset progress for this list.');
    }
  };

  const handleShuffleToggle = (enabled: boolean) => {
    if (selectedMode !== 'flashcard') {
      return;
    }

    setIsShuffleEnabled(enabled);

    if (!enabled || !session) {
      return;
    }

    const current = session.queue.slice(0, session.currentIndex + 1);
    const remaining = session.queue.slice(session.currentIndex + 1);

    if (remaining.length <= 1) {
      return;
    }

    setSession({
      ...session,
      queue: [...current, ...shuffleCards(remaining)],
    });
  };

  const getMultipleChoiceOptions = (card: ReviewCard, cardBank: ReviewCard[], sessionSeed: number) => {
    const optionSeed = `${sessionSeed}:${card.itemId}:${selectedDirection}`;
    const distractors = stableShuffle(
      cardBank.filter((candidate) => candidate.itemId !== card.itemId),
      `${optionSeed}:distractors`,
      (candidate) => candidate.itemId,
    ).slice(0, 3);

    return stableShuffle([
      selectedDirection === 'jp_to_vi'
        ? {
          label: card.meaning,
          isCorrect: true,
        }
        : {
          label: `${card.word} (${card.reading})`,
          isCorrect: true,
        },
      ...distractors.map((candidate) => ({
        label:
          selectedDirection === 'jp_to_vi'
            ? candidate.meaning
            : `${candidate.word} (${candidate.reading})`,
        isCorrect: false,
      })),
    ], `${optionSeed}:positions`, (option) => `${option.label}:${option.isCorrect}`);
  };

  const levelSummary = list?.items.reduce<Record<number, number>>((acc, item) => {
    acc[item.level] = (acc[item.level] || 0) + 1;
    return acc;
  }, {});

  const getScopeCount = (scope: ReviewScope) => {
    if (!levelSummary) return 0;
    const selectedLevel = getLevelForScope(scope);
    if (selectedLevel !== undefined) {
      return levelSummary[selectedLevel] || 0;
    }

    switch (scope) {
      case 'due': return dueCount ?? 0;
      case 'mastered': return levelSummary[3] || 0;
      case 'reviewed': return (levelSummary[2] || 0) + (levelSummary[3] || 0);
      case 'all': return list?.wordCount || 0;
      default: return 0;
    }
  };

  if (isLoading) {
    return (
      <div className="flex h-[60vh] items-center justify-center text-text-secondary">
        <Loader2 size={40} className="animate-spin text-accent-primary" />
      </div>
    );
  }

  if (!list) return null;

  const finished = !!session && session.currentIndex >= session.queue.length;
  const options = currentCard && session
    ? getMultipleChoiceOptions(currentCard, session.cardBank, session.startedAt)
    : [];

  return (
    <div className={`space-y-4 ${inline ? '' : 'pb-10'} animate-fade-in`}>
      <div className="flex flex-wrap items-center justify-between gap-3">
        <button
          type="button"
          onClick={() => {
            if (session && !finished) {
              void exitSessionToModeSelection();
              return;
            }
            onClose();
          }}
          className="group inline-flex min-h-11 items-center gap-3 rounded-full border-2 border-white/80 bg-white/80 px-2.5 py-1.5 pr-5 text-sm font-extrabold text-text-secondary shadow-pop backdrop-blur-sm transition-all hover:-translate-y-0.5 hover:border-blue-200 hover:bg-blue-50 hover:text-blue-700 hover:shadow-card focus:outline-none focus:ring-4 focus:ring-blue-500/15"
        >
          <span className="flex h-8 w-8 items-center justify-center rounded-full bg-blue-100 text-blue-600 transition-all group-hover:bg-blue-600 group-hover:text-white">
            <ArrowLeft size={17} strokeWidth={2.6} />
          </span>
          <span>{session && !finished ? 'Exit session' : 'Back to word list'}</span>
        </button>

        <button
          type="button"
          onClick={resetProgress}
          className="group inline-flex min-h-11 items-center gap-3 rounded-full border-2 border-rose-100 bg-white/85 px-2.5 py-1.5 pl-4 text-sm font-extrabold text-text-secondary shadow-pop backdrop-blur-sm transition-all hover:-translate-y-0.5 hover:border-rose-200 hover:bg-rose-50 hover:text-rose-700 hover:shadow-card focus:outline-none focus:ring-4 focus:ring-rose-500/15"
        >
          <span>Reset Progress</span>
          <span className="flex h-8 w-8 items-center justify-center rounded-full bg-rose-100 text-rose-600 transition-all group-hover:bg-rose-600 group-hover:text-white">
            <RotateCcw size={16} strokeWidth={2.6} />
          </span>
        </button>
      </div>

      <section className="rounded-[20px] border-2 border-border bg-white/85 p-3 shadow-card backdrop-blur-sm">
        <div className="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
          <div className="min-w-0">
            <div className="flex items-center gap-2">
              <div className="icon-badge h-7 w-7 bg-blue-600 text-white">
                <BookMarked size={14} />
              </div>
              <p className="text-[10px] font-black uppercase tracking-[0.2em] text-accent-primary">
                Workspace
              </p>
              <div className="h-1 w-1 rounded-full bg-accent-primary" />
              <p className="text-[10px] font-bold text-text-tertiary">
                {list.wordCount} words
              </p>
            </div>
            <h2 className="mt-1 truncate font-heading text-2xl font-black tracking-tight text-text-primary">{list.name}</h2>
          </div>

          <div className="flex flex-wrap gap-1.5">
            {Array.from({ length: 3 }, (_, level) => (
              <div
                key={level}
                className={`rounded-full border border-white px-3 py-1 text-[10px] font-black shadow-sm transition-all hover:scale-105 ${levelPillStyles[level]}`}
              >
                L{level}: {levelSummary?.[level] || 0}
              </div>
            ))}
            <div className={`rounded-full border border-white px-3 py-1 text-[10px] font-black shadow-sm transition-all hover:scale-105 ${levelPillStyles[3]}`}>
              Mastered: {levelSummary?.[3] || 0}
            </div>
          </div>
        </div>
      </section>

      {!session && (
        <section className="space-y-4">
          {/* Word Scope - Horizontal Top */}
          <div className="rounded-[24px] border-2 border-border bg-white/88 p-4 shadow-card backdrop-blur-sm">
            <div className="mb-4 flex items-center justify-between">
              <div className="flex items-center gap-3 text-text-primary">
                <div className="icon-badge h-9 w-9 bg-blue-600 text-white">
                  <Target size={18} />
                </div>
                <div>
                  <h2 className="font-heading text-xl font-black leading-none">Word Scope</h2>
                  <p className="mt-1 text-[11px] font-bold text-text-muted">Pick exactly which memory bucket to train.</p>
                </div>
              </div>
              <div className="flex items-center gap-2">
                <button
                  type="button"
                  onClick={() => setIsReviewNotesOpen((current) => !current)}
                  className="inline-flex items-center gap-2 rounded-full border-2 border-border bg-white px-3 py-1.5 text-[11px] font-extrabold text-text-secondary shadow-pop transition-colors hover:bg-blue-50 hover:text-accent-primary"
                >
                  <ShieldAlert size={14} className="text-accent-primary" />
                  Notes
                </button>
                <div className="hidden items-center gap-2 rounded-full border-2 border-border bg-blue-50 px-3 py-1 text-[11px] font-extrabold text-text-secondary shadow-pop sm:flex">
                  <Layers3 size={14} className="text-accent-primary" />
                  {list.wordCount} words total
                </div>
              </div>
            </div>

            {isReviewNotesOpen && (
              <div className="mb-4 rounded-2xl border border-blue-100 bg-blue-50/70 p-4">
                <div className="mb-3 flex items-center justify-between gap-3">
                  <div className="flex items-center gap-2 text-text-primary">
                    <ShieldAlert size={17} className="text-accent-primary" />
                    <h3 className="text-sm font-black">Review Notes</h3>
                  </div>
                  <button
                    type="button"
                    onClick={() => setIsReviewNotesOpen(false)}
                    className="flex h-8 w-8 items-center justify-center rounded-full text-text-muted transition-colors hover:bg-white hover:text-text-primary"
                    aria-label="Close review notes"
                  >
                    <X size={16} />
                  </button>
                </div>
                <div className="grid gap-2.5 text-sm text-text-secondary md:grid-cols-2">
                  <p><span className="font-semibold text-text-primary">Level 0:</span> từ mới, chưa học hoặc vừa reset.</p>
                  <p><span className="font-semibold text-text-primary">Level 1:</span> đang học, cần lặp lại trong thời gian ngắn.</p>
                  <p><span className="font-semibold text-text-primary">Level 2:</span> đã vào giai đoạn review theo lịch SRS.</p>
                  <p><span className="font-semibold text-text-primary">Mastered:</span> từ đã thuộc tốt, tương ứng level cao nhất.</p>
                  <p><span className="font-semibold text-text-primary">Due:</span> các từ đã tới hạn ôn theo Next Review.</p>
                  <p><span className="font-semibold text-text-primary">Reviewed:</span> các từ đã học ổn định, gồm Level 2 và Mastered.</p>
                </div>
              </div>
            )}
             
            <div className="grid grid-cols-2 gap-2 sm:grid-cols-4 lg:grid-cols-7">
              {scopeOptions.map((option) => {
                const active = selectedScope === option.value;
                const count = getScopeCount(option.value);
                const isEmpty = count === 0;
                const Icon = option.icon;

                return (
                  <button
                    key={option.value}
                    onClick={() => setSelectedScope(option.value)}
                    disabled={isEmpty && option.value !== 'all'}
                    className={`group relative flex min-h-[118px] flex-col items-center justify-center rounded-2xl border-2 p-3 text-center transition-all duration-200 ${active
                        ? 'border-blue-600 bg-gradient-to-br from-blue-50 via-sky-50 to-white text-blue-700 shadow-pop ring-4 ring-blue-600/10'
                        : isEmpty && option.value !== 'all'
                          ? 'border-slate-200 bg-white/40 opacity-35 grayscale pointer-events-none'
                          : 'border-border bg-white/75 text-text-secondary hover:-translate-y-0.5 hover:bg-blue-50 hover:shadow-pop'
                      }`}
                  >
                    <div className={`mb-2 flex h-11 w-11 items-center justify-center rounded-2xl border-2 transition-all ${active ? 'border-blue-700 bg-blue-600 text-white shadow-pop' : 'border-transparent bg-blue-100 text-blue-600 group-hover:border-border group-hover:bg-white'
                      }`}>
                      <Icon size={20} />
                    </div>

                    <div className={`text-[10px] font-black uppercase tracking-wider ${active ? 'text-blue-700' : 'text-text-tertiary'}`}>
                      {option.label}
                    </div>
                    <div className={`mt-0.5 font-heading text-2xl font-black leading-none tracking-tight ${active ? 'text-blue-700' : 'text-text-primary'}`}>
                      {count}
                    </div>
                    <div className="mt-1 line-clamp-1 text-[10px] font-bold text-text-muted">
                      {option.description}
                    </div>

                    {active && (
                      <div className="absolute -bottom-1 left-1/2 h-1.5 w-8 -translate-x-1/2 rounded-t-full bg-blue-600" />
                    )}
                  </button>
                );
              })}
            </div>
          </div>

          {/* Review Mode - Horizontal Bottom */}
          <div className="rounded-[24px] border-2 border-border bg-white/88 p-4 shadow-card backdrop-blur-sm">
            <div className="mb-4 flex items-center gap-3 text-text-primary">
              <div className="icon-badge h-9 w-9 bg-sky-500 text-white">
                <MousePointerClick size={18} />
              </div>
              <div>
                <h2 className="font-heading text-xl font-black leading-none">Review Mode</h2>
                <p className="mt-1 text-[11px] font-bold text-text-muted">Choose how the session should challenge your recall.</p>
              </div>
            </div>
            
            <div className="grid gap-2.5 md:grid-cols-3">
              {modeOptions.map((option) => {
                const active = selectedMode === option.value;
                const colorMap: Record<string, string> = {
                  blue: active ? 'border-blue-600 bg-gradient-to-br from-blue-50 to-white text-blue-700 shadow-pop ring-4 ring-blue-600/10' : 'hover:border-blue-500 hover:bg-blue-50',
                  sky: active ? 'border-sky-500 bg-gradient-to-br from-sky-50 to-white text-sky-700 shadow-pop ring-4 ring-sky-500/10' : 'hover:border-sky-500 hover:bg-sky-50',
                  indigo: active ? 'border-indigo-500 bg-gradient-to-br from-indigo-50 to-white text-indigo-700 shadow-pop ring-4 ring-indigo-500/10' : 'hover:border-indigo-500 hover:bg-indigo-50',
                };
                
                const iconColorMap: Record<string, string> = {
                  blue: active ? 'border-blue-700 bg-blue-600 text-white' : 'border-transparent bg-blue-100 text-blue-600',
                  sky: active ? 'border-sky-700 bg-sky-500 text-white' : 'border-transparent bg-sky-100 text-sky-600',
                  indigo: active ? 'border-indigo-700 bg-indigo-600 text-white' : 'border-transparent bg-indigo-100 text-indigo-600',
                };
                const ModeIcon = option.icon;

                return (
                  <button
                    key={option.value}
                    onClick={() => setSelectedMode(option.value)}
                    className={`group relative flex items-center gap-4 rounded-2xl border-2 p-4 text-left transition-all duration-300 ${colorMap[option.color]
                      } ${active ? 'scale-[1.01]' : 'border-border bg-white/75 text-text-secondary hover:-translate-y-0.5 hover:shadow-pop'}`}
                  >
                    <div className={`flex h-12 w-12 shrink-0 items-center justify-center rounded-2xl border-2 transition-all duration-300 ${iconColorMap[option.color]
                      } ${active ? 'shadow-pop' : 'group-hover:border-border group-hover:bg-white'}`}>
                      <ModeIcon size={22} />
                    </div>
                    
                    <div className="text-left">
                      <div className={`text-base font-black transition-colors ${active ? '' : 'text-text-primary'}`}>
                        {option.label}
                      </div>
                      <div className="mt-1 text-xs font-bold leading-none opacity-70">
                        {option.description}
                      </div>
                    </div>

                    {active && (
                      <div className="absolute right-4 flex h-7 w-7 items-center justify-center rounded-full bg-white text-blue-600 shadow-pop">
                        <CheckCircle2 size={16} />
                      </div>
                    )}
                  </button>
                );
              })}
            </div>

            <div className="mt-6 flex items-center gap-4">
              <button
                onClick={startSession}
                disabled={isStarting}
                className="group relative flex flex-1 items-center justify-center gap-3 overflow-hidden rounded-2xl border-2 border-border bg-blue-600 py-4 font-black text-white shadow-card transition-all hover:-translate-x-0.5 hover:-translate-y-0.5 hover:bg-blue-700 hover:shadow-lift disabled:pointer-events-none disabled:opacity-60"
              >
                <div className="absolute inset-0 bg-gradient-to-r from-white/0 via-white/10 to-white/0 -translate-x-full group-hover:animate-shimmer" />
                {isStarting ? (
                  <Loader2 size={20} className="animate-spin" />
                ) : (
                  <Rocket size={21} className="fill-current" />
                )}
                <span className="relative">
                  {isStarting ? 'Preparing...' : 'Start Review Now'}
                </span>
              </button>
              
              <div className="hidden lg:block text-right">
                <p className="text-[10px] font-medium text-text-tertiary uppercase tracking-wider">Target List</p>
                <p className="text-xs font-bold text-accent-primary truncate max-w-[150px]">{list.name}</p>
              </div>
            </div>
          </div>
        </section>
      )}

      {session && !finished && currentCard && (
        <section className="space-y-5">
          <div className="w-full">
            {selectedMode === 'flashcard' && (
              <FlashcardMode
                card={currentCard}
                direction={selectedDirection}
                currentIndex={session.currentIndex}
                totalCards={session.queue.length}
                isShuffleEnabled={isShuffleEnabled}
                onAnswer={(quality, correct, message) => finalizeAnswer(quality, correct, message)}
                revealBack={session.revealBack}
                onReveal={() => setSession({ ...session, revealBack: true })}
                answered={session.answered}
                onNext={nextCard}
                onPrev={prevCard}
                onToggleDirection={() => setSelectedDirection((current) => current === 'jp_to_vi' ? 'vi_to_jp' : 'jp_to_vi')}
                onToggleShuffle={() => handleShuffleToggle(!isShuffleEnabled)}
              />
            )}

            {selectedMode === 'multichoice' && (
              <MultipleChoiceMode
                card={currentCard}
                options={options}
                direction={selectedDirection}
                onAnswer={(quality, correct, message) => finalizeAnswer(quality, correct, message)}
                answered={session.answered}
                feedback={session.feedback}
                onNext={nextCard}
                onToggleDirection={() => setSelectedDirection((current) => current === 'jp_to_vi' ? 'vi_to_jp' : 'jp_to_vi')}
              />
            )}

            {selectedMode === 'typing' && (
              <TypingMode
                card={currentCard}
                direction={selectedDirection}
                onAnswer={(quality, correct, message) => finalizeAnswer(quality, correct, message)}
                answered={session.answered}
                feedback={session.feedback}
                onNext={nextCard}
                onToggleDirection={() => setSelectedDirection((current) => current === 'jp_to_vi' ? 'vi_to_jp' : 'jp_to_vi')}
              />
            )}
          </div>
        </section>
      )}

      {finished && session && (
        <section className="rounded-3xl border border-border bg-bg-secondary p-8 shadow-card">
          <div className="mb-5 flex items-center gap-3 text-accent-success">
            <CheckCircle2 size={28} />
            <h2 className="text-2xl font-bold text-text-primary">Session Complete</h2>
          </div>

          <div className="grid gap-4 md:grid-cols-3">
            <div className="rounded-2xl bg-bg-primary p-5">
              <div className="text-sm text-text-secondary">Correct</div>
              <div className="mt-2 text-3xl font-bold text-text-primary">{session.correctCount}</div>
            </div>
            <div className="rounded-2xl bg-bg-primary p-5">
              <div className="text-sm text-text-secondary">Wrong</div>
              <div className="mt-2 text-3xl font-bold text-text-primary">{session.wrongCount}</div>
            </div>
            <div className="rounded-2xl bg-bg-primary p-5">
              <div className="text-sm text-text-secondary">Accuracy</div>
              <div className="mt-2 text-3xl font-bold text-text-primary">
                {Math.round((session.correctCount / Math.max(1, session.correctCount + session.wrongCount)) * 100)}%
              </div>
            </div>
          </div>

          <div className="mt-6 flex flex-wrap items-center gap-3 text-sm text-text-secondary">
            <div className="inline-flex items-center gap-2 rounded-full bg-bg-primary px-4 py-2">
              <Clock3 size={14} />
              <span>{session.completedDurationSeconds ?? 1} sec</span>
            </div>
            {savedSessionId && (
              <div className="inline-flex items-center gap-2 rounded-full bg-accent-primary/10 px-4 py-2 text-accent-primary">
                <CheckCircle2 size={14} />
                <span>Saved session {savedSessionId.slice(0, 8)}</span>
              </div>
            )}
            {isSavingSession && (
              <div className="inline-flex items-center gap-2 rounded-full bg-bg-primary px-4 py-2">
                <Loader2 size={14} className="animate-spin" />
                <span>Saving session</span>
              </div>
            )}
          </div>

          <div className="mt-8 flex flex-wrap gap-3">
            <button
              onClick={() => setSession(null)}
              className="rounded-xl bg-accent-primary px-5 py-3 font-semibold text-white transition-all hover:-translate-y-0.5 hover:bg-accent-hover"
            >
              Start Another Session
            </button>
            <button
              onClick={onClose}
              className="rounded-xl border border-border px-5 py-3 font-semibold text-text-secondary transition-colors hover:bg-bg-tertiary hover:text-text-primary"
            >
              Back to List
            </button>
          </div>
        </section>
      )}

      {error && !session && (
        <div className="rounded-2xl border border-accent-danger/20 bg-accent-danger/10 px-5 py-4 text-sm text-accent-danger">
          {error}
        </div>
      )}

      {error && session && (
        <div className="rounded-2xl border border-accent-danger/20 bg-accent-danger/10 px-5 py-4 text-sm text-accent-danger">
          <div className="flex items-center gap-2">
            <RefreshCw size={16} />
            <span>{error}</span>
          </div>
        </div>
      )}
    </div>
  );
};
