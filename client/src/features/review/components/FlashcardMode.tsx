import { useCallback, useEffect, useState } from 'react';
import { Check, ChevronLeft, ChevronRight, Shuffle, Star, Undo, X } from 'lucide-react';
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
  onPrev?: () => void;
  onToggleDirection: () => void;
  onToggleShuffle: () => void;

  // Progress tracking props
  isProgressTracking?: boolean;
  onToggleProgressTracking?: () => void;
  isRoundFinished?: boolean;
  unknownCount?: number;
  onStudyUnknown?: () => void;
  onResetAll?: () => void;
  onUndo?: () => void;
  canUndo?: boolean;
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
  onPrev,
  onToggleDirection,
  onToggleShuffle,
  isProgressTracking = false,
  onToggleProgressTracking,
  isRoundFinished = false,
  unknownCount = 0,
  onStudyUnknown,
  onResetAll,
  onUndo,
  canUndo = false,
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

      if (!isProgressTracking) {
        if (event.code === 'ArrowLeft' && onPrev) {
          event.preventDefault();
          onPrev();
        }

        if (event.code === 'ArrowRight') {
          event.preventDefault();
          onNext();
        }
      } else {
        if (event.code === 'Backspace' && onUndo && canUndo) {
          event.preventDefault();
          onUndo();
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
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [answered, handleAnswer, handleFlip, onPrev, onNext, isProgressTracking, onUndo, canUndo]);

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

  if (isRoundFinished) {
    return (
      <div className="mx-auto w-full max-w-4xl transform-gpu transition-all duration-700 ease-[cubic-bezier(0.22,1,0.36,1)]">
        <div className="overflow-hidden rounded-[18px] bg-[#1b2239] shadow-[0_6px_0_rgba(15,23,42,0.92)]">
          <div className="relative min-h-[300px] bg-[#303b5d] flex flex-col items-center justify-center p-8 text-center md:min-h-[360px]">
            {unknownCount > 0 ? (
              <div className="space-y-6">
                <div className="space-y-2">
                  <h2 className="text-3xl font-black text-white">Kết quả vòng học</h2>
                  <p className="text-lg font-bold text-slate-300">
                    Bạn còn <span className="text-rose-400 font-extrabold">{unknownCount}</span> từ chưa thuộc.
                  </p>
                </div>
                <div className="flex flex-wrap justify-center gap-4">
                  {onStudyUnknown && (
                    <button
                      onClick={onStudyUnknown}
                      className="inline-flex h-12 items-center gap-2 rounded-xl border-2 border-slate-800 bg-emerald-600 px-6 text-sm font-black text-white shadow-pop transition-all hover:-translate-y-0.5"
                    >
                      Học tiếp {unknownCount} từ chưa thuộc
                    </button>
                  )}
                  {onResetAll && (
                    <button
                      onClick={onResetAll}
                      className="inline-flex h-12 items-center gap-2 rounded-xl border-2 border-slate-800 bg-slate-700 px-6 text-sm font-black text-white shadow-pop transition-all hover:-translate-y-0.5"
                    >
                      Học lại toàn bộ từ đầu
                    </button>
                  )}
                </div>
              </div>
            ) : (
              <div className="space-y-6">
                <div className="space-y-2">
                  <div className="mx-auto flex h-16 w-16 items-center justify-center rounded-full bg-emerald-500/20 text-emerald-400 animate-bounce">
                    <Check size={40} strokeWidth={3} />
                  </div>
                  <h2 className="text-3xl font-black text-white">Chúc mừng! 🎉</h2>
                  <p className="text-lg font-bold text-slate-300">
                    Bạn đã học thuộc toàn bộ từ vựng!
                  </p>
                </div>
                <div className="flex justify-center">
                  {onResetAll && (
                    <button
                      onClick={onResetAll}
                      className="inline-flex h-12 items-center gap-2 rounded-xl border-2 border-slate-800 bg-emerald-600 px-6 text-sm font-black text-white shadow-pop transition-all hover:-translate-y-0.5"
                    >
                      Học lại từ đầu
                    </button>
                  )}
                </div>
              </div>
            )}
          </div>
          <div className="grid min-h-[76px] grid-cols-[1fr_auto_1fr] items-center gap-4 bg-[#171d33] px-5 py-3 text-white">
            <div className="flex items-center gap-3">
              {onToggleProgressTracking ? (
                <>
                  <span className="text-xs font-black tracking-wider text-slate-300">Track progress</span>
                  <button
                    type="button"
                    onClick={onToggleProgressTracking}
                    className="relative inline-flex h-5 w-9 shrink-0 cursor-pointer rounded-full border-2 border-transparent transition-colors duration-200 ease-in-out focus:outline-none bg-blue-600"
                    aria-label="Toggle progress tracking"
                  >
                    <span className="pointer-events-none inline-block h-4 w-4 transform rounded-full bg-white shadow ring-0 transition duration-200 ease-in-out translate-x-4" />
                  </button>
                </>
              ) : (
                <span className="text-xs font-black tracking-wider text-slate-400">Track progress active</span>
              )}
            </div>
            <div className="text-center text-slate-400 font-bold text-sm">
              Hoàn thành vòng
            </div>
            <div className="flex items-center justify-end gap-4 text-slate-400">
              {/* Spacer */}
            </div>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className={`mx-auto w-full max-w-4xl transform-gpu transition-all duration-700 ease-[cubic-bezier(0.22,1,0.36,1)] motion-reduce:transform-none motion-reduce:transition-none ${motionClass}`}>
      <div className="overflow-hidden rounded-[18px] bg-[#1b2239] shadow-[0_6px_0_rgba(15,23,42,0.92)]">
        <div className="relative min-h-[300px] bg-[#303b5d] md:min-h-[360px]">
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

        <div className="flex min-h-11 items-center justify-center bg-[#9ea5f4] text-slate-900 px-4 text-xs font-bold gap-2">
          <span className="inline-flex items-center gap-1.5">
            <span className="flex h-5 items-center justify-center rounded bg-amber-400 px-1.5 py-0.5 text-[10px] font-extrabold text-slate-900 border border-slate-900/10">
              ⌨️
            </span>
            <span>Shortcut: Press</span>
            <kbd className="rounded bg-slate-900 text-white px-1.5 py-0.5 text-[10px] font-black font-mono">Space</kbd>
            <span>or click on the card to flip</span>
          </span>
          {isProgressTracking ? (
            <>
              <span className="text-slate-950/40">|</span>
              <span className="inline-flex items-center gap-1">
                Press <kbd className="rounded bg-slate-900 text-white px-1.5 py-0.5 text-[10px] font-black font-mono">Z</kbd> to know,
                <kbd className="rounded bg-slate-900 text-white px-1.5 py-0.5 text-[10px] font-black font-mono">X</kbd> to study again,
                <kbd className="rounded bg-slate-900 text-white px-1.5 py-0.5 text-[10px] font-black font-mono">Backspace</kbd> to go back
              </span>
            </>
          ) : (
            <>
              <span className="text-slate-950/40">|</span>
              <span className="inline-flex items-center gap-1">
                Press <kbd className="rounded bg-slate-900 text-white px-1.5 py-0.5 text-[10px] font-black font-mono">←</kbd> to prev,
                <kbd className="rounded bg-slate-900 text-white px-1.5 py-0.5 text-[10px] font-black font-mono">→</kbd> to next
              </span>
            </>
          )}
        </div>

        <div className="grid min-h-[76px] grid-cols-[1fr_auto_1fr] items-center gap-4 bg-[#171d33] px-5 py-3 text-white">
          {/* Left: Track progress Toggle Switch */}
          <div className="flex items-center gap-3">
            {onToggleProgressTracking ? (
              <>
                <span className="text-xs font-black tracking-wider text-slate-300">Track progress</span>
                <button
                  type="button"
                  onClick={onToggleProgressTracking}
                  className={`relative inline-flex h-5 w-9 shrink-0 cursor-pointer rounded-full border-2 border-transparent transition-colors duration-200 ease-in-out focus:outline-none ${
                    isProgressTracking ? 'bg-blue-600' : 'bg-slate-700'
                  }`}
                  aria-label="Toggle progress tracking"
                >
                  <span
                    className={`pointer-events-none inline-block h-4 w-4 transform rounded-full bg-white shadow ring-0 transition duration-200 ease-in-out ${
                      isProgressTracking ? 'translate-x-4' : 'translate-x-0'
                    }`}
                  />
                </button>
              </>
            ) : (
              <span className="text-xs font-black tracking-wider text-slate-400">Track progress active</span>
            )}
          </div>

          {/* Center: Navigation or Rating Pill */}
          <div className="flex items-center justify-center">
            {isProgressTracking ? (
              <div className="inline-flex items-center gap-4 rounded-full bg-slate-800/90 p-1 border border-slate-700/50 shadow-inner">
                <button
                  type="button"
                  onClick={() => handleAnswer(1, false, 'Chưa biết - Học lại')}
                  disabled={answered}
                  className="flex h-10 w-10 items-center justify-center rounded-full text-rose-500 hover:bg-rose-500/20 disabled:pointer-events-none disabled:opacity-30 transition-all"
                  aria-label="Chưa biết"
                >
                  <X size={20} strokeWidth={3} />
                </button>

                <div className="min-w-[60px] text-center text-sm font-black tracking-wider text-white">
                  {currentIndex + 1} / {totalCards}
                </div>

                <button
                  type="button"
                  onClick={() => handleAnswer(5, true, 'Biết - Tăng level')}
                  disabled={answered}
                  className="flex h-10 w-10 items-center justify-center rounded-full text-emerald-400 hover:bg-emerald-500/20 disabled:pointer-events-none disabled:opacity-30 transition-all"
                  aria-label="Biết"
                >
                  <Check size={20} strokeWidth={3} />
                </button>
              </div>
            ) : (
              <div className="inline-flex items-center gap-4 rounded-full bg-slate-800/90 p-1 border border-slate-700/50 shadow-inner">
                <button
                  type="button"
                  onClick={onPrev}
                  disabled={!onPrev || currentIndex === 0}
                  className="flex h-10 w-10 items-center justify-center rounded-full text-slate-300 hover:bg-white/10 hover:text-white disabled:pointer-events-none disabled:opacity-20 transition-all"
                  aria-label="Previous card"
                >
                  <ChevronLeft size={20} />
                </button>

                <div className="min-w-[60px] text-center text-sm font-black tracking-wider text-white">
                  {currentIndex + 1} / {totalCards}
                </div>

                <button
                  type="button"
                  onClick={onNext}
                  className="flex h-10 w-10 items-center justify-center rounded-full text-slate-300 hover:bg-white/10 hover:text-white transition-all"
                  aria-label="Next card"
                >
                  <ChevronRight size={20} />
                </button>
              </div>
            )}
          </div>

          {/* Right: Actions (Direction, Shuffle, Back/Undo) */}
          <div className="flex items-center justify-end gap-3 text-slate-300">
            {isProgressTracking && onUndo && (
              <button
                type="button"
                onClick={onUndo}
                disabled={!canUndo}
                className="flex h-10 w-10 items-center justify-center rounded-full bg-slate-800/90 border border-slate-700/50 text-slate-300 hover:bg-white/10 hover:text-white disabled:pointer-events-none disabled:opacity-20 transition-all"
                title="Quay lại thẻ trước (Backspace)"
              >
                <Undo size={18} />
              </button>
            )}

            <button
              type="button"
              onClick={onToggleShuffle}
              className={`flex h-10 w-10 items-center justify-center rounded-full bg-slate-800/90 border border-slate-700/50 transition-all hover:bg-white/10 hover:text-white ${
                isShuffleEnabled ? 'text-blue-400 border-blue-500/50' : 'text-slate-300'
              }`}
              title="Trộn thẻ"
            >
              <Shuffle size={18} />
            </button>

            <button
              type="button"
              onClick={onToggleDirection}
              className="flex h-10 w-10 items-center justify-center rounded-full bg-slate-800/90 border border-slate-700/50 text-slate-300 hover:bg-white/10 hover:text-white transition-all"
              title={`Đổi chiều: ${directionLabel}`}
            >
              <span className="text-[11px] font-extrabold tracking-tight">{isJpToVi ? 'JA' : 'VI'}</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
