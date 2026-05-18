import { Check, Eye, Lightbulb, Loader2, RotateCcw, Sparkles, X } from 'lucide-react';
import { convertRomajiToKana, type KanaInputMode } from '../../../shared/utils/kanaInput';
import type { GrammarExercise } from '../types/grammar.types';
import type { GrammarExerciseState } from '../utils/grammarExerciseState';
import {
  getAnswerLabel,
  getAnswerPlaceholder,
  getPromptLabel,
  getSelectedOptionOrder,
} from '../utils/grammarPractice';

type GrammarPracticeCardProps = {
  exercise: GrammarExercise;
  index: number;
  state: GrammarExerciseState;
  hintVisible: boolean;
  checking: boolean;
  revealing: boolean;
  kanaMode: KanaInputMode;
  onAnswerTextChange: (value: string) => void;
  onToggleHint: (visible: boolean) => void;
  onToggleOption: (optionIndex: number) => void;
  onCheck: (submittedAnswerText?: string) => void;
  onRevealAnswer: () => void;
  onReset: () => void;
};

export const GrammarPracticeCard = ({
  exercise,
  index,
  state,
  hintVisible,
  checking,
  revealing,
  kanaMode,
  onAnswerTextChange,
  onToggleHint,
  onToggleOption,
  onCheck,
  onRevealAnswer,
  onReset,
}: GrammarPracticeCardProps) => {
  const isArrange = exercise.exerciseType === 'arrange';
  const shouldShowKanaToggle = exercise.exerciseType === 'vi_to_ja';
  const selectedOptionOrder = getSelectedOptionOrder(exercise, state.selectedOptionIndexes);
  const canCheck = isArrange ? selectedOptionOrder.length > 0 : state.answerText.trim().length > 0;
  const handleCheck = () => {
    if (!isArrange && shouldShowKanaToggle) {
      const finalizedAnswer = convertRomajiToKana(state.answerText, kanaMode, { finalize: true });
      if (finalizedAnswer !== state.answerText) {
        onAnswerTextChange(finalizedAnswer);
      }
      onCheck(finalizedAnswer);
      return;
    }

    onCheck();
  };

  return (
    <article className="rounded-2xl border border-border/10 bg-white p-6 shadow-sm">
      <div className="space-y-4">
        <div className="flex items-center gap-3">
          <span
            className="flex h-9 w-9 shrink-0 items-center justify-center rounded-md bg-black text-sm font-black text-white shadow-sm"
            aria-label={`Bài tập ${index + 1}`}
          >
            {index + 1}
          </span>
          <h4 className="text-[10px] font-bold uppercase tracking-wider text-text-muted/60">
            {getPromptLabel(exercise.exerciseType)}
          </h4>
        </div>

        <div className="space-y-1.5">
          <p className="text-base font-bold leading-tight text-text-primary">{exercise.prompt}</p>
          {exercise.promptReading ? (
            <p className="font-jp text-xs font-medium text-text-muted/70">{exercise.promptReading}</p>
          ) : null}
        </div>

        {!hintVisible && exercise.hint ? (
          <button
            onClick={() => onToggleHint(true)}
            className="flex items-center gap-1.5 text-xs font-medium text-text-muted/80 transition-colors hover:text-text-primary"
          >
            <Lightbulb size={14} strokeWidth={1.5} />
            Hiện gợi ý
          </button>
        ) : hintVisible && exercise.hint ? (
          <div className="relative flex gap-3 rounded-lg border border-orange-100 bg-orange-50/30 p-2.5">
            <Lightbulb size={14} className="mt-0.5 shrink-0 text-orange-400" />
            <p className="font-jp text-xs font-medium leading-relaxed text-orange-900">{exercise.hint}</p>
            <button
              onClick={() => onToggleHint(false)}
              className="ml-auto text-orange-300 hover:text-orange-500"
              aria-label="Ẩn gợi ý"
            >
              <X size={12} />
            </button>
          </div>
        ) : null}

        <div className="space-y-1.5">
          <h4 className="text-[10px] font-bold uppercase tracking-wider text-text-muted/60">
            {getAnswerLabel(exercise.exerciseType)}
          </h4>

          {isArrange ? (
            <div className="space-y-3">
              <div className="min-h-[48px] rounded-lg border border-border/10 bg-slate-50/50 p-2.5 font-jp text-base font-black text-text-primary">
                {selectedOptionOrder.length > 0 ? (
                  selectedOptionOrder.join(' ')
                ) : (
                  <span className="text-xs font-medium italic text-text-muted/40">
                    Nhấn các mảnh bên dưới...
                  </span>
                )}
              </div>

              <div className="flex flex-wrap gap-1.5">
                {(exercise.options ?? []).map((option, optionIndex) => {
                  const isSelected = state.selectedOptionIndexes.includes(optionIndex);

                  return (
                    <button
                      key={`${option}-${optionIndex}`}
                      onClick={() => onToggleOption(optionIndex)}
                      className={`rounded-md border px-3 py-1 font-jp text-xs font-bold transition-all ${
                        isSelected
                          ? 'border-transparent bg-[#111827] text-white'
                          : 'border-border/10 bg-white text-text-primary hover:border-border'
                      }`}
                    >
                      {option}
                    </button>
                  );
                })}
              </div>
            </div>
          ) : (
            <div className="overflow-hidden rounded-lg border border-border/10 bg-white transition-colors focus-within:border-sky-300">
              <textarea
                value={state.answerText}
                onChange={(event) =>
                  onAnswerTextChange(
                    convertRomajiToKana(event.target.value, shouldShowKanaToggle ? kanaMode : 'off')
                  )
                }
                rows={2}
                className="w-full resize-none bg-white px-3 py-2.5 font-jp text-sm font-medium text-text-primary outline-none"
                placeholder={getAnswerPlaceholder(exercise.exerciseType)}
              />
            </div>
          )}
        </div>

        <div className="flex flex-wrap items-center gap-2 pt-1">
          <button
            onClick={handleCheck}
            disabled={!canCheck || checking}
            className="inline-flex h-9 items-center gap-2 rounded-md bg-[#7dd3fc] px-4 text-xs font-black text-white shadow-sm transition-all hover:bg-sky-400 disabled:opacity-50 disabled:hover:bg-[#7dd3fc] active:scale-95"
          >
            {checking ? <Loader2 size={14} className="animate-spin" /> : <Check size={16} strokeWidth={2.5} />}
            Kiểm tra
          </button>

          <button
            className="inline-flex h-9 cursor-not-allowed items-center gap-2 rounded-md border border-border/10 bg-white px-3 text-xs font-black text-text-muted/60 opacity-50"
            disabled
            title="AI đánh giá sẽ được nối API ở bước sau"
          >
            <Sparkles size={16} />
            AI đánh giá
          </button>

          <button
            onClick={onRevealAnswer}
            disabled={revealing}
            className="ml-1 inline-flex items-center gap-1.5 text-xs font-bold text-text-muted/70 transition-colors hover:text-text-primary"
          >
            {revealing ? <Loader2 size={14} className="animate-spin" /> : <Eye size={16} strokeWidth={1.5} />}
            Xem đáp án
          </button>

          <button
            onClick={onReset}
            className="ml-auto p-2 text-text-muted/30 transition-colors hover:text-accent-danger"
            title="Reset"
          >
            <RotateCcw size={16} />
          </button>
        </div>
      </div>

      {state.error ? (
        <div className="mt-5 rounded-xl border border-accent-danger/30 bg-accent-danger/10 p-4 text-sm font-bold text-accent-danger">
          {state.error}
        </div>
      ) : null}

      {state.result ? (
        <div
          className={`mt-5 rounded-xl border p-4 text-sm font-bold ${
            state.result.isCorrect
              ? 'border-accent-success/30 bg-accent-success/10 text-accent-success'
              : 'border-accent-danger/30 bg-accent-danger/10 text-accent-danger'
          }`}
          aria-live="polite"
        >
          <p>{state.result.feedback}</p>
          <p className="mt-2 font-jp">Đáp án mẫu: {state.result.expectedAnswer}</p>
        </div>
      ) : null}

      {state.revealedAnswer && !state.result ? (
        <div
          className="mt-5 rounded-xl border border-border/30 bg-bg-tertiary p-4 text-sm font-bold text-text-primary"
          aria-live="polite"
        >
          Đáp án: <span className="font-jp">{state.revealedAnswer}</span>
        </div>
      ) : null}
    </article>
  );
};
