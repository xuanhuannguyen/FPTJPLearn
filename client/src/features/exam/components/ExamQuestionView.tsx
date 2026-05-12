import { CheckCircle2, XCircle } from 'lucide-react';
import type { ExamQuestionOption } from '../types/exam.types';

interface ExamQuestionViewProps {
  index: number;
  questionText: string;
  passage?: { title: string; content: string } | null;
  options: ExamQuestionOption[];
  selectedOptionId?: string | null;
  mode: 'exam' | 'review';
  // Review-only props
  correctOptionId?: string;
  isCorrect?: boolean;
  explanation?: string;
  // Review option extension
  reviewOptions?: Array<ExamQuestionOption & { isCorrect: boolean }>;
  // Exam-only
  onSelectOption?: (optionId: string) => void;
}

export const ExamQuestionView = ({
  index, questionText, passage, options, selectedOptionId,
  mode, correctOptionId, isCorrect, explanation, reviewOptions,
  onSelectOption,
}: ExamQuestionViewProps) => {
  const displayOptions = mode === 'review' && reviewOptions ? reviewOptions : options;

  return (
    <div className="mx-auto max-w-3xl space-y-5">
      {passage && (
        <div className="border-2 border-slate-200 bg-white p-5">
          <p className="mb-2 text-xs font-black uppercase tracking-widest text-slate-500">{passage.title}</p>
          <p className="whitespace-pre-wrap font-jp text-base font-bold leading-8 text-slate-900">{passage.content}</p>
        </div>
      )}

      <div>
        <p className="text-xs font-black uppercase tracking-widest text-slate-500">Câu {index + 1}</p>
        <h2 className="mt-1 text-xl font-black leading-snug text-slate-900">{questionText}</h2>
      </div>

      <div className="grid gap-2">
        {displayOptions.map((option) => {
          if (mode === 'review') {
            const isUserChoice = option.id === selectedOptionId;
            const optionIsCorrect = 'isCorrect' in option ? (option as { isCorrect: boolean }).isCorrect : option.id === correctOptionId;
            const isWrongChoice = isUserChoice && !optionIsCorrect;

            let style = 'border-slate-200 text-slate-500';
            if (optionIsCorrect) style = 'border-emerald-600 bg-[#ECFDF5] text-emerald-700 shadow-[3px_3px_0_#10B981]';
            else if (isWrongChoice) style = 'border-red-500 bg-[#FEF2F2] text-red-600 shadow-[3px_3px_0_#EF4444]';

            return (
              <div key={option.id} className={`flex min-h-[48px] items-center gap-3 border-2 px-4 py-2.5 ${style}`}>
                <span className="grid h-7 w-7 shrink-0 place-items-center border-2 border-current text-xs font-black">{option.label}</span>
                <span className="flex-1 font-bold">{option.text}</span>
                {optionIsCorrect && <CheckCircle2 size={18} className="shrink-0" />}
                {isWrongChoice && <XCircle size={18} className="shrink-0" />}
              </div>
            );
          }

          // Exam mode
          const isSelected = option.id === selectedOptionId;
          return (
            <button
              key={option.id}
              onClick={() => onSelectOption?.(option.id)}
              className={`flex min-h-[48px] items-center gap-3 border-2 bg-white px-4 py-2.5 text-left transition-all ${
                isSelected
                  ? 'border-[#2563EB] bg-[#EFF6FF] text-[#2563EB] shadow-[3px_3px_0_#2563EB]'
                  : 'border-slate-200 text-slate-900 hover:border-slate-900 hover:shadow-[3px_3px_0_#111827]'
              }`}
            >
              <span className="grid h-7 w-7 shrink-0 place-items-center border-2 border-current text-xs font-black">{option.label}</span>
              <span className="font-bold">{option.text}</span>
            </button>
          );
        })}
      </div>

      {/* Review explanation block */}
      {mode === 'review' && (
        <div className={`border-2 p-4 ${isCorrect ? 'border-emerald-600 bg-[#ECFDF5]' : 'border-red-500 bg-[#FEF2F2]'}`}>
          <div className="flex items-center gap-2 font-black text-base">
            {isCorrect
              ? <><CheckCircle2 size={20} className="text-emerald-600" /><span className="text-emerald-700">Chính xác</span></>
              : <><XCircle size={20} className="text-red-500" /><span className="text-red-600">Chưa đúng</span></>
            }
          </div>
          <div className="mt-2 space-y-1 text-sm font-bold">
            <p className="text-slate-700">
              Bạn chọn: <span className={isCorrect ? 'text-emerald-700' : 'text-red-600'}>
                {options.find((o) => o.id === selectedOptionId)?.label ?? '—'} {options.find((o) => o.id === selectedOptionId)?.text ?? 'Chưa trả lời'}
              </span>
            </p>
            {!isCorrect && correctOptionId && (() => {
              const correct = options.find((o) => o.id === correctOptionId);
              return correct ? <p className="text-emerald-700">Đáp án đúng: {correct.label} {correct.text}</p> : null;
            })()}
          </div>
          {explanation && (
            <p className="mt-3 text-sm font-bold leading-relaxed text-slate-700 border-t border-slate-300 pt-3">
              💡 {explanation}
            </p>
          )}
        </div>
      )}
    </div>
  );
};
