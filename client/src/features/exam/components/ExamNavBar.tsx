import { ChevronLeft, ChevronRight } from 'lucide-react';

interface ExamNavBarProps {
  currentIndex: number;
  total: number;
  onPrev: () => void;
  onNext: () => void;
}

const BTN = 'inline-flex items-center gap-1 border-2 border-slate-900 bg-white px-3 py-1.5 text-sm font-black text-slate-900 shadow-[3px_3px_0_#111827] transition-all hover:translate-x-[1px] hover:translate-y-[1px] hover:shadow-[2px_2px_0_#111827] disabled:opacity-40 disabled:pointer-events-none';

export const ExamNavBar = ({ currentIndex, total, onPrev, onNext }: ExamNavBarProps) => (
  <div className="flex shrink-0 items-center justify-between border-t-2 border-slate-900 bg-white px-4 py-2">
    <button disabled={currentIndex === 0} onClick={onPrev} className={BTN}>
      <ChevronLeft size={16} /> Trước
    </button>
    <span className="text-xs font-black text-slate-500 hidden md:inline">← / → để chuyển câu</span>
    <button disabled={currentIndex >= total - 1} onClick={onNext} className={BTN}>
      Sau <ChevronRight size={16} />
    </button>
  </div>
);
