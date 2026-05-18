import { useState, useRef, useEffect, useCallback } from 'react';
import { Link, Navigate, useParams } from 'react-router-dom';
import { ArrowLeft, ArrowRight, ArrowDown, BookOpenText, Keyboard, Play, Volume2, CheckCircle2, RefreshCw, X } from 'lucide-react';
import { getKanaRows } from '../data/kana';
import { getTypingCategories } from '../data/typingData';
import type { TypingCategory, TypingItem } from '../data/typingData';

const playPronunciation = (text: string) => {
  if (typeof window === 'undefined' || !('speechSynthesis' in window)) {
    return;
  }
  window.speechSynthesis.cancel();
  const utterance = new SpeechSynthesisUtterance(text);
  utterance.lang = 'ja-JP';
  utterance.rate = 0.8;
  utterance.pitch = 1;
  window.speechSynthesis.speak(utterance);
};

const scriptLabels = {
  hiragana: {
    title: 'Hiragana',
    jp: 'ひらがな',
  },
  katakana: {
    title: 'Katakana',
    jp: 'カタカナ',
  },
} as const;

const modeLabels = {
  mnemonic: {
    title: 'Học nhớ mẹo',
    description: 'Màn này sẽ hiển thị từng chữ kèm hình minh họa và mẹo ghi nhớ.',
    icon: <BookOpenText size={30} />,
  },
  typing: {
    title: 'Typing',
    description: 'Luyện gõ romaji tương ứng cho từng mặt chữ Kana. Chọn các nhóm chữ bên dưới để bắt đầu thử thách!',
    icon: <Keyboard size={30} />,
  },
} as const;

type ScriptKey = keyof typeof scriptLabels;
type ModeKey = keyof typeof modeLabels;

const isScriptKey = (value?: string): value is ScriptKey => value === 'hiragana' || value === 'katakana';
const isModeKey = (value?: string): value is ModeKey => value === 'mnemonic' || value === 'typing';

const calculateScore = (totalChars: number, mistakes: number, elapsedMs: number) => {
  const accuracy = totalChars + mistakes > 0 ? (totalChars / (totalChars + mistakes)) * 100 : 100;
  const avgTimePerChar = totalChars > 0 ? (elapsedMs / 1000) / totalChars : 0;
  const targetTime = 5; // giây
  const speedBonus = Math.min(100, Math.max(0, (1 - (avgTimePerChar - targetTime) / targetTime)) * 100);
  const rawScore = accuracy * 0.7 + speedBonus * 0.3;

  let stars = 1;
  if (rawScore >= 95) stars = 5;
  else if (rawScore >= 85) stars = 4;
  else if (rawScore >= 70) stars = 3;
  else if (rawScore >= 50) stars = 2;

  const starLabels = ['Tập lại nhé!', 'Cần cải thiện', 'Khá tốt!', 'Xuất sắc!', 'Hoàn hảo!'];

  return { accuracy, speedBonus, rawScore, stars, label: starLabels[stars - 1] };
};

