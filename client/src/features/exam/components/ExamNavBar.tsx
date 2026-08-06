import { ChevronLeft, ChevronRight } from 'lucide-react';

interface ExamNavBarProps {
  currentIndex: number;
  total: number;
  onPrev: () => void;
  onNext: () => void;
}

export const ExamNavBar = ({ currentIndex, total, onPrev, onNext }: ExamNavBarProps) => (
  <div className="flex shrink-0 items-center justify-between border-t border-sky-200 bg-white px-4 md:px-6 py-3 shadow-sm">
    <button
      disabled={currentIndex === 0}
      onClick={onPrev}
      className="inline-flex items-center gap-1.5 rounded-full border border-sky-300 bg-white px-4 md:px-5 py-2 text-xs md:text-sm font-extrabold text-[#0b2554] shadow-sm transition-all hover:bg-sky-50 active:scale-95 disabled:opacity-40 disabled:pointer-events-none"
    >
      <ChevronLeft size={16} /> Câu trước
    </button>
    <span className="text-xs font-bold text-slate-500 hidden md:inline">Dùng phím ← / → để chuyển câu</span>
    <button
      disabled={currentIndex >= total - 1}
      onClick={onNext}
      className="inline-flex items-center gap-1.5 rounded-full border border-blue-600 bg-gradient-to-r from-blue-600 to-[#2563EB] px-5 md:px-6 py-2 text-xs md:text-sm font-extrabold uppercase tracking-wider text-white shadow-md shadow-blue-200 transition-all hover:from-blue-700 hover:to-blue-800 active:scale-95 disabled:opacity-40 disabled:pointer-events-none disabled:shadow-none"
    >
      Câu tiếp theo <ChevronRight size={16} />
    </button>
  </div>
);
