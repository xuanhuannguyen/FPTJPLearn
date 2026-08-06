interface ExamQuestionGridProps {
  total: number;
  currentIndex: number;
  onSelect: (index: number) => void;
  getStatus: (index: number) => 'current' | 'correct' | 'wrong' | 'answered' | 'unanswered';
}

const STATUS_STYLES: Record<string, string> = {
  current_correct: 'border-2 border-emerald-600 bg-emerald-600 text-white shadow-sm ring-2 ring-emerald-300 ring-offset-1',
  current_wrong: 'border-2 border-rose-600 bg-rose-600 text-white shadow-sm ring-2 ring-rose-300 ring-offset-1',
  current_answered: 'border-2 border-blue-600 bg-[#2563EB] text-white shadow-sm ring-2 ring-blue-300 ring-offset-1 font-extrabold',
  current_unanswered: 'border-2 border-blue-600 bg-[#2563EB] text-white shadow-sm ring-2 ring-blue-300 ring-offset-1 font-extrabold',
  correct: 'border border-emerald-400 bg-emerald-50 text-emerald-700 font-extrabold hover:bg-emerald-100 shadow-xs',
  wrong: 'border border-rose-300 bg-rose-50 text-rose-600 font-extrabold hover:bg-rose-100 shadow-xs',
  answered: 'border border-blue-300 bg-blue-50/80 text-[#2563EB] font-extrabold hover:bg-blue-100 shadow-xs',
  unanswered: 'border border-sky-200 bg-white text-slate-600 hover:border-blue-400 hover:bg-sky-50 shadow-xs',
};

function getStyle(status: string, isCurrent: boolean): string {
  if (isCurrent) return STATUS_STYLES[`current_${status}`] ?? STATUS_STYLES.current_unanswered;
  return STATUS_STYLES[status] ?? STATUS_STYLES.unanswered;
}

export const ExamQuestionGrid = ({ total, currentIndex, onSelect, getStatus }: ExamQuestionGridProps) => (
  <div className="flex shrink-0 flex-wrap gap-2 border-b border-sky-200 bg-white px-4 md:px-6 py-2.5 shadow-sm">
    {Array.from({ length: total }, (_, i) => {
      const status = getStatus(i);
      const isCurrent = i === currentIndex;
      return (
        <button
          key={i}
          onClick={() => onSelect(i)}
          className={`grid h-8 w-8 place-items-center rounded-xl text-xs font-black transition-all active:scale-95 ${getStyle(status, isCurrent)}`}
        >
          {i + 1}
        </button>
      );
    })}
  </div>
);
