import { Languages } from 'lucide-react';
import { useEffect, useState } from 'react';
import type { ReviewCard } from '../types';
import { VocabularyAudioControl } from './VocabularyAudioControl';

interface Option {
  label: string;
  isCorrect: boolean;
}

interface MultipleChoiceModeProps {
  card: ReviewCard;
  options: Option[];
  direction: 'jp_to_vi' | 'vi_to_jp';
  onAnswer: (quality: number, correct: boolean, message: string) => void;
  answered: boolean;
  feedback?: { correct: boolean; message: string };
  onNext: () => void;
  onToggleDirection: () => void;
}

export const MultipleChoiceMode = ({
  card,
  options,
  direction,
  onAnswer,
  answered,
  feedback,
  onNext,
  onToggleDirection,
}: MultipleChoiceModeProps) => {
  const [shakeIndex, setShakeIndex] = useState<number | null>(null);
  const isJpToVi = direction === 'jp_to_vi';

  useEffect(() => {
    // Auto advance if correct
    if (answered && feedback?.correct) {
      const timer = setTimeout(() => {
        onNext();
      }, 2000);
      return () => clearTimeout(timer);
    }
  }, [answered, feedback, onNext]);

  const handleOptionClick = (option: Option, index: number) => {
    if (answered) {
      return;
    }

    if (!option.isCorrect) {
      setShakeIndex(index);
      setTimeout(() => setShakeIndex(null), 500);
    }

    const message = option.isCorrect
      ? isJpToVi
        ? `Correct: ${card.word} = ${card.meaning}`
        : `Correct: ${card.meaning} = ${card.word}`
      : isJpToVi
        ? `Incorrect. Correct answer: ${card.meaning}`
        : `Incorrect. Correct answer: ${card.word} (${card.reading})`;

    onAnswer(option.isCorrect ? 5 : 1, option.isCorrect, message);
  };

  const getBgColor = () => {
    if (!answered) return 'bg-[#00aaff]';
    return feedback?.correct ? 'bg-emerald-500' : 'bg-rose-500';
  };

  return (
    <div className="w-full max-w-xl mx-auto flex flex-col items-center">
      {/* Prompt Area */}
      <div className={`w-full rounded-2xl p-6 md:p-8 flex flex-col items-center justify-center text-white transition-colors duration-300 shadow-xl ${getBgColor()}`}>
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
        <span className="text-[10px] font-bold uppercase tracking-wider text-white/70 mb-1.5">
          {isJpToVi ? 'Select the meaning' : 'Select the Japanese'}
        </span>
        <div className="text-3xl md:text-5xl font-jp font-bold drop-shadow-sm mb-3 text-center">
          {isJpToVi ? card.word : card.meaning}
        </div>
        <div className="text-lg md:text-xl font-medium opacity-90 font-jp">
          {isJpToVi ? card.reading : card.wordType}
        </div>
      </div>

      {/* Options Grid */}
      <div className="w-full grid grid-cols-1 sm:grid-cols-2 gap-3 mt-4">
        {options.map((option, index) => {
          let btnClass = 'bg-white text-slate-700 border-2 border-transparent hover:border-[#00aaff] hover:text-[#00aaff] shadow-[0_3px_0_rgb(226,232,240)]';
          
          if (answered) {
            if (option.isCorrect) {
              btnClass = 'bg-emerald-50 text-emerald-600 border-2 border-emerald-500 shadow-[0_3px_0_rgb(16,185,129)]';
            } else if (shakeIndex === index) {
              btnClass = 'bg-rose-50 text-rose-600 border-2 border-rose-500 shadow-[0_3px_0_rgb(244,63,94)]';
            } else {
              btnClass = 'bg-slate-50 text-slate-400 border-2 border-transparent shadow-[0_3px_0_rgb(241,245,249)] opacity-50';
            }
          }

          return (
            <button
              key={`${option.label}-${option.isCorrect ? 'correct' : 'distractor'}`}
              onClick={() => handleOptionClick(option, index)}
              className={`p-4 md:p-5 rounded-xl text-base md:text-lg font-bold font-jp transition-all hover:-translate-y-0.5 hover:shadow-[0_4px_0_rgb(226,232,240)] active:translate-y-1 active:shadow-[0_0px_0_rgb(226,232,240)] ${btnClass} ${shakeIndex === index ? 'animate-[shake_0.4s_ease-in-out]' : ''}`}
            >
              {option.label}
            </button>
          );
        })}
      </div>

      {/* Answer feedback area if wrong */}
      <div className={`w-full mt-6 transition-all duration-300 overflow-hidden ${answered && !feedback?.correct ? 'opacity-100 max-h-96' : 'opacity-0 max-h-0'}`}>
        <div className="bg-white rounded-2xl p-4 shadow-md border-l-4 border-rose-500 flex justify-between items-center">
          <div>
            <div className="text-xs text-slate-500 mb-0.5">Correct Answer</div>
            <div className="text-lg font-bold text-slate-800">
              {isJpToVi ? card.meaning : `${card.word} (${card.reading})`}
            </div>
            {(isJpToVi ? card.exampleSentence : card.exampleMeaning) && (
              <div className="mt-1.5 text-xs text-slate-600">
                {isJpToVi ? card.exampleSentence : card.exampleMeaning}
              </div>
            )}
          </div>
          <button
            onClick={onNext}
            className="py-2.5 px-5 rounded-lg bg-slate-800 hover:bg-slate-700 text-white text-sm font-bold transition-colors whitespace-nowrap ml-4"
          >
            Continue
          </button>
        </div>
      </div>
    </div>
  );
};
