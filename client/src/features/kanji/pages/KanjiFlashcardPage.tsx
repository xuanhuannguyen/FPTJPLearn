import { useState, useEffect, useCallback } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { 
  ArrowLeft, 
  Shuffle, 
  Check, 
  X,
  Info
} from 'lucide-react';
import { kanjiApi } from '../api/kanjiApi';
import type { KanjiItem, KanjiLesson } from '../types/kanji.types';

export const KanjiFlashcardPage = () => {
  const { level, lessonId } = useParams<{ level: string; lessonId: string }>();
  const navigate = useNavigate();
  
  const [kanjis, setKanjis] = useState<KanjiItem[]>([]);
  const [lesson, setLesson] = useState<KanjiLesson | null>(null);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [isFlipped, setIsFlipped] = useState(false);
  const [isShuffleEnabled, setIsShuffleEnabled] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [completed, setCompleted] = useState(false);
  const [stats, setStats] = useState({ known: 0, unknown: 0 });

  useEffect(() => {
    const fetchData = async () => {
      if (!lessonId) return;
      try {
        const [lessonData, kanjiData] = await Promise.all([
          kanjiApi.getLessonById(lessonId),
          kanjiApi.getKanjiItemsByLesson(lessonId),
        ]);
        setLesson(lessonData);
        setKanjis(kanjiData);
      } catch (error) {
        console.error('Failed to fetch data:', error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchData();
  }, [lessonId]);

  const handleNext = useCallback(() => {
    if (currentIndex < kanjis.length - 1) {
      setCurrentIndex(prev => prev + 1);
      setIsFlipped(false);
    } else {
      setCompleted(true);
    }
  }, [currentIndex, kanjis.length]);

  const handlePrev = useCallback(() => {
    if (currentIndex > 0) {
      setCurrentIndex(prev => prev - 1);
      setIsFlipped(false);
    }
  }, [currentIndex]);

  const handleFlip = () => {
    setIsFlipped(!isFlipped);
  };

  const handleAnswer = (known: boolean) => {
    setStats(prev => ({
      known: prev.known + (known ? 1 : 0),
      unknown: prev.unknown + (known ? 0 : 1),
    }));
    handleNext();
  };

  const handleShuffle = () => {
    setIsShuffleEnabled(!isShuffleEnabled);
    const shuffled = [...kanjis].sort(() => Math.random() - 0.5);
    setKanjis(shuffled);
    setCurrentIndex(0);
    setIsFlipped(false);
    setCompleted(false);
    setStats({ known: 0, unknown: 0 });
  };

  const handleReset = () => {
    setCurrentIndex(0);
    setIsFlipped(false);
    setCompleted(false);
    setStats({ known: 0, unknown: 0 });
  };

  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (completed) return;
      if (e.code === 'Space') {
        e.preventDefault();
        handleFlip();
      } else if (e.code === 'ArrowRight') {
        handleNext();
      } else if (e.code === 'ArrowLeft') {
        handlePrev();
      } else if (e.key.toLowerCase() === 'z') {
        handleAnswer(true);
      } else if (e.key.toLowerCase() === 'x') {
        handleAnswer(false);
      }
    };
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [completed, handleFlip, handleNext, handlePrev, handleAnswer]);

  if (isLoading) {
    return (
      <div className="flex h-screen items-center justify-center blue-grid">
        <div className="h-10 w-10 animate-spin border-4 border-black border-t-transparent"></div>
      </div>
    );
  }

  if (!lesson || kanjis.length === 0) {
    return <div className="p-8 text-center font-mono">No data found</div>;
  }

  const currentKanji = kanjis[currentIndex];
  const progress = ((currentIndex) / kanjis.length) * 100;

  return (
    <div className="min-h-screen blue-grid flex flex-col font-sans">
      {/* Top Navbar */}
      <header className="flex h-14 shrink-0 items-center justify-between border-b-2 border-black bg-white px-4 shadow-[2px_0_0_0_#000]">
        <div className="flex items-center gap-3">
          <button
            onClick={() => navigate(`/kanji/${level}/lessons/${lessonId}`)}
            className="flex h-8 w-8 items-center justify-center border border-black hover:bg-black hover:text-white transition-colors"
          >
            <ArrowLeft size={16} />
          </button>
          <div className="flex flex-col">
            <span className="text-[10px] font-black uppercase tracking-widest text-text-tertiary">
              {level} • L{lesson.lessonNumber}
            </span>
            <span className="text-sm font-bold leading-none text-text-primary uppercase">
              Flashcards
            </span>
          </div>
        </div>

        <div className="flex items-center gap-4">
          <div className="hidden md:flex flex-col items-end">
            <span className="text-[10px] font-bold uppercase text-text-tertiary tracking-tighter">Tiến độ</span>
            <span className="font-mono text-xs font-bold">
              {currentIndex + 1} / {kanjis.length}
            </span>
          </div>
          <button
            onClick={handleShuffle}
            className={`flex h-8 w-8 items-center justify-center border border-black transition-colors ${isShuffleEnabled ? 'bg-black text-white' : 'hover:bg-slate-100'
              }`}
            title="Shuffle"
          >
            <Shuffle size={14} />
          </button>
        </div>
      </header>

      {/* Progress Bar */}
      <div className="h-1.5 w-full bg-white border-b-2 border-black overflow-hidden">
        <div
          className="h-full bg-accent-primary transition-all duration-500 ease-out border-r-2 border-black"
          style={{ width: `${progress}%` }}
        />
      </div>

      <main className="flex-1 flex flex-col items-center justify-center p-4">
        {completed ? (
          <div className="w-full max-w-[600px] animate-fade-in px-4">
            <div className="overflow-hidden rounded-[18px] bg-[#1b2239] shadow-[0_6px_0_rgba(15,23,42,0.92)] text-center text-white p-10">
              <div className="w-24 h-24 bg-emerald-500/20 rounded-full flex items-center justify-center text-emerald-400 mx-auto mb-8 border-4 border-emerald-500/30">
                <Check size={40} strokeWidth={4} />
              </div>
              
              <h2 className="text-4xl font-black uppercase mb-4 tracking-tight">Hoàn thành!</h2>
              <p className="text-slate-400 mb-10 font-bold uppercase text-xs tracking-[0.2em]">Thành tích ôn tập</p>
              
              <div className="grid grid-cols-2 gap-6 mb-10">
                <div className="bg-[#303b5d] p-6 rounded-xl">
                  <div className="text-4xl font-black text-emerald-400 mb-2">{stats.known}</div>
                  <div className="text-[10px] font-black uppercase text-slate-400 tracking-wider">Đã thuộc</div>
                </div>
                <div className="bg-[#303b5d] p-6 rounded-xl">
                  <div className="text-4xl font-black text-rose-400 mb-2">{stats.unknown}</div>
                  <div className="text-[10px] font-black uppercase text-slate-400 tracking-wider">Cần ôn lại</div>
                </div>
              </div>

              <div className="flex flex-col gap-4">
                <button
                  onClick={handleReset}
                  className="w-full py-4 bg-accent-primary text-white font-black uppercase tracking-[0.2em] text-xs hover:bg-blue-600 transition-colors rounded-xl"
                >
                  Luyện tập lại
                </button>
                <button
                  onClick={() => navigate(`/kanji/${level}/lessons/${lessonId}`)}
                  className="w-full py-4 bg-[#303b5d] text-white font-black uppercase tracking-[0.2em] text-xs hover:bg-[#465174] transition-colors rounded-xl"
                >
                  Về trang bài học
                </button>
              </div>
            </div>
          </div>
        ) : (
          <div className="mx-auto w-full max-w-2xl transform-gpu transition-all duration-500">
            <div className="overflow-hidden rounded-[18px] bg-[#1b2239] shadow-[0_6px_0_rgba(15,23,42,0.92)] perspective-1000">
              {/* Card Faces Container */}
              <div 
                className={`relative min-h-[270px] md:min-h-[360px] transition-all duration-500 transform-style-3d cursor-pointer ${
                  isFlipped ? 'rotate-y-180' : ''
                }`}
                onClick={handleFlip}
              >
                {/* Front Side */}
                <div className="absolute inset-0 backface-hidden bg-[#303b5d] flex flex-col items-center justify-center px-10 py-10 text-center">
                  <div className="font-jp text-[72px] md:text-[90px] font-bold leading-none tracking-wide text-white drop-shadow-md">
                    {currentKanji.character}
                  </div>
                </div>

                {/* Back Side */}
                <div className="absolute inset-0 backface-hidden rotate-y-180 bg-[#303b5d] flex flex-col items-center justify-center px-8 py-8 text-center overflow-y-auto">
                  <div className="text-2xl md:text-3xl font-black text-emerald-400 mb-2">
                    {currentKanji.hanViet}
                  </div>
                  <div className="text-lg md:text-xl font-medium text-slate-300 italic mb-6">
                    {currentKanji.meaning}
                  </div>

                  <div className="grid grid-cols-2 gap-4 w-full max-w-md mx-auto">
                    <div className="bg-[#1b2239]/50 p-4 rounded-xl border border-slate-600/30">
                      <div className="text-[10px] font-black uppercase text-slate-400 mb-2 tracking-wider">Kunyomi</div>
                      <div className="text-lg font-bold font-jp text-slate-200">{currentKanji.kunReading || '—'}</div>
                    </div>
                    <div className="bg-[#1b2239]/50 p-4 rounded-xl border border-slate-600/30">
                      <div className="text-[10px] font-black uppercase text-blue-400 mb-2 tracking-wider">Onyomi</div>
                      <div className="text-lg font-bold font-jp text-blue-300">{currentKanji.onReading || '—'}</div>
                    </div>
                  </div>

                  {currentKanji.mnemonic && (
                    <div className="mt-6 w-full max-w-md mx-auto bg-orange-900/20 border border-orange-500/20 p-4 rounded-xl text-left">
                      <div className="flex items-center gap-2 text-[10px] font-black uppercase text-orange-400 mb-2">
                        <Info size={14} />
                        Mnemonic
                      </div>
                      <p className="text-sm text-orange-200/80 leading-relaxed italic">
                        {currentKanji.mnemonic}
                      </p>
                    </div>
                  )}
                </div>
              </div>

              {/* Shortcuts */}
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

              {/* Controls */}
              <div className="flex min-h-[76px] items-center justify-center gap-8 bg-[#171d33] px-5 py-4 text-white">
                <button
                  onClick={() => handleAnswer(false)}
                  className="flex h-14 w-14 items-center justify-center rounded-full bg-rose-600/35 text-rose-400 transition-all hover:bg-rose-600/50 hover:scale-105 active:scale-95"
                  aria-label="Chưa biết"
                >
                  <X size={28} strokeWidth={3} />
                </button>

                <div className="min-w-[80px] text-center text-xl font-bold tracking-wider">
                  {currentIndex + 1} / {kanjis.length}
                </div>

                <button
                  onClick={() => handleAnswer(true)}
                  className="flex h-14 w-14 items-center justify-center rounded-full bg-emerald-600/35 text-emerald-400 transition-all hover:bg-emerald-600/50 hover:scale-105 active:scale-95"
                  aria-label="Biết"
                >
                  <Check size={28} strokeWidth={3} />
                </button>
              </div>
            </div>
          </div>
        )}
      </main>
    </div>
  );
};
