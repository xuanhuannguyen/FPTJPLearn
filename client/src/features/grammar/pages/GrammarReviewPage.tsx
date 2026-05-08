import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { ArrowLeft, CheckCircle, Clock, Loader2, RotateCcw, XCircle } from 'lucide-react';
import { grammarApi } from '../api/grammarApi';
import type { GrammarAnswerResult, GrammarDueResponse, GrammarReviewCard } from '../types/grammar.types';
import { getStatusClass, getStudyLevelLabel } from '../utils/grammarDisplay';

export const GrammarReviewPage = () => {
  const navigate = useNavigate();
  const [due, setDue] = useState<GrammarDueResponse>({ dueCount: 0, cards: [] });
  const [currentIndex, setCurrentIndex] = useState(0);
  const [isLoading, setIsLoading] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [lastResult, setLastResult] = useState<GrammarAnswerResult | null>(null);
  const [error, setError] = useState('');

  const loadDue = async () => {
    const data = await grammarApi.getDueCards();
    setDue(data);
    setCurrentIndex(0);
  };

  useEffect(() => {
    let cancelled = false;

    const run = async () => {
      try {
        setIsLoading(true);
        setError('');
        const data = await grammarApi.getDueCards();
        if (!cancelled) {
          setDue(data);
        }
      } catch (err) {
        console.error(err);
        if (!cancelled) {
          setError('Không tải được thẻ ôn ngữ pháp.');
        }
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    };

    void run();

    return () => {
      cancelled = true;
    };
  }, []);

  const currentCard: GrammarReviewCard | undefined = due.cards[currentIndex];

  const submitAnswer = async (quality: number) => {
    if (!currentCard) {
      return;
    }

    try {
      setIsSubmitting(true);
      setLastResult(null);
      const result = await grammarApi.submitReviewAnswer(currentCard.patternId, quality);
      setLastResult(result);

      setDue((prev) => {
        const nextCards = prev.cards.filter((card) => card.patternId !== currentCard.patternId);
        return { dueCount: nextCards.length, cards: nextCards };
      });
      setCurrentIndex(0);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="space-y-6 animate-fade-in">
      <div className="flex flex-wrap items-center justify-between gap-3">
        <button onClick={() => navigate('/grammar')} className="btn-secondary">
          <ArrowLeft size={18} />
          Back to grammar
        </button>
        <button onClick={() => void loadDue()} className="btn-secondary">
          <RotateCcw size={18} />
          Refresh
        </button>
      </div>

      <section className="app-surface rounded-[28px] p-6 md:p-8">
        <div className="flex flex-col gap-5 md:flex-row md:items-end md:justify-between">
          <div>
            <p className="text-xs font-black uppercase tracking-[0.2em] text-accent-primary">Grammar SRS</p>
            <h1 className="mt-2 font-heading text-4xl font-black leading-none text-text-primary md:text-5xl">
              Ôn ngữ pháp cần học lại
            </h1>
            <p className="mt-3 max-w-2xl text-base font-bold leading-6 text-text-secondary">
              Đây là SRS riêng cho ngữ pháp. Các thẻ chỉ xuất hiện sau khi bạn thêm mẫu vào study.
            </p>
          </div>
          <div className="grid grid-cols-2 gap-2 text-center">
            <div className="rounded-2xl border-2 border-border bg-bg-tertiary px-4 py-3 shadow-pop">
              <p className="text-2xl font-black">{due.dueCount}</p>
              <p className="text-xs font-black text-text-muted">Due</p>
            </div>
            <div className="rounded-2xl border-2 border-border bg-white px-4 py-3 shadow-pop">
              <p className="text-2xl font-black">{currentCard ? currentIndex + 1 : 0}</p>
              <p className="text-xs font-black text-text-muted">Current</p>
            </div>
          </div>
        </div>
      </section>

      {isLoading ? (
        <div className="flex h-64 flex-col items-center justify-center text-text-secondary">
          <Loader2 size={36} className="mb-4 animate-spin text-accent-primary" />
          <p className="font-bold">Loading due cards...</p>
        </div>
      ) : error ? (
        <div className="rounded-2xl border-2 border-accent-danger bg-accent-danger/10 p-5 font-bold text-accent-danger shadow-pop">
          {error}
        </div>
      ) : !currentCard ? (
        <div className="glass-card flex min-h-[360px] flex-col items-center justify-center p-10 text-center">
          <CheckCircle size={48} className="mb-4 text-accent-success" />
          <h2 className="font-heading text-3xl font-black text-text-primary">Không có thẻ cần ôn</h2>
          <p className="mt-2 max-w-xl font-bold text-text-secondary">
            Hãy mở một mẫu ngữ pháp và bấm Add to study để bắt đầu tạo lịch ôn riêng.
          </p>
          <Link to="/grammar/N5" className="btn-primary mt-6">
            Browse N5 grammar
          </Link>
        </div>
      ) : (
        <div className="grid gap-5 lg:grid-cols-[minmax(0,1fr)_320px]">
          <article className="rounded-[28px] border-2 border-border bg-white p-6 shadow-card md:p-8">
            <div className="mb-4 flex flex-wrap items-center gap-2">
              <span className="rounded-xl border border-border/40 bg-bg-tertiary px-3 py-1 text-xs font-black text-text-secondary">
                {currentCard.level}
              </span>
              <span className={`rounded-xl border px-3 py-1 text-xs font-black ${getStatusClass(currentCard.status)}`}>
                {currentCard.status}
              </span>
              <span className="rounded-xl border border-accent-primary/20 bg-accent-primary/10 px-3 py-1 text-xs font-black text-accent-primary">
                {getStudyLevelLabel(currentCard.studyLevel)}
              </span>
            </div>

            <h2 className="font-jp text-5xl font-black leading-tight text-text-primary">{currentCard.pattern}</h2>
            <p className="mt-2 font-heading text-2xl font-black text-text-primary">{currentCard.title}</p>
            <p className="mt-3 text-xl font-extrabold text-accent-primary">{currentCard.meaning}</p>

            <div className="mt-6 rounded-2xl border border-border/40 bg-bg-tertiary p-4">
              <p className="text-xs font-black uppercase tracking-[0.16em] text-text-muted">Structure</p>
              <p className="mt-2 font-jp text-2xl font-black text-text-primary">{currentCard.structure}</p>
            </div>

            <div className="mt-5 grid gap-3 md:grid-cols-2">
              {currentCard.examples.slice(0, 2).map((example) => (
                <div key={example.id} className="rounded-2xl border border-border/40 bg-white p-4">
                  <p className="font-jp text-lg font-black text-text-primary">{example.japanese}</p>
                  <p className="mt-1 font-jp text-sm font-bold text-text-secondary">{example.reading}</p>
                  <p className="mt-2 text-sm font-extrabold text-accent-primary">{example.meaning}</p>
                </div>
              ))}
            </div>

            {lastResult ? (
              <div className="mt-5 rounded-2xl border border-accent-primary/20 bg-accent-primary/10 p-4 font-bold text-accent-primary" aria-live="polite">
                Updated: Level {lastResult.oldLevel} {'->'} {lastResult.newLevel}, status {lastResult.newStatus}.
              </div>
            ) : null}
          </article>

          <aside className="rounded-[28px] border-2 border-border bg-white p-5 shadow-card">
            <p className="text-xs font-black uppercase tracking-[0.16em] text-text-tertiary">How did you remember?</p>
            <div className="mt-4 space-y-3">
              <button
                onClick={() => submitAnswer(5)}
                className="btn-primary w-full justify-start"
                disabled={isSubmitting}
              >
                {isSubmitting ? <Loader2 size={18} className="animate-spin" /> : <CheckCircle size={18} />}
                Nhớ rõ
              </button>
              <button
                onClick={() => submitAnswer(3)}
                className="btn-secondary w-full justify-start"
                disabled={isSubmitting}
              >
                <Clock size={18} />
                Khó nhớ
              </button>
              <button
                onClick={() => submitAnswer(1)}
                className="btn-secondary w-full justify-start border-accent-danger/40 text-accent-danger hover:bg-accent-danger/10"
                disabled={isSubmitting}
              >
                <XCircle size={18} />
                Quên
              </button>
            </div>
            <p className="mt-5 text-sm font-bold leading-6 text-text-muted">
              Nhớ rõ tăng level. Quên ở mastered sẽ đưa mẫu về relearning và hẹn ôn lại gần hơn.
            </p>
          </aside>
        </div>
      )}
    </div>
  );
};
