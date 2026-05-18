import { useEffect, useMemo, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { ArrowLeft, Layers3, Loader2, Sparkles, Pencil, Calendar, MessageCircle, Info, Volume2, ArrowRight } from 'lucide-react';
import { grammarApi } from '../api/grammarApi';
import { memoryApi } from '../../memory/api/memoryApi';
import { GrammarStructure } from '../components/GrammarStructure';
import type { GrammarPattern } from '../types/grammar.types';
import type { MemoryGrammarStatus } from '../../memory/types/memory.types';
import { parseGrammarTags } from '../utils/grammarTags';

export const GrammarPatternDetailPage = () => {
  const { patternId } = useParams();
  const navigate = useNavigate();
  const [pattern, setPattern] = useState<GrammarPattern | null>(null);
  const [allPatterns, setAllPatterns] = useState<GrammarPattern[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isSavingStudy, setIsSavingStudy] = useState(false);
  const [error, setError] = useState('');
  const [memoryStatus, setMemoryStatus] = useState<MemoryGrammarStatus>({ isInMemory: false, isActive: false });

  const loadPattern = async () => {
    if (!patternId) {
      return;
    }

    const data = await grammarApi.getPatternById(patternId);
    setPattern(data);
    try {
      const status = await memoryApi.getGrammarPatternStatus(patternId);
      setMemoryStatus(status);
    } catch (error) {
      console.warn('Grammar memory status unavailable, using static fallback.', error);
      setMemoryStatus({ isInMemory: false, isActive: false });
    }

    // Fetch lesson patterns to find next/prev
    if (data.lessonId) {
      const lessonData = await grammarApi.getLessonById(data.lessonId);
      setAllPatterns(lessonData.patterns);
    }
  };

  useEffect(() => {
    let cancelled = false;

    const run = async () => {
      try {
        setIsLoading(true);
        setError('');
        await loadPattern();
      } catch (err) {
        console.error(err);
        if (!cancelled) {
          setError('Không tải được mẫu ngữ pháp này.');
        }
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    };

    void run();

    return () => {
      cancelled = true;
    };
  }, [patternId]);

  const nextPattern = useMemo(() => {
    if (!pattern || allPatterns.length === 0) return null;
    const currentIndex = allPatterns.findIndex(p => p.id === pattern.id);
    if (currentIndex === -1 || currentIndex === allPatterns.length - 1) return null;
    return allPatterns[currentIndex + 1];
  }, [pattern, allPatterns]);

  const tags = useMemo(() => parseGrammarTags(pattern?.tagsJson), [pattern?.tagsJson]);

  const addToMemory = async () => {
    if (!pattern) {
      return;
    }

    try {
      setIsSavingStudy(true);
      await memoryApi.addGrammarFromPattern(pattern.id);
      await loadPattern();
    } finally {
      setIsSavingStudy(false);
    }
  };

  const removeFromMemory = async () => {
    if (!memoryStatus.memoryItemId) {
      return;
    }

    try {
      setIsSavingStudy(true);
      await memoryApi.removeGrammarItem(memoryStatus.memoryItemId);
      await loadPattern();
    } finally {
      setIsSavingStudy(false);
    }
  };

  const handleSpeak = (text: string, rate: number = 1) => {
    // Cancel any ongoing speech
    window.speechSynthesis.cancel();

    const utterance = new SpeechSynthesisUtterance(text);
    utterance.lang = 'ja-JP';
    utterance.rate = rate;

    // Find a Japanese voice if available
    const voices = window.speechSynthesis.getVoices();
    const jaVoice = voices.find(v => v.lang.includes('ja'));
    if (jaVoice) utterance.voice = jaVoice;

    window.speechSynthesis.speak(utterance);
  };

  if (isLoading) {
    return (
      <div className="flex h-64 flex-col items-center justify-center text-text-secondary">
        <Loader2 size={36} className="mb-4 animate-spin text-accent-primary" />
        <p className="font-bold">Loading grammar pattern...</p>
      </div>
    );
  }

  if (error || !pattern) {
    return (
      <div className="space-y-4">
        <button onClick={() => navigate('/grammar')} className="btn-secondary">
          <ArrowLeft size={18} />
          Back
        </button>
        <div className="rounded-2xl border-2 border-accent-danger bg-accent-danger/10 p-5 font-bold text-accent-danger shadow-pop">
          {error || 'Pattern not found.'}
        </div>
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-7xl space-y-10 pb-20 animate-fade-in px-4 md:px-6">
      {/* Navigation & Header */}
      <div className="mb-12 space-y-4">
        {/* Navigation */}
        <button
          onClick={() => navigate(`/grammar/${pattern.level}/lessons/${pattern.lessonId}?course=${pattern.courseCode}`)}
          className="flex items-center gap-2 text-sm font-bold text-accent-primary hover:opacity-70 transition-all active:scale-95"
        >
          <ArrowLeft size={16} />
          <span>第{pattern.level} – Bài học</span>
        </button>

        {/* Header Content */}
        <div className="flex flex-col md:flex-row md:items-center justify-between gap-6">
          <div className="space-y-2">
            <h1 className="text-3xl md:text-4xl font-black text-text-primary tracking-tight font-jp">
              {pattern.pattern}
            </h1>
            {tags.length > 0 ? (
              <div className="flex flex-wrap gap-2">
                {tags.map((tag) => (
                  <span
                    key={tag}
                    className="rounded-full border border-border bg-white px-3 py-1 text-xs font-black uppercase tracking-wide text-accent-primary"
                  >
                    {tag}
                  </span>
                ))}
              </div>
            ) : null}
            <div className="relative inline-block">
              <p className="text-xl font-bold text-text-tertiary">
                {pattern.meaning}
              </p>
              <div className="absolute -bottom-1 left-0 h-1.5 w-full bg-amber-400/60 -z-10" />
            </div>
          </div>

          <div className="flex flex-wrap items-center gap-6">
            <button
              onClick={memoryStatus.isInMemory ? removeFromMemory : addToMemory}
              disabled={isSavingStudy}
              className="flex items-center gap-2 text-xs font-bold text-text-muted/60 hover:text-accent-primary transition-all"
            >
              {isSavingStudy ? <Loader2 size={16} className="animate-spin" /> : <Calendar size={16} />}
              <span>{memoryStatus.isInMemory ? 'Đã thêm vào ghi nhớ' : 'Thêm vào ghi nhớ'}</span>
            </button>
            {memoryStatus.isInMemory ? (
              <button
                type="button"
                onClick={() => navigate('/memory/grammar/review')}
                className="flex items-center gap-2 text-xs font-bold text-accent-primary transition-all hover:opacity-70"
              >
                <ArrowRight size={16} />
                <span>Ôn trong Ghi nhớ</span>
              </button>
            ) : null}
            <button
              onClick={() => navigate(`/grammar/patterns/${pattern.id}/practice`)}
              className="flex items-center gap-2 rounded-xl bg-accent-primary px-6 py-3 text-sm font-black text-white shadow-sm transition-all hover:bg-accent-hover hover:-translate-y-0.5 active:scale-95"
            >
              <Pencil size={18} />
              <span>Làm bài tập</span>
            </button>
          </div>
        </div>
      </div>

      {/* Detail Cards Grid */}
      <section className="grid gap-6 md:grid-cols-2">
        <div className="group relative rounded-[24px] border-4 border-border bg-white p-6 shadow-[2px_2px_0px_0px_rgba(0,0,0,1)] transition-all">
          <div className="absolute left-1/2 top-0 h-4 w-16 -translate-x-1/2 -translate-y-1/2 rounded bg-violet-300 border-2 border-border shadow-sm" />
          <div className="mb-4 flex items-center gap-3">
            <Layers3 size={20} className="text-violet-500" />
            <h3 className="text-lg font-black uppercase tracking-tighter">Cấu trúc</h3>
          </div>
          <div className="mb-4">
            <GrammarStructure structure={pattern.structure} />
          </div>
          <div className="border-t-2 border-border/10 pt-3">
            <GrammarStructure structure={pattern.formation} small />
          </div>
        </div>

        <div className="group relative rounded-[24px] border-4 border-border bg-[#1a1a1a] p-6 shadow-[2px_2px_0px_0px_rgba(0,0,0,1)] transition-all">
          <div className="absolute left-1/2 top-0 h-4 w-16 -translate-x-1/2 -translate-y-1/2 rounded bg-sky-300 border-2 border-border shadow-sm" />
          <div className="mb-4 flex items-center gap-3">
            <MessageCircle size={20} className="text-sky-400" />
            <h3 className="text-lg font-black uppercase tracking-tighter text-white">Giải nghĩa</h3>
          </div>
          <div className="space-y-3">
            <p className="text-xl font-black leading-relaxed text-white">
              {pattern.meaning}
            </p>
            <p className="text-sm font-bold leading-relaxed text-slate-200">
              {pattern.title}
            </p>
          </div>
        </div>

        <div className="group relative rounded-[24px] border-4 border-border bg-white p-6 shadow-[2px_2px_0px_0px_rgba(0,0,0,1)] transition-all">
          <div className="absolute left-1/2 top-0 h-4 w-16 -translate-x-1/2 -translate-y-1/2 rounded bg-emerald-300 border-2 border-border shadow-sm" />
          <div className="mb-4 flex items-center gap-3">
            <Sparkles size={20} className="text-emerald-500" />
            <h3 className="text-lg font-black uppercase tracking-tighter">Phạm vi sử dụng</h3>
          </div>
          <p className="text-sm font-bold leading-relaxed text-text-secondary">
            {pattern.usageScope}
          </p>
        </div>

        <div className="group relative rounded-[24px] border-4 border-border bg-white p-6 shadow-[2px_2px_0px_0px_rgba(0,0,0,1)] transition-all">
          <div className="absolute left-1/2 top-0 h-4 w-16 -translate-x-1/2 -translate-y-1/2 rounded bg-amber-200 border-2 border-border shadow-sm" />
          <div className="mb-4 flex items-center gap-3">
            <Info size={20} className="text-amber-500" />
            <h3 className="text-lg font-black uppercase tracking-tighter">Lưu ý</h3>
          </div>
          <p className="text-sm font-bold leading-relaxed text-text-primary">
            {pattern.notes || "Không có lưu ý đặc biệt cho mẫu ngữ pháp này."}
          </p>
        </div>
      </section>

      <section className="space-y-4">
        <div className="flex items-center gap-2 px-1">
          <div className="h-4 w-1.5 rounded-full bg-accent-primary shadow-pop" />
          <h3 className="font-heading text-xl font-black text-text-primary uppercase tracking-wider">Ví dụ</h3>
        </div>
        <div className="grid gap-3 max-w-[85%] mx-auto">
          {(pattern.examples ?? []).map((example, index) => (
            <div 
              key={index} 
              className="group relative rounded-[16px] border-4 border-border bg-white p-4 shadow-[2px_2px_0px_0px_rgba(0,0,0,1)] transition-all animate-fade-in"
              style={{ animationDelay: `${index * 0.1}s` }}
            >
              {/* Tape/Tab */}
              <div className={`absolute left-1/2 top-0 h-3 w-10 -translate-x-1/2 -translate-y-1/2 rounded ${index % 3 === 0 ? 'bg-pink-300' : index % 3 === 1 ? 'bg-yellow-300' : 'bg-emerald-300'} border-2 border-border shadow-sm`} />
              
              <div className="flex items-center justify-between gap-4">
                {/* 1. Left Column: Japanese & Reading */}
                <div className="w-[35%] shrink-0 space-y-0.5">
                  <p className="font-jp text-2xl font-black text-text-primary tracking-tight leading-tight">
                    {example.japanese}
                  </p>
                  {example.reading && (
                    <p className="font-jp text-[13px] font-bold text-text-muted/40">
                      {example.reading}
                    </p>
                  )}
                </div>

                {/* 2. Middle Column: Meaning (Centered) */}
                <div className="flex-1 text-center">
                  <p className="text-base font-bold text-text-secondary leading-snug">
                    {example.meaning}
                  </p>
                  {example.note && (
                    <p className="mt-0.5 text-[8px] font-bold text-text-tertiary italic">
                      {example.note}
                    </p>
                  )}
                </div>

                {/* 3. Right Column: Audio Buttons (Small) */}
                <div className="flex flex-col gap-1.5 shrink-0">
                  <button 
                    onClick={() => handleSpeak(example.japanese, 1)}
                    className="flex items-center justify-center gap-1.5 px-2 py-1 rounded-lg bg-slate-50 border border-border/10 transition-all text-text-muted hover:text-text-primary hover:border-border/20 active:scale-95 shadow-sm"
                    title="Đọc tốc độ bình thường"
                  >
                    <span className="text-[9px] font-black">1x</span>
                    <Volume2 size={12} />
                  </button>
                  <button 
                    onClick={() => handleSpeak(example.japanese, 0.7)}
                    className="flex items-center justify-center gap-1.5 px-2 py-1 rounded-lg bg-slate-50 border border-border/10 transition-all text-text-muted hover:text-text-primary hover:border-border/20 active:scale-95 shadow-sm"
                    title="Đọc chậm"
                  >
                    <span className="text-[9px] font-black">2x</span>
                    <Volume2 size={12} />
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      </section>

      {nextPattern && (
        <div className="mt-12 flex justify-end">
          <Link 
            to={`/grammar/patterns/${nextPattern.id}`}
            className="group flex flex-col items-end gap-1 text-right transition-all hover:-translate-x-1"
          >
            <span className="text-[10px] font-black uppercase tracking-widest text-orange-400">Mẫu tiếp</span>
            <div className="flex items-center gap-3">
              <span className="font-jp text-lg font-black text-orange-500 tracking-tight leading-tight group-hover:text-orange-600 transition-colors">
                {nextPattern.pattern}
              </span>
              <div className="flex h-10 w-10 items-center justify-center rounded-full bg-orange-50 border-2 border-orange-200 text-orange-500 shadow-sm group-hover:bg-orange-500 group-hover:text-white group-hover:border-orange-500 transition-all">
                <ArrowRight size={20} />
              </div>
            </div>
          </Link>
        </div>
      )}

    </div>
  );
};
