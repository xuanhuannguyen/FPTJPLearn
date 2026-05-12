import { useEffect } from 'react';

export function useKeyboardNav(
  isActive: boolean,
  totalQuestions: number,
  setCurrentIndex: React.Dispatch<React.SetStateAction<number>>,
) {
  useEffect(() => {
    if (!isActive || totalQuestions <= 0) return;
    const handler = (e: KeyboardEvent) => {
      if (e.key === 'ArrowLeft') setCurrentIndex((p) => Math.max(0, p - 1));
      if (e.key === 'ArrowRight') setCurrentIndex((p) => Math.min(totalQuestions - 1, p + 1));
    };
    window.addEventListener('keydown', handler);
    return () => window.removeEventListener('keydown', handler);
  }, [isActive, totalQuestions, setCurrentIndex]);
}
