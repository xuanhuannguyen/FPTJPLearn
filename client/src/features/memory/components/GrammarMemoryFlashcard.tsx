import { ExternalLink, Keyboard } from 'lucide-react';
import { GrammarStructure } from '../../grammar/components/GrammarStructure';
import type { MemoryCard } from '../types/memory.types';

type GrammarMemoryFlashcardProps = {
  card: MemoryCard;
  isBackVisible: boolean;
  onFlip: () => void;
  onViewDetail?: () => void;
};

export const GrammarMemoryFlashcard = ({ card, isBackVisible, onFlip, onViewDetail }: GrammarMemoryFlashcardProps) => {
  return (
    <section className="perspective-1000 mx-auto w-full max-w-[800px]">
      <div
        role="button"
        tabIndex={0}
        onClick={onFlip}
        onKeyDown={(event) => {
          if (event.key === 'Enter') {
            event.preventDefault();
            onFlip();
          }
        }}
        className="group block w-full text-left outline-none"
        aria-label={isBackVisible ? 'Lật về mặt trước' : 'Lật sang mặt sau'}
      >
        <div
          className={`transform-style-3d relative min-h-[320px] rounded-[18px] transition-transform duration-500 ease-out md:min-h-[380px] ${
            isBackVisible ? 'rotate-y-180' : ''
          }`}
        >
          <div className="backface-hidden absolute inset-0 overflow-hidden rounded-[18px] bg-[#303b5f] p-5 text-slate-200 shadow-[0_18px_42px_rgba(30,41,59,0.2)] md:p-6">
            <CardTopLabel onViewDetail={onViewDetail} />
            <div className="flex min-h-[205px] flex-col items-center justify-center px-4 pb-10 pt-6 text-center md:min-h-[265px]">
              <h1 className="font-jp text-[28px] font-medium leading-tight tracking-normal text-white md:text-[36px]">
                {card.frontPrimary}
              </h1>
            </div>
            <ShortcutBar />
          </div>

          <div className="backface-hidden rotate-y-180 absolute inset-0 overflow-hidden rounded-[18px] bg-[#303b5f] p-5 text-slate-200 shadow-[0_18px_42px_rgba(30,41,59,0.2)] md:p-6">
            <CardTopLabel onViewDetail={onViewDetail} />
            <div className="mx-auto flex min-h-[210px] max-w-[650px] flex-col items-center justify-center px-3 pb-10 pt-2 text-center md:min-h-[270px]">
              <h2 className="font-jp text-[26px] font-medium leading-tight tracking-normal text-white md:text-[34px]">
                {card.frontPrimary}
              </h2>
              {card.frontSecondary ? (
                <div className="mt-1.5 flex justify-center">
                  <GrammarStructure structure={card.frontSecondary} small tone="dark" />
                </div>
              ) : null}
              {card.backPrimary ? (
                <p className="mt-3 rounded-[9px] bg-white/12 px-4 py-1.5 text-base font-black text-amber-300 md:text-lg">{card.backPrimary}</p>
              ) : null}

              {(card.backSecondary || card.example || card.notes) ? <div className="my-3 h-px w-full bg-white/10" /> : null}

              {card.backSecondary ? (
                <div className="flex justify-center">
                  <GrammarStructure structure={card.backSecondary} small tone="dark" />
                </div>
              ) : null}
              {card.example ? (
                <div className="mt-2.5 w-full text-left">
                  <p className="font-jp text-base font-bold leading-snug text-white md:text-lg">{card.example}</p>
                  {card.exampleReading ? <p className="mt-1 font-jp text-xs font-bold text-slate-400 md:text-sm">{card.exampleReading}</p> : null}
                  {card.exampleMeaning ? <p className="mt-1 text-xs font-extrabold text-slate-400 md:text-sm">{card.exampleMeaning}</p> : null}
                </div>
              ) : null}
              {card.notes ? <p className="mt-2.5 text-left text-xs font-bold leading-snug text-slate-300 md:text-sm">{card.notes}</p> : null}
            </div>
            <ShortcutBar />
          </div>
        </div>
      </div>
    </section>
  );
};

type CardTopLabelProps = {
  onViewDetail?: () => void;
};

const CardTopLabel = ({ onViewDetail }: CardTopLabelProps) => {
  const content = (
    <>
      <ExternalLink size={16} />
      <span>Xem chi tiết</span>
    </>
  );

  if (!onViewDetail) {
    return <div className="flex items-center gap-2 text-sm font-black text-slate-400">{content}</div>;
  }

  return (
    <button
      type="button"
      onClick={(event) => {
        event.stopPropagation();
        onViewDetail();
      }}
      onKeyDown={(event) => event.stopPropagation()}
      className="relative z-10 flex items-center gap-2 text-sm font-black text-slate-400 transition-colors hover:text-white"
    >
      {content}
    </button>
  );
};

const ShortcutBar = () => {
  return (
    <div className="absolute inset-x-0 bottom-0 flex min-h-10 flex-wrap items-center justify-center gap-1.5 bg-[#354163]/86 px-3 py-1.5 text-xs font-bold text-slate-400 md:flex-nowrap md:gap-2 md:text-sm">
      <Keyboard size={16} />
      <span>Nhấn</span>
      <kbd className="rounded-md bg-white/10 px-2 py-1 font-main text-xs text-slate-300 md:px-3 md:text-sm">Space</kbd>
      <span>để lật, bấm</span>
      <kbd className="rounded-md bg-white/10 px-2 py-1 font-main text-xs text-slate-300 md:px-3 md:text-sm">1-4</kbd>
      <span>để đánh giá</span>
    </div>
  );
};
