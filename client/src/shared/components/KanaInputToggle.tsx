import type { KanaInputMode } from '../utils/kanaInput';

type KanaInputToggleProps = {
  mode: KanaInputMode;
  onModeChange: (mode: KanaInputMode) => void;
  disabled?: boolean;
  className?: string;
};

const options: Array<{ mode: Exclude<KanaInputMode, 'off'>; label: string; title: string }> = [
  { mode: 'hiragana', label: 'あ', title: 'Bật gõ Hiragana bằng romaji' },
  { mode: 'katakana', label: 'ア', title: 'Bật gõ Katakana bằng romaji' },
];

export const KanaInputToggle = ({
  mode,
  onModeChange,
  disabled = false,
  className = '',
}: KanaInputToggleProps) => {
  const handleModeClick = (nextMode: Exclude<KanaInputMode, 'off'>) => {
    onModeChange(mode === nextMode ? 'off' : nextMode);
  };

  return (
    <div className={`inline-flex items-center gap-1 rounded-lg border border-slate-200 bg-white p-1 ${className}`}>
      {options.map((option) => {
        const isActive = mode === option.mode;

        return (
          <button
            key={option.mode}
            type="button"
            onClick={() => handleModeClick(option.mode)}
            disabled={disabled}
            title={isActive ? 'Tắt bộ gõ trong app' : option.title}
            aria-pressed={isActive}
            className={`flex h-8 min-w-8 items-center justify-center rounded-md px-2 font-jp text-base font-black transition-colors disabled:cursor-not-allowed disabled:opacity-50 ${
              isActive
                ? 'bg-slate-900 text-white shadow-sm'
                : 'bg-white text-slate-500 hover:bg-slate-100 hover:text-slate-900'
            }`}
          >
            {option.label}
          </button>
        );
      })}
    </div>
  );
};
