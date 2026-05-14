import { useCallback, useEffect, useRef, useState } from 'react';

const MAX_FULLSCREEN_EXITS = 3;

interface UseExamProctorReturn {
  containerRef: React.RefObject<HTMLDivElement | null>;
  tabWarnings: number;
  fullscreenExits: number;
  showFullscreenOverlay: boolean;
  enterFullscreen: () => void;
  exitFullscreen: () => void;
  reEnterFullscreen: () => void;
}

export function useExamProctor(
  isActive: boolean,
  onForceSubmit: () => void,
): UseExamProctorReturn {
  const containerRef = useRef<HTMLDivElement>(null);
  const [tabWarnings, setTabWarnings] = useState(0);
  const [fullscreenExits, setFullscreenExits] = useState(0);
  const [showFullscreenOverlay, setShowFullscreenOverlay] = useState(false);

  const onForceSubmitRef = useRef(onForceSubmit);
  useEffect(() => {
    onForceSubmitRef.current = onForceSubmit;
  }, [onForceSubmit]);

  // Tab visibility — re-fullscreen on return
  useEffect(() => {
    if (!isActive) return;
    const handler = () => {
      if (document.hidden) {
        setTabWarnings((prev) => prev + 1);
      } else if (!document.fullscreenElement) {
        containerRef.current?.requestFullscreen?.().catch(() => { /* noop */ });
      }
    };
    document.addEventListener('visibilitychange', handler);
    return () => document.removeEventListener('visibilitychange', handler);
  }, [isActive]);

  // Fullscreen exit — overlay or force submit
  useEffect(() => {
    if (!isActive) return;
    const handler = () => {
      if (!document.fullscreenElement) {
        setTabWarnings((prev) => prev + 1);
        setFullscreenExits((prev) => {
          const next = prev + 1;
          if (next >= MAX_FULLSCREEN_EXITS) {
            onForceSubmitRef.current();
          } else {
            setShowFullscreenOverlay(true);
          }
          return next;
        });
      }
    };
    document.addEventListener('fullscreenchange', handler);
    return () => document.removeEventListener('fullscreenchange', handler);
  }, [isActive]);

  // Block page close/refresh
  useEffect(() => {
    if (!isActive) return;
    const handler = (e: BeforeUnloadEvent) => {
      e.preventDefault();
      e.returnValue = 'Bạn đang làm bài thi. Nếu thoát, bài thi sẽ không được lưu!';
      return e.returnValue;
    };
    window.addEventListener('beforeunload', handler);
    return () => window.removeEventListener('beforeunload', handler);
  }, [isActive]);

  // Block F11 and Escape
  useEffect(() => {
    if (!isActive) return;
    const handler = (e: KeyboardEvent) => {
      if (e.key === 'F11' || e.key === 'Escape') {
        e.preventDefault();
      }
    };
    window.addEventListener('keydown', handler);
    return () => window.removeEventListener('keydown', handler);
  }, [isActive]);

  const enterFullscreen = useCallback(() => {
    containerRef.current?.requestFullscreen?.().catch(() => { /* noop */ });
  }, []);

  const exitFullscreen = useCallback(() => {
    if (document.fullscreenElement) document.exitFullscreen?.().catch(() => { /* noop */ });
  }, []);

  const reEnterFullscreen = useCallback(() => {
    setShowFullscreenOverlay(false);
    containerRef.current?.requestFullscreen?.().catch(() => { /* noop */ });
  }, []);

  return {
    containerRef, tabWarnings, fullscreenExits, showFullscreenOverlay,
    enterFullscreen, exitFullscreen, reEnterFullscreen,
  };
}
