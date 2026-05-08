import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { BrainCircuit, Castle, Crown, Loader2, RotateCcw, Sparkles } from 'lucide-react';
import { memoryApi } from '../api/memoryApi';
import { MemoryStatsBar } from '../components/MemoryStatsBar';
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

const tabOptions: Array<{ type: MemoryItemType; label: string; icon: typeof BrainCircuit }> = [
  { type: 'kanji', label: 'Kanji', icon: Castle },
  { type: 'vocabulary', label: 'Từ vựng', icon: Crown },
  { type: 'grammar', label: 'Ngữ pháp', icon: BrainCircuit },
];

const labels: Record<MemoryItemType, string> = {
  kanji: 'Kanji',
  vocabulary: 'Từ vựng',
  grammar: 'Thẻ ngữ pháp',
};

const emptyText: Record<MemoryItemType, string> = {
  kanji: 'Kanji trong Ghi nhớ sẽ được thêm sau khi Kanji module sẵn sàng.',
  vocabulary: 'Từ vựng trong Ghi nhớ sẽ dùng source riêng, không dùng Vocabulary hiện tại.',
  grammar: 'Hãy mở một mẫu ngữ pháp và bấm "Thêm vào ghi nhớ".',
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
  const [error, setError] = useState('');
  const [nowTick, setNowTick] = useState(Date.now());

  useEffect(() => {
    let cancelled = false;

    const load = async () => {
      try {
        setIsLoading(true);
        setError('');
        const data = await memoryApi.getSummary();
        if (!cancelled) {
          setSummary(data);
          if (data.grammar.due > 0) {
            setActiveType('grammar');
          }
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
    <div className="mx-auto max-w-7xl space-y-8 px-4 pb-20 pt-4">
      <header>
        <h1 className="text-4xl font-black tracking-normal text-text-primary">Thống kê học tập</h1>
        <p className="mt-2 max-w-4xl text-lg font-bold leading-8 text-text-secondary">
          Theo dõi tiến độ và kế hoạch ôn tập của bạn, theo các báo cáo 21 ngày ôn tập kiến thức sẽ được nạp vào trí nhớ dài hạn
        </p>
      </header>

      <section className="grid gap-3 rounded-2xl bg-white p-2 shadow-sm md:grid-cols-3">
        {tabOptions.map((tab) => {
          const Icon = tab.icon;
          const due = summary?.[tab.type]?.due ?? 0;
          const isActive = tab.type === activeType;
          return (
            <button
              key={tab.type}
              type="button"
              onClick={() => setActiveType(tab.type)}
              className={`flex min-h-[54px] items-center justify-center gap-3 rounded-xl px-4 font-black transition-all ${
                isActive
                  ? 'bg-violet-500 text-white shadow-pop'
                  : 'text-text-secondary hover:bg-slate-50'
              }`}
            >
              <Icon size={20} />
              <span>{tab.label}</span>
              {due > 0 ? (
                <span className={`rounded-full px-2 py-0.5 text-xs font-black ${isActive ? 'bg-white/20 text-white' : 'bg-orange-100 text-orange-500'}`}>
                  {due}
                </span>
              ) : null}
            </button>
          );
        })}
      </section>

      <MemoryStatsBar summary={activeSummary} />

      <section className="grid gap-5 lg:grid-cols-2">
        <div className="rounded-2xl bg-violet-300/80 p-8 text-white shadow-sm">
          <div className="flex items-start justify-between gap-4">
            <div>
              <p className="text-lg font-bold">{labels[activeType]} cần ôn hôm nay</p>
              <p className="mt-4 text-6xl font-black">{activeSummary.due}</p>
            </div>
            {activeType === 'grammar' ? (
              <button
                type="button"
                onClick={() => navigate('/memory/grammar/review')}
                className="rounded-2xl bg-white px-4 py-2 text-sm font-black text-violet-600 transition-all hover:-translate-y-0.5"
              >
                <RotateCcw size={16} className="mr-1 inline" />
                Ôn lại từ đầu
              </button>
            ) : null}
          </div>
          {activeSummary.due > 0 ? (
            <button
              type="button"
              onClick={() => navigate(`/memory/${activeType}/review`)}
              className="mt-5 rounded-xl bg-white px-5 py-3 font-black text-violet-700 transition-all hover:-translate-y-0.5"
            >
              Ôn tập ngay
            </button>
          ) : (
            <p className="mt-5 max-w-md rounded-xl bg-white/20 p-4 font-bold text-white">{emptyText[activeType]}</p>
          )}
        </div>

        <div className="rounded-2xl bg-blue-400/90 p-8 text-white shadow-sm">
          <div className="flex items-center gap-2">
            <Sparkles size={20} />
            <p className="text-lg font-bold">Lượt ôn tập tiếp theo</p>
          </div>
          <p className="mt-8 text-3xl font-black tracking-wide">{nextReviewText}</p>
          {nextReviewTimeText && activeSummary.due === 0 ? (
            <p className="mt-3 text-sm font-bold text-white/80">{nextReviewTimeText}</p>
          ) : null}
        </div>
      </section>
    </div>
  );
};
