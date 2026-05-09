import { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import {
  BookOpen,
  ChevronRight,
  Lock,
  ArrowLeft
} from 'lucide-react';
import { kanjiApi } from '../api/kanjiApi';
import type { KanjiLevel, KanjiLesson } from '../types/kanji.types';

export const KanjiLevelPage = () => {
  const { level } = useParams<{ level: string }>();
  const [lessons, setLessons] = useState<KanjiLesson[]>([]);
  const [isLoading, setIsLoading] = useState(true);

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
        <div className="h-10 w-10 animate-spin border-4 border-black border-t-transparent"></div>
      </div>
    );
  }

  return (
    <div className="max-w-7xl mx-auto px-4 py-2 animate-fade-in">
      {/* Brutalist Tiny Header */}
      <div className="mb-4 flex items-center gap-3">
        <Link 
          to="/kanji" 
          className="flex h-8 w-8 items-center justify-center border border-black bg-white hover:bg-black hover:text-white transition-colors shadow-[2px_2px_0px_#0F172A]"
        >
          <ArrowLeft size={16} />
        </Link>
        <span className="border border-black bg-accent-primary px-4 py-1 text-2xl font-black uppercase tracking-tight text-white shadow-[2px_2px_0px_#0F172A]">
          {level}
        </span>
        <span className="border border-black bg-white px-3 py-1.5 text-xs font-black uppercase text-text-primary shadow-[2px_2px_0px_#0F172A]">
          {lessons.length} LESSONS
        </span>
      </div>

      {/* Dense Lessons Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
        {lessons.map((lesson) => (
          <Link
            key={lesson.id}
            to={lesson.isLocked ? '#' : `/kanji/${level}/lessons/${lesson.id}`}
            className={`group flex items-center justify-between border border-black p-3 transition-all ${
              lesson.isLocked 
                ? 'bg-slate-50 cursor-not-allowed opacity-60'
                : 'bg-white hover:bg-slate-50 hover:border-accent-primary shadow-[2px_2px_0px_#0F172A] hover:shadow-[1px_1px_0px_#0F172A] hover:translate-x-[1px] hover:translate-y-[1px]'
            }`}
          >
            <div className="flex items-center gap-3">
              <div className={`flex h-10 w-10 shrink-0 items-center justify-center border border-black ${
                lesson.isLocked ? 'bg-slate-200' : 'bg-blue-50 text-accent-primary'
              }`}>
                {lesson.isLocked ? <Lock size={16} /> : <BookOpen size={16} />}
              </div>
              
              <div className="flex flex-col">
                <span className="text-[10px] font-black uppercase font-mono text-text-tertiary">
                  Lesson {lesson.lessonNumber}
                </span>
                <span className={`text-base font-bold leading-tight ${lesson.isLocked ? 'text-text-secondary' : 'text-text-primary'}`}>
                  {lesson.title}
                </span>
              </div>
            </div>

            <div className="flex flex-col items-end gap-1">
              {lesson.isLocked ? (
                <span className="border border-black bg-slate-200 px-2 py-0.5 text-[10px] font-black uppercase">
                  PRO
                </span>
              ) : (
                <div className="flex items-center gap-2">
                  <div className="flex flex-col items-end">
                    <span className="text-[10px] font-mono text-text-secondary">{lesson.kanjiCount} Kanji</span>
                    <span className="text-[10px] font-mono text-text-secondary">{lesson.vocabularyCount} Vocab</span>
                  </div>
                  <ChevronRight size={16} className="text-text-tertiary group-hover:text-accent-primary transition-transform group-hover:translate-x-1" />
                </div>
              )}
            </div>
          </Link>
        ))}
      </div>
      
      {lessons.length === 0 && (
        <div className="border border-black border-dashed p-8 text-center bg-slate-50">
          <p className="text-sm font-mono text-text-secondary">No lessons available for this level yet.</p>
        </div>
      )}
    </div>
  );
};
