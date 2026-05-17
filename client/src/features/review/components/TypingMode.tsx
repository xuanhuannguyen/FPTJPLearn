import type { FormEvent } from 'react';
import { useEffect, useRef, useState } from 'react';
import { Languages } from 'lucide-react';
import type { ReviewCard } from '../types';
import { VocabularyAudioControl } from './VocabularyAudioControl';
import { KanaInputToggle } from '../../../shared/components/KanaInputToggle';
import {
  convertRomajiToKana,
  normalizeKanaAnswer,
  type KanaInputMode,
} from '../../../shared/utils/kanaInput';

interface TypingModeProps {
  card: ReviewCard;
  direction: 'jp_to_vi' | 'vi_to_jp';
  onAnswer: (quality: number, correct: boolean, message: string) => void;
  answered: boolean;
  feedback?: { correct: boolean; message: string };
  onNext: () => void;
  onToggleDirection: () => void;
}

export const TypingMode = ({
  card,
  direction,
  onAnswer,
  answered,
  feedback,
  onNext,
  onToggleDirection,
}: TypingModeProps) => {
  const [inputState, setInputState] = useState({ cardId: card.itemId, value: '' });
  const [kanaMode, setKanaMode] = useState<KanaInputMode>('off');
  const inputRef = useRef<HTMLInputElement>(null);
  const isJpToVi = direction === 'jp_to_vi';
  const shouldShowKanaToggle = !isJpToVi;
  const value = inputState.cardId === card.itemId ? inputState.value : '';
  const showWrongFeedback = answered && feedback && !feedback.correct;

  useEffect(() => {
    if (!answered) {
      inputRef.current?.focus();
    }
  }, [card, answered]);

  // When auto-advance or just showing feedback
  useEffect(() => {
    if (answered && feedback?.correct) {
      const timer = setTimeout(() => {
        onNext();
      }, 800);
      return () => clearTimeout(timer);
    }
  }, [answered, feedback, onNext]);

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    if (answered || !value.trim()) {
      if (answered && !feedback?.correct) {
        onNext(); // Allow moving to next on second enter if wrong
      }
      return;
    }

    const normalized = normalizeKanaAnswer(value, shouldShowKanaToggle ? kanaMode : 'off');
    const isCorrect = isJpToVi
      ? normalized === card.meaning.trim().normalize('NFKC').toLowerCase()
      : normalized === normalizeKanaAnswer(card.word) || normalized === normalizeKanaAnswer(card.reading);
    const message = isCorrect
      ? 'Correct! Level moved up.'
      : isJpToVi
        ? `Incorrect. Correct answer: ${card.meaning}`
        : `Incorrect. Correct answers: ${card.word} or ${card.reading}`;

    onAnswer(isCorrect ? 5 : 1, isCorrect, message);
  };

  const getBgColor = () => {
    if (!answered) return 'bg-[#00aaff]';
    return feedback?.correct ? 'bg-emerald-500' : 'bg-rose-500';
  };

  return (
    <div className="w-full max-w-xl mx-auto flex flex-col items-center">
      <div className={`w-full rounded-t-2xl p-6 md:p-8 flex flex-col items-center justify-center text-white transition-colors duration-300 ${getBgColor()}`}>
        <div className="mb-4 flex flex-wrap items-center justify-center gap-3">
          <VocabularyAudioControl card={card} tone="dark" />
          <button
            type="button"
            onClick={onToggleDirection}
            className="inline-flex min-h-9 items-center justify-center gap-2 rounded-xl border-2 border-white/10 bg-white/10 px-3 text-xs font-extrabold text-white/80 transition-all hover:bg-white/15 hover:text-white"
            aria-label="Chuyển hướng tiếng Nhật tiếng Việt"
          >
            <Languages size={16} />
            <span>{isJpToVi ? 'JP → VI' : 'VI → JP'}</span>
          </button>
        </div>
        <div className="text-3xl md:text-5xl font-jp font-bold drop-shadow-sm text-center">
          {isJpToVi ? card.word : card.meaning}
        </div>
        {isJpToVi && (
          <div className="mt-2 text-xl md:text-2xl font-jp font-bold text-white/90">
            {card.reading}
          </div>
        )}
        <div className="mt-3 text-lg md:text-xl font-medium opacity-90">
          {card.wordType}
        </div>
      </div>

      <form 
        onSubmit={handleSubmit} 
        className={`w-full bg-white rounded-b-2xl shadow-xl overflow-hidden ${showWrongFeedback ? 'animate-[shake_0.4s_ease-in-out]' : ''}`}
      >
        {shouldShowKanaToggle ? (
          <div className="flex items-center justify-between gap-3 border-b border-slate-100 bg-slate-50/70 px-4 py-2">
            <span className="text-[11px] font-black uppercase tracking-wider text-slate-400">
              Bộ gõ trong app
            </span>
            <KanaInputToggle mode={kanaMode} onModeChange={setKanaMode} disabled={answered} />
          </div>
        ) : null}
        <div className="flex relative">
          <input
            ref={inputRef}
            type="text"
            value={value}
            onChange={(e) =>
              setInputState({
                cardId: card.itemId,
                value: convertRomajiToKana(e.target.value, shouldShowKanaToggle ? kanaMode : 'off'),
              })
            }
            disabled={answered}
            placeholder={isJpToVi ? 'Type meaning...' : 'Type Japanese...'}
            className={`w-full p-4 md:p-5 text-xl md:text-2xl font-bold text-center outline-none transition-colors ${
              answered 
                ? feedback?.correct ? 'text-emerald-600 bg-emerald-50' : 'text-rose-600 bg-rose-50'
                : 'text-slate-800'
            }`}
          />
          <button 
            type="submit"
            className="absolute right-4 top-1/2 -translate-y-1/2 p-2 text-slate-400 hover:text-slate-600"
          >
            <span className="sr-only">Submit</span>
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round">
              <path d="M5 12h14"></path>
              <path d="m12 5 7 7-7 7"></path>
            </svg>
          </button>
        </div>
      </form>

      {/* Answer feedback area if wrong */}
      <div className={`w-full mt-4 transition-all duration-300 overflow-hidden ${showWrongFeedback ? 'opacity-100 max-h-96' : 'opacity-0 max-h-0'}`}>
        <div className="bg-white rounded-2xl p-4 shadow-md border-l-4 border-rose-500">
          <div className="text-[10px] font-bold text-rose-500 uppercase tracking-wider mb-2">Item Info</div>
          <div className="grid md:grid-cols-2 gap-3">
            <div>
              <div className="text-[10px] text-slate-500 mb-0.5">Meaning</div>
              <div className="text-lg font-bold text-slate-800">{card.meaning}</div>
            </div>
            <div>
              <div className="text-[10px] text-slate-500 mb-0.5">Reading</div>
              <div className="text-lg font-bold text-slate-800 font-jp">{card.word} ({card.reading})</div>
            </div>
          </div>
          <div className="mt-4 flex justify-end">
             <button
               onClick={onNext}
               className="py-2 px-4 rounded-lg bg-slate-800 hover:bg-slate-700 text-white text-sm font-bold transition-colors"
             >
               Press Enter to Continue
             </button>
          </div>
        </div>
      </div>
    </div>
  );
};
