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
    <div className="mx-auto max-w-3xl space-y-6">
      {passage && (
        <div className="rounded-2xl border border-sky-200 bg-sky-50/40 p-5 md:p-6 shadow-sm">
          <p className="mb-2 text-xs font-black uppercase tracking-widest text-[#2563EB]">{passage.title}</p>
          <p className="whitespace-pre-wrap font-jp text-base font-bold leading-8 text-[#0b2554]">{passage.content}</p>
        </div>
      )}

      <div>
        <p className="text-xs font-black uppercase tracking-widest text-[#2563EB]">Câu {index + 1}</p>
        <h2 className="mt-1 text-xl md:text-2xl font-black leading-snug text-[#0b2554]">{questionText}</h2>
      </div>

      <div className="grid gap-3">
        {displayOptions.map((option) => {
          if (mode === 'review') {
            const isUserChoice = option.id === selectedOptionId;
            const optionIsCorrect = 'isCorrect' in option ? (option as { isCorrect: boolean }).isCorrect : option.id === correctOptionId;
            const isWrongChoice = isUserChoice && !optionIsCorrect;

            let cardStyle = 'border-sky-200/80 bg-white text-slate-700 shadow-sm';
            let badgeStyle = 'border-sky-200 bg-sky-50 text-slate-700';

            if (optionIsCorrect) {
              cardStyle = 'border-2 border-emerald-500 bg-[#ECFDF5] text-emerald-900 shadow-sm font-extrabold';
              badgeStyle = 'border-emerald-600 bg-emerald-600 text-white';
            } else if (isWrongChoice) {
              cardStyle = 'border-2 border-rose-400 bg-[#FEF2F2] text-rose-900 shadow-sm font-extrabold';
              badgeStyle = 'border-rose-500 bg-rose-500 text-white';
            }

            return (
              <div key={option.id} className={`flex min-h-[52px] items-center gap-3.5 rounded-2xl border px-4 py-3 text-sm md:text-base ${cardStyle}`}>
                <span className={`grid h-8 w-8 shrink-0 place-items-center rounded-xl border text-xs font-black ${badgeStyle}`}>{option.label}</span>
                <span className="flex-1 font-bold">{option.text}</span>
                {optionIsCorrect && <CheckCircle2 size={20} className="shrink-0 text-emerald-600" />}
                {isWrongChoice && <XCircle size={20} className="shrink-0 text-rose-500" />}
              </div>
            );
          }

          // Exam mode
          const isSelected = option.id === selectedOptionId;
          return (
            <button
              key={option.id}
              onClick={() => onSelectOption?.(option.id)}
              className={`flex min-h-[52px] items-center gap-3.5 rounded-2xl border px-4 py-3 text-left text-sm md:text-base transition-all shadow-sm active:scale-[0.99] ${
                isSelected
                  ? 'border-2 border-[#2563EB] bg-[#EFF6FF] text-[#2563EB] shadow-md shadow-blue-100 font-extrabold'
                  : 'border-sky-200/80 bg-white text-[#0b2554] hover:border-[#2563EB] hover:bg-sky-50/50'
              }`}
            >
              <span className={`grid h-8 w-8 shrink-0 place-items-center rounded-xl border text-xs font-black transition-colors ${
                isSelected ? 'border-[#2563EB] bg-[#2563EB] text-white' : 'border-sky-200 bg-sky-50 text-slate-700'
              }`}>{option.label}</span>
              <span className="font-bold">{option.text}</span>
            </button>
          );
        })}
      </div>

      {/* Review explanation block */}
      {mode === 'review' && (
        <div className={`rounded-2xl border-2 p-5 shadow-sm space-y-3 ${isCorrect ? 'border-emerald-300 bg-[#ECFDF5]' : 'border-rose-300 bg-[#FEF2F2]'}`}>
          <div className="flex items-center gap-2 font-black text-base">
            {isCorrect
              ? <><CheckCircle2 size={20} className="text-emerald-600" /><span className="text-emerald-800">Chính xác</span></>
              : <><XCircle size={20} className="text-rose-600" /><span className="text-rose-800">Chưa đúng</span></>
            }
          </div>
          <div className="space-y-1 text-sm font-bold">
            <p className="text-slate-700">
              Bạn chọn: <span className={isCorrect ? 'text-emerald-800 font-extrabold' : 'text-rose-700 font-extrabold'}>
                {options.find((o) => o.id === selectedOptionId)?.label ?? '—'} {options.find((o) => o.id === selectedOptionId)?.text ?? 'Chưa trả lời'}
              </span>
            </p>
            {!isCorrect && correctOptionId && (() => {
              const correct = options.find((o) => o.id === correctOptionId);
              return correct ? <p className="text-emerald-800 font-extrabold">Đáp án đúng: {correct.label} {correct.text}</p> : null;
            })()}
          </div>
          {explanation && (
            <p className="text-sm font-bold leading-relaxed text-slate-700 border-t border-slate-200/80 pt-3">
              💡 {explanation}
            </p>
          )}
        </div>
      )}
    </div>
  );
};