export const IntroModePlaceholderPage = () => {
  const { script, mode } = useParams<{ script: string; mode: string }>();
  const [isPracticing, setIsPracticing] = useState(false);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [isFlippedAll, setIsFlippedAll] = useState(false);

  // Selection / Quiz states for Typing
  const [selectedGroups, setSelectedGroups] = useState<string[]>([]);
  const [quizStarted, setQuizStarted] = useState(false);
  const [quizItems, setQuizItems] = useState<TypingItem[]>([]);
  const [quizAnswers, setQuizAnswers] = useState<{
    typed: string;
    isCorrect: boolean;
    isWrong: boolean;
  }[]>([]);
  const [mistakeCount, setMistakeCount] = useState(0);

  // Fullscreen + Timer states
  const quizRootRef = useRef<HTMLDivElement>(null);
  const isExitingRef = useRef(false);
  const [startedAt, setStartedAt] = useState<number | null>(null);
  const [elapsedMs, setElapsedMs] = useState(0);
  const [completedElapsedMs, setCompletedElapsedMs] = useState<number | null>(null);

  // Anti-cheat: timestamps
  const [quizStartTime, setQuizStartTime] = useState<string | null>(null);
  const [quizEndTime, setQuizEndTime] = useState<string | null>(null);

  // doExit: single exit point — exit fullscreen + go back to config
  const doExit = useCallback(() => {
    if (isExitingRef.current) return;
    isExitingRef.current = true;
    if (document.fullscreenElement) {
      document.exitFullscreen?.().catch(() => undefined);
    }
    setQuizStarted(false);
    setStartedAt(null);
    setElapsedMs(0);
    setCompletedElapsedMs(null);
    setQuizStartTime(null);
    setQuizEndTime(null);
    setMistakeCount(0);
    isExitingRef.current = false;
  }, []);

  // Request fullscreen when quiz starts
  useEffect(() => {
    if (!quizStarted) return;
    const root = quizRootRef.current;
    if (!root) return;

    isExitingRef.current = false;
    root.requestFullscreen?.().catch(() => undefined);

    const handleFullscreenChange = () => {
      if (document.fullscreenElement !== root && !isExitingRef.current) {
        doExit();
      }
    };

    const handleVisibilityChange = () => {
      if (document.visibilityState === 'hidden') {
        doExit();
      }
    };

    document.addEventListener('fullscreenchange', handleFullscreenChange);
    document.addEventListener('visibilitychange', handleVisibilityChange);
    return () => {
      document.removeEventListener('fullscreenchange', handleFullscreenChange);
      document.removeEventListener('visibilitychange', handleVisibilityChange);
    };
  }, [quizStarted, doExit]);

  // Anti-cheat: block DevTools & right-click (only during quiz)
  useEffect(() => {
    if (!quizStarted) return;

    const blockKeys = (e: KeyboardEvent) => {
      if (e.key === 'F12') { e.preventDefault(); e.stopPropagation(); return; }
      if (e.ctrlKey && e.shiftKey && ['I', 'J', 'C'].includes(e.key.toUpperCase())) { e.preventDefault(); e.stopPropagation(); return; }
      if (e.ctrlKey && e.key.toUpperCase() === 'U') { e.preventDefault(); e.stopPropagation(); return; }
    };

    const blockContextMenu = (e: MouseEvent) => e.preventDefault();

    document.addEventListener('keydown', blockKeys, true);
    document.addEventListener('contextmenu', blockContextMenu, true);
    return () => {
      document.removeEventListener('keydown', blockKeys, true);
      document.removeEventListener('contextmenu', blockContextMenu, true);
    };
  }, [quizStarted]);

  // Timer tick
  useEffect(() => {
    if (!startedAt || completedElapsedMs !== null) return;
    const tick = () => setElapsedMs(Date.now() - startedAt);
    tick();
    const timer = window.setInterval(tick, 250);
    return () => window.clearInterval(timer);
  }, [startedAt, completedElapsedMs]);

  if (!isScriptKey(script) || !isModeKey(mode)) {
    return <Navigate to="/intro" replace />;
  }

  const scriptLabel = scriptLabels[script];
  const modeLabel = modeLabels[mode];
  const rows = getKanaRows(script);
  const allKanas = rows.flatMap(r => r.items);
  const currentKana = allKanas[currentIndex];

  const typingCategories = getTypingCategories(script);
  const allGroups = typingCategories.flatMap(c => c.groups);
  const allGroupIds = allGroups.map(g => g.id);

  // Selection Helpers
  const toggleGroup = (id: string) => {
    setSelectedGroups(prev =>
      prev.includes(id) ? prev.filter(gId => gId !== id) : [...prev, id]
    );
  };

  const isAllSelected = allGroupIds.length > 0 && allGroupIds.every(id => selectedGroups.includes(id));

  const toggleAllKana = () => {
    if (isAllSelected) {
      setSelectedGroups([]);
    } else {
      setSelectedGroups(allGroupIds);
    }
  };

  const toggleCategory = (category: TypingCategory) => {
    const categoryGroupIds = category.groups.map(g => g.id);
    const isCategoryAllSelected = categoryGroupIds.every(id => selectedGroups.includes(id));
    if (isCategoryAllSelected) {
      setSelectedGroups(prev => prev.filter(id => !categoryGroupIds.includes(id)));
    } else {
      setSelectedGroups(prev => {
        const otherSelected = prev.filter(id => !categoryGroupIds.includes(id));
        return [...otherSelected, ...categoryGroupIds];
      });
    }
  };

  const startQuiz = () => {
    if (selectedGroups.length === 0) return;
    const selectedItems = selectedGroups.flatMap(gId => {
      const g = allGroups.find(group => group.id === gId);
      return g ? g.items : [];
    });
    const shuffled = [...selectedItems].sort(() => Math.random() - 0.5);
    setQuizItems(shuffled);
    setQuizAnswers(shuffled.map(() => ({ typed: '', isCorrect: false, isWrong: false })));
    setQuizStarted(true);
    setStartedAt(null);
    setElapsedMs(0);
    setQuizStartTime(new Date().toLocaleTimeString('vi-VN'));
    setQuizEndTime(null);
    setCompletedElapsedMs(null);
    setMistakeCount(0);

    // Autofocus the very first input box
    setTimeout(() => {
      document.getElementById('typing-input-0')?.focus();
    }, 100);
  };

  const handleInputChange = (index: number, val: string) => {
    // Start timer on first keystroke
    if (!startedAt && val.trim()) {
      setStartedAt(Date.now());
    }
    setQuizAnswers(prev => {
      const next = [...prev];
      next[index] = { ...next[index], typed: val, isWrong: false };
      return next;
    });
  };

  const handleVerify = (index: number) => {
    const typedClean = quizAnswers[index].typed.trim().toLowerCase();
    const item = quizItems[index];
    const isCorrect = typedClean === item.romaji.toLowerCase() || 
                      (item.alternatives && item.alternatives.some(alt => alt.toLowerCase() === typedClean));
    
    setQuizAnswers(prev => {
      const next = [...prev];
      next[index] = {
        ...next[index],
        isCorrect,
        isWrong: !isCorrect
      };
      return next;
    });

    if (!isCorrect) {
      setMistakeCount(prev => prev + 1);
    }

    if (isCorrect) {
      // Check if this was the last card to complete the quiz
      const remainingIncorrect = quizAnswers.filter((ans, idx) => idx !== index && !ans.isCorrect).length;
      if (remainingIncorrect === 0 && startedAt) {
        const total = Date.now() - startedAt;
        setCompletedElapsedMs(total);
        setElapsedMs(total);
        setQuizEndTime(new Date().toLocaleTimeString('vi-VN'));
      }

      // Advance focus to the next unfinished card
      const nextIncomplete = quizAnswers.findIndex((ans, idx) => idx > index && !ans.isCorrect);
      const targetIndex = nextIncomplete !== -1 ? nextIncomplete : quizAnswers.findIndex((ans) => !ans.isCorrect);
      
      if (targetIndex !== -1 && targetIndex !== index) {
        setTimeout(() => {
          document.getElementById(`typing-input-${targetIndex}`)?.focus();
        }, 50);
      }
    }
  };

  const correctCount = quizAnswers.filter(ans => ans.isCorrect).length;
  const isQuizFinished = quizItems.length > 0 && correctCount === quizItems.length;

  // Render practice slider if active
  if (isPracticing && currentKana) {
    return (
      <div className="flex min-h-[calc(100vh-3rem)] flex-col bg-slate-50 px-4 py-6 md:px-12 animate-fade-in">
        {/* Header */}
        <div className="mb-8 flex items-center justify-between">
          <button
            onClick={() => setIsPracticing(false)}
            className="flex items-center gap-2 font-black text-slate-500 transition-colors hover:text-slate-900"
          >
            <ArrowLeft size={20} />
            Đóng
          </button>
          <div className="text-sm font-black tracking-widest text-slate-400">
            {currentIndex + 1} / {allKanas.length}
          </div>
        </div>

        {/* Content */}
        <div className="mx-auto flex w-full max-w-4xl flex-1 flex-col items-center justify-center gap-6 md:flex-row md:items-center md:gap-10">
          {/* Left Column: Box 1 */}
          <div className="flex w-full max-w-[260px] shrink-0 flex-col gap-4 md:max-w-[310px]">
            {/* Box 1: Character */}
            <div className="relative flex aspect-square w-full flex-col items-center justify-center rounded-[24px] border-2 border-slate-900 bg-white shadow-[5px_5px_0_#111827]">
              <button
                onClick={() => playPronunciation(currentKana.kana)}
                className="absolute top-3 right-3 flex h-8 w-8 items-center justify-center rounded-lg border-2 border-slate-900 bg-white text-slate-900 shadow-[2px_2px_0_#111827] transition-all hover:bg-sky-100 hover:text-blue-700 active:translate-y-0 active:shadow-none"
                title={`Nghe phát âm chữ ${currentKana.kana}`}
              >
                <Volume2 size={15} className="stroke-[2.5]" />
              </button>
              <span className="font-jp text-[110px] font-bold leading-none text-slate-950 md:text-[145px]">
                {currentKana.kana}
              </span>
              <span className="mt-3 rounded-lg border-2 border-slate-900 bg-[#F4F4F5] px-4 py-1 text-lg font-black uppercase tracking-widest text-slate-900">
                {currentKana.romaji}
              </span>
            </div>
          </div>

          {/* Arrow */}
          <div className="flex items-center justify-center text-slate-800 md:px-4 py-2">
            <ArrowRight size={40} className="hidden stroke-[2.5] md:block animate-pulse" />
            <ArrowDown size={32} className="block stroke-[2.5] md:hidden animate-pulse" />
          </div>

          {/* Right Column: Box 2 Image + Note */}
          <div className="flex w-full max-w-[240px] shrink-0 flex-col gap-4 md:max-w-[280px]">
            <div className="flex aspect-square w-full shrink-0 items-center justify-center overflow-hidden rounded-[24px] border-2 border-slate-900 bg-white p-4 shadow-[5px_5px_0_#111827]">
              <img
                src={`/materials/${script}/${currentKana.romaji.toLowerCase()}.png`}
                alt={`Mnemonic for ${currentKana.kana}`}
                className="h-full w-full object-contain"
                onError={(e) => {
                  e.currentTarget.style.display = 'none';
                  e.currentTarget.parentElement!.innerHTML = '<span class="text-slate-400 font-bold text-xs">Chưa có ảnh minh họa</span>';
                }}
              />
            </div>

            {/* Note badge */}
            <div className="w-full whitespace-pre-wrap rounded-[20px] border-2 border-slate-900 bg-[#C8FF00] p-4 text-center text-xs font-black leading-relaxed text-slate-950 shadow-[4px_4px_0_#111827] md:text-sm">
              {currentKana.mnemonic}
            </div>
          </div>
        </div>

        {/* Navigation (Placed underneath cards) */}
        <div className="mt-6 flex justify-center gap-4 pb-4">
          <button
            onClick={() => setCurrentIndex(prev => Math.max(0, prev - 1))}
            disabled={currentIndex === 0}
            className="rounded-xl border-2 border-slate-900 bg-white p-3 text-slate-900 shadow-[3px_3px_0_#111827] transition-all hover:-translate-y-0.5 hover:shadow-[4px_4px_0_#111827] active:translate-y-0 active:shadow-[0_0_0_#111827] disabled:opacity-50 disabled:hover:translate-y-0 disabled:hover:shadow-[3px_3px_0_#111827]"
          >
            <ArrowLeft size={24} />
          </button>
          <button
            onClick={() => setCurrentIndex(prev => Math.min(allKanas.length - 1, prev + 1))}
            disabled={currentIndex === allKanas.length - 1}
            className="rounded-xl border-2 border-slate-900 bg-[#FF3366] p-3 text-white shadow-[3px_3px_0_#111827] transition-all hover:-translate-y-0.5 hover:shadow-[4px_4px_0_#111827] active:translate-y-0 active:shadow-[0_0_0_#111827] disabled:opacity-50 disabled:hover:translate-y-0 disabled:hover:shadow-[3px_3px_0_#111827]"
          >
            <ArrowRight size={24} />
          </button>
        </div>
      </div>
    );
  }

  // Render typing quiz if active (fullscreen)
  if (mode === 'typing' && quizStarted) {
    const displayTime = formatDuration(completedElapsedMs ?? elapsedMs);

    if (isQuizFinished) {
      const { accuracy, rawScore, stars, label } = calculateScore(
        quizItems.length,
        mistakeCount,
        completedElapsedMs ?? elapsedMs
      );

      return (
        <section
          ref={quizRootRef}
          className="fixed inset-0 z-50 flex min-h-screen flex-col overflow-auto bg-slate-950 px-4 py-5 text-white md:px-8 animate-fade-in"
        >
          <div className="mx-auto flex w-full max-w-2xl flex-1 flex-col items-center justify-center py-8">
            {/* Animated Stars */}
            <div className="mb-4 flex items-center justify-center gap-2">
              {[1, 2, 3, 4, 5].map((starIndex) => {
                const isFilled = starIndex <= stars;
                return (
                  <span
                    key={starIndex}
                    style={{ animationDelay: `${starIndex * 150}ms` }}
                    className={`text-4xl md:text-5xl animate-bounce transition-all ${
                      isFilled ? 'text-yellow-400 drop-shadow-[0_0_8px_rgba(250,204,21,0.6)] font-black' : 'text-slate-700 font-light'
                    }`}
                  >
                    ★
                  </span>
                );
              })}
            </div>

            <h2 className="text-4xl font-black text-center">{label} 🎉</h2>
            <p className="mt-3 text-sm font-bold text-slate-400 text-center">
              Bạn đã hoàn thành <span className="font-extrabold text-cyan-300">{quizItems.length}</span> âm {scriptLabel.title}!
            </p>

            {/* High fidelity stats grid */}
            <div className="mt-8 grid grid-cols-2 gap-4 w-full max-w-md">
              <div className="flex flex-col items-center rounded-2xl border border-white/10 bg-slate-900/60 p-4 text-center">
                <span className="text-[10px] font-black uppercase tracking-[0.2em] text-slate-400">Độ chính xác</span>
                <span className="mt-1 text-2xl font-black text-[#C8FF00]">{accuracy.toFixed(1)}%</span>
              </div>
              <div className="flex flex-col items-center rounded-2xl border border-white/10 bg-slate-900/60 p-4 text-center">
                <span className="text-[10px] font-black uppercase tracking-[0.2em] text-slate-400">Số lần gõ sai</span>
                <span className="mt-1 text-2xl font-black text-rose-500">{mistakeCount}</span>
              </div>
              <div className="flex flex-col items-center rounded-2xl border border-white/10 bg-slate-900/60 p-4 text-center">
                <span className="text-[10px] font-black uppercase tracking-[0.2em] text-slate-400">Tổng thời gian</span>
                <span className="mt-1 font-mono text-2xl font-black text-cyan-300">{displayTime}</span>
              </div>
              <div className="flex flex-col items-center rounded-2xl border border-white/10 bg-slate-900/60 p-4 text-center">
                <span className="text-[10px] font-black uppercase tracking-[0.2em] text-slate-400">Điểm số</span>
                <span className="mt-1 text-2xl font-black text-emerald-400">{rawScore.toFixed(1)}</span>
              </div>
            </div>

            {/* Timestamps */}
            <div className="mt-6 flex gap-6 text-xs font-bold text-slate-500">
              {quizStartTime && (
                <span>🕐 Bắt đầu: <span className="text-slate-300">{quizStartTime}</span></span>
              )}
              {quizEndTime && (
                <span>🏁 Hoàn thành: <span className="text-emerald-400">{quizEndTime}</span></span>
              )}
            </div>

            <div className="mt-8 flex gap-3">
              <button
                onClick={startQuiz}
                className="rounded-2xl border-2 border-emerald-400 bg-emerald-600 px-6 py-3.5 text-sm font-black uppercase tracking-wider text-white shadow-[3px_3px_0_rgba(16,185,129,0.3)] transition-all hover:bg-emerald-500"
              >
                Gõ Lại
              </button>
              <button
                onClick={doExit}
                className="rounded-2xl border-2 border-cyan-300 bg-cyan-300 px-6 py-3.5 text-sm font-black uppercase tracking-wider text-slate-950 transition-all hover:-translate-y-0.5"
              >
                Quay lại chọn bảng
              </button>
            </div>
          </div>
        </section>
      );
    }

    return (
      <section
        ref={quizRootRef}
        className="fixed inset-0 z-50 flex min-h-screen flex-col overflow-auto bg-slate-950 px-4 py-5 text-white md:px-8 animate-fade-in"
      >
        {/* Header */}
        <div className="mb-5 flex flex-col justify-between gap-4 border-b border-white/10 pb-4 sm:flex-row sm:items-center">
          <div>
            <p className="text-xs font-black uppercase tracking-[0.22em] text-cyan-300">Luyện gõ {scriptLabel.title}</p>
            <h2 className="mt-1 text-2xl font-black">
              {correctCount} / {quizItems.length}
            </h2>
          </div>

          <div className="flex items-center gap-6">
            <div className="text-center">
              <p className="text-[10px] font-black uppercase tracking-[0.22em] text-slate-500">Sai</p>
              <p className="mt-1 text-xl font-black text-rose-500">
                {mistakeCount}
              </p>
            </div>
            <div className="text-center">
              <p className="text-[10px] font-black uppercase tracking-[0.22em] text-slate-500">Thời gian</p>
              <p className="mt-1 font-mono text-xl font-black text-white">
                {startedAt ? displayTime : '00:00'}
              </p>
            </div>
            <button
              onClick={doExit}
              className="inline-flex h-10 items-center gap-2 rounded-xl border-2 border-white/20 bg-white/10 px-3 text-sm font-black text-white transition-all hover:bg-white/15"
            >
              <X size={16} />
              Thoát
            </button>
          </div>
        </div>

        {/* Progress Bar */}
        <div className="mb-5 h-3 w-full overflow-hidden rounded-full bg-white/10">
          <div
            className="h-full rounded-full bg-emerald-400 transition-all duration-300"
            style={{ width: `${(correctCount / quizItems.length) * 100}%` }}
          />
        </div>

        {/* Grid cards */}
        <div className="grid grid-cols-3 gap-3 sm:grid-cols-4 md:grid-cols-6 lg:grid-cols-8 xl:grid-cols-10 pb-12">
          {quizItems.map((item, index) => {
            const answer = quizAnswers[index];
            const isCorrect = answer.isCorrect;
            const isWrong = answer.isWrong;
            const isActive = index === quizAnswers.findIndex(ans => !ans.isCorrect);

            let cardBgClass = 'bg-[#29b6f6]';
            if (isCorrect) cardBgClass = 'bg-[#00c853]';
            else if (isWrong) cardBgClass = 'bg-[#d50000] animate-[shake_0.4s_ease-in-out]';
            else if (isActive) cardBgClass = 'bg-[#0d47a1] ring-4 ring-sky-300 ring-offset-2 scale-[1.03]';

            return (
              <div
                key={index}
                className={`group relative flex flex-col items-center justify-between rounded-2xl border-2 border-slate-700 p-3 shadow-[3px_3px_0_rgba(255,255,255,0.08)] transition-all text-white ${cardBgClass}`}
              >
                {isWrong && (
                  <div className="absolute -top-12 left-1/2 -translate-x-1/2 scale-0 group-hover:scale-100 transition-all z-20 whitespace-nowrap rounded-lg border-2 border-slate-700 bg-amber-200 px-3 py-1.5 text-xs font-black text-slate-900 shadow-[3px_3px_0_rgba(255,255,255,0.1)]">
                    Sai: "{answer.typed || 'trống'}"
                  </div>
                )}

                <span className="font-jp text-4xl font-bold leading-none py-2 select-none">
                  {item.kana}
                </span>

                <div className="mt-2 w-full">
                  {isCorrect ? (
                    <div className="flex h-8 w-full items-center justify-center rounded-lg border-2 border-white/20 bg-white/20 text-xs font-extrabold uppercase tracking-widest text-white/90">
                      {item.romaji}
                    </div>
                  ) : (
                    <input
                      id={`typing-input-${index}`}
                      type="text"
                      value={answer.typed}
                      onChange={(e) => handleInputChange(index, e.target.value)}
                      onKeyDown={(e) => {
                        if (e.key === 'Enter') {
                          handleVerify(index);
                        }
                      }}
                      autoComplete="off"
                      autoCorrect="off"
                      autoCapitalize="none"
                      spellCheck="false"
                      placeholder=""
                      className="h-8 w-full rounded-lg border-2 border-slate-600 bg-white text-center text-xs font-black uppercase text-slate-950 focus:outline-none focus:ring-2 focus:ring-cyan-400"
                    />
                  )}
                </div>
              </div>
            );
          })}
        </div>
      </section>
    );
  }

  return (
    <div className="mx-auto max-w-5xl space-y-6 px-4 py-3 animate-fade-in">
      <Link
        to={`/intro/${script}`}
        className="inline-flex items-center gap-2 text-sm font-black text-blue-700 transition-colors hover:text-blue-900"
      >
        <ArrowLeft size={17} />
        Quay lại {scriptLabel.title}
      </Link>

      <section className="relative overflow-hidden rounded-[24px] border-2 border-slate-900 bg-white shadow-[6px_6px_0_#111827]">
        {/* Decorative corner accent */}
        <div className="absolute -right-6 -top-6 h-16 w-16 rotate-45 border-b-2 border-l-2 border-slate-900 bg-[#C8FF00]" />

        <div className="border-b-2 border-slate-900 bg-[#FF3366] px-5 py-2.5">
          <span className="text-[10px] font-black uppercase tracking-[0.25em] text-white">
            Nhập Môn / {scriptLabel.title}
          </span>
        </div>

        <div className="flex flex-col gap-5 p-5 md:flex-row md:items-center">
          <div className="flex h-28 w-28 shrink-0 items-center justify-center rounded-[16px] border-2 border-slate-900 bg-[#F4F4F5] font-jp text-5xl font-black text-slate-950 shadow-[3px_3px_0_#111827]">
            {scriptLabel.jp}
          </div>

          <div className="flex-1">
            <div className="flex flex-col gap-4 sm:flex-row sm:items-center">
              <div className="flex items-center gap-3">
                <div className="flex h-12 w-12 shrink-0 items-center justify-center rounded-xl border-2 border-slate-900 bg-[#C8FF00] text-slate-950 shadow-[2px_2px_0_#111827]">
                  {modeLabel.icon}
                </div>
                <h1 className="text-3xl font-black tracking-tight text-slate-950">{modeLabel.title}</h1>
              </div>

              {mode === 'mnemonic' && (
                <div className="sm:ml-auto flex flex-wrap gap-2">
                  <button
                    onClick={() => setIsPracticing(true)}
                    className="group flex w-fit items-center gap-2 rounded-xl border-2 border-slate-900 bg-[#C8FF00] px-6 py-2.5 text-sm font-black uppercase tracking-wider text-slate-950 shadow-[3px_3px_0_#111827] transition-all hover:-translate-y-1 hover:-translate-x-1 hover:shadow-[6px_6px_0_#FF3366] active:translate-y-0 active:translate-x-0 active:shadow-[0_0_0_#111827]"
                  >
                    <Play size={16} className="transition-transform group-hover:scale-110" />
                    Học mẹo
                  </button>
                  <button
                    onClick={() => setIsFlippedAll(prev => !prev)}
                    className={`group flex w-fit items-center gap-2 rounded-xl border-2 border-slate-900 px-6 py-2.5 text-sm font-black uppercase tracking-wider shadow-[3px_3px_0_#111827] transition-all hover:-translate-y-1 hover:-translate-x-1 active:translate-y-0 active:translate-x-0 active:shadow-[0_0_0_#111827] ${
                      isFlippedAll
                        ? 'bg-slate-950 text-white hover:shadow-[6px_6px_0_#C8FF00]'
                        : 'bg-white text-slate-950 hover:shadow-[6px_6px_0_#C8FF00]'
                    }`}
                  >
                    <RefreshCw size={16} className={`transition-transform duration-500 ${isFlippedAll ? 'rotate-180' : ''}`} />
                    Lật mặt sau
                  </button>
                </div>
              )}
            </div>
            <p className="mt-2 max-w-xl text-sm font-bold leading-6 text-slate-600">{modeLabel.description}</p>
          </div>
        </div>
      </section>

      {mode === 'mnemonic' ? (
        <section className="space-y-6">
          {rows.map((row) => (
            <div key={row.label} className="rounded-[24px] border-2 border-slate-900 bg-white p-5 shadow-[4px_4px_0_#111827]">
              <div className="mb-5 flex items-center justify-between gap-3 border-b-2 border-slate-900/10 pb-4">
                <h2 className="text-xl font-black text-slate-950">{row.label}</h2>
                <span className="rounded-full border-2 border-slate-900 bg-[#C8FF00] px-3 py-1 text-[11px] font-black uppercase tracking-[0.2em] text-slate-950 shadow-[2px_2px_0_#111827]">
                  {row.items.length} chữ
                </span>
              </div>
              <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5">
                {row.items.map((item) => {
                  const index = allKanas.findIndex(k => k.romaji === item.romaji);
                  return (
                    <div key={item.romaji} className="flex flex-col gap-2">
                      <article
                        onClick={() => {
                          if (index !== -1) {
                            setCurrentIndex(index);
                            setIsPracticing(true);
                          }
                        }}
                        className={`group relative flex aspect-square cursor-pointer flex-col items-center justify-center gap-3 overflow-hidden rounded-[20px] border-2 border-slate-900 transition-all hover:-translate-y-1.5 hover:-translate-x-1.5 active:translate-y-0 active:translate-x-0 active:shadow-[0_0_0_#111827] ${
                          isFlippedAll
                            ? 'bg-white shadow-[3px_3px_0_#111827] hover:shadow-[7px_7px_0_#C8FF00]'
                            : 'bg-[#F4F4F5] shadow-[3px_3px_0_#111827] hover:bg-white hover:shadow-[7px_7px_0_#FF3366]'
                        }`}
                      >
                        <button
                          onClick={(e) => {
                            e.stopPropagation();
                            playPronunciation(item.kana);
                          }}
                          className="absolute top-2 right-2 z-10 flex h-7 w-7 items-center justify-center rounded-lg border-2 border-slate-900 bg-white text-slate-900 shadow-[2px_2px_0_#111827] transition-all hover:bg-sky-100 hover:text-blue-700 active:translate-y-0 active:shadow-none"
                          title={`Nghe phát âm chữ ${item.kana}`}
                        >
                          <Volume2 size={13} className="stroke-[2.5]" />
                        </button>

                        {isFlippedAll ? (
                          <div className="flex h-full w-full items-center justify-center p-3 animate-fade-in">
                            <img
                              src={`/materials/${script}/${item.romaji.toLowerCase()}.png`}
                              alt={`Mnemonic for ${item.kana}`}
                              className="h-full w-full object-contain"
                              onError={(e) => {
                                e.currentTarget.style.display = 'none';
                                e.currentTarget.parentElement!.innerHTML = `<span class="font-jp text-5xl font-black text-slate-900 transition-transform duration-300 group-hover:scale-110 group-hover:text-[#FF3366]">${item.kana}</span>`;
                              }}
                            />
                          </div>
                        ) : (
                          <>
                            <span className="font-jp text-7xl font-black text-slate-900 transition-transform duration-300 group-hover:scale-110 group-hover:text-[#FF3366]">
                              {item.kana}
                            </span>
                            <span className="rounded-lg border-2 border-slate-900 bg-white px-3 py-1.5 text-sm font-black uppercase tracking-widest text-slate-900 shadow-[2px_2px_0_#111827] transition-colors group-hover:bg-[#FF3366] group-hover:text-white">
                              {item.romaji}
                            </span>
                          </>
                        )}
                      </article>

                      {isFlippedAll && (
                        <div className="flex min-h-[44px] items-center justify-center rounded-xl border-2 border-slate-900 bg-[#C8FF00] px-2 py-1 text-center text-[10px] font-black leading-tight text-slate-950 shadow-[2px_2px_0_#111827] animate-fade-in">
                          {item.mnemonic}
                        </div>
                      )}
                    </div>
                  );
                })}
              </div>
            </div>
          ))}
        </section>
      ) : null}

      {mode === 'typing' ? (
        <section className="space-y-6 animate-fade-in">
          {/* All Kana full-width selector */}
          <button
            onClick={toggleAllKana}
            className={`w-full py-4 rounded-2xl border-4 text-center text-lg font-black uppercase tracking-wider transition-all shadow-[4px_4px_0_#111827] active:translate-y-0 active:shadow-[0_0_0_#111827] ${
              isAllSelected
                ? 'bg-sky-500 text-white border-emerald-500 ring-2 ring-emerald-400 ring-offset-2'
                : 'bg-white text-slate-800 border-slate-900 hover:bg-slate-50'
            }`}
          >
            All Kana
          </button>

          {/* Dynamic category grid */}
          <div className={`grid grid-cols-1 gap-6 ${typingCategories.length === 4 ? 'xl:grid-cols-4 md:grid-cols-2' : 'md:grid-cols-3'}`}>
            {typingCategories.map((category) => {
              const categoryGroupIds = category.groups.map(g => g.id);
              const isCategoryAllSelected = categoryGroupIds.every(id => selectedGroups.includes(id));

              return (
                <div
                  key={category.title}
                  className="rounded-[28px] border-2 border-slate-900 bg-white p-5 shadow-[5px_5px_0_#111827] flex flex-col"
                >
                  <h3 className="text-center text-xl font-black text-sky-600 mb-4 uppercase tracking-wider">
                    {category.title}
                  </h3>

                  <button
                    onClick={() => toggleCategory(category)}
                    className={`w-full py-2.5 rounded-xl border-2 text-center text-sm font-black uppercase tracking-wide transition-all shadow-[3px_3px_0_#111827] active:translate-y-0 active:shadow-[0_0_0_#111827] mb-4 ${
                      isCategoryAllSelected
                        ? 'bg-sky-500 text-white border-slate-900 shadow-[3px_3px_0_#111827]'
                        : 'bg-white text-slate-800 border-slate-900 hover:bg-slate-50'
                    }`}
                  >
                    {category.allLabel}
                  </button>

                  <div className={
                    category.title === 'Dakuten Kana'
                      ? 'flex flex-col gap-2 flex-1'
                      : 'grid grid-cols-2 gap-2 flex-1'
                  }>
                    {category.groups.map((group) => {
                      const isSelected = selectedGroups.includes(group.id);

                      return (
                        <button
                          key={group.id}
                          onClick={() => toggleGroup(group.id)}
                          className={`py-2 rounded-lg border-2 text-center text-sm font-bold tracking-wide transition-all shadow-[2px_2px_0_#111827] active:translate-y-0 active:shadow-[0_0_0_#111827] ${
                            isSelected
                              ? 'bg-sky-500 text-white border-slate-900 font-extrabold shadow-[2px_2px_0_#111827]'
                              : 'bg-white text-slate-800 border-slate-900 hover:bg-slate-50'
                          }`}
                        >
                          {group.label}
                        </button>
                      );
                    })}
                  </div>
                </div>
              );
            })}
          </div>

          {/* Start Quiz CTA */}
          <div className="flex justify-center pt-4">
            <button
              onClick={startQuiz}
              disabled={selectedGroups.length === 0}
              className="rounded-2xl border-4 border-slate-900 bg-[#C8FF00] px-12 py-4 text-lg font-black uppercase tracking-widest text-slate-950 shadow-[5px_5px_0_#111827] transition-all hover:-translate-y-1 hover:shadow-[7px_7px_0_#FF3366] active:translate-y-0 active:shadow-[0_0_0_#111827] disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:translate-y-0 disabled:hover:shadow-[5px_5px_0_#111827]"
            >
              Start Quiz!
            </button>
          </div>
        </section>
      ) : null}
    </div>
  );
};

const formatDuration = (durationMs: number) => {
  const totalSeconds = Math.max(0, Math.floor(durationMs / 1000));
  const minutes = Math.floor(totalSeconds / 60);
  const seconds = totalSeconds % 60;
  return `${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`;
};
