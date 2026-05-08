import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { ArrowLeft, CheckCircle2, Loader2 } from 'lucide-react';
import { memoryApi } from '../api/memoryApi';
import { GrammarMemoryFlashcard } from '../components/GrammarMemoryFlashcard';
import { MemoryRatingButtons } from '../components/MemoryRatingButtons';
import type { MemoryAnswerResult, MemoryCard } from '../types/memory.types';

type SessionStats = {
  again: number;
  hard: number;
  good: number;
  easy: number;
};

const initialStats: SessionStats = { again: 0, hard: 0, good: 0, easy: 0 };

export const MemoryGrammarReviewPage = () => {
  const navigate = useNavigate();
  const [queue, setQueue] = useState<MemoryCard[]>([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [isLoading, setIsLoading] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isBackVisible, setIsBackVisible] = useState(false);
  const [error, setError] = useState('');
  const [lastResult, setLastResult] = useState<MemoryAnswerResult | null>(null);
  const [stats, setStats] = useState<SessionStats>(initialStats);

  useEffect(() => {
    let cancelled = false;

    const load = async () => {
      try {
        setIsLoading(true);
        setError('');
        const data = await memoryApi.getGrammarCards('due');
        if (!cancelled) {
          setQueue(data.cards);
          setCurrentIndex(0);
        }
      } catch (err) {
        console.error(err);
        if (!cancelled) {
          setError('Không tải được thẻ ngữ pháp cần ôn.');
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

  const currentCard = queue[currentIndex] ?? null;
  const isCompleted = !isLoading && queue.length > 0 && currentIndex >= queue.length;

  const answeredCount = useMemo(() => stats.again + stats.hard + stats.good + stats.easy, [stats]);

  const submitAnswer = async (quality: number) => {
    if (!currentCard) return;

    try {
      setIsSubmitting(true);
      const result = await memoryApi.submitGrammarAnswer(currentCard.id, quality);
      setLastResult(result);
      setStats((prev) => ({
        again: prev.again + (quality === 1 ? 1 : 0),
        hard: prev.hard + (quality === 3 ? 1 : 0),
        good: prev.good + (quality === 4 ? 1 : 0),
        easy: prev.easy + (quality === 5 ? 1 : 0),
      }));

      setQueue((prev) => {
        if (!result.requeueInSession) {
          return prev;
        }

        const next = [...prev];
        next.push(currentCard);
        return next;
      });
      setCurrentIndex((prev) => prev + 1);
      setIsBackVisible(false);
    } catch (err) {
      console.error(err);
      setError('Không lưu được kết quả ôn tập.');
    } finally {
      setIsSubmitting(false);
    }
  };

  if (isLoading) {
    return (
      <div className="flex h-64 flex-col items-center justify-center text-text-secondary">
        <Loader2 size={36} className="mb-4 animate-spin text-accent-primary" />
        <p className="font-bold">Đang tải thẻ ghi nhớ...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="mx-auto max-w-4xl space-y-4">
        <button type="button" onClick={() => navigate('/memory')} className="btn-secondary">
          <ArrowLeft size={18} />
          Quay lại Ghi nhớ
        </button>
        <div className="rounded-2xl border-2 border-accent-danger bg-accent-danger/10 p-5 font-bold text-accent-danger">
          {error}
        </div>
      </div>
    );
  }

  if (queue.length === 0) {
    return (
      <div className="mx-auto max-w-3xl space-y-5 rounded-2xl bg-white p-8 text-center shadow-card">
        <CheckCircle2 size={48} className="mx-auto text-emerald-500" />
        <h1 className="text-3xl font-black text-text-primary">Không có ngữ pháp cần ôn</h1>
        <p className="font-bold text-text-secondary">Hãy mở một mẫu ngữ pháp và bấm "Thêm vào ghi nhớ".</p>
        <button type="button" onClick={() => navigate('/memory')} className="btn-primary">
          Quay lại Ghi nhớ
        </button>
      </div>
    );
  }

  if (isCompleted || !currentCard) {
    return (
      <div className="mx-auto max-w-4xl space-y-6 rounded-2xl bg-white p-8 shadow-card">
        <div className="text-center">
          <CheckCircle2 size={52} className="mx-auto text-emerald-500" />
          <h1 className="mt-4 text-3xl font-black text-text-primary">Hoàn thành lượt ôn</h1>
          <p className="mt-2 font-bold text-text-secondary">Bạn đã xử lý {answeredCount} lượt card ngữ pháp trong Ghi nhớ.</p>
        </div>
        <div className="grid gap-3 sm:grid-cols-4">
          <div className="rounded-2xl bg-rose-50 p-4 text-center font-black text-rose-600">Quên rồi: {stats.again}</div>
          <div className="rounded-2xl bg-orange-50 p-4 text-center font-black text-orange-600">Khó: {stats.hard}</div>
          <div className="rounded-2xl bg-emerald-50 p-4 text-center font-black text-emerald-600">Tốt: {stats.good}</div>
          <div className="rounded-2xl bg-blue-50 p-4 text-center font-black text-blue-600">Dễ: {stats.easy}</div>
        </div>
        {lastResult ? <p className="text-center font-bold text-text-secondary">{lastResult.message}</p> : null}
        <div className="flex justify-center">
          <button type="button" onClick={() => navigate('/memory')} className="btn-primary">
            Quay lại thống kê
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-5xl space-y-5 px-4 pb-20">
      <button type="button" onClick={() => navigate('/memory')} className="btn-secondary">
        <ArrowLeft size={18} />
        Ghi nhớ
      </button>

      <div className="flex items-end justify-between gap-4">
        <div>
          <p className="text-xs font-black uppercase tracking-[0.22em] text-violet-500">Memory Grammar</p>
          <h1 className="text-3xl font-black text-text-primary">Ôn ngữ pháp</h1>
        </div>
        <p className="rounded-full bg-white px-4 py-2 text-sm font-black text-text-secondary shadow-sm">
          {currentIndex + 1} / {queue.length}
        </p>
      </div>

      <GrammarMemoryFlashcard card={currentCard} isBackVisible={isBackVisible} onFlip={() => setIsBackVisible(true)} />

      {isBackVisible ? (
        <MemoryRatingButtons disabled={isSubmitting} onRate={submitAnswer} />
      ) : null}
    </div>
  );
};
