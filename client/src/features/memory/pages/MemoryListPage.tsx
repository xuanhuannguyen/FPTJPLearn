import { useEffect, useMemo, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { ArrowLeft, BookOpen, BrainCircuit, CalendarClock, Loader2, Trash2 } from 'lucide-react';
import { memoryApi } from '../api/memoryApi';
import { getMemoryTypeConfig } from '../memory.config';
import type { MemoryCard, MemoryItemType, MemoryStatus } from '../types/memory.types';

const statusLabels: Record<MemoryStatus, string> = {
  new: 'Mới',
  learning: 'Đang học',
  review: 'Ôn tập',
  mastered: 'Dài hạn',
  relearning: 'Học lại',
};

const statusClassNames: Record<MemoryStatus, string> = {
  new: 'bg-sky-100 text-sky-700 border-sky-200',
  learning: 'bg-amber-100 text-amber-700 border-amber-200',
  review: 'bg-violet-100 text-violet-700 border-violet-200',
  mastered: 'bg-emerald-100 text-emerald-700 border-emerald-200',
  relearning: 'bg-rose-100 text-rose-700 border-rose-200',
};

const isMemoryItemType = (value: string | undefined): value is MemoryItemType => {
  return value === 'kanji' || value === 'vocabulary' || value === 'grammar';
};

const formatReviewTime = (value: string) => {
  const reviewDate = new Date(value);
  if (Number.isNaN(reviewDate.getTime())) {
    return '--';
  }

  if (reviewDate.getTime() <= Date.now()) {
    return 'Đến hạn';
  }

  return reviewDate.toLocaleString('vi-VN', {
    day: '2-digit',
    month: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
  });
};

const getItemTitle = (card: MemoryCard) => {
  if (card.itemType === 'grammar') {
    return card.frontMeta || card.frontPrimary;
  }

  return card.frontPrimary;
};

const getItemSubtitle = (card: MemoryCard) => {
  if (card.itemType === 'grammar') {
    return card.frontPrimary;
  }

  return [card.frontSecondary, card.frontMeta].filter(Boolean).join(' · ');
};

export const MemoryListPage = () => {
  const navigate = useNavigate();
  const params = useParams<{ type: string }>();
  const type = isMemoryItemType(params.type) ? params.type : null;
  const [cards, setCards] = useState<MemoryCard[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [removingId, setRemovingId] = useState<string | null>(null);
  const [error, setError] = useState('');

  const config = useMemo(() => (type ? getMemoryTypeConfig(type) : null), [type]);

  useEffect(() => {
    if (!type) return;

    let cancelled = false;

    const loadCards = async () => {
      try {
        setIsLoading(true);
        setError('');
        const response = await memoryApi.getCards(type, 'all');
        if (!cancelled) {
          setCards(response.cards.filter((card) => card.itemType === type));
        }
      } catch (err) {
        console.error(err);
        if (!cancelled) {
          setError('Không tải được danh sách ghi nhớ.');
        }
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    };

    void loadCards();

    return () => {
      cancelled = true;
    };
  }, [type]);

  const removeItem = async (memoryItemId: string) => {
    if (!type) return;

    try {
      setRemovingId(memoryItemId);
      setError('');
      await memoryApi.removeItem(type, memoryItemId);
      setCards((current) => current.filter((card) => card.id !== memoryItemId));
    } catch (err) {
      console.error(err);
      setError('Không thể xóa mục khỏi Ghi nhớ. Hãy thử lại.');
    } finally {
      setRemovingId(null);
    }
  };

  if (!type || !config) {
    return (
      <div className="mx-auto max-w-4xl px-4 pt-8">
        <div className="rounded-2xl border-2 border-accent-danger bg-accent-danger/10 p-5 font-bold text-accent-danger">
          Loại ghi nhớ không hợp lệ.
        </div>
        <button type="button" onClick={() => navigate('/memory')} className="btn-secondary mt-4">
          Quay lại
        </button>
      </div>
    );
  }

  const Icon = config.icon;

  return (
    <div className="mx-auto max-w-6xl space-y-5 px-4 pb-20 pt-4 animate-fade-in">
      <header className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
        <div>
          <button
            type="button"
            onClick={() => navigate('/memory')}
            className="inline-flex items-center gap-2 text-sm font-extrabold text-text-muted transition hover:text-text-primary"
          >
            <ArrowLeft size={18} />
            Quay lại Memory
          </button>
          <div className="mt-4 flex items-center gap-3">
            <span className={`grid h-12 w-12 place-items-center rounded-2xl text-white shadow-md ${config.accentClassName}`}>
              <Icon size={24} />
            </span>
            <div>
              <p className="text-xs font-black uppercase tracking-[0.18em] text-text-muted">Danh sách ghi nhớ</p>
              <h1 className="text-3xl font-black tracking-normal text-text-primary">{config.label}</h1>
            </div>
          </div>
        </div>

      </header>

      {error ? (
        <div className="rounded-2xl border-2 border-accent-danger bg-accent-danger/10 p-4 text-sm font-bold text-accent-danger">
          {error}
        </div>
      ) : null}

      <section className="rounded-[28px] border-2 border-slate-900 bg-white p-4 shadow-[0_8px_0_#111827] md:p-5">
        <div className="mb-4 flex flex-wrap items-center justify-between gap-3 border-b-2 border-slate-100 pb-4">
          <div>
            <p className="text-xs font-black uppercase tracking-[0.18em] text-text-muted">Tổng số item</p>
            <p className="text-2xl font-black text-text-primary">{cards.length}</p>
          </div>
          <button type="button" onClick={() => navigate(config.reviewPath)} className="btn-primary">
            Ôn tập ngay
          </button>
        </div>

        {isLoading ? (
          <div className="flex min-h-48 flex-col items-center justify-center text-text-secondary">
            <Loader2 size={34} className="mb-3 animate-spin text-accent-primary" />
            <p className="font-bold">Đang tải danh sách...</p>
          </div>
        ) : cards.length === 0 ? (
          <div className="flex min-h-48 flex-col items-center justify-center rounded-2xl bg-slate-50 p-6 text-center">
            <BookOpen size={38} className="mb-3 text-text-muted" />
            <p className="text-lg font-black text-text-primary">Chưa có item trong {config.label}</p>
            <p className="mt-1 max-w-lg text-sm font-bold text-text-muted">{config.emptyText}</p>
          </div>
        ) : (
          <div className="space-y-3">
            {cards.map((card, index) => (
              <article
                key={card.id}
                className="grid gap-4 rounded-2xl border-2 border-slate-200 bg-slate-50 p-4 transition hover:border-slate-900 hover:bg-white md:grid-cols-[56px_1fr_auto]"
              >
                <div className="grid h-12 w-12 place-items-center rounded-2xl border-2 border-slate-900 bg-white text-sm font-black text-text-primary shadow-[0_3px_0_#111827]">
                  {index + 1}
                </div>

                <div className="min-w-0">
                  <div className="flex flex-wrap items-center gap-2">
                    <h2 className="break-words text-2xl font-black tracking-normal text-text-primary">
                      {getItemTitle(card)}
                    </h2>
                    <span className={`rounded-full border px-2.5 py-1 text-[11px] font-black ${statusClassNames[card.status] ?? statusClassNames.new}`}>
                      {statusLabels[card.status] ?? card.status}
                    </span>
                  </div>
                  {getItemSubtitle(card) ? (
                    <p className="mt-1 break-words text-sm font-extrabold text-text-muted">{getItemSubtitle(card)}</p>
                  ) : null}
                  <p className="mt-3 break-words text-base font-bold text-text-secondary">{card.backPrimary}</p>
                  {card.backSecondary && card.itemType !== 'kanji' ? (
                    <p className="mt-1 break-words text-xs font-bold uppercase tracking-[0.12em] text-text-muted">{card.backSecondary}</p>
                  ) : null}
                </div>

                <div className="flex flex-col items-start gap-3 md:items-end">
                  <div className="flex flex-wrap gap-2 md:justify-end">
                    <span className="inline-flex items-center gap-1 rounded-full border border-slate-200 bg-white px-3 py-1 text-xs font-black text-text-primary">
                      <BrainCircuit size={14} />
                      Mức {card.level}
                    </span>
                    <span className="inline-flex items-center gap-1 rounded-full border border-slate-200 bg-white px-3 py-1 text-xs font-black text-text-muted">
                      <CalendarClock size={14} />
                      {formatReviewTime(card.nextReviewAt)}
                    </span>
                  </div>

                  <button
                    type="button"
                    disabled={removingId === card.id}
                    onClick={() => removeItem(card.id)}
                    className="inline-flex items-center gap-2 rounded-xl border-2 border-rose-200 bg-white px-4 py-2 text-xs font-black text-rose-600 transition hover:border-rose-500 hover:bg-rose-50 disabled:pointer-events-none disabled:opacity-60"
                  >
                    {removingId === card.id ? <Loader2 size={14} className="animate-spin" /> : <Trash2 size={14} />}
                    Remove
                  </button>
                </div>
              </article>
            ))}
          </div>
        )}
      </section>
    </div>
  );
};
