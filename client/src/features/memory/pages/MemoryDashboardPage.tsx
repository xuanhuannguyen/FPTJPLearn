import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { ListChecks, Loader2, RotateCcw, Sparkles } from 'lucide-react';
import { memoryApi } from '../api/memoryApi';
import { MemoryStatsBar } from '../components/MemoryStatsBar';
import { ConfirmModal } from '../../../shared/components/ConfirmModal';
import { getMemoryTypeConfig, memoryTypeConfigs, memoryTypeOrder } from '../memory.config';
import type { MemoryItemType, MemorySummary } from '../types/memory.types';

const emptySummary = {
  due: 0,
  new: 0,
  learning: 0,
  shortTerm: 0,
  longTerm: 0,
  totalStudied: 0,
  nextReviewAt: null,
};

const formatCountdown = (target?: string | null) => {
  if (!target) {
    return '--:--';
  }

  const remainingMs = new Date(target).getTime() - Date.now();
  if (remainingMs <= 0) {
    return 'Ôn ngay';
  }

  const totalSeconds = Math.ceil(remainingMs / 1000);
  const days = Math.floor(totalSeconds / 86400);
  const hours = Math.floor((totalSeconds % 86400) / 3600);
  const minutes = Math.floor((totalSeconds % 3600) / 60);
  const seconds = totalSeconds % 60;

  if (days > 0) {
    return `${days} ngày ${hours} giờ`;
  }

  if (hours > 0) {
    return `${hours} giờ ${minutes} phút`;
  }

  if (minutes > 0) {
    return `${minutes} phút ${seconds.toString().padStart(2, '0')} giây`;
  }

  return `${seconds} giây`;
};

