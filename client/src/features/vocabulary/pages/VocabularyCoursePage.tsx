import { useState, useEffect } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { ArrowLeft, BookOpen, CheckCircle, Clock3, Lock, Play } from 'lucide-react';
import { staticVocabularyApi } from '../api/vocabularyApi';
import type { StaticVocabularyLesson } from '../types/vocabulary.types';
import { useUserAccess } from '../../../shared/hooks/useUserAccess';

export const VocabularyCoursePage = () => {
  const { courseCode } = useParams<{ courseCode: string }>();
  const navigate = useNavigate();
  const [lessons, setLessons] = useState<StaticVocabularyLesson[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const { isContentLocked } = useUserAccess();

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

      <div className="grid grid-cols-1 gap-4 md:grid-cols-2 xl:grid-cols-3">
        {lessons.map((lesson) => {
          const lessonLabel = lesson.title.split(':')[0]?.trim() || String(lesson.lessonNumber);
          const isLocked = isContentLocked(lesson);
          const progressPercent = lesson.wordCount > 0
            ? Math.round((lesson.learnedCount / lesson.wordCount) * 100)
            : 0;
          const isComplete = lesson.wordCount > 0 && progressPercent === 100;

          return (
            <Link
              key={lesson.id}
              to={isLocked ? '/pricing' : `/vocabulary/${courseCode}/lessons/${lesson.id}`}
              className={`group relative flex min-h-[86px] cursor-pointer gap-3 overflow-hidden rounded-[14px] border-2 border-slate-900 bg-white p-2.5 shadow-[4px_4px_0_#111827] transition-all hover:translate-x-[1px] hover:translate-y-[1px] hover:shadow-[2px_2px_0_#111827] ${
                isLocked ? 'opacity-70 grayscale-[0.25]' : ''
              }`}
            >
              <div className={`absolute left-0 top-0 h-full w-1.5 ${isComplete ? 'bg-accent-success' : 'bg-blue-500'}`} />

              <img
                src="/images/vocabulary/vocabulary-card.webp?v=2"
                alt=""
                className="h-[66px] w-[48px] shrink-0 rounded-lg border-2 border-slate-900 object-cover shadow-[2px_2px_0_#111827]"
                loading="lazy"
                decoding="async"
              />

              <div className="relative flex min-w-0 flex-1 flex-col justify-between py-0.5">
                <div className="flex min-w-0 items-start justify-between gap-2">
                  <div className="min-w-0">
                    <h3 className="line-clamp-1 font-heading text-base font-black leading-tight text-slate-950 transition-colors group-hover:text-blue-600">
                      {lesson.title || `Lesson ${lessonLabel}`}
                    </h3>
                    <div className="mt-1.5 flex flex-wrap items-center gap-1.5">
                      <span className="inline-flex h-5 items-center gap-1 rounded-full border border-slate-200 bg-slate-50 px-2 text-[10px] font-black text-slate-600">
                        <BookOpen size={11} />
                        {lesson.wordCount}
                      </span>
                      <span className="inline-flex h-5 items-center gap-1 rounded-full border border-sky-100 bg-sky-50 px-2 text-[10px] font-black text-sky-600">
                        <CheckCircle size={11} />
                        {lesson.learnedCount}
                      </span>
                      <span className="inline-flex h-5 items-center gap-1 rounded-full border border-orange-100 bg-orange-50 px-2 text-[10px] font-black text-orange-500">
                        <Clock3 size={11} />
                        {Math.max(0, lesson.wordCount - lesson.learnedCount)}
                      </span>
                    </div>
                  </div>

                  <div className="shrink-0">
                    {isLocked ? (
                      <span className="flex h-8 w-8 items-center justify-center rounded-lg border-2 border-slate-900 bg-slate-100 text-slate-500 shadow-[2px_2px_0_#111827]">
                        <Lock size={15} />
                      </span>
                    ) : (
                      <span className="flex h-8 w-8 items-center justify-center rounded-lg border-2 border-slate-900 bg-orange-600 text-white shadow-[2px_2px_0_#111827] transition-all group-hover:bg-orange-500">
                        <Play size={14} className="ml-0.5 fill-current" />
                      </span>
                    )}
                  </div>
                </div>

                <div className="mt-2 flex items-center gap-2">
                  <div className="h-1.5 flex-1 overflow-hidden rounded-full border border-slate-900 bg-blue-50">
                    <div
                      className={`h-full rounded-full ${isComplete ? 'bg-accent-success' : 'bg-blue-600'}`}
                      style={{ width: `${progressPercent}%` }}
                    />
                  </div>
                  <span className="w-7 text-right text-[10px] font-black text-slate-900">{progressPercent}%</span>
                </div>
              </div>
            </Link>
          );
        })}
      </div>
    </div>
  );
};
