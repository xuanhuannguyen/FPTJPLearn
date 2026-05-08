import { RotateCcw, AlertTriangle, CheckCircle2, Sparkles } from 'lucide-react';

type MemoryRatingButtonsProps = {
  disabled?: boolean;
  onRate: (quality: number) => void;
};

const ratings = [
  { quality: 1, label: 'Quên rồi', delay: '1 phút', icon: RotateCcw, className: 'bg-rose-100 text-rose-600 border-rose-200 hover:bg-rose-200' },
  { quality: 3, label: 'Khó', delay: '6 phút', icon: AlertTriangle, className: 'bg-orange-100 text-orange-600 border-orange-200 hover:bg-orange-200' },
  { quality: 4, label: 'Tốt', delay: '1 ngày', icon: CheckCircle2, className: 'bg-emerald-100 text-emerald-600 border-emerald-200 hover:bg-emerald-200' },
  { quality: 5, label: 'Dễ', delay: '4 ngày', icon: Sparkles, className: 'bg-blue-100 text-blue-600 border-blue-200 hover:bg-blue-200' },
];

export const MemoryRatingButtons = ({ disabled, onRate }: MemoryRatingButtonsProps) => {
  return (
    <div className="grid gap-3 sm:grid-cols-4">
      {ratings.map((rating) => {
        const Icon = rating.icon;
        return (
          <button
            key={rating.quality}
            type="button"
            disabled={disabled}
            onClick={() => onRate(rating.quality)}
            className={`flex min-h-[86px] flex-col items-center justify-center gap-1 rounded-2xl border-2 px-4 py-3 font-black transition-all active:scale-95 disabled:pointer-events-none disabled:opacity-60 ${rating.className}`}
          >
            <span className="flex items-center gap-2 text-base">
              <Icon size={18} />
              {rating.label}
            </span>
            <span className="text-sm font-extrabold opacity-80">{rating.delay}</span>
          </button>
        );
      })}
    </div>
  );
};
