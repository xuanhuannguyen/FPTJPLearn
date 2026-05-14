import { useCallback, useEffect, useRef, useState } from 'react';
import { examApi } from '../api/examApi';
import type { ExamAttempt, ExamAttemptReview } from '../types/exam.types';

export type ExamPhase = 'loading' | 'confirm' | 'exam' | 'submitting' | 'result';

interface UseExamAttemptReturn {
  phase: ExamPhase;
  attempt: ExamAttempt | null;
  review: ExamAttemptReview | null;
  currentIndex: number;
  selectedAnswers: Record<string, string>;
  isSaving: boolean;
  error: string;
  answeredCount: number;
  setPhase: (phase: ExamPhase) => void;
  setCurrentIndex: React.Dispatch<React.SetStateAction<number>>;
  selectAnswer: (questionId: string, optionId: string) => Promise<void>;
  handleSubmit: () => Promise<void>;
}

export function useExamAttempt(attemptId: string | undefined): UseExamAttemptReturn {
  const [phase, setPhase] = useState<ExamPhase>('loading');
  const [attempt, setAttempt] = useState<ExamAttempt | null>(null);
  const [review, setReview] = useState<ExamAttemptReview | null>(null);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [selectedAnswers, setSelectedAnswers] = useState<Record<string, string>>({});
  const [isSaving, setIsSaving] = useState(false);
  const [error, setError] = useState('');

  const phaseRef = useRef(phase);
  useEffect(() => {
    phaseRef.current = phase;
  }, [phase]);

  useEffect(() => {
    if (!attemptId) return;
    let cancelled = false;

    const load = async () => {
      try {
        const data = await examApi.getAttempt(attemptId);
        if (cancelled) return;
        setAttempt(data);

        if (data.status !== 'in_progress') {
          const rev = await examApi.getAttemptReview(attemptId);
          if (!cancelled) { setReview(rev); setPhase('result'); }
        } else {
          const answers: Record<string, string> = {};
          data.questions.forEach((q) => { if (q.selectedOptionId) answers[q.id] = q.selectedOptionId; });
          setSelectedAnswers(answers);
          setPhase('confirm');
        }
      } catch {
        if (!cancelled) setError('Không tải được bài thi.');
      }
    };

    void load();
    return () => { cancelled = true; };
  }, [attemptId]);

  const selectAnswer = useCallback(async (questionId: string, optionId: string) => {
    if (!attemptId) return;
    setSelectedAnswers((prev) => ({ ...prev, [questionId]: optionId }));
    try {
      setIsSaving(true);
      await examApi.saveAttemptAnswer(attemptId, questionId, optionId);
    } catch {
      /* silent — answer saved locally */
    } finally {
      setIsSaving(false);
    }
  }, [attemptId]);

  const handleSubmit = useCallback(async () => {
    if (!attemptId || phaseRef.current === 'submitting' || phaseRef.current === 'result') return;
    setPhase('submitting');
    try {
      await examApi.submitAttempt(attemptId);
      const rev = await examApi.getAttemptReview(attemptId);
      setReview(rev);
      setCurrentIndex(0);
      setPhase('result');
    } catch {
      setError('Nộp bài thất bại. Vui lòng thử lại.');
      setPhase('exam');
    }
  }, [attemptId]);

  const answeredCount = Object.keys(selectedAnswers).length;

  return {
    phase, attempt, review, currentIndex, selectedAnswers,
    isSaving, error, answeredCount,
    setPhase, setCurrentIndex, selectAnswer, handleSubmit,
  };
}
