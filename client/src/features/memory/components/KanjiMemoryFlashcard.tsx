import { Info } from 'lucide-react';
import type { MemoryCard } from '../types/memory.types';

type KanjiMemoryFlashcardProps = {
  card: MemoryCard;
  isBackVisible: boolean;
  onFlip: () => void;
};

export const KanjiMemoryFlashcard = ({ card, isBackVisible, onFlip }: KanjiMemoryFlashcardProps) => {
  // Parse backSecondary to extract Kun, On, and Mnemonic if we encoded it as JSON or string
  // Based on Kanji data, we might need to parse. For now, we will render it safely.
  
  // Safely parse frontMeta and backSecondary
  let kunReading = '—';
  let onReading = '—';
  let mnemonic = '';
  
  try {
    if (card.backSecondary) {
      const parsed = JSON.parse(card.backSecondary);
      kunReading = parsed.kunReading || '—';
      onReading = parsed.onReading || '—';
      mnemonic = parsed.mnemonic || '';
    }
  } catch {
    // If not JSON, maybe it's just raw text
    mnemonic = card.backSecondary || '';
  }

  return (
    <div className="mx-auto w-full max-w-2xl transform-gpu transition-all duration-500 mb-6">
      <div className="overflow-hidden rounded-[18px] bg-[#1b2239] shadow-[0_6px_0_rgba(15,23,42,0.92)] perspective-1000">
        <div 
          className={`relative min-h-[270px] md:min-h-[360px] transition-all duration-500 transform-style-3d cursor-pointer ${
            isBackVisible ? 'rotate-y-180' : ''
          }`}
          onClick={onFlip}
        >
          {/* Front Side */}
          <div className="absolute inset-0 backface-hidden bg-[#303b5d] flex flex-col items-center justify-center px-10 py-10 text-center">
            <div className="font-jp text-[72px] md:text-[90px] font-bold leading-none tracking-wide text-white drop-shadow-md">
              {card.frontPrimary}
            </div>
            {!isBackVisible && (
              <p className="mt-8 text-xs font-bold uppercase tracking-[0.2em] text-slate-400">
                Nhấn để lật thẻ
              </p>
            )}
          </div>

          {/* Back Side */}
          <div className="absolute inset-0 backface-hidden rotate-y-180 bg-[#303b5d] flex flex-col items-center justify-center px-8 py-8 text-center overflow-y-auto">
            <div className="text-2xl md:text-3xl font-black text-emerald-400 mb-2">
              {card.frontSecondary || card.frontMeta} {/* HanViet */}
            </div>
            <div className="text-lg md:text-xl font-medium text-slate-300 italic mb-6">
              {card.backPrimary} {/* Meaning */}
            </div>

            <div className="grid grid-cols-2 gap-4 w-full max-w-md mx-auto">
              <div className="bg-[#1b2239]/50 p-4 rounded-xl border border-slate-600/30">
                <div className="text-[10px] font-black uppercase text-slate-400 mb-2 tracking-wider">Kunyomi</div>
                <div className="text-lg font-bold font-jp text-slate-200">{kunReading}</div>
              </div>
              <div className="bg-[#1b2239]/50 p-4 rounded-xl border border-slate-600/30">
                <div className="text-[10px] font-black uppercase text-blue-400 mb-2 tracking-wider">Onyomi</div>
                <div className="text-lg font-bold font-jp text-blue-300">{onReading}</div>
              </div>
            </div>

            {mnemonic && (
              <div className="mt-6 w-full max-w-md mx-auto bg-orange-900/20 border border-orange-500/20 p-4 rounded-xl text-left">
                <div className="flex items-center gap-2 text-[10px] font-black uppercase text-orange-400 mb-2">
                  <Info size={14} />
                  Mnemonic
                </div>
                <p className="text-sm text-orange-200/80 leading-relaxed italic">
                  {mnemonic}
                </p>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};
