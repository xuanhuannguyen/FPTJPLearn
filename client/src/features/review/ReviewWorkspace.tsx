import { useEffect, useMemo, useRef, useState } from 'react';
import {
  CheckCircle2,
  ChevronRight,
  Clock3,
  Layers3,
  Loader2,
  RefreshCw,
  RotateCcw,
  Settings2,
  ShieldAlert,
  Shuffle,
  Sparkles,
  BarChart3,
  Trophy,
  History,
  Zap,
  CreditCard,
  ListTodo,
  Keyboard,
} from 'lucide-react';
import { vocabularyApi } from '../vocabulary/api/vocabularyApi';
import { reviewApi } from './api/reviewApi';
import type {
  ReviewAnswerResult,
  ReviewCard,
  ReviewMode,
  ReviewScope,
  ReviewSessionPayload,
} from './types';
import { FlashcardMode } from './components/FlashcardMode';
import { MultipleChoiceMode } from './components/MultipleChoiceMode';
import { TypingMode } from './components/TypingMode';

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

const levelScopes: Array<{ value: ReviewScope; level: number; label: string; description: string; icon: any }> = [
  { value: 'level-0', level: 0, label: 'Level 0', description: 'New words', icon: Sparkles },
  { value: 'level-1', level: 1, label: 'Level 1', description: 'Early learning', icon: BarChart3 },
  { value: 'level-2', level: 2, label: 'Level 2', description: 'Learning review', icon: BarChart3 },
  { value: 'level-3', level: 3, label: 'Level 3', description: 'Review stage', icon: History },
  { value: 'level-4', level: 4, label: 'Level 4', description: 'Strong recall', icon: Trophy },
  { value: 'level-5', level: 5, label: 'Level 5', description: 'Mastered words', icon: CheckCircle2 },
];

const scopeOptions: Array<{ value: ReviewScope; label: string; description: string; icon: any }> = [
  { value: 'due', label: 'Due now', description: 'Scheduled for review', icon: Clock3 },
  ...levelScopes,
  { value: 'mastered', label: 'Mastered', description: 'Mastered only', icon: CheckCircle2 },
  { value: 'reviewed', label: 'Reviewed', description: 'Reviewed items', icon: History },
  { value: 'all', label: 'Study All', description: 'All words in list', icon: Layers3 },
];

const getLevelForScope = (scope: ReviewScope) => {
  return levelScopes.find((option) => option.value === scope)?.level;
};

const modeOptions: Array<{ value: ReviewMode; label: string; description: string; icon: any; color: string }> = [
  {
    value: 'flashcard',
    label: 'Flashcards',
    description: 'Manual recall with SRS',
    icon: CreditCard,
    color: 'indigo'
  },
  {
    value: 'multichoice',
    label: 'Multiple Choice',
    description: 'Select the correct meaning',
    icon: ListTodo,
    color: 'emerald'
  },
  {
    value: 'typing',
    label: 'Typing',
    description: 'Verify your retention',
    icon: Keyboard,
    color: 'amber'
  },
];

const directionOptions: Array<{ value: StudyDirection; label: string; description: string }> = [
  { value: 'jp_to_vi', label: 'JP -> VI', description: 'Japanese prompt, Vietnamese answer.' },
  { value: 'vi_to_jp', label: 'VI -> JP', description: 'Vietnamese prompt, Japanese answer.' },
];