export const MemoryDashboardPage = () => {
  const navigate = useNavigate();
  const [summary, setSummary] = useState<MemorySummary | null>(null);
  const [activeType, setActiveType] = useState<MemoryItemType>('grammar');
  const [isLoading, setIsLoading] = useState(true);
  const [isResetModalOpen, setIsResetModalOpen] = useState(false);
  const [isResettingGrammar, setIsResettingGrammar] = useState(false);
  const [error, setError] = useState('');
  const [resetError, setResetError] = useState('');
  const [nowTick, setNowTick] = useState(() => Date.now());
  const activeConfig = getMemoryTypeConfig(activeType);

  const loadSummary = async () => {
    const data = await memoryApi.getSummary();
    setSummary(data);
    const dueType = memoryTypeOrder.find((type) => data[type].due > 0);
    if (dueType) setActiveType(dueType);
  };

  useEffect(() => {
    let cancelled = false;

    const load = async () => {
      try {
        setIsLoading(true);
        setError('');
        const data = await memoryApi.getSummary();
        if (!cancelled) {
          setSummary(data);
          const dueType = memoryTypeOrder.find((type) => data[type].due > 0);
          if (dueType) setActiveType(dueType);
        }
      } catch (err) {
        console.error(err);
        if (!cancelled) {
          setError('Không tải được thống kê ghi nhớ.');
        }
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    };

    void load();

    return () => {
      cancelled = true;
    };
  }, []);

  useEffect(() => {
    const timer = window.setInterval(() => {
      setNowTick(Date.now());
    }, 1000);

    return () => window.clearInterval(timer);
  }, []);

  const activeSummary = useMemo(() => {
    if (!summary) return emptySummary;
    return summary[activeType] ?? emptySummary;
  }, [summary, activeType]);

  const nextReviewText = activeSummary.due > 0 ? 'Ôn ngay' : formatCountdown(activeSummary.nextReviewAt);
  const nextReviewTimeText = activeSummary.nextReviewAt
    ? new Date(activeSummary.nextReviewAt).toLocaleString('vi-VN')
    : '';
  void nowTick;

  const resetGrammarProgress = async () => {
    try {
      setIsResettingGrammar(true);
      setResetError('');
      await memoryApi.resetProgress(activeType);
      await loadSummary();
      setIsResetModalOpen(false);
    } catch (err) {
      console.error(err);
      setResetError('Không thể reset tiến độ ngữ pháp. Hãy thử lại.');
      throw err;
    } finally {
      setIsResettingGrammar(false);
    }
  };

  if (isLoading) {
    return (
      <div className="flex h-64 flex-col items-center justify-center text-text-secondary">
        <Loader2 size={36} className="mb-4 animate-spin text-accent-primary" />
        <p className="font-bold">Đang tải thống kê ghi nhớ...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="rounded-2xl border-2 border-accent-danger bg-accent-danger/10 p-5 font-bold text-accent-danger">
        {error}
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-6xl space-y-6 px-4 pb-20 pt-4 animate-fade-in">
      <header className="mb-4">
        <h1 className="text-3xl font-black tracking-normal text-text-primary">Thống kê học tập</h1>
        <p className="mt-1 max-w-2xl text-sm font-bold text-text-muted leading-relaxed">
          Theo dõi tiến độ và kế hoạch ôn tập của bạn, theo các báo cáo 21 ngày ôn tập kiến thức sẽ được nạp vào trí nhớ dài hạn
        </p>
      </header>

      {/* Marketing Hook */}
      <div className="mb-6 max-w-3xl overflow-hidden rounded-2xl border border-teal-200/70 bg-gradient-to-br from-teal-50/70 via-white to-emerald-50/40 p-5 shadow-sm">
        <div className="flex items-start gap-4">
          <div className="flex h-12 w-12 shrink-0 items-center justify-center rounded-2xl bg-gradient-to-br from-teal-600 to-emerald-600 text-white shadow-md shadow-teal-600/20">
            <Sparkles size={24} />
          </div>
          <div>
            <h3 className="font-heading text-lg font-black text-slate-900 leading-tight">
              Cơ chế học tập thông minh (Spaced Repetition)
            </h3>
            <p className="mt-1 text-sm font-bold leading-relaxed text-slate-600">
              Khi học Kanji, Từ vựng, Ngữ pháp các bạn có thể <span className="text-teal-700 font-extrabold bg-teal-50/90 border border-teal-200/80 px-2 py-0.5 rounded-md">Add to Memory</span>. Hệ thống sẽ tự động tính toán thời gian bạn sẽ quên theo các mức độ để nhắc nhở bạn luyện tập đúng lúc, giúp ghi nhớ vĩnh viễn.
            </p>
          </div>
        </div>
      </div>

      {/* Tabs - Centered */}
      <section className="flex gap-2 rounded-2xl border border-slate-200/90 bg-slate-100/80 p-1.5 shadow-sm max-w-2xl mx-auto backdrop-blur-sm">
        {memoryTypeOrder.map((type) => {
          const config = memoryTypeConfigs[type];
          const Icon = config.icon;
          const due = summary?.[type]?.due ?? 0;
          const isActive = type === activeType;
          return (
            <button
              key={type}
              type="button"
              onClick={() => setActiveType(type)}
              className={`flex flex-1 min-h-[44px] items-center justify-center gap-2 rounded-xl px-4 text-xs font-black transition-all duration-200 ${
                isActive
                  ? config.tabActiveClassName
                  : 'text-slate-600 hover:text-slate-900 hover:bg-white/80'
              }`}
            >
              <Icon size={16} strokeWidth={isActive ? 2.5 : 2} />
              <span>{config.label}</span>
              {due > 0 ? (
                <span className={`ml-1 rounded-full px-2 py-0.5 text-[10px] font-black ${
                  isActive ? 'bg-white/25 text-white' : 'bg-orange-100 text-orange-600 border border-orange-200'
                }`}>
                  {due}
                </span>
              ) : null}
            </button>
          );
        })}
      </section>

      {/* Stats Bar */}
      <MemoryStatsBar summary={activeSummary} />

      {/* Summary Panels - Reduced size by 20% and centered */}
      <div className="max-w-5xl mx-auto">
        <section className="grid gap-4 lg:grid-cols-2">
          <div className={`relative rounded-[28px] border-2 border-white/40 ${activeConfig.accentClassName} p-6 text-white shadow-xl overflow-hidden flex flex-col justify-between`}>
            {/* Subtle decorative glow watermark */}
            <div className="absolute -top-16 -right-16 w-48 h-48 rounded-full bg-white/10 blur-2xl pointer-events-none"></div>

            <div>
              {/* Header row */}
              <div className="flex items-center justify-between mb-4">
                 <p className="text-[11px] font-black uppercase tracking-wider opacity-90">{activeConfig.summaryLabel} cần ôn hôm nay</p>
                 <div className="flex flex-wrap justify-end gap-2">
                   <button
                     type="button"
                     onClick={() => navigate(activeConfig.listPath)}
                     className="inline-flex items-center gap-1.5 rounded-full border border-white/30 bg-white/20 px-3 py-1 text-[11px] font-black text-white backdrop-blur-md transition-all hover:bg-white/30"
                   >
                     <ListChecks size={12} strokeWidth={2.5} />
                     Xem danh sách
                   </button>
                   {activeConfig.canResetProgress ? (
                      <button
                        type="button"
                        disabled={isResettingGrammar}
                        onClick={() => {
                          setResetError('');
                          setIsResetModalOpen(true);
                        }}
                        className="inline-flex items-center gap-1.5 rounded-full bg-white/20 backdrop-blur-md px-3 py-1 text-[11px] font-black text-white border border-white/30 transition-all hover:bg-white/30 disabled:pointer-events-none disabled:opacity-60"
                      >
                        {isResettingGrammar ? <Loader2 size={12} className="animate-spin" /> : <RotateCcw size={12} strokeWidth={2.5} />}
                        {isResettingGrammar ? 'Đang reset' : 'Học lại từ đầu'}
                      </button>
                   ) : null}
                 </div>
              </div>

              <p className="text-6xl font-black drop-shadow-md tracking-tight">{activeSummary.due}</p>
            </div>

            <div className="mt-6">
              {activeSummary.due > 0 ? (
                <button
                  type="button"
                  onClick={() => navigate(activeConfig.reviewPath)}
                  className={`rounded-xl bg-white px-7 py-2.5 text-sm font-black ${activeConfig.btnTextClassName} shadow-lg shadow-black/10 transition-all hover:scale-105 active:scale-95`}
                >
                  Ôn tập ngay
                </button>
              ) : (
                <p className="max-w-md rounded-xl bg-white/15 backdrop-blur-sm border border-white/20 p-3.5 text-xs font-bold text-white/95">{activeConfig.emptyText}</p>
              )}
            </div>
          </div>

          <div className="relative rounded-[28px] border-2 border-slate-800/80 bg-gradient-to-br from-slate-900 via-slate-850 to-slate-900 p-6 text-white shadow-xl overflow-hidden flex flex-col justify-between">
            {/* Ambient glowing radial highlights */}
            <div className="absolute -top-16 -right-16 w-48 h-48 rounded-full bg-amber-500/10 blur-2xl pointer-events-none"></div>
            <div className="absolute -bottom-16 -left-16 w-48 h-48 rounded-full bg-teal-500/10 blur-2xl pointer-events-none"></div>

            <div>
              <div className="flex items-center justify-between mb-4">
                <div className="flex items-center gap-2">
                  <div className="p-1.5 rounded-lg bg-amber-400/20 text-amber-300 border border-amber-400/30">
                    <Sparkles size={14} className="text-amber-300" />
                  </div>
                  <p className="text-[11px] font-black uppercase tracking-wider text-slate-300">Lượt ôn tập tiếp theo</p>
                </div>
                <span className="inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full bg-slate-800 border border-slate-700 text-[10px] font-bold text-amber-300">
                  <span className="w-1.5 h-1.5 rounded-full bg-amber-400 animate-pulse"></span>
                  Hẹn giờ SRS
                </span>
              </div>
              <p className="text-4xl font-black tracking-tight mt-5 text-white drop-shadow-sm flex items-center gap-2">
                {nextReviewText}
              </p>
            </div>

            <div className="mt-6">
              {nextReviewTimeText && activeSummary.due === 0 ? (
                <p className="inline-flex items-center gap-1.5 rounded-xl bg-white/10 backdrop-blur-md border border-white/10 px-3 py-1.5 text-xs font-bold text-slate-200">
                  <span className="opacity-70">Lịch dự kiến:</span> {nextReviewTimeText}
                </p>
              ) : (
                <p className="text-xs text-slate-400 font-medium">
                  Hệ thống tự động đồng bộ theo thuật toán giãn cách thời gian (Spaced Repetition).
                </p>
              )}
            </div>
          </div>
        </section>
      </div>

      <ConfirmModal
        isOpen={isResetModalOpen}
        title={`Học lại ${activeConfig.label.toLowerCase()} từ đầu?`}
        message={`Toàn bộ tiến độ ôn ${activeConfig.label.toLowerCase()} trong Ghi nhớ sẽ về trạng thái mới, số lần ôn và lịch ôn sẽ được reset. Các mục đã thêm vẫn được giữ lại.`}
        confirmText="Reset tiến độ"
        cancelText="Hủy"
        processingText="Đang reset..."
        onConfirm={resetGrammarProgress}
        onCancel={() => setIsResetModalOpen(false)}
        errorMessage={resetError}
      />
    </div>
  );
};
