import { useEffect, useState } from 'react';
import type { ReviewCard } from '../types';

interface FlashcardModeProps {
  card: ReviewCard;
  direction: 'jp_to_vi' | 'vi_to_jp';
  onAnswer: (quality: number, correct: boolean, message: string) => void;
  revealBack: boolean;
  onReveal: () => void;
  answered: boolean;
  onNext: () => void;
}

export const FlashcardMode = ({
  card,
  direction,
  onAnswer,
  revealBack,
  onReveal,
  answered,
  onNext,
}: FlashcardModeProps) => {
  const isJpToVi = direction === 'jp_to_vi';
  const [isFlipped, setIsFlipped] = useState(false);

  // Auto advance after answering
  useEffect(() => {
    if (answered) {
      const timer = setTimeout(() => {
        onNext();
      }, 200);
      return () => clearTimeout(timer);
    }
  }, [answered, onNext]);

  // Sync with parent's revealBack (e.g. when moving to next card)
  useEffect(() => {
    setIsFlipped(revealBack);
  }, [revealBack]);

  const handleFlip = () => {
    if (!revealBack) {
      onReveal();
    }
    setIsFlipped(!isFlipped);
  };

  return (
    <div className="w-full max-w-2xl mx-auto flex flex-col items-center">
      {/* 3D Flip Container */}
      <div 
        className="relative w-full aspect-[16/8] md:aspect-[3/1] perspective-1000"
        onClick={handleFlip}
      >
        <div 
          className={`w-full h-full transition-all duration-500 transform-style-3d cursor-pointer ${
            isFlipped ? 'rotate-y-180' : ''
          }`}
        >
          {/* FRONT FACE */}
          <div className="absolute w-full h-full rounded-[2rem] shadow-2xl backface-hidden flex flex-col items-center justify-center p-6 bg-gradient-to-br from-slate-900 via-slate-800 to-slate-900 text-white overflow-hidden border border-white/10">
            {/* Background pattern */}
            <div className="absolute inset-0 opacity-10 bg-[radial-gradient(circle_at_center,_var(--tw-gradient-from)_0%,_transparent_70%)]" />
            
            <span className="absolute top-5 left-8 text-[10px] font-black uppercase tracking-[0.2em] text-accent-primary">
              {card.wordType}
            </span>
            
            <div className="relative text-4xl md:text-6xl font-jp font-black tracking-tight drop-shadow-2xl">
              {isJpToVi ? card.word : card.meaning}
            </div>
            
            {isJpToVi && (
              <div className="mt-3 text-lg md:text-xl font-jp font-medium text-slate-400">
                {card.reading}
              </div>
            )}
            
            <div className="absolute bottom-5 text-slate-500 text-[10px] font-bold uppercase tracking-widest animate-pulse">
              Click to reveal
            </div>
          </div>

          {/* BACK FACE */}
          <div className="absolute w-full h-full rounded-[2rem] shadow-2xl backface-hidden rotate-y-180 flex flex-col items-center justify-center p-6 bg-bg-secondary border-2 border-accent-primary/30">
            <span className="absolute top-5 left-8 text-[10px] font-black uppercase tracking-[0.2em] text-accent-primary">
              Result
            </span>
            
            <div className="text-2xl md:text-4xl font-black text-text-primary text-center tracking-tight">
              {isJpToVi ? card.meaning : `${card.word} (${card.reading})`}
            </div>
            
            {(isJpToVi ? card.exampleSentence : card.exampleMeaning) && (
              <div className="mt-4 text-center max-w-lg px-4">
                <div className="text-sm md:text-base text-text-secondary font-jp italic mb-1 line-clamp-1">
                  "{card.exampleSentence}"
                </div>
                <div className="text-[10px] md:text-xs text-text-tertiary font-medium">
                  {card.exampleMeaning}
                </div>
              </div>
            )}

            <div className="absolute bottom-5 text-accent-primary/40 text-[10px] font-bold uppercase tracking-widest">
              Click to flip back
            </div>
          </div>
        </div>
      </div>

      {/* Action Buttons */}
      <div className="mt-6 w-full">
        <div className={`grid grid-cols-3 gap-3 transition-opacity duration-300 ${answered ? 'opacity-50 pointer-events-none' : 'opacity-100'}`}>
          <button
            onClick={() => onAnswer(1, false, 'Quên - Học lại')}
            disabled={answered}
            className="py-3 md:py-4 rounded-2xl bg-rose-500 hover:bg-rose-600 text-white font-bold text-base md:text-lg shadow-[0_4px_0_rgb(190,18,60)] hover:shadow-[0_2px_0_rgb(190,18,60)] hover:translate-y-[2px] transition-all disabled:hover:translate-y-0 disabled:hover:shadow-[0_4px_0_rgb(190,18,60)]"
          >
            Quên
          </button>
          <button
            onClick={() => onAnswer(3, true, 'Khó - Ôn lại sớm')}
            disabled={answered}
            className="py-3 md:py-4 rounded-2xl bg-amber-500 hover:bg-amber-600 text-white font-bold text-base md:text-lg shadow-[0_4px_0_rgb(180,83,9)] hover:shadow-[0_2px_0_rgb(180,83,9)] hover:translate-y-[2px] transition-all disabled:hover:translate-y-0 disabled:hover:shadow-[0_4px_0_rgb(180,83,9)]"
          >
            Khó
          </button>
          <button
            onClick={() => onAnswer(5, true, 'Nhớ - Tăng level')}
            disabled={answered}
            className="py-3 md:py-4 rounded-2xl bg-emerald-500 hover:bg-emerald-600 text-white font-bold text-base md:text-lg shadow-[0_4px_0_rgb(4,120,87)] hover:shadow-[0_2px_0_rgb(4,120,87)] hover:translate-y-[2px] transition-all disabled:hover:translate-y-0 disabled:hover:shadow-[0_4px_0_rgb(4,120,87)]"
          >
            Nhớ
          </button>
        </div>
      </div>
    </div>
  );
};
