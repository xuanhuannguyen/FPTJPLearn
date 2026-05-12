import { useCallback, useEffect, useMemo, useState } from 'react';
import type { MemoryAnswerResult, MemoryCard, MemoryCardsResponse } from '../types/memory.types';

type SessionStats = {
  again: number;
  hard: number;
  good: number;
  easy: number;
};

type UseMemoryReviewSessionOptions = {
  loadCards: () => Promise<MemoryCardsResponse>;
  submitCardAnswer: (memoryItemId: string, quality: number) => Promise<MemoryAnswerResult>;
  loadErrorMessage: string;
  submitErrorMessage: string;
};

const initialStats: SessionStats = { again: 0, hard: 0, good: 0, easy: 0 };

const qualityToStatKey: Record<number, keyof SessionStats> = {
  1: 'again',
  3: 'hard',
  4: 'good',
  5: 'easy',
};

const qualityByKey: Record<string, number> = {
  Digit1: 1,
  Numpad1: 1,
  Digit2: 3,
  Numpad2: 3,
  Digit3: 4,
  Numpad3: 4,
  Digit4: 5,
  Numpad4: 5,
};

export const useMemoryReviewSession = ({
  loadCards,
  submitCardAnswer,
  loadErrorMessage,
  submitErrorMessage,
}: UseMemoryReviewSessionOptions) => {
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

    const run = async () => {
      try {
        setIsLoading(true);
        setError('');
        const data = await loadCards();
        if (!cancelled) {
          setQueue(data.cards);
          setCurrentIndex(0);
          setIsBackVisible(false);
          setLastResult(null);
          setStats(initialStats);
        }
      } catch (err) {
        console.error(err);
        if (!cancelled) {
          setError(loadErrorMessage);
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
  }, [loadCards, loadErrorMessage]);

  const currentCard = queue[currentIndex] ?? null;
  const isCompleted = !isLoading && queue.length > 0 && currentIndex >= queue.length;
  const answeredCount = useMemo(() => stats.again + stats.hard + stats.good + stats.easy, [stats]);

  const submitAnswer = useCallback(async (quality: number) => {
    if (!currentCard) return;

    try {
      setIsSubmitting(true);
      const result = await submitCardAnswer(currentCard.id, quality);
      setLastResult(result);
      setStats((prev) => {
        const key = qualityToStatKey[quality];
        return key ? { ...prev, [key]: prev[key] + 1 } : prev;
      });

      setQueue((prev) => {
        if (!result.requeueInSession) {
          return prev;
        }

        return [...prev, currentCard];
      });
      setCurrentIndex((prev) => prev + 1);
      setIsBackVisible(false);
    } catch (err) {
      console.error(err);
      setError(submitErrorMessage);
    } finally {
      setIsSubmitting(false);
    }
  }, [currentCard, submitCardAnswer, submitErrorMessage]);

  useEffect(() => {
    const handleKeyDown = (event: KeyboardEvent) => {
      const target = event.target as HTMLElement | null;
      const isTyping =
        target?.tagName === 'INPUT' ||
        target?.tagName === 'TEXTAREA' ||
        target?.tagName === 'SELECT' ||
        target?.isContentEditable;

      if (isTyping || isLoading || isCompleted || !currentCard || isSubmitting) {
        return;
      }

      if (event.code === 'Space') {
        event.preventDefault();
        setIsBackVisible((prev) => !prev);
        return;
      }

      if (!isBackVisible) {
        return;
      }

      const quality = qualityByKey[event.code];
      if (quality) {
        event.preventDefault();
        void submitAnswer(quality);
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [currentCard, isBackVisible, isCompleted, isLoading, isSubmitting, submitAnswer]);

  return {
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
  };
};
