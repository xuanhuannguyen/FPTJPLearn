import { useEffect, useMemo, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  ArrowLeft,
  Languages,
  Loader2,
  Puzzle,
  Repeat,
} from 'lucide-react';
import { KanaInputToggle } from '../../../shared/components/KanaInputToggle';
import type { KanaInputMode } from '../../../shared/utils/kanaInput';
import { grammarApi } from '../api/grammarApi';
import {
  GrammarPracticeCard,
} from '../components/GrammarPracticeCard';
import {
  emptyGrammarExerciseState,
  getGrammarExerciseState,
  type GrammarExerciseState,
} from '../utils/grammarExerciseState';
import type {
  GrammarExercise,
  GrammarExerciseType,
  GrammarPattern,
} from '../types/grammar.types';
import { getExerciseTitle } from '../utils/grammarDisplay';
import { getPracticeInstructions, getSelectedOptionOrder } from '../utils/grammarPractice';

type ExerciseTab = {
  type: GrammarExerciseType;
  icon: 'repeat' | 'languages' | 'puzzle';
};

const exerciseTabs: ExerciseTab[] = [
  { type: 'vi_to_ja', icon: 'repeat' },
  { type: 'ja_to_vi', icon: 'languages' },
  { type: 'arrange', icon: 'puzzle' },
];

const getTabIcon = (icon: ExerciseTab['icon']) => {
  switch (icon) {
    case 'languages':
      return <Languages size={16} />;
    case 'puzzle':
      return <Puzzle size={16} />;
    default:
      return <Repeat size={16} />;
  }
};

