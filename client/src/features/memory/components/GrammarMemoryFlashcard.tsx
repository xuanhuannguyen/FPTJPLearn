import { RotateCcw } from 'lucide-react';
import type { MemoryCard } from '../types/memory.types';

type GrammarMemoryFlashcardProps = {
  card: MemoryCard;
  isBackVisible: boolean;
  onFlip: () => void;
};

export const GrammarMemoryFlashcard = ({ card, isBackVisible, onFlip }: GrammarMemoryFlashcardProps) => {
  return (
    <section className="rounded-[28px] bg-white p-5 shadow-card">
      <div className="min-h-[390px] rounded-[22px] border-2 border-border bg-gradient-to-br from-violet-50 via-white to-sky-50 p-8">
        {!isBackVisible ? (
          <div className="flex h-full min-h-[330px] flex-col items-center justify-center text-center">
            <p className="text-xs font-black uppercase tracking-[0.22em] text-violet-500">Ngữ pháp</p>
            <h1 className="mt-4 font-jp text-5xl font-black tracking-normal text-text-primary">{card.frontPrimary}</h1>
            {card.frontSecondary ? (
              <p className="mt-6 rounded-2xl bg-white px-5 py-3 font-jp text-xl font-black text-text-secondary shadow-sm">
                {card.frontSecondary}
              </p>
            ) : null}
            {card.frontMeta ? (
              <p className="mt-4 max-w-2xl text-lg font-bold text-text-muted">{card.frontMeta}</p>
            ) : null}
            <button type="button" onClick={onFlip} className="btn-primary mt-10">
              <RotateCcw size={18} />
              Lật card
            </button>
          </div>
        ) : (
          <div className="mx-auto flex min-h-[330px] max-w-4xl flex-col justify-center space-y-6">
            <div>
              <p className="text-xs font-black uppercase tracking-[0.22em] text-violet-500">Đáp án</p>
              <h2 className="mt-3 text-3xl font-black text-text-primary">{card.backPrimary}</h2>
            </div>
            {card.backSecondary ? (
              <div className="rounded-2xl bg-white p-4 shadow-sm">
                <p className="text-xs font-black uppercase tracking-widest text-text-tertiary">Cấu trúc</p>
                <p className="mt-2 font-jp text-xl font-black text-text-secondary">{card.backSecondary}</p>
              </div>
            ) : null}
            {card.example ? (
              <div className="rounded-2xl border-2 border-border/10 bg-white p-4">
                <p className="font-jp text-2xl font-black text-text-primary">{card.example}</p>
                {card.exampleReading ? <p className="mt-1 font-jp text-sm font-bold text-text-muted">{card.exampleReading}</p> : null}
                {card.exampleMeaning ? <p className="mt-3 text-base font-bold text-text-secondary">{card.exampleMeaning}</p> : null}
              </div>
            ) : null}
            {card.notes ? (
              <p className="rounded-2xl bg-amber-50 p-4 text-sm font-bold leading-relaxed text-amber-800">{card.notes}</p>
            ) : null}
          </div>
        )}
      </div>
    </section>
  );
};
