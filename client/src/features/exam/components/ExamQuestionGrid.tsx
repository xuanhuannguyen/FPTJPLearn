interface ExamQuestionGridProps {
  total: number;
  currentIndex: number;
  onSelect: (index: number) => void;
  getStatus: (index: number) => 'current' | 'correct' | 'wrong' | 'answered' | 'unanswered';
}

const STATUS_STYLES: Record<string, string> = {
  current_correct: 'border-emerald-700 bg-emerald-600 text-white',
  current_wrong: 'border-red-700 bg-red-500 text-white',
  current_answered: 'border-[#2563EB] bg-[#2563EB] text-white',
  current_unanswered: 'border-[#2563EB] bg-[#2563EB] text-white',
  correct: 'border-emerald-500 bg-emerald-50 text-emerald-700 hover:bg-emerald-100',
  wrong: 'border-red-400 bg-red-50 text-red-600 hover:bg-red-100',
  answered: 'border-emerald-600 bg-emerald-50 text-emerald-700',
  unanswered: 'border-slate-300 bg-white text-slate-500 hover:border-slate-900',
};

function getStyle(status: string, isCurrent: boolean): string {
  if (isCurrent) return STATUS_STYLES[`current_${status}`] ?? STATUS_STYLES.current_unanswered;
  return STATUS_STYLES[status] ?? STATUS_STYLES.unanswered;
}

export const ExamQuestionGrid = ({ total, currentIndex, onSelect, getStatus }: ExamQuestionGridProps) => (
  <div className="flex shrink-0 flex-wrap gap-1.5 border-b-2 border-slate-200 bg-white px-4 py-2">
    {Array.from({ length: total }, (_, i) => {
      const status = getStatus(i);
      const isCurrent = i === currentIndex;
      return (
        <button
          key={i}
          onClick={() => onSelect(i)}
          className={`grid h-8 w-8 place-items-center border-2 text-xs font-black transition-all ${getStyle(status, isCurrent)}`}
        >
          {i + 1}
        </button>
      );
    })}
  </div>
);