export const GrammarPatternPracticePage = () => {
  const { patternId } = useParams();
  const navigate = useNavigate();
  const [pattern, setPattern] = useState<GrammarPattern | null>(null);
  const [exercises, setExercises] = useState<GrammarExercise[]>([]);
  const [activeTab, setActiveTab] = useState<GrammarExerciseType>('vi_to_ja');
  const [isLoading, setIsLoading] = useState(true);
  const [checkingId, setCheckingId] = useState<string | null>(null);
  const [revealingId, setRevealingId] = useState<string | null>(null);
  const [exerciseState, setExerciseState] = useState<Record<string, GrammarExerciseState>>({});
  const [showHint, setShowHint] = useState<Record<string, boolean>>({});
  const [kanaMode, setKanaMode] = useState<KanaInputMode>('off');
  const [error, setError] = useState('');

  useEffect(() => {
    let cancelled = false;

    const loadPracticeData = async () => {
      if (!patternId) {
        setError('Thiếu mã mẫu ngữ pháp.');
        setIsLoading(false);
        return;
      }

      try {
        setIsLoading(true);
        setError('');

        const [patternData, exerciseData] = await Promise.all([
          grammarApi.getPatternById(patternId),
          grammarApi.getPatternExercises(patternId),
        ]);

        if (cancelled) {
          return;
        }

        setPattern(patternData);
        setExercises(exerciseData);
        setExerciseState({});
        setShowHint({});

        const firstAvailableTab = exerciseTabs.find((tab) =>
          exerciseData.some((exercise) => exercise.exerciseType === tab.type)
        );

        setActiveTab(firstAvailableTab?.type ?? 'vi_to_ja');
      } catch (err) {
        console.error(err);
        if (!cancelled) {
          setError('Không tải được bài tập của mẫu ngữ pháp này.');
        }
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    };

    void loadPracticeData();

    return () => {
      cancelled = true;
    };
  }, [patternId]);

  const exerciseCounts = useMemo(() => {
    return exercises.reduce<Record<GrammarExerciseType, number>>(
      (acc, exercise) => {
        acc[exercise.exerciseType] += 1;
        return acc;
      },
      { vi_to_ja: 0, ja_to_vi: 0, arrange: 0 }
    );
  }, [exercises]);

  const filteredExercises = useMemo(
    () => exercises.filter((exercise) => exercise.exerciseType === activeTab),
    [activeTab, exercises]
  );

  const updateExerciseState = (exerciseId: string, next: Partial<GrammarExerciseState>) => {
    setExerciseState((prev) => ({
      ...prev,
      [exerciseId]: {
        ...getGrammarExerciseState(prev, exerciseId),
        ...next,
      },
    }));
  };

  const toggleSelectedOption = (exercise: GrammarExercise, optionIndex: number) => {
    const currentState = getGrammarExerciseState(exerciseState, exercise.id);
    const selectedOptionIndexes = currentState.selectedOptionIndexes.includes(optionIndex)
      ? currentState.selectedOptionIndexes.filter((index) => index !== optionIndex)
      : [...currentState.selectedOptionIndexes, optionIndex];

    updateExerciseState(exercise.id, {
      selectedOptionIndexes,
      result: undefined,
      revealedAnswer: undefined,
      error: undefined,
    });
  };

  const checkExercise = async (exercise: GrammarExercise, submittedAnswerText?: string) => {
    const currentState = getGrammarExerciseState(exerciseState, exercise.id);
    const isArrange = exercise.exerciseType === 'arrange';

    try {
      setCheckingId(exercise.id);
      updateExerciseState(exercise.id, { error: undefined });

      const result = await grammarApi.checkExercise(exercise.id, {
        answerText: isArrange ? undefined : (submittedAnswerText ?? currentState.answerText).trim(),
        selectedOptionOrder: isArrange
          ? getSelectedOptionOrder(exercise, currentState.selectedOptionIndexes)
          : undefined,
      });

      updateExerciseState(exercise.id, {
        result,
        revealedAnswer: undefined,
      });
    } catch (err) {
      console.error(err);
      updateExerciseState(exercise.id, {
        error: 'Không kiểm tra được câu trả lời. Hãy thử lại.',
      });
    } finally {
      setCheckingId(null);
    }
  };

  const revealAnswer = async (exercise: GrammarExercise) => {
    try {
      setRevealingId(exercise.id);
      updateExerciseState(exercise.id, { error: undefined });

      const answer = await grammarApi.revealExerciseAnswer(exercise.id);

      updateExerciseState(exercise.id, {
        result: undefined,
        revealedAnswer:
          exercise.exerciseType === 'arrange'
            ? answer.correctOrder.join(' / ')
            : answer.expectedAnswer,
      });
    } catch (err) {
      console.error(err);
      updateExerciseState(exercise.id, {
        error: 'Không hiển thị được đáp án. Hãy thử lại.',
      });
    } finally {
      setRevealingId(null);
    }
  };

  const resetExercise = (exerciseId: string) => {
    updateExerciseState(exerciseId, {
      ...emptyGrammarExerciseState,
      result: undefined,
      revealedAnswer: undefined,
      error: undefined,
    });
    setShowHint((prev) => ({ ...prev, [exerciseId]: false }));
  };

  if (isLoading) {
    return (
      <div className="flex h-64 flex-col items-center justify-center text-text-secondary">
        <Loader2 size={36} className="mb-4 animate-spin text-accent-primary" />
        <p className="font-bold">Loading grammar exercises...</p>
      </div>
    );
  }

  if (error || !pattern) {
    return (
      <div className="space-y-4">
        <button onClick={() => navigate('/grammar')} className="btn-secondary">
          <ArrowLeft size={18} />
          Back
        </button>
        <div className="rounded-2xl border-2 border-accent-danger bg-accent-danger/10 p-5 font-bold text-accent-danger shadow-pop">
          {error || 'Pattern not found.'}
        </div>
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-5xl space-y-6 px-4 pb-14 animate-fade-in md:px-6">
      <header className="space-y-4">
        <button
          onClick={() => navigate(`/grammar/patterns/${pattern.id}`)}
          className="group inline-flex items-center gap-2 text-sm font-black text-accent-primary transition-colors hover:text-accent-hover"
        >
          <ArrowLeft size={16} className="transition-transform group-hover:-translate-x-1" />
          Quay lại mẫu ngữ pháp
        </button>

        <div className="py-2">
          <div className="flex flex-wrap items-baseline gap-4">
            <h1 className="font-jp text-3xl font-black text-text-primary">{pattern.pattern}</h1>
            <span className="text-lg font-bold text-text-secondary">{pattern.meaning}</span>
          </div>
          <p className="mt-2 text-sm font-bold text-text-muted">{getPracticeInstructions(activeTab)}</p>
        </div>
      </header>

      <div className="flex overflow-x-auto border-b-2 border-border/10">
        {exerciseTabs.map((tab) => {
          const count = exerciseCounts[tab.type];
          const isActive = activeTab === tab.type;

          return (
            <button
              key={tab.type}
              onClick={() => count > 0 && setActiveTab(tab.type)}
              disabled={count === 0}
              className={`flex items-center gap-2 border-b-4 px-5 py-3 text-sm font-black transition-colors disabled:cursor-not-allowed disabled:opacity-40 ${isActive
                ? 'border-accent-primary text-accent-primary'
                : 'border-transparent text-text-muted hover:text-text-secondary'
                }`}
            >
              {getTabIcon(tab.icon)}
              {getExerciseTitle(tab.type)}
              <span className="rounded-full bg-bg-tertiary px-2 py-0.5 text-xs">{count}</span>
            </button>
          );
        })}
      </div>

      {filteredExercises.length === 0 ? (
        <div className="rounded-[24px] border-2 border-border bg-white p-8 text-center font-bold text-text-secondary shadow-card">
          Chưa có bài tập cho loại này.
        </div>
      ) : (
        <div className="space-y-5">
          {activeTab === 'vi_to_ja' ? (
            <div className="flex flex-col gap-3 rounded-2xl border border-border/10 bg-white px-5 py-4 shadow-sm sm:flex-row sm:items-center sm:justify-between">
              <div>
                <p className="text-[10px] font-black uppercase tracking-[0.22em] text-text-muted/60">
                  Bộ gõ trong app
                </p>
                <p className="mt-1 text-xs font-bold text-text-secondary">
                  Bật một lần, áp dụng cho toàn bộ câu Việt sang Nhật.
                </p>
              </div>
              <KanaInputToggle mode={kanaMode} onModeChange={setKanaMode} />
            </div>
          ) : null}

          {filteredExercises.map((exercise, index) => {
            return (
              <GrammarPracticeCard
                key={exercise.id}
                exercise={exercise}
                index={index}
                state={getGrammarExerciseState(exerciseState, exercise.id)}
                hintVisible={Boolean(showHint[exercise.id])}
                checking={checkingId === exercise.id}
                revealing={revealingId === exercise.id}
                kanaMode={kanaMode}
                onAnswerTextChange={(value) =>
                  updateExerciseState(exercise.id, {
                    answerText: value,
                    result: undefined,
                    revealedAnswer: undefined,
                    error: undefined,
                  })
                }
                onToggleHint={(visible) => setShowHint((prev) => ({ ...prev, [exercise.id]: visible }))}
                onToggleOption={(optionIndex) => toggleSelectedOption(exercise, optionIndex)}
                onCheck={(submittedAnswerText) => checkExercise(exercise, submittedAnswerText)}
                onRevealAnswer={() => revealAnswer(exercise)}
                onReset={() => resetExercise(exercise.id)}
              />
            );
          })}
        </div>
      )}
    </div>
  );
};
