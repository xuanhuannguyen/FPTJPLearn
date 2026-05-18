import { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { ArrowLeft, Loader2, Lock, Play, Volume2 } from 'lucide-react';
import { speakingApi } from '../api/speakingApi';
import type { SpeakingLesson } from '../types/speaking.types';
import { useUserAccess } from '../../../shared/hooks/useUserAccess';

export const SpeakingCoursePage = () => {
  const { courseCode } = useParams<{ courseCode: string }>();
  const navigate = useNavigate();
  const [lessons, setLessons] = useState<SpeakingLesson[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');
  const { isContentLocked } = useUserAccess();

  useEffect(() => {
    if (!courseCode) return;
    let cancelled = false;

    const loadLessons = async () => {
      try {
        setIsLoading(true);
        setError('');
        const data = await speakingApi.getLessonsByCourse(courseCode);
        if (!cancelled) setLessons(data);
      } catch (err) {
        console.error(err);
        if (!cancelled) setError('Không tải được bài đọc.');
      } finally {
        if (!cancelled) setIsLoading(false);
      }
    };

    void loadLessons();
    return () => {
      cancelled = true;
    };
  }, [courseCode]);

  return (
    <div className="mx-auto max-w-[1295px] space-y-6 px-4 pb-14 animate-fade-in md:px-6">
      <header className="space-y-4">
        <button
          onClick={() => navigate('/speaking')}
          className="group inline-flex items-center gap-2 text-sm font-black text-accent-primary transition-colors hover:text-accent-hover"
        >
          <ArrowLeft size={16} className="transition-transform group-hover:-translate-x-1" />
          Back to Speaking
        </button>

        <div className="py-2">
          <h1 className="text-[32px] font-black leading-none tracking-tight text-text-primary">
            Luyện nói {courseCode?.toUpperCase()}
          </h1>
          <p className="mt-1 text-sm font-bold uppercase tracking-widest text-text-secondary">
            Chọn một bài đọc để bắt đầu
          </p>
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
      ) : (
        <div className="grid grid-cols-1 gap-5 md:grid-cols-2 lg:grid-cols-3">
          {lessons.map((lesson) => {
            const isLocked = isContentLocked(lesson);
            return (
            <Link
              key={lesson.id}
              to={isLocked ? '/pricing' : `/speaking/${courseCode}/lessons/${lesson.id}`}
              className={`interactive-surface group relative flex min-h-[118px] cursor-pointer gap-4 overflow-hidden rounded-[18px] p-3.5 ${
                isLocked ? 'grayscale-[0.35]' : ''
              }`}
            >
              <div className="absolute left-0 top-0 h-full w-2 bg-[#8B3A22]" />

              <div className="relative flex h-24 w-[68px] shrink-0 items-center justify-center rounded-xl border-2 border-slate-900 bg-[#FFF7ED] text-[#8B3A22] shadow-[3px_3px_0_#111827]">
                <Volume2 size={28} />
              </div>

              <div className="relative flex min-w-0 flex-1 flex-col justify-center">
                <div className="flex items-start justify-between gap-3">
                  <div className="min-w-0 flex-1">
                    <p className="text-[11px] font-black uppercase tracking-[0.16em] text-text-muted">
                      Lesson {lesson.lessonNumber}
                    </p>
                    <h3 className="mt-0.5 line-clamp-1 font-heading text-xl font-black leading-none tracking-tight text-text-primary transition-colors group-hover:text-accent-primary">
                      {lesson.title}
                    </h3>

                    {lesson.description ? (
                      <p className="mt-1.5 line-clamp-2 text-xs font-bold leading-5 text-text-secondary">
                        {lesson.description}
                      </p>
                    ) : null}

                    <p className="mt-2 text-[11px] font-black uppercase tracking-[0.14em] text-[#8B3A22]">
                      {isLocked ? 'Cần kích hoạt gói Premium' : `${lesson.sentenceCount} câu đọc`}
                    </p>
                  </div>

                  {isLocked ? (
                    <span className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg border-2 border-border bg-bg-tertiary text-text-muted">
                      <Lock size={16} />
                    </span>
                  ) : (
                    <span className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg border-2 border-border bg-accent-cta text-white shadow-pop transition-all group-hover:-translate-y-0.5">
                      <Play size={16} className="ml-0.5 fill-current" />
                    </span>
                  )}
                </div>
              </div>
            </Link>
            );
          })}
        </div>
      )}
    </div>
  );
};
