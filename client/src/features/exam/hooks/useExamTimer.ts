import { useEffect, useRef, useState } from 'react';
import type { ExamAttempt } from '../types/exam.types';

interface UseExamTimerReturn {
  timeLeft: number;
  isUrgent: boolean;
  formatTime: (seconds: number) => string;
}

export function useExamTimer(
  isActive: boolean,
  attempt: ExamAttempt | null,
  onExpire: () => void,
): UseExamTimerReturn {
  const [timeLeft, setTimeLeft] = useState(0);
  const timerRef = useRef<ReturnType<typeof setInterval> | undefined>(undefined);
  const onExpireRef = useRef(onExpire);

  useEffect(() => {
    onExpireRef.current = onExpire;
  }, [onExpire]);

  // Initialize time from attempt
  useEffect(() => {
    if (!attempt) return;
    const expiresAt = new Date(attempt.expiresAt).getTime();

    const timer = window.setTimeout(() => {
      setTimeLeft(Math.max(0, Math.floor((expiresAt - Date.now()) / 1000)));
    }, 0);

    return () => window.clearTimeout(timer);
  }, [attempt]);

  // Countdown
  useEffect(() => {
    if (!isActive) return;

    timerRef.current = setInterval(() => {
      setTimeLeft((prev) => {
        if (prev <= 1) {
          clearInterval(timerRef.current);
          onExpireRef.current();
          return 0;
        }
        return prev - 1;
      });
    }, 1000);

    return () => clearInterval(timerRef.current);
  }, [isActive]);

  const formatTime = (seconds: number) => {
    const m = Math.floor(seconds / 60);
    const s = seconds % 60;
    return `${String(m).padStart(2, '0')}:${String(s).padStart(2, '0')}`;
  };

  return { timeLeft, isUrgent: timeLeft < 120, formatTime };
}
