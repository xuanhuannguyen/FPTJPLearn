import { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import {
  ArrowLeft,
  Check,
  ChevronRight,
  Loader2,
  Plus,
  Pencil,
} from 'lucide-react';
import { grammarApi } from '../api/grammarApi';
import { memoryApi } from '../../memory/api/memoryApi';
import type { GrammarLesson, GrammarPattern } from '../types/grammar.types';
import { PremiumLock } from '../../../shared/components/PremiumLock';
import { useUserAccess } from '../../../shared/hooks/useUserAccess';

export const GrammarLessonPage = () => {
  const { level, lessonId } = useParams();
  const navigate = useNavigate();
  const [lesson, setLesson] = useState<GrammarLesson | null>(null);
  const [patterns, setPatterns] = useState<GrammarPattern[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [addingId, setAddingId] = useState<string | null>(null);
  const [memoryPatternIds, setMemoryPatternIds] = useState<Set<string>>(new Set());
  const { isContentLocked } = useUserAccess();

  const loadLesson = async () => {
    if (!lessonId) return;
    const data = await grammarApi.getLessonById(lessonId);
    setLesson(data.lesson);
    setPatterns(data.patterns);

    const statuses = await Promise.allSettled(
      data.patterns.map(async (pattern) => {
        const status = await memoryApi.getGrammarPatternStatus(pattern.id);
        return { patternId: pattern.id, isInMemory: status.isInMemory };
      })
    );
    setMemoryPatternIds(new Set(statuses
      .filter((status): status is PromiseFulfilledResult<{ patternId: string; isInMemory: boolean }> => status.status === 'fulfilled')
      .filter((status) => status.value.isInMemory)
      .map((status) => status.value.patternId)));
  };

  useEffect(() => {
    const run = async () => {
      try {
        setIsLoading(true);
        setError(null);
        await loadLesson();
      } catch (err) {
        console.error(err);
        setError('Không thể tải dữ liệu bài học. Vui lòng thử lại sau.');
      } finally {
        setIsLoading(false);
      }
    };
    run();
  }, [lessonId]);

  const addToMemory = async (e: React.MouseEvent, patternId: string) => {
    e.preventDefault();
    e.stopPropagation();

    if (memoryPatternIds.has(patternId)) {
      navigate('/memory/grammar/review');
      return;
    }

    try {
      setAddingId(patternId);
      await memoryApi.addGrammarFromPattern(patternId);
      setMemoryPatternIds((prev) => new Set(prev).add(patternId));
    } finally {
      setAddingId(null);
    }
  };

  if (isLoading) {
    return (
      <div className="flex h-64 items-center justify-center">
        <div className="h-6 w-6 animate-spin rounded-full border-2 border-accent-primary border-t-transparent"></div>
      </div>
    );
  }

  if (error || (!isLoading && !lesson)) {
    return (
      <div className="max-w-5xl mx-auto px-4 py-20 text-center">
        <div className="inline-flex h-16 w-16 items-center justify-center rounded-full bg-accent-danger/10 text-accent-danger mb-4">
          <ArrowLeft size={32} />
        </div>
        <h2 className="text-2xl font-black text-text-primary mb-2">
          {error || 'Không tìm thấy bài học'}
        </h2>
        <p className="text-text-tertiary mb-8 font-bold">
          Dữ liệu bài học không tồn tại hoặc đã có lỗi xảy ra.
        </p>
        <button
          onClick={() => navigate(`/grammar/${level}`)}
          className="btn-primary px-8 py-3"
        >
          Quay lại danh sách
        </button>
      </div>
    );
  }

  if (!lesson) return null;

  return (
    <div className="max-w-5xl mx-auto px-4 pt-2 pb-6 animate-fade-in">
      {/* Navigation & Header */}
      <div className="mb-10 space-y-4">
        {/* Navigation */}
        <button
          onClick={() => navigate(`/grammar/${level}`)}
          className="flex items-center gap-2 text-sm font-bold text-accent-primary hover:opacity-70 transition-all active:scale-95"
        >
          <ArrowLeft size={16} />
          <span>{level?.toUpperCase()} – Bài {lesson.lessonNumber}</span>
        </button>

        {/* Header Content */}
        <div className="flex flex-col md:flex-row md:items-center justify-between gap-6">
          <div className="space-y-2">
            <h1 className="text-3xl md:text-4xl font-black text-text-primary tracking-tight">
              {lesson.title}
            </h1>
            <div className="relative inline-block">
              <p className="text-lg font-bold text-text-tertiary">
                {lesson.description || lesson.title}
              </p>
              <div className="absolute -bottom-1 left-0 h-1.5 w-full bg-amber-400/60 -z-10" />
            </div>
          </div>

        </div>
      </div>

      <PremiumLock isLocked={isContentLocked(lesson)} packageCode={lesson.packageCode}>
      {/* Patterns Grid - 2 Columns Compact Layout */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {patterns.map((pattern, idx) => (
          <Link
            key={pattern.id}
            to={`/grammar/patterns/${pattern.id}`}
            className={`group relative flex items-center justify-between gap-3 rounded-xl border-2 border-border bg-white p-3 shadow-sm transition-all duration-300 hover:-translate-y-0.5 hover:shadow-pop ${idx % 2 === 0 ? 'rotate-0.5' : '-rotate-0.5'} hover:rotate-0`}
          >
            {/* Compact Colored Tape Element */}
            <div className={`absolute left-1/2 top-0 h-3 w-10 -translate-x-1/2 -translate-y-1/2 rounded-sm ${idx % 4 === 0 ? 'bg-pink-300' : idx % 4 === 1 ? 'bg-yellow-300' : idx % 4 === 2 ? 'bg-emerald-300' : 'bg-violet-300'} border border-border/10 shadow-sm z-10`} />
            
            <div className="flex-1 min-w-0">
              <div className="flex items-center gap-1.5 mb-1">
                <h3 className="font-jp text-base md:text-lg font-black text-text-primary leading-tight truncate">
                  {pattern.pattern}
                </h3>
                <ChevronRight size={14} className="text-text-tertiary/30 group-hover:translate-x-0.5 group-hover:text-accent-primary transition-all" />
              </div>
              <p className="text-[10px] md:text-xs font-bold text-text-tertiary italic truncate">
                {pattern.meaning}
              </p>
            </div>

            <div className="flex items-center gap-1.5">
              <button
                onClick={(e) => {
                  e.preventDefault();
                  e.stopPropagation();
                  navigate(`/grammar/patterns/${pattern.id}/practice`);
                }}
                className="h-8 w-8 flex-shrink-0 flex items-center justify-center rounded-lg border-2 border-border transition-all active:scale-90 shadow-[2px_2px_0px_0px_rgba(0,0,0,1)] bg-amber-400 text-text-primary hover:brightness-110"
                title="Làm bài tập"
              >
                <Pencil size={16} strokeWidth={3} />
              </button>
              
              <button
                onClick={(e) => addToMemory(e, pattern.id)}
                disabled={addingId === pattern.id}
                className={`h-8 w-8 flex-shrink-0 flex items-center justify-center rounded-lg border-2 border-border transition-all active:scale-90 shadow-[2px_2px_0px_0px_rgba(0,0,0,1)] ${
                  memoryPatternIds.has(pattern.id) 
                    ? 'bg-emerald-500 text-white border-emerald-700' 
                    : 'bg-accent-primary text-white hover:brightness-110'
                }`}
                title={memoryPatternIds.has(pattern.id) ? 'Đã thêm vào Ghi nhớ' : 'Thêm vào Ghi nhớ'}
              >
                {addingId === pattern.id ? (
                  <Loader2 size={14} className="animate-spin" />
                ) : memoryPatternIds.has(pattern.id) ? (
                  <Check size={14} strokeWidth={4} />
                ) : (
                  <Plus size={14} strokeWidth={4} />
                )}
              </button>
            </div>
          </Link>
        ))}
      </div>
      </PremiumLock>
    </div>
  );
};
