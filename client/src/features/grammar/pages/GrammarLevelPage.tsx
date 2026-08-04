import { useState, useEffect } from 'react';
import { useParams, Link, useLocation } from 'react-router-dom';
import {
  BookText,
  ChevronRight,
  Lock,
  ArrowLeft
} from 'lucide-react';
import { grammarApi } from '../api/grammarApi';
import type { GrammarLevel, GrammarLesson } from '../types/grammar.types';
import { useUserAccess } from '../../../shared/hooks/useUserAccess';

export const GrammarLevelPage = () => {
  const { level: paramLevel } = useParams<{ level: string }>();
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  
  // Logic: if paramLevel starts with 'jpd', it's a course level (N5)
  const isCourseLevel = paramLevel?.toLowerCase().startsWith('jpd');
  const level = isCourseLevel ? 'N5' : paramLevel;
  const courseCode = isCourseLevel ? paramLevel : queryParams.get('course');
  
  const [lessons, setLessons] = useState<GrammarLesson[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const { isContentLocked } = useUserAccess();

  useEffect(() => {
    const fetchLessons = async () => {
      if (!level) return;
      try {
        const data = await grammarApi.getLessonsByLevel(level as GrammarLevel, courseCode || undefined);
        setLessons(data);
      } catch (error) {
        console.error('Failed to fetch lessons:', error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchLessons();
  }, [level, courseCode]);

  if (isLoading) {
    return (
      <div className="flex h-64 items-center justify-center">
        <div className="h-10 w-10 animate-spin rounded-full border-4 border-accent-primary border-t-transparent"></div>
      </div>
    );
  }

  return (
    <div className="max-w-7xl mx-auto px-4 py-2 animate-fade-in">
      {/* Tiny Header */}
      <div className="mb-3 flex items-center gap-2">
        <Link 
          to="/grammar" 
          className="flex h-7 w-7 items-center justify-center rounded-full bg-white border border-border/10 text-text-secondary hover:bg-bg-tertiary hover:text-text-primary transition-all shadow-sm"
        >
          <ArrowLeft size={14} />
        </Link>
        <span className="rounded-2xl border border-emerald-100 bg-emerald-50 px-5 py-2 text-[32px] font-black uppercase leading-none text-emerald-600 shadow-sm">
          {courseCode ? courseCode.toUpperCase() : level}
        </span>
        <span className="rounded-xl border border-border/10 bg-white px-3 py-1.5 text-sm font-black leading-none text-text-secondary shadow-sm">
          {lessons.length} bài học
        </span>
      </div>

      {/* Lessons Grid */}
      <div className="mx-auto max-w-4xl grid grid-cols-1 gap-4">
        {lessons.map((lesson) => {
          const isLocked = isContentLocked(lesson);
          return (
          <Link
            key={lesson.id}
            to={isLocked ? '/pricing' : `/grammar/${paramLevel}/lessons/${lesson.id}`}
            className={`group relative flex min-h-[150px] flex-col gap-4 overflow-hidden rounded-3xl border bg-white/90 p-5 backdrop-blur-sm transition-all duration-200 cursor-pointer sm:flex-row sm:items-center ${
              isLocked
                ? 'border-border/5 bg-slate-50/60 opacity-70'
                : 'border-violet-100 bg-white/90 shadow-[0_8px_24px_-12px_rgba(124,58,237,0.18)] hover:-translate-y-0.5 hover:border-violet-200 hover:shadow-[0_14px_34px_-12px_rgba(124,58,237,0.28)]'
            }`}
          >
            {/* Decorative wave + sakura */}
            <span aria-hidden="true" className="grammar-card-wave" />
            <span aria-hidden="true" className={`grammar-card-sakura top-4 right-10 opacity-60 ${isLocked ? 'hidden' : ''}`} />

            {/* Icon block */}
            <div className={`relative z-10 flex h-20 w-20 shrink-0 items-center justify-center rounded-2xl transition-transform duration-200 group-hover:scale-105 ${
              isLocked
                ? 'bg-slate-200 text-slate-500'
                : 'bg-gradient-to-br from-violet-500 to-indigo-600 text-white shadow-lg shadow-violet-200'
            }`}>
              {isLocked ? <Lock size={28} /> : <BookText size={30} />}
            </div>

            {/* Content */}
            <div className="relative z-10 min-w-0 flex-1">
              <span className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-[9px] font-black uppercase tracking-[0.18em] ${
                isLocked ? 'bg-slate-200 text-slate-500' : 'bg-violet-100 text-violet-700'
              }`}>
                NGỮ PHÁP
              </span>
              <h3 className={`mt-2 line-clamp-2 text-lg font-black leading-snug sm:text-xl ${
                isLocked ? 'text-slate-500' : 'text-text-primary'
              }`}>
                Bài {lesson.lessonNumber}: {lesson.title}
              </h3>
            </div>

            {/* Right meta */}
            <div className="relative z-10 flex shrink-0 items-center gap-3">
              <div className={`flex flex-col items-end rounded-2xl px-4 py-2 ${
                isLocked ? 'bg-slate-100' : 'border border-violet-100 bg-violet-50'
              }`}>
                <span className={`text-lg font-black leading-none ${isLocked ? 'text-slate-500' : 'text-violet-700'}`}>
                  {lesson.patternCount} mẫu
                </span>
                <span className="mt-1 text-[9px] font-bold uppercase tracking-wider text-slate-400">
                  mẫu ngữ pháp
                </span>
              </div>
              {isLocked ? (
                <span className="rounded-full bg-slate-200 px-2 py-0.5 text-[9px] font-black uppercase tracking-wider text-slate-500">
                  Pro
                </span>
              ) : (
                <ChevronRight size={22} className="text-violet-500 transition-transform group-hover:translate-x-1" />
              )}
            </div>
          </Link>
          );
        })}
      </div>
    </div>
  );
};
