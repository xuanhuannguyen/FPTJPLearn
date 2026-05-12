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

export const GrammarLevelPage = () => {
  const { level } = useParams<{ level: string }>();
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const courseCode = queryParams.get('course');
  
  const [lessons, setLessons] = useState<GrammarLesson[]>([]);
  const [isLoading, setIsLoading] = useState(true);

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
      <div className="grid grid-cols-1 gap-3 md:grid-cols-2">
        {lessons.map((lesson) => (
          <Link
            key={lesson.id}
            to={lesson.isLocked ? '#' : `/grammar/${level}/lessons/${lesson.id}${courseCode ? `?course=${courseCode}` : ''}`}
            className={`group relative flex min-h-[58px] items-center gap-3 rounded-xl border bg-white px-4 py-2.5 transition-colors duration-200 ${
              lesson.isLocked 
                ? 'border-border/5 bg-slate-50/50 cursor-not-allowed opacity-60'
                : 'border-border/10 bg-white shadow-sm hover:bg-white/90'
            }`}
          >
            {/* Icon */}
            <div className={`flex h-8 w-8 flex-shrink-0 items-center justify-center rounded-lg border ${
              lesson.isLocked ? 'border-border/5 bg-slate-100 text-slate-400' : 'border-border/10 bg-blue-50 text-accent-primary'
            }`}>
              {lesson.isLocked ? <Lock size={15} /> : <BookText size={15} />}
            </div>

            {/* Content */}
            <div className="flex-1 min-w-0">
              <div className="flex items-center justify-between gap-1">
                <span className="text-[10px] font-black uppercase tracking-wider text-text-tertiary">
                  第{lesson.lessonNumber}課
                </span>
                <div className="flex items-center gap-0.5 text-text-muted">
                  <span className="text-[11px] font-bold">{lesson.patternCount} mẫu</span>
                  {!lesson.isLocked && <ChevronRight size={14} className="transition-transform group-hover:translate-x-0.5" />}
                </div>
              </div>
              <h3 className={`truncate text-base font-black leading-tight ${lesson.isLocked ? 'text-slate-500' : 'text-text-primary'}`}>
                Bài {lesson.lessonNumber}: {lesson.title}
              </h3>
            </div>

            {/* Premium Tag if locked */}
            {lesson.isLocked && (
              <div className="absolute bottom-0.5 right-1">
                <span className="text-[7px] font-black uppercase bg-slate-200 text-slate-500 px-1 py-0 rounded-full">Pro</span>
              </div>
            )}
          </Link>
        ))}
      </div>
    </div>
  );
};
