import { Keyboard } from 'lucide-react';
import type { MemoryCard } from '../types/memory.types';

type VocabularyMemoryFlashcardProps = {
  card: MemoryCard;
  isBackVisible: boolean;
  onFlip: () => void;
};

export const VocabularyMemoryFlashcard = ({ card, isBackVisible, onFlip }: VocabularyMemoryFlashcardProps) => {
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
            <CardTopLabel />
            <div className="flex min-h-[210px] flex-col items-center justify-center px-4 pb-10 pt-6 text-center md:min-h-[270px]">
              <h1 className="font-jp text-[44px] font-black leading-tight tracking-normal text-white md:text-[58px]">
                {card.frontPrimary}
              </h1>
              {card.frontSecondary ? (
                <p className="mt-3 font-jp text-xl font-black text-slate-300 md:text-2xl">{card.frontSecondary}</p>
              ) : null}
              {card.frontMeta ? (
                <p className="mt-4 rounded-lg bg-white/10 px-3 py-1 text-xs font-black uppercase tracking-[0.18em] text-slate-300">
                  {card.frontMeta}
                </p>
              ) : null}
            </div>
            <ShortcutBar />
          </div>

          <div className="backface-hidden rotate-y-180 absolute inset-0 overflow-hidden rounded-[18px] bg-[#303b5f] p-5 text-slate-200 shadow-[0_18px_42px_rgba(30,41,59,0.2)] md:p-6">
            <CardTopLabel />
            <div className="mx-auto flex min-h-[210px] max-w-[650px] flex-col items-center justify-center px-3 pb-10 pt-4 text-center md:min-h-[270px]">
              <p className="text-[11px] font-black uppercase tracking-[0.22em] text-emerald-300">Nghĩa</p>
              <h2 className="mt-3 text-3xl font-black leading-tight text-white md:text-4xl">{card.backPrimary}</h2>

              <div className="my-6 h-px w-full bg-white/10" />

              <div className="grid w-full gap-3 sm:grid-cols-2">
                <div className="rounded-xl border border-white/10 bg-white/8 p-4">
                  <p className="text-[10px] font-black uppercase tracking-wider text-slate-400">Từ vựng</p>
                  <p className="mt-2 font-jp text-2xl font-black text-white">{card.frontPrimary}</p>
                </div>
                <div className="rounded-xl border border-white/10 bg-white/8 p-4">
                  <p className="text-[10px] font-black uppercase tracking-wider text-slate-400">Cách đọc</p>
                  <p className="mt-2 font-jp text-2xl font-black text-emerald-200">{card.frontSecondary || '—'}</p>
                </div>
              </div>

              {card.notes ? (
                <p className="mt-4 w-full rounded-xl border border-white/10 bg-white/8 p-3 text-left text-sm font-bold leading-relaxed text-slate-300">
                  {card.notes}
                </p>
              ) : null}
            </div>
            <ShortcutBar />
          </div>
        </div>
      </div>
    </section>
  );
};

const CardTopLabel = () => {
  return (
    <div className="flex items-center justify-between text-sm font-black text-slate-400">
      <span>Từ vựng flashcard</span>
      <span>Nhấn để lật</span>
    </div>
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
