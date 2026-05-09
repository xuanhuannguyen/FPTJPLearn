import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { ArrowLeft, CheckCircle2, Loader2 } from 'lucide-react';
import { memoryApi } from '../api/memoryApi';
import { GrammarMemoryFlashcard } from '../components/GrammarMemoryFlashcard';
import { MemoryRatingButtons } from '../components/MemoryRatingButtons';
import { useMemoryReviewSession } from '../hooks/useMemoryReviewSession';

export const MemoryGrammarReviewPage = () => {
  const navigate = useNavigate();
  const loadCards = useCallback(() => memoryApi.getCards('grammar', 'due'), []);
  const submitCardAnswer = useCallback((memoryItemId: string, quality: number) => memoryApi.submitAnswer('grammar', memoryItemId, quality), []);
  const {
    queue,
    currentIndex,
    currentCard,
    isCompleted,
    isLoading,
    isSubmitting,
    isBackVisible,
    error,
    lastResult,
    stats,
    answeredCount,
    setIsBackVisible,
    submitAnswer,
  } = useMemoryReviewSession({
    loadCards,
    submitCardAnswer,
    loadErrorMessage: 'Không tải được thẻ ngữ pháp cần ôn.',
    submitErrorMessage: 'Không lưu được kết quả ôn tập.',
  });

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
    <div className="mx-auto max-w-[820px] space-y-2 px-4 pb-10">
      <GrammarMemoryFlashcard
        card={currentCard}
        isBackVisible={isBackVisible}
        onFlip={() => setIsBackVisible((prev) => !prev)}
        onViewDetail={
          currentCard.sourceGrammarPatternId
            ? () => window.open(`/grammar/patterns/${currentCard.sourceGrammarPatternId}`, '_blank', 'noopener,noreferrer')
            : undefined
        }
      />

      <div className="grid grid-cols-3 items-center py-0.5 text-text-secondary">
        <button type="button" onClick={() => navigate('/memory')} className="inline-flex items-center gap-2 justify-self-start text-sm font-extrabold hover:text-text-primary md:text-base">
          <ArrowLeft size={18} />
          Thống kê
        </button>
        <p className="justify-self-center text-sm font-black md:text-base">
          {currentIndex + 1} / {queue.length}
        </p>
      </div>

      <MemoryRatingButtons disabled={isSubmitting || !isBackVisible} onRate={submitAnswer} />
    </div>
  );
};
