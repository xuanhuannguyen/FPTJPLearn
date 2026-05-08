import { useCallback, useEffect, useState } from 'react';
import { Check, ChevronLeft, ChevronRight, RotateCcw, Shuffle, Star, X } from 'lucide-react';
import type { ReviewCard } from '../types';
import { VocabularyAudioControl } from './VocabularyAudioControl';

interface FlashcardModeProps {
  card: ReviewCard;
  direction: 'jp_to_vi' | 'vi_to_jp';
  currentIndex: number;
  totalCards: number;
  isShuffleEnabled: boolean;
  onAnswer: (quality: number, correct: boolean, message: string) => void;
  revealBack: boolean;
  onReveal: () => void;
  answered: boolean;
  onNext: () => void;
  onToggleDirection: () => void;
  onToggleShuffle: () => void;
}

export const FlashcardMode = ({
  card,
  direction,
  currentIndex,
  totalCards,
  isShuffleEnabled,
  onAnswer,
  revealBack,
  onReveal,
  answered,
  onNext,
  onToggleDirection,
  onToggleShuffle,
}: FlashcardModeProps) => {
  const isJpToVi = direction === 'jp_to_vi';
  const [flipState, setFlipState] = useState({
    cardId: card.itemId,
    isFlipped: false,
    syncedRevealBack: revealBack,
  });
  const [motionState, setMotionState] = useState({
    cardId: card.itemId,
    phase: 'idle' as 'enter' | 'idle' | 'exit-correct' | 'exit-wrong',
  });
  const isSameCard = flipState.cardId === card.itemId;
  const isFlipped = isSameCard && flipState.syncedRevealBack === revealBack
    ? flipState.isFlipped
    : revealBack;
  const isNewCard = motionState.cardId !== card.itemId;
  const motionPhase = isNewCard ? 'enter' : motionState.phase;

  const handleFlip = useCallback(() => {
    if (!isFlipped && !revealBack) {
      onReveal();
    }
    setFlipState({
      cardId: card.itemId,
      isFlipped: !isFlipped,
      syncedRevealBack: !isFlipped ? true : revealBack,
    });
  }, [card.itemId, isFlipped, onReveal, revealBack]);

  const handleAnswer = useCallback((quality: number, correct: boolean, message: string) => {
    if (answered) {
      return;
    }

    setMotionState({
      cardId: card.itemId,
      phase: correct ? 'exit-correct' : 'exit-wrong',
    });
    onAnswer(quality, correct, message);
  }, [answered, card.itemId, onAnswer]);

  // Auto advance after answering
  useEffect(() => {
    if (answered) {
      const timer = setTimeout(() => {
        onNext();
      }, 360);
      return () => clearTimeout(timer);
    }
  }, [answered, onNext]);

  useEffect(() => {
    if (!isNewCard && motionState.phase !== 'enter') {
      return;
    }

    let secondFrame = 0;
    const firstFrame = window.requestAnimationFrame(() => {
      secondFrame = window.requestAnimationFrame(() => {
        setMotionState({
          cardId: card.itemId,
          phase: 'idle',
        });
      });
    });

    return () => {
      window.cancelAnimationFrame(firstFrame);
      window.cancelAnimationFrame(secondFrame);
    };
  }, [card.itemId, isNewCard, motionState.phase]);

  useEffect(() => {
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.target instanceof HTMLInputElement || event.target instanceof HTMLTextAreaElement) {
        return;
      }

      if (event.code === 'Space') {
        event.preventDefault();
        handleFlip();
      }

      if (answered) {
        return;
      }

      if (event.key.toLowerCase() === 'x') {
        handleAnswer(1, false, 'Chưa biết - Học lại');
      }

      if (event.key.toLowerCase() === 'z') {
        handleAnswer(5, true, 'Biết - Tăng level');
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [answered, handleAnswer, handleFlip]);

  const promptText = isJpToVi ? card.word : card.meaning;
  const answerText = isJpToVi ? card.meaning : `${card.word} (${card.reading})`;
  const exampleText = isJpToVi ? card.exampleSentence : card.exampleMeaning;
  const directionLabel = isJpToVi ? 'JP→VI' : 'VI→JP';
  const motionClass = {
    enter: 'translate-x-8 scale-[0.985] opacity-0',
    idle: 'translate-x-0 scale-100 opacity-100',
    'exit-correct': '-translate-x-12 scale-[0.985] opacity-0 rotate-1',
    'exit-wrong': 'translate-x-12 scale-[0.985] opacity-0 -rotate-1',
  }[motionPhase];

  return (
    <div className={`mx-auto w-full max-w-4xl transform-gpu transition-all duration-700 ease-[cubic-bezier(0.22,1,0.36,1)] motion-reduce:transform-none motion-reduce:transition-none ${motionClass}`}>
      <div className="overflow-hidden rounded-[18px] bg-[#1b2239] shadow-[0_6px_0_rgba(15,23,42,0.92)]">
        <div className="relative min-h-[300px] bg-[#303b5d] md:min-h-[360px]">
          <button
            type="button"
            className="absolute left-3 top-1/2 flex h-10 w-10 -translate-y-1/2 items-center justify-center rounded-full bg-white/5 text-white/35 transition-colors hover:bg-white/10 hover:text-white/70"
            aria-label="Previous card"
          >
            <ChevronLeft size={24} />
          </button>

          <button
            type="button"
            onClick={onNext}
            className="absolute right-3 top-1/2 flex h-10 w-10 -translate-y-1/2 items-center justify-center rounded-full bg-white/10 text-white/55 transition-colors hover:bg-white/15 hover:text-white"
            aria-label="Next card"
          >
            <ChevronRight size={24} />
          </button>

          <button
            type="button"
            className="absolute right-5 top-5 text-white/35 transition-colors hover:text-yellow-200"
            aria-label="Mark favorite"
          >
            <Star size={20} />
          </button>

          <div className="absolute left-5 top-5">
            <VocabularyAudioControl card={card} compact tone="dark" />
          </div>

          <button
            type="button"
            onClick={handleFlip}
            className="flex min-h-[300px] w-full cursor-pointer flex-col items-center justify-center px-10 py-10 text-center md:min-h-[360px]"
          >
            <div className="font-jp text-4xl font-medium leading-none tracking-wide text-white md:text-5xl">
              {isFlipped ? answerText : promptText}
            </div>

            {!isFlipped && isJpToVi && card.reading && (
              <div className="mt-4 font-jp text-lg font-medium text-white/45">
                {card.reading}
              </div>
            )}

            {isFlipped && exampleText && (
              <div className="mt-5 max-w-xl text-xs font-semibold leading-5 text-white/55 md:text-sm">
                {card.exampleSentence && <p className="font-jp">{card.exampleSentence}</p>}
                {card.exampleMeaning && <p>{card.exampleMeaning}</p>}
              </div>
            )}
          </button>
        </div>

        <div className="flex min-h-10 items-center justify-center bg-[#465174] px-4 text-[11px] font-semibold text-slate-300/75">
          <div className="flex flex-wrap items-center justify-center gap-2">
            <span>Phím tắt:</span>
            <kbd className="rounded bg-slate-500/55 px-2 py-0.5 text-[10px] text-white/80">Space</kbd>
            <span>lật</span>
            <kbd className="rounded bg-slate-500/55 px-2 py-0.5 text-[10px] text-white/80">Z</kbd>
            <span>biết</span>
            <kbd className="rounded bg-slate-500/55 px-2 py-0.5 text-[10px] text-white/80">X</kbd>
            <span>chưa biết</span>
          </div>
        </div>

        <div className="grid min-h-[76px] grid-cols-[1fr_auto_1fr] items-center gap-4 bg-[#171d33] px-5 py-3 text-white">
          <div className="flex items-center gap-3">
            <div className="inline-flex rounded-full bg-white/10 p-0.5">
              <button className="rounded-full bg-blue-600 px-4 py-1.5 text-xs font-bold text-white shadow-[0_2px_0_rgba(0,0,0,0.35)]">
                Từ đơn
              </button>
              <button className="rounded-full px-4 py-1.5 text-xs font-bold text-slate-300">
                Ví dụ
              </button>
            </div>
          </div>

          <div className="flex items-center gap-6">
            <button
              onClick={() => handleAnswer(1, false, 'Chưa biết - Học lại')}
              disabled={answered}
              className="flex h-12 w-12 items-center justify-center rounded-full bg-rose-600/35 text-rose-400 transition-all hover:bg-rose-600/50 disabled:pointer-events-none disabled:opacity-50"
              aria-label="Chưa biết"
            >
              <X size={24} />
            </button>

            <div className="min-w-[60px] text-center text-lg font-bold">
              {currentIndex + 1} / {totalCards}
            </div>

            <button
              onClick={() => handleAnswer(5, true, 'Biết - Tăng level')}
              disabled={answered}
              className="flex h-12 w-12 items-center justify-center rounded-full bg-emerald-600/35 text-emerald-400 transition-all hover:bg-emerald-600/50 disabled:pointer-events-none disabled:opacity-50"
              aria-label="Biết"
            >
              <Check size={26} />
            </button>
          </div>

          <div className="flex items-center justify-end gap-4 text-slate-300/75">
            <button
              onClick={onToggleDirection}
              className="inline-flex items-center rounded-full bg-white/10 px-3 py-1.5 text-xs font-bold transition-colors hover:bg-white/15 hover:text-white"
            >
              ↔ {directionLabel}
            </button>
            <button
              onClick={handleFlip}
              className="transition-colors hover:text-white"
              aria-label="Flip card"
            >
              <RotateCcw size={20} />
            </button>
            <button
              onClick={onToggleShuffle}
              className={`transition-colors hover:text-white ${isShuffleEnabled ? 'text-accent-primary' : ''}`}
              aria-label="Toggle shuffle"
            >
              <Shuffle size={20} />
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
