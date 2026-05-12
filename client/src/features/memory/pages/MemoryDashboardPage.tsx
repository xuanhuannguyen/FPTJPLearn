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

      {/* Tabs - Centered */}
      <section className="flex gap-2 rounded-xl border border-slate-200 bg-white p-1 shadow-sm max-w-2xl mx-auto">
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
              className={`flex flex-1 min-h-[44px] items-center justify-center gap-2 rounded-lg px-4 text-xs font-black transition-all ${isActive
                  ? 'bg-violet-600 text-white shadow-md'
                  : 'text-text-muted hover:bg-slate-50'
                }`}
            >
              <Icon size={16} />
              <span>{config.label}</span>
              {due > 0 ? (
                <span className={`ml-1 rounded-full px-1.5 py-0.5 text-[9px] font-black ${isActive ? 'bg-white/20 text-white' : 'bg-orange-100 text-orange-500'}`}>
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
          <div className={`relative rounded-[28px] border-2 border-white/50 ${activeConfig.accentClassName} p-5 text-white shadow-lg overflow-hidden`}>
            {/* Header row */}
            <div className="flex items-center justify-between mb-4">
               <p className="text-[11px] font-black uppercase tracking-wider opacity-90">{activeConfig.summaryLabel} cần ôn hôm nay</p>
               <div className="flex flex-wrap justify-end gap-2">
                 <button
                   type="button"
                   onClick={() => navigate(activeConfig.listPath)}
                   className="inline-flex items-center gap-1.5 rounded-full border border-white/30 bg-white/20 px-2.5 py-1 text-[10px] font-black text-white backdrop-blur-md transition-all hover:bg-white/30"
                 >
                   <ListChecks size={10} strokeWidth={3} />
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
                      className="inline-flex items-center gap-1.5 rounded-full bg-white/20 backdrop-blur-md px-2.5 py-1 text-[10px] font-black text-white border border-white/30 transition-all hover:bg-white/30 disabled:pointer-events-none disabled:opacity-60"
                    >
                      {isResettingGrammar ? <Loader2 size={10} className="animate-spin" /> : <RotateCcw size={10} strokeWidth={3} />}
                      {isResettingGrammar ? 'Đang reset' : 'Học lại từ đầu'}
                    </button>
                 ) : null}
               </div>
            </div>

            <p className="text-6xl font-black drop-shadow-md">{activeSummary.due}</p>

            <div className="mt-6">
              {activeSummary.due > 0 ? (
                <button
                  type="button"
                  onClick={() => navigate(activeConfig.reviewPath)}
                  className="rounded-xl bg-white px-6 py-2 text-sm font-black text-violet-600 shadow-md transition-all hover:scale-105 active:scale-95"
                >
                  Ôn tập ngay
                </button>
              ) : (
                <p className="max-w-md rounded-xl bg-white/10 backdrop-blur-sm border border-white/20 p-4 text-xs font-bold text-white/90">{activeConfig.emptyText}</p>
              )}
            </div>
          </div>

          <div className="rounded-[28px] border-2 border-white/50 bg-blue-500 p-5 text-white shadow-lg">
            <div className="flex items-center gap-2 mb-4">
               <Sparkles size={16} className="text-yellow-200" />
              <p className="text-[11px] font-black uppercase tracking-wider opacity-90">Lượt ôn tập tiếp theo</p>
            </div>
            <p className="text-4xl font-black tracking-tight mt-6 drop-shadow-md">{nextReviewText}</p>
            {nextReviewTimeText && activeSummary.due === 0 ? (
              <p className="mt-3 inline-block rounded-lg bg-black/10 px-2 py-1 text-[10px] font-black text-white/80">{nextReviewTimeText}</p>
            ) : null}
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