const levelPillStyles = [
  'bg-slate-200 text-slate-700',
  'bg-amber-100 text-amber-700',
  'bg-orange-100 text-orange-700',
  'bg-sky-100 text-sky-700',
  'bg-emerald-100 text-emerald-700',
  'bg-emerald-200 text-emerald-900',
];

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
  const [isSettingsOpen, setIsSettingsOpen] = useState(false);
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
          durationSeconds: Math.max(1, Math.round((Date.now() - session.startedAt) / 1000)),
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

  useEffect(() => {
    if (!session) {
      setIsSettingsOpen(false);
    }
  }, [session]);

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

      const cards = await fetchCardsForScope(selectedScope);

      if (selectedMode === 'multichoice' && cards.length < 4) {
        setError('Multiple choice needs at least 4 words in the selected scope.');
        return;
      }

      if (cards.length === 0) {
        setError('No cards matched this review scope in the current list.');
        return;
      }

      const preparedCards = selectedMode === 'flashcard' && isShuffleEnabled ? shuffleCards(cards) : cards;

      setSession({
        queue: [...preparedCards],
        cardBank: cards,
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

    setSession({
      ...session,
      currentIndex: session.currentIndex + 1,
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

  const getMultipleChoiceOptions = (card: ReviewCard, cardBank: ReviewCard[]) => {
    const distractors = cardBank
      .filter((candidate) => candidate.itemId !== card.itemId)
      .sort(() => Math.random() - 0.5)
      .slice(0, 3);

    return [
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
    ].sort(() => Math.random() - 0.5);
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
      case 'mastered': return levelSummary[5] || 0;
      case 'reviewed': return (levelSummary[3] || 0) + (levelSummary[4] || 0) + (levelSummary[5] || 0);
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

  const finished = !!session && session.currentIndex >= session.totalCards;
  const options = currentCard && session ? getMultipleChoiceOptions(currentCard, session.cardBank) : [];

  return (
    <div className={`space-y-8 ${inline ? '' : 'pb-20'} animate-fade-in`}>
      <div className="flex items-center justify-between gap-4">
        <button
          onClick={() => {
            if (session && !finished) {
              void exitSessionToModeSelection();
              return;
            }
            onClose();
          }}
          className="inline-flex items-center gap-2 text-sm font-medium text-text-secondary transition-colors hover:text-accent-primary"
        >
          <span>{session && !finished ? 'Exit session' : 'Back to word list'}</span>
        </button>

        <button
          onClick={resetProgress}
          className="inline-flex items-center gap-2 rounded-xl border border-border px-4 py-2 text-sm font-medium text-text-secondary transition-colors hover:bg-bg-tertiary hover:text-text-primary"
        >
          <RotateCcw size={16} />
          <span>Reset Progress</span>
        </button>
      </div>

      <section className="rounded-2xl border border-border bg-bg-secondary p-4 shadow-sm">
        <div className="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
          <div className="min-w-0">
            <div className="flex items-center gap-2">
              <p className="text-[9px] font-black uppercase tracking-[0.2em] text-accent-primary/80">
                Workspace
              </p>
              <div className="h-1 w-1 rounded-full bg-border" />
              <p className="text-[10px] font-bold text-text-tertiary">
                {list.wordCount} words
              </p>
            </div>
            <h2 className="text-lg font-black tracking-tight text-text-primary truncate">{list.name}</h2>
          </div>

          <div className="flex flex-wrap gap-1.5">
            {Array.from({ length: 6 }, (_, level) => (
              <div
                key={level}
                className={`rounded-lg px-2 py-0.5 text-[10px] font-black shadow-sm transition-all hover:scale-105 ${levelPillStyles[level]}`}
              >
                L{level}: {levelSummary?.[level] || 0}
              </div>
            ))}
          </div>
        </div>
      </section>

      {!session && (
        <section className="space-y-4">
          {/* Word Scope - Horizontal Top */}
          <div className="rounded-3xl border border-border bg-bg-secondary p-5 shadow-sm">
            <div className="mb-4 flex items-center justify-between">
              <div className="flex items-center gap-2 text-text-primary">
                <div className="flex h-8 w-8 items-center justify-center rounded-lg bg-accent-primary/10 text-accent-primary">
                  <Clock3 size={18} />
                </div>
                <h2 className="text-lg font-bold">Word Scope</h2>
              </div>
              <div className="flex items-center gap-2 text-[11px] font-medium text-text-secondary">
                <div className="h-1.5 w-1.5 rounded-full bg-accent-primary" />
                {list.wordCount} words total
              </div>
            </div>
            
            <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-6 gap-2.5">
              {scopeOptions.map((option) => {
                const active = selectedScope === option.value;
                const count = getScopeCount(option.value);
                const isEmpty = count === 0;

                return (
                  <button
                    key={option.value}
                    onClick={() => setSelectedScope(option.value)}
                    disabled={isEmpty && option.value !== 'all'}
                    className={`group relative flex flex-col items-center justify-center rounded-2xl border p-3 text-center transition-all duration-200 ${active
                        ? 'border-accent-primary bg-accent-primary/5 ring-1 ring-accent-primary/20 shadow-sm'
                        : isEmpty && option.value !== 'all'
                          ? 'border-transparent bg-bg-primary/50 opacity-30 grayscale pointer-events-none'
                          : 'border-border bg-bg-primary hover:border-accent-primary/30 hover:bg-bg-tertiary hover:shadow-md'
                      }`}
                  >
                    <div className={`mb-2 flex h-9 w-9 items-center justify-center rounded-xl transition-all ${active ? 'bg-accent-primary text-white shadow-glow-sm' : 'bg-bg-tertiary text-text-secondary group-hover:text-accent-primary'
                      }`}>
                      <option.icon size={18} />
                    </div>

                    <div className={`text-[10px] font-bold uppercase tracking-wider ${active ? 'text-accent-primary' : 'text-text-tertiary'}`}>
                      {option.label.split(' ')[0]}
                    </div>
                    <div className={`text-base font-black tracking-tight ${active ? 'text-accent-primary' : 'text-text-primary'}`}>
                      {count}
                    </div>

                    {active && (
                      <div className="absolute -bottom-1 left-1/2 -translate-x-1/2 h-1 w-4 rounded-t-full bg-accent-primary" />
                    )}
                  </button>
                );
              })}
            </div>
          </div>

          {/* Review Mode - Horizontal Bottom */}
          <div className="rounded-3xl border border-border bg-bg-secondary p-5 shadow-sm">
            <div className="mb-4 flex items-center gap-2 text-text-primary">
              <div className="flex h-8 w-8 items-center justify-center rounded-lg bg-accent-primary/10 text-accent-primary">
                <Layers3 size={18} />
              </div>
              <h2 className="text-lg font-bold">Review Mode</h2>
            </div>
            
            <div className="grid gap-3 md:grid-cols-3">
              {modeOptions.map((option) => {
                const active = selectedMode === option.value;
                const colorMap: Record<string, string> = {
                  indigo: active ? 'border-indigo-500 bg-indigo-500/10 text-indigo-600' : 'hover:border-indigo-500/50 hover:bg-indigo-500/5',
                  emerald: active ? 'border-emerald-500 bg-emerald-500/10 text-emerald-600' : 'hover:border-emerald-500/50 hover:bg-emerald-500/5',
                  amber: active ? 'border-amber-500 bg-amber-500/10 text-amber-600' : 'hover:border-amber-500/50 hover:bg-amber-500/5',
                };
                
                const iconColorMap: Record<string, string> = {
                  indigo: active ? 'bg-indigo-500 text-white shadow-indigo-200' : 'bg-indigo-50 text-indigo-500',
                  emerald: active ? 'bg-emerald-500 text-white shadow-emerald-200' : 'bg-emerald-50 text-emerald-500',
                  amber: active ? 'bg-amber-500 text-white shadow-amber-200' : 'bg-amber-50 text-amber-500',
                };

                return (
                  <button
                    key={option.value}
                    onClick={() => setSelectedMode(option.value)}
                    className={`group relative flex items-center gap-4 rounded-2xl border p-3.5 transition-all duration-300 ${colorMap[option.color]
                      } ${active ? 'ring-1 ring-offset-0 shadow-md scale-[1.01]' : 'border-border bg-bg-primary text-text-secondary hover:shadow-md'}`}
                  >
                    <div className={`flex h-10 w-10 shrink-0 items-center justify-center rounded-xl transition-all duration-300 ${iconColorMap[option.color]
                      } ${active ? 'shadow-glow-sm' : 'group-hover:scale-105'}`}>
                      <option.icon size={20} />
                    </div>
                    
                    <div className="text-left">
                      <div className={`text-sm font-bold transition-colors ${active ? '' : 'text-text-primary'}`}>
                        {option.label}
                      </div>
                      <div className="text-[10px] opacity-70 leading-none mt-0.5">
                        {option.description}
                      </div>
                    </div>

                    {active && (
                      <div className={`absolute right-3 h-2 w-2 rounded-full ${
                        option.color === 'indigo' ? 'bg-indigo-500' : 
                        option.color === 'emerald' ? 'bg-emerald-500' : 'bg-amber-500'
                      }`} />
                    )}
                  </button>
                );
              })}
            </div>

            <div className="mt-6 flex items-center gap-4">
              <button
                onClick={startSession}
                disabled={isStarting}
                className="group relative flex flex-1 items-center justify-center gap-3 overflow-hidden rounded-2xl bg-accent-primary py-3.5 font-bold text-white shadow-glow transition-all hover:-translate-y-0.5 hover:bg-accent-hover hover:shadow-glow-lg disabled:opacity-60 disabled:pointer-events-none"
              >
                <div className="absolute inset-0 bg-gradient-to-r from-white/0 via-white/10 to-white/0 -translate-x-full group-hover:animate-shimmer" />
                {isStarting ? (
                  <Loader2 size={20} className="animate-spin" />
                ) : (
                  <Zap size={20} className="fill-current" />
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

          {error && (
            <div className="rounded-2xl border border-accent-danger/20 bg-accent-danger/10 px-5 py-4 text-sm text-accent-danger">
              {error}
            </div>
          )}

        </section>
      )}

      {session && !finished && currentCard && (
        <section className="space-y-5">
          <div className="flex items-center justify-between rounded-2xl border border-border bg-bg-secondary px-5 py-4 shadow-sm">
            <div className="flex flex-wrap items-center gap-3 text-sm text-text-secondary">
              <div>
                Card {session.currentIndex + 1} / {session.totalCards}
              </div>
              <div className="rounded-full bg-bg-primary px-3 py-1 font-medium">
                {selectedDirection === 'jp_to_vi' ? 'JP -> VI' : 'VI -> JP'}
              </div>
              {selectedMode === 'flashcard' && isShuffleEnabled && (
                <div className="rounded-full bg-bg-primary px-3 py-1 font-medium">
                  Shuffle on
                </div>
              )}
            </div>
            <div className="flex items-center gap-3">
              <button
                onClick={() => setIsSettingsOpen((current) => !current)}
                className="inline-flex items-center gap-2 rounded-xl border border-border px-3 py-2 text-sm font-medium text-text-secondary transition-colors hover:bg-bg-tertiary hover:text-text-primary"
              >
                <Settings2 size={16} />
                <span>Settings</span>
              </button>
              <div className={`rounded-full px-3 py-1 text-xs font-semibold ${levelPillStyles[currentCard.level]}`}>
                Level {currentCard.level} · {currentCard.status}
              </div>
            </div>
          </div>

          {isSettingsOpen && (
            <div className="rounded-2xl border border-border bg-bg-secondary p-5 shadow-sm">
              <div className="grid gap-5 lg:grid-cols-[1.3fr_1fr]">
                <div>
                  <div className="mb-3 text-sm font-semibold text-text-primary">Direction</div>
                  <div className="grid gap-3 sm:grid-cols-2">
                    {directionOptions.map((option) => {
                      const active = selectedDirection === option.value;
                      return (
                        <button
                          key={option.value}
                          onClick={() => setSelectedDirection(option.value)}
                          className={`rounded-2xl border px-4 py-4 text-left transition-colors ${active
                              ? 'border-accent-primary bg-accent-primary/10 text-accent-primary'
                              : 'border-border bg-bg-primary text-text-secondary hover:border-accent-primary/30 hover:bg-bg-tertiary'
                            }`}
                        >
                          <div className="font-bold">{option.label}</div>
                          <div className="text-xs opacity-70">{option.description}</div>
                        </button>
                      );
                    })}
                  </div>
                </div>

                <div className="flex flex-col justify-between">
                  <div>
                    <div className="mb-3 text-sm font-semibold text-text-primary">Session Options</div>
                    <div className="space-y-3">
                      <button
                        onClick={() => handleShuffleToggle(!isShuffleEnabled)}
                        className={`flex w-full items-center justify-between rounded-xl border px-4 py-3 transition-colors ${isShuffleEnabled
                            ? 'border-accent-primary bg-accent-primary/5 text-accent-primary'
                            : 'border-border bg-bg-primary text-text-secondary hover:bg-bg-tertiary'
                          }`}
                      >
                        <div className="flex items-center gap-3 font-medium">
                          <Shuffle size={18} />
                          Shuffle cards
                        </div>
                        <div className={`h-5 w-9 rounded-full p-1 transition-colors ${isShuffleEnabled ? 'bg-accent-primary' : 'bg-border'}`}>
                          <div className={`h-3 w-3 rounded-full bg-white transition-transform ${isShuffleEnabled ? 'translate-x-4' : 'translate-x-0'}`} />
                        </div>
                      </button>
                    </div>
                  </div>

                  <div className="mt-4 flex items-center justify-end gap-3 text-xs text-text-secondary">
                    <span>Press <kbd className="rounded border bg-bg-tertiary px-1 text-[10px]">ESC</kbd> to toggle</span>
                    <button
                      onClick={() => setIsSettingsOpen(false)}
                      className="text-accent-primary hover:underline"
                    >
                      Close Settings
                    </button>
                  </div>
                </div>
              </div>
            </div>
          )}

          <div className="w-full">
            {selectedMode === 'flashcard' && (
              <FlashcardMode
                card={currentCard}
                direction={selectedDirection}
                onAnswer={(quality, correct, message) => finalizeAnswer(quality, correct, message)}
                revealBack={session.revealBack}
                onReveal={() => setSession({ ...session, revealBack: true })}
                answered={session.answered}
                onNext={nextCard}
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
                {Math.round((session.correctCount / Math.max(1, session.totalCards)) * 100)}%
              </div>
            </div>
          </div>

          <div className="mt-6 flex flex-wrap items-center gap-3 text-sm text-text-secondary">
            <div className="inline-flex items-center gap-2 rounded-full bg-bg-primary px-4 py-2">
              <Clock3 size={14} />
              <span>{Math.max(1, Math.round((Date.now() - session.startedAt) / 1000))} sec</span>
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

      {!session && (
        <section className="rounded-3xl border border-border bg-bg-secondary p-6 shadow-sm">
          <div className="mb-4 flex items-center gap-2 text-text-primary">
            <ShieldAlert size={18} className="text-accent-primary" />
            <h2 className="text-lg font-bold">Review Notes</h2>
          </div>
          <div className="space-y-2 text-sm text-text-secondary">
            <p>Use the same list repeatedly and each word will keep its own level in the same list.</p>
            <p>
              Try <span className="font-medium">Level 0</span>, answer correctly a few times, then return to the
              word list to verify the updated levels.
            </p>
            <p>
              Use <span className="font-medium">Mastered only</span> and answer incorrectly once to verify the word
              drops into relearning.
            </p>
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
