import React, { useEffect, useRef, useState } from 'react';
import type { ReviewCard } from '../types';

interface TypingModeProps {
  card: ReviewCard;
  direction: 'jp_to_vi' | 'vi_to_jp';
  onAnswer: (quality: number, correct: boolean, message: string) => void;
  answered: boolean;
  feedback?: { correct: boolean; message: string };
  onNext: () => void;
}

export const TypingMode = ({
  card,
  direction,
  onAnswer,
  answered,
  feedback,
  onNext,
}: TypingModeProps) => {
  const [value, setValue] = useState('');
  const [shake, setShake] = useState(false);
  const inputRef = useRef<HTMLInputElement>(null);
  const isJpToVi = direction === 'jp_to_vi';

  useEffect(() => {
    if (!answered) {
      setValue('');
      inputRef.current?.focus();
    }
  }, [card, answered]);

  // When auto-advance or just showing feedback
  useEffect(() => {
    if (answered && feedback) {
      if (!feedback.correct) {
        setShake(true);
        setTimeout(() => setShake(false), 500);
      } else {
        // Auto advance if correct after a brief delay
        const timer = setTimeout(() => {
          onNext();
        }, 800);
        return () => clearTimeout(timer);
      }
    }
  }, [answered, feedback, onNext]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (answered || !value.trim()) {
      if (answered && !feedback?.correct) {
        onNext(); // Allow moving to next on second enter if wrong
      }
      return;
    }

    const normalized = value.trim().toLowerCase();
    
    let isCorrect = false;
    let message = '';

    if (isJpToVi) {
      isCorrect = normalized === card.meaning.toLowerCase();
      message = isCorrect 
        ? `Correct! Level moved up.` 
        : `Incorrect. Correct answer: ${card.meaning}`;
    } else {
      isCorrect = normalized === card.word.toLowerCase() || normalized === card.reading.toLowerCase();
      message = isCorrect
        ? `Correct! Level moved up.`
        : `Incorrect. Correct answers: ${card.word} or ${card.reading}`;
    }

    onAnswer(isCorrect ? 5 : 1, isCorrect, message);
  };

  const getBgColor = () => {
    if (!answered) return 'bg-[#00aaff]';
    return feedback?.correct ? 'bg-emerald-500' : 'bg-rose-500';
  };

  return (
    <div className="w-full max-w-2xl mx-auto flex flex-col items-center">
      <div className={`w-full rounded-t-3xl p-12 flex flex-col items-center justify-center text-white transition-colors duration-300 ${getBgColor()}`}>
        <div className="text-5xl md:text-7xl font-jp font-bold drop-shadow-sm mb-4 text-center">
          {isJpToVi ? card.word : card.meaning}
        </div>
        <div className="text-xl md:text-2xl font-medium opacity-90 font-jp">
          {isJpToVi ? card.wordType : ''}
        </div>
      </div>

      <form 
        onSubmit={handleSubmit} 
        className={`w-full bg-white rounded-b-3xl shadow-xl overflow-hidden ${shake ? 'animate-[shake_0.4s_ease-in-out]' : ''}`}
      >
        <div className="flex relative">
          <input
            ref={inputRef}
            type="text"
            value={value}
            onChange={(e) => setValue(e.target.value)}
            disabled={answered}
            placeholder={isJpToVi ? 'Type the Vietnamese meaning' : 'Type the Japanese word or reading'}
            className={`w-full p-6 md:p-8 text-2xl md:text-3xl font-bold text-center outline-none transition-colors ${
              answered 
                ? feedback?.correct ? 'text-emerald-600 bg-emerald-50' : 'text-rose-600 bg-rose-50'
                : 'text-slate-800'
            }`}
          />
          <button 
            type="submit"
            className="absolute right-6 top-1/2 -translate-y-1/2 p-3 text-slate-400 hover:text-slate-600"
          >
            <span className="sr-only">Submit</span>
            <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round">
              <path d="M5 12h14"></path>
              <path d="m12 5 7 7-7 7"></path>
            </svg>
          </button>
        </div>
      </form>

      {/* Answer feedback area if wrong */}
      <div className={`w-full mt-6 transition-all duration-300 overflow-hidden ${answered && !feedback?.correct ? 'opacity-100 max-h-96' : 'opacity-0 max-h-0'}`}>
        <div className="bg-white rounded-2xl p-6 shadow-md border-l-4 border-rose-500">
          <div className="text-sm font-bold text-rose-500 uppercase tracking-wider mb-2">Item Info</div>
          <div className="grid md:grid-cols-2 gap-4">
            <div>
              <div className="text-sm text-slate-500 mb-1">Meaning</div>
              <div className="text-xl font-bold text-slate-800">{card.meaning}</div>
            </div>
            <div>
              <div className="text-sm text-slate-500 mb-1">Reading</div>
              <div className="text-xl font-bold text-slate-800 font-jp">{card.word} ({card.reading})</div>
            </div>
          </div>
          <div className="mt-6 flex justify-end">
             <button
               onClick={onNext}
               className="py-3 px-6 rounded-xl bg-slate-800 hover:bg-slate-700 text-white font-bold transition-colors"
             >
               Press Enter to Continue
             </button>
          </div>
        </div>
      </div>
    </div>
  );
};
