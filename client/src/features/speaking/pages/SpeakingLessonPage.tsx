import { useCallback, useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { ArrowLeft, ChevronLeft, ChevronRight, Eye, Languages, Loader2, Volume2, VolumeX } from 'lucide-react';
import { speakingApi } from '../api/speakingApi';
import type { SpeakingLesson, SpeakingLessonDetail, SpeakingSentence } from '../types/speaking.types';
import { PremiumLock } from '../../../shared/components/PremiumLock';
import { useUserAccess } from '../../../shared/hooks/useUserAccess';

export const SpeakingLessonPage = () => {
  const { courseCode, lessonId } = useParams<{ courseCode: string; lessonId: string }>();
  const navigate = useNavigate();
  const [detail, setDetail] = useState<SpeakingLessonDetail | null>(null);
  const [lessons, setLessons] = useState<SpeakingLesson[]>([]);
  const [showMeaning, setShowMeaning] = useState(true);
  const [showReading, setShowReading] = useState(true);
  const [showRomaji, setShowRomaji] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');
  const { isContentLocked } = useUserAccess();

  useEffect(() => {
    if (!lessonId) return;
    let cancelled = false;

    const loadLesson = async () => {
      try {
        setIsLoading(true);
        setError('');
        const data = await speakingApi.getLesson(lessonId);
        if (!cancelled) setDetail(data);
      } catch (err) {
        console.error(err);
        if (!cancelled) setError('Không tải được bài đọc.');
      } finally {
        if (!cancelled) setIsLoading(false);
      }
    };

    void loadLesson();
    return () => {
      cancelled = true;
    };
  }, [lessonId]);

  useEffect(() => {
    if (!courseCode) return;
    let cancelled = false;

    const loadLessons = async () => {
      try {
        const data = await speakingApi.getLessonsByCourse(courseCode);
        if (!cancelled) setLessons(data);
      } catch (err) {
        console.error(err);
      }
    };

    void loadLessons();
    return () => {
      cancelled = true;
    };
  }, [courseCode]);

  const currentLessonIndex = lessons.findIndex((lesson) => lesson.id === lessonId);
  const previousLesson = currentLessonIndex > 0 ? lessons[currentLessonIndex - 1] : null;
  const nextLesson = currentLessonIndex >= 0 && currentLessonIndex + 1 < lessons.length
    ? lessons[currentLessonIndex + 1]
    : null;

  const goToLesson = useCallback((targetLesson: SpeakingLesson | null) => {
    if (!courseCode || !targetLesson) return;
    navigate(`/speaking/${courseCode}/lessons/${targetLesson.id}`);
  }, [courseCode, navigate]);

  useEffect(() => {
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.target instanceof HTMLInputElement || event.target instanceof HTMLTextAreaElement) {
        return;
      }

      if (event.key === 'ArrowLeft' && previousLesson) {
        goToLesson(previousLesson);
      }

      if (event.key === 'ArrowRight' && nextLesson) {
        goToLesson(nextLesson);
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [goToLesson, nextLesson, previousLesson]);

  return (
    <div className="mx-auto max-w-6xl space-y-5 px-4 pb-20 pt-3 animate-fade-in">
      <header className="flex flex-col gap-4 border-b border-[#e8d9cf] pb-5 md:flex-row md:items-end md:justify-between">
        <div>
          <button
            type="button"
            onClick={() => navigate(`/speaking/${courseCode}`)}
            className="inline-flex items-center gap-2 text-sm font-extrabold text-[#8B3A22] transition hover:text-[#5f2716]"
          >
            <ArrowLeft size={18} />
            Quay lại {courseCode?.toUpperCase()}
          </button>
          <p className="mt-4 text-xs font-black uppercase tracking-[0.18em] text-text-muted">
            Lesson {detail?.lesson.lessonNumber ?? ''}
          </p>
          <h1 className="text-3xl font-black text-text-primary">
            {detail?.lesson.title ?? 'Luyện nói'}
          </h1>
        </div>

        <div className="flex flex-wrap gap-2">
          <button
            type="button"
            onClick={() => setShowMeaning((value) => !value)}
            className={`inline-flex h-11 items-center gap-2 rounded-full border-2 px-4 text-sm font-black shadow-pop transition-all ${
              showMeaning
                ? 'border-border-active bg-white text-slate-900'
                : 'border-slate-300 bg-slate-100 text-slate-500'
            }`}
          >
            <Languages size={17} />
            Nghĩa
          </button>
          <button
            type="button"
            onClick={() => setShowReading((value) => !value)}
            className={`inline-flex h-11 items-center gap-2 rounded-full border-2 px-4 text-sm font-black shadow-pop transition-all ${
              showReading
                ? 'border-border-active bg-[#2b160f] text-white'
                : 'border-slate-300 bg-slate-100 text-slate-500'
            }`}
          >
            <Eye size={17} />
            Cách đọc Kanji
          </button>
          <button
            type="button"
            onClick={() => setShowRomaji((value) => !value)}
            className={`inline-flex h-11 items-center gap-2 rounded-full border-2 px-4 text-sm font-black shadow-pop transition-all ${
              showRomaji
                ? 'border-border-active bg-blue-600 text-white'
                : 'border-slate-300 bg-slate-100 text-slate-500'
            }`}
          >
            <span className="text-xs">Abc</span>
            Romaji
          </button>
        </div>
      </header>

      {error ? (
        <div className="border-2 border-accent-danger bg-accent-danger/10 p-4 text-sm font-bold text-accent-danger">
          {error}
        </div>
      ) : null}

      {isLoading ? (
        <div className="flex h-64 flex-col items-center justify-center text-text-secondary">
          <Loader2 size={36} className="mb-4 animate-spin text-accent-primary" />
          <p className="font-bold">Đang tải bài đọc...</p>
        </div>
      ) : detail ? (
        <PremiumLock isLocked={isContentLocked(detail.lesson)} packageCode={detail.lesson.packageCode}>
        <section className="space-y-6">
          <div className="speaking-passage">
            {detail.sentences.map((sentence) => (
              <div key={sentence.id} className="speaking-passage-sentence">
                <span className="speaking-sentence-badge">Câu {sentence.sentenceNumber}</span>

                <div
                  className={`speaking-content ${showReading ? 'speaking-reading-on' : 'speaking-reading-hover'}`}
                  dangerouslySetInnerHTML={{ __html: sentence.contentHtml }}
                />

                <SpeakingSentenceAudioButton sentence={sentence} compact />

                {showRomaji && sentence.romaji ? (
                  <p className="speaking-romaji">{sentence.romaji}</p>
                ) : null}

                {showMeaning ? (
                  <p className="speaking-meaning">{sentence.meaningVi}</p>
                ) : null}
              </div>
            ))}
          </div>

          <div className="flex flex-col gap-3 border-t border-[#eadfd6] pt-6 sm:flex-row sm:items-center sm:justify-between">
            <button
              type="button"
              disabled={!previousLesson}
              onClick={() => goToLesson(previousLesson)}
              className="inline-flex h-12 items-center justify-center gap-2 rounded-full border-2 border-border bg-white px-5 text-sm font-black text-slate-900 shadow-card transition-all hover:-translate-y-0.5 hover:shadow-lift disabled:pointer-events-none disabled:opacity-40"
            >
              <ChevronLeft size={18} />
              Bài trước
            </button>

            <span className="text-center text-xs font-black uppercase tracking-[0.16em] text-text-muted">
              Dùng phím ← / → để chuyển bài
            </span>

            <button
              type="button"
              disabled={!nextLesson}
              onClick={() => goToLesson(nextLesson)}
              className="inline-flex h-12 items-center justify-center gap-2 rounded-full border-2 border-border bg-[#2b160f] px-5 text-sm font-black text-white shadow-card transition-all hover:-translate-y-0.5 hover:shadow-lift disabled:pointer-events-none disabled:opacity-40"
            >
              Bài sau
              <ChevronRight size={18} />
            </button>
          </div>
        </section>
        </PremiumLock>
      ) : null}
    </div>
  );
};

type SpeakingSentenceAudioButtonProps = {
  sentence: SpeakingSentence;
  compact?: boolean;
};

const canUseSpeech = () => typeof window !== 'undefined' && 'speechSynthesis' in window;

const SpeakingSentenceAudioButton = ({ sentence, compact = false }: SpeakingSentenceAudioButtonProps) => {
  const [isSpeaking, setIsSpeaking] = useState(false);
  const supported = canUseSpeech();
  const speechText = sentence.plainText.trim();

  useEffect(() => {
    return () => {
      if (canUseSpeech()) {
        window.speechSynthesis.cancel();
      }
    };
  }, [sentence.id]);

  const speak = useCallback(() => {
    if (!supported || !speechText) {
      return;
    }

    window.speechSynthesis.cancel();

    const utterance = new SpeechSynthesisUtterance(speechText);
    utterance.lang = 'ja-JP';
    utterance.rate = 0.82;
    utterance.pitch = 1;
    utterance.onstart = () => setIsSpeaking(true);
    utterance.onend = () => setIsSpeaking(false);
    utterance.onerror = () => setIsSpeaking(false);

    window.speechSynthesis.speak(utterance);
  }, [speechText, supported]);

  const stop = () => {
    if (!supported) {
      return;
    }

    window.speechSynthesis.cancel();
    setIsSpeaking(false);
  };

  return (
    <button
      type="button"
      onClick={isSpeaking ? stop : speak}
      disabled={!supported || !speechText}
      className={
        compact
          ? 'speaking-inline-audio inline-flex shrink-0 items-center justify-center rounded-full border-2 border-border bg-white text-[#8B3A22] shadow-pop transition-all hover:-translate-y-0.5 hover:bg-[#fff7ed] hover:shadow-lift disabled:pointer-events-none disabled:opacity-40'
          : 'inline-flex h-10 shrink-0 items-center justify-center gap-2 rounded-full border-2 border-border bg-white px-3 text-xs font-black text-[#8B3A22] shadow-pop transition-all hover:-translate-y-0.5 hover:bg-[#fff7ed] hover:shadow-lift disabled:pointer-events-none disabled:opacity-40'
      }
      aria-label={isSpeaking ? `Dừng đọc câu ${sentence.sentenceNumber}` : `Đọc câu ${sentence.sentenceNumber}`}
      title={supported ? 'Đọc câu tiếng Nhật' : 'Trình duyệt không hỗ trợ đọc tự động'}
    >
      {isSpeaking ? <VolumeX size={16} /> : <Volume2 size={16} />}
      {!compact ? <span>{isSpeaking ? 'Dừng' : 'Nghe câu'}</span> : null}
    </button>
  );
};
