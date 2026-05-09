type MemoryRatingButtonsProps = {
  disabled?: boolean;
  onRate: (quality: number) => void;
};

const ratings = [
  { quality: 1, label: 'Quên rồi', delay: '1 phút', className: 'bg-rose-200/70 text-red-500 hover:bg-rose-200' },
  { quality: 3, label: 'Khó', delay: '6 phút', className: 'bg-orange-200/55 text-orange-600 hover:bg-orange-200' },
  { quality: 4, label: 'Tốt', delay: '1 ngày', className: 'bg-emerald-200/65 text-emerald-600 hover:bg-emerald-200' },
  { quality: 5, label: 'Dễ', delay: '4 ngày', className: 'bg-blue-200/70 text-blue-500 hover:bg-blue-200' },
];

export const MemoryRatingButtons = ({ disabled, onRate }: MemoryRatingButtonsProps) => {
  return (
    <div className="grid gap-2 md:grid-cols-4">
      {ratings.map((rating) => {
        return (
          <button
            key={rating.quality}
            type="button"
            disabled={disabled}
            onClick={() => onRate(rating.quality)}
            className={`flex min-h-[60px] flex-col items-center justify-center rounded-[10px] px-4 py-2 text-center font-black transition-all active:scale-[0.98] disabled:pointer-events-none disabled:opacity-60 md:min-h-[68px] ${rating.className}`}
          >
            <span className="text-base leading-tight md:text-lg">{rating.label}</span>
            <span className="mt-0.5 text-xs font-extrabold opacity-90 md:text-sm">{rating.delay}</span>
          </button>
        );
      })}
    </div>
  );
};
