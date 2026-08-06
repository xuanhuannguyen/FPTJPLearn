import { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import {
  BookOpen,
  ChevronRight,
  Lock,
  ArrowLeft,
  Star
} from 'lucide-react';
import { kanjiApi } from '../api/kanjiApi';
import type { KanjiLevel, KanjiLesson } from '../types/kanji.types';
import { useUserAccess } from '../../../shared/hooks/useUserAccess';

export const KanjiLevelPage = () => {
  const { level: paramLevel } = useParams<{ level: string }>();
  
  const level = paramLevel?.toLowerCase().startsWith('jpd')
    ? paramLevel.toLowerCase()
    : paramLevel as KanjiLevel;

  const [lessons, setLessons] = useState<KanjiLesson[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const { isContentLocked } = useUserAccess();

  useEffect(() => {
    const fetchLessons = async () => {
      if (!level) return;
      try {
        const data = await kanjiApi.getLessonsByLevel(level as KanjiLevel);
        setLessons(data);
      } catch (error) {
        console.error('Failed to fetch lessons:', error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchLessons();
  }, [level]);

  if (isLoading) {
    return (
      <div className="flex h-64 items-center justify-center">
        <div className="h-10 w-10 animate-spin border-4 border-accent-primary border-t-transparent"></div>
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-[1440px] px-4 py-4 animate-fade-in md:px-6 lg:px-8">
      <div className="mb-10 flex items-center justify-between gap-4">
        <div className="flex min-w-0 items-center gap-5">
          <Link
            to="/kanji"
            className="flex h-[58px] w-[58px] shrink-0 items-center justify-center rounded-lg border-2 border-blue-300 bg-white font-jp text-3xl font-black text-blue-600 shadow-[0_10px_24px_rgba(37,99,235,0.16)] transition-all hover:-translate-x-0.5 hover:-translate-y-0.5 hover:border-blue-500"
            aria-label="Quay lại danh sách Hán tự"
          >
            漢
          </Link>
          <div className="min-w-0">
            <h1 className="font-heading text-4xl font-black uppercase tracking-wide text-slate-900">
              Hán tự
            </h1>
            <p className="mt-2 text-lg font-bold text-slate-500">
              Học và luyện tập hán tự theo chủ đề
            </p>
          </div>
        </div>

        <Link
          to="/kanji"
          className="hidden h-10 items-center gap-2 rounded-full border border-blue-100 bg-white px-4 text-xs font-black uppercase tracking-wider text-blue-600 shadow-sm transition-colors hover:bg-blue-50 sm:inline-flex"
        >
          <ArrowLeft size={15} />
          {paramLevel?.toUpperCase()}
        </Link>
      </div>

      <div className="grid grid-cols-1 gap-7 lg:grid-cols-2">
        {lessons.map((lesson) => {
          const isLocked = isContentLocked(lesson);
          return (
          <Link
            key={lesson.id}
            to={isLocked ? '/pricing' : `/kanji/${paramLevel}/lessons/${lesson.id}`}
            className={`group relative flex min-h-[150px] items-center gap-7 overflow-hidden rounded-2xl border bg-white p-6 shadow-[0_18px_45px_rgba(15,23,42,0.08)] transition-all ${
              isLocked
                ? 'border-slate-100 opacity-70 grayscale-[0.2]'
                : 'border-blue-100 hover:-translate-y-0.5 hover:border-blue-300 hover:shadow-[0_24px_55px_rgba(37,99,235,0.14)]'
            }`}
          >
            <div className={`absolute left-0 top-0 h-full w-1.5 ${isLocked ? 'bg-slate-300' : 'bg-blue-500'}`} />

            <img
              src="/images/kanji/kanji-card.webp"
              alt=""
              className="h-24 w-24 shrink-0 rounded-2xl object-cover shadow-[0_12px_24px_rgba(37,99,235,0.18)]"
              loading="lazy"
              decoding="async"
            />

            <div className="min-w-0 flex-1">
              <span className="inline-flex h-8 items-center rounded-full bg-blue-50 px-4 text-sm font-black uppercase tracking-wide text-blue-600">
                  Lesson {lesson.lessonNumber}
              </span>

              <h2 className={`mt-4 line-clamp-2 font-heading text-2xl font-black leading-tight ${isLocked ? 'text-slate-500' : 'text-slate-950'}`}>
                {lesson.title}
              </h2>

              <div className="mt-4 flex flex-wrap items-center gap-3">
                <span className="inline-flex h-8 items-center gap-1.5 rounded-full bg-blue-50 px-3 text-sm font-black uppercase tracking-wide text-blue-600">
                  <BookOpen size={16} />
                  {lesson.kanjiCount ?? 0} Kanji
                </span>
                <span className="text-slate-300">•</span>
                <span className="inline-flex h-8 items-center gap-1.5 rounded-full bg-violet-50 px-3 text-sm font-black uppercase tracking-wide text-violet-600">
                  <Star size={16} />
                  {lesson.vocabularyCount ?? 0} Vocab
                </span>
              </div>
            </div>

            <span className={`flex h-14 w-14 shrink-0 items-center justify-center rounded-full transition-all ${
              isLocked
                ? 'bg-slate-100 text-slate-400'
                : 'bg-blue-50 text-blue-600 group-hover:bg-blue-600 group-hover:text-white group-hover:translate-x-1'
            }`}>
              {isLocked ? <Lock size={22} /> : <ChevronRight size={28} />}
            </span>
          </Link>
          );
        })}
      </div>
      
      {lessons.length === 0 && (
        <div className="rounded-2xl border border-dashed border-blue-200 bg-blue-50/40 p-8 text-center">
          <p className="text-sm font-bold text-slate-500">Chưa có bài học cho cấp độ này.</p>
        </div>
      )}
    </div>
  );
};
