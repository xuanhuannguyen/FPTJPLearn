import { ChevronLeft, ChevronRight } from 'lucide-react';

interface ExamNavBarProps {
  currentIndex: number;
  total: number;
  onPrev: () => void;
  onNext: () => void;
}

export const ExamNavBar = ({ currentIndex, total, onPrev, onNext }: ExamNavBarProps) => (
  <div className="flex shrink-0 items-center justify-between border-t border-sky-200 bg-white px-4 py-3 shadow-sm md:px-6">
    <button
      disabled={currentIndex === 0}
      onClick={onPrev}
      className="inline-flex min-h-[44px] items-center gap-1.5 rounded-full border border-sky-300 bg-white px-4 py-2 text-[14px] font-extrabold text-[#0b2554] shadow-sm transition-all hover:bg-sky-50 active:scale-95 disabled:opacity-40 disabled:pointer-events-none md:min-h-[48px] md:px-5 md:text-[15px]"
    >
      <ChevronLeft size={16} /> Câu trước
    </button>
    <span className="hidden text-[13px] font-bold text-slate-500 md:inline">Dùng phím ← / → để chuyển câu</span>
    <button
      disabled={currentIndex >= total - 1}
      onClick={onNext}
      className="inline-flex min-h-[44px] items-center gap-1.5 rounded-full border border-blue-600 bg-gradient-to-r from-blue-600 to-[#2563EB] px-5 py-2 text-[14px] font-extrabold uppercase tracking-wide text-white shadow-md shadow-blue-200 transition-all hover:from-blue-700 hover:to-blue-800 active:scale-95 disabled:opacity-40 disabled:pointer-events-none disabled:shadow-none md:min-h-[48px] md:px-6 md:text-[15px]"
    >
      Câu tiếp theo <ChevronRight size={16} />
    </button>
  </div>
);
