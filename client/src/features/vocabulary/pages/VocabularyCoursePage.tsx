import { useState, useEffect } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { ArrowLeft, CheckCircle, Play } from 'lucide-react';
import { staticVocabularyApi } from '../api/vocabularyApi';
import type { StaticVocabularyLesson } from '../types/vocabulary.types';

export const VocabularyCoursePage = () => {
  const { courseCode } = useParams<{ courseCode: string }>();
  const navigate = useNavigate();
  const [lessons, setLessons] = useState<StaticVocabularyLesson[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchLessons = async () => {
      if (!courseCode) return;
      try {
        const data = await staticVocabularyApi.getLessonsByCourse(courseCode);
        setLessons(data);
      } catch (error) {
        console.error('Failed to fetch lessons:', error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchLessons();
  }, [courseCode]);

  if (isLoading) {
    return (
      <div className="flex h-64 items-center justify-center">
        <div className="h-10 w-10 animate-spin rounded-full border-4 border-accent-primary border-t-transparent"></div>
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-[1295px] space-y-6 px-4 pb-14 animate-fade-in md:px-6">
      <header className="space-y-4">
        <button
          onClick={() => navigate('/vocabulary')}
          className="group inline-flex items-center gap-2 text-sm font-black text-accent-primary transition-colors hover:text-accent-hover"
        >
          <ArrowLeft size={16} className="transition-transform group-hover:-translate-x-1" />
          Back to Dashboard
        </button>

        <div className="py-2">
          <h1 className="text-[32px] font-black leading-none tracking-tight text-text-primary">
            Course {courseCode}
          </h1>
          <p className="mt-1 text-sm font-bold text-text-secondary uppercase tracking-widest">
            Select a lesson to begin
          </p>
        </div>
      </header>

      <div className="grid grid-cols-1 gap-5 md:grid-cols-2 lg:grid-cols-3">
        {lessons.map((lesson) => {
          const progressPercent = lesson.wordCount > 0
            ? Math.round((lesson.learnedCount / lesson.wordCount) * 100)
            : 0;
          const isComplete = lesson.wordCount > 0 && progressPercent === 100;

          return (
            <Link
              key={lesson.id}
              to={lesson.isLocked ? '/pricing' : `/vocabulary/${courseCode}/lessons/${lesson.id}`}
              className="interactive-surface group relative flex min-h-[110px] cursor-pointer gap-4 overflow-hidden rounded-[18px] p-3.5"
            >
              <div className={`absolute left-0 top-0 h-full w-2 ${isComplete ? 'bg-accent-success' : 'bg-accent-primary'}`} />

              <div className="book-cover relative flex h-24 w-[68px] shrink-0 items-center justify-center rounded-xl">
                <div className="flex h-10 w-10 items-center justify-center rounded-full bg-white text-center font-heading text-[7px] font-black leading-none text-text-primary">
                  TỪ<br />VỰNG
                </div>
              </div>

              <div className="relative flex min-w-0 flex-1 flex-col justify-center">
                <div className="flex items-start justify-between gap-3">
                  <div className="min-w-0 flex-1">
                    <p className="text-[11px] font-black uppercase tracking-[0.16em] text-text-muted">
                      Lesson {lesson.lessonNumber}
                    </p>
                    <div className="mt-0.5 flex items-center gap-2">
                      <h3 className="line-clamp-1 font-heading text-xl font-black leading-none tracking-tight text-text-primary transition-colors group-hover:text-accent-primary">
                        {lesson.title}
                      </h3>
                      {isComplete && <CheckCircle className="shrink-0 text-accent-success" size={18} />}
                    </div>

                    {lesson.description && (
                      <p className="mt-1.5 line-clamp-2 text-xs font-bold leading-5 text-text-secondary">
                        {lesson.description}
                      </p>
                    )}
                  </div>

                  <div className="flex shrink-0 items-center gap-1">
                    {lesson.isLocked ? (
                      <span className="rounded-lg border border-border bg-bg-tertiary px-2 py-1 text-[10px] font-black uppercase text-text-muted">
                        Locked
                      </span>
                    ) : (
                      <span className="flex h-10 w-10 items-center justify-center rounded-lg border-2 border-border bg-accent-cta text-white shadow-pop transition-all group-hover:-translate-y-0.5">
                        <Play size={16} className="ml-0.5 fill-current" />
                      </span>
                    )}
                  </div>
                </div>
              </div>
            </Link>
          );
        })}
      </div>
    </div>
  );
};
