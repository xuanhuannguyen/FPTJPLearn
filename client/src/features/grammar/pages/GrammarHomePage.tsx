import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { grammarApi } from '../api/grammarApi';
import type { GrammarLevelSummary, GrammarLevel } from '../types/grammar.types';

const courses: { courseCode: string; level: GrammarLevel; title: string; jpTitle: string; subtitle: string }[] = [
  { 
    courseCode: 'jpd113',
    level: 'N5', 
    jpTitle: 'JPD113 - Japanese 1',
    title: 'JPD113', 
    subtitle: 'Minna no Nihongo Sơ cấp 1 – Ngữ pháp (N5)'
  },
  { 
    courseCode: 'jpd123',
    level: 'N5', 
    jpTitle: 'JPD123 - Japanese 2',
    title: 'JPD123', 
    subtitle: 'N5 Nâng cao – Ngữ pháp (N5)'
  }
];

export const GrammarHomePage = () => {
  const [summaries, setSummaries] = useState<GrammarLevelSummary[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchSummaries = async () => {
      try {
        const data = await grammarApi.getLevels();
        setSummaries(data);
      } catch (error) {
        console.error('Failed to fetch grammar levels:', error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchSummaries();
  }, []);

  return (
    <div className="max-w-7xl mx-auto px-4 py-2 animate-fade-in">
      {/* Header */}
      <div className="mb-4">
        <h1 className="text-[40px] font-black uppercase leading-none tracking-tight text-text-primary font-mono flex items-center gap-2">
          Ngữ pháp <span className="text-blue-600">文法</span>
        </h1>
        <p className="text-sm font-bold text-text-secondary uppercase tracking-widest mt-1">
          JPD113 / JPD123 GRAMMAR MASTERY
        </p>
      </div>

      {/* Legend / Abbreviations */}
      <section className="mb-8 rounded-2xl border-2 border-border/10 bg-white/50 p-6 shadow-sm">
        <h4 className="text-[10px] font-black uppercase text-text-muted mb-4 tracking-widest px-1">Ký hiệu viết tắt trong cấu trúc</h4>
        <div className="flex flex-wrap gap-x-8 gap-y-4">
          <div className="flex items-center gap-3">
            <span className="inline-flex items-center justify-center min-w-[28px] h-7 px-2 rounded-md border font-black text-xs bg-blue-50 text-blue-600 border-blue-200 shadow-sm">N</span>
            <span className="text-xs font-bold text-text-tertiary">Danh từ (名詞)</span>
          </div>
          <div className="flex items-center gap-3">
            <span className="inline-flex items-center justify-center min-w-[28px] h-7 px-2 rounded-md border font-black text-xs bg-emerald-50 text-emerald-600 border-emerald-200 shadow-sm">V</span>
            <span className="text-xs font-bold text-text-tertiary">Động từ (動詞)</span>
          </div>
          <div className="flex items-center gap-3">
            <span className="inline-flex items-center justify-center min-w-[28px] h-7 px-2 rounded-md border font-black text-xs bg-orange-50 text-orange-600 border-orange-200 shadow-sm">A</span>
            <span className="text-xs font-bold text-text-tertiary">Tính từ (形容詞)</span>
          </div>
          <div className="flex items-center gap-3">
            <span className="inline-flex items-center justify-center min-w-[28px] h-7 px-2 rounded-md border font-black text-xs bg-orange-50 text-orange-600 border-orange-200 shadow-sm">Aい</span>
            <span className="text-xs font-bold text-text-tertiary">Tính từ đuôi い</span>
          </div>
          <div className="flex items-center gap-3">
            <span className="inline-flex items-center justify-center min-w-[28px] h-7 px-2 rounded-md border font-black text-xs bg-orange-50 text-orange-600 border-orange-200 shadow-sm">Aな</span>
            <span className="text-xs font-bold text-text-tertiary">Tính từ đuôi な</span>
          </div>
          <div className="flex items-center gap-3">
            <span className="inline-flex items-center justify-center min-w-[28px] h-7 px-2 rounded-md border font-black text-xs bg-violet-50 text-violet-600 border-violet-200 shadow-sm">S</span>
            <span className="text-xs font-bold text-text-tertiary">Câu (文)</span>
          </div>
          <div className="flex items-center gap-3">
            <span className="inline-flex items-center justify-center min-w-[28px] h-7 px-3 rounded-md border font-black text-xs bg-emerald-50 text-emerald-700 border-emerald-200 shadow-sm">Thể-TT</span>
            <span className="text-xs font-bold text-text-tertiary">Thể thông thường (普通形)</span>
          </div>
        </div>
      </section>

      {/* Levels Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 max-w-3xl">
        {courses.map((item) => {
          const summary = summaries.find(s => s.courseCode.toLowerCase() === item.courseCode.toLowerCase());
          const cardTone = item.courseCode === 'jpd113' ? 'jp-course-card--113' : 'jp-course-card--123';

          return (
            <Link 
              key={item.courseCode}
              to={`/grammar/${item.courseCode}`}
              className={`jp-course-card group ${cardTone}`}
            >
              {/* Top Section */}
              <div className="jp-course-card-top">
                <span className="jp-course-card-kicker mb-1 text-[10px] font-bold uppercase tracking-widest text-text-secondary/70">
                  {item.jpTitle.toUpperCase()}
                </span>
                <span className="jp-course-card-title text-5xl font-black uppercase tracking-tighter text-[#061452]">
                  {item.title}
                </span>
              </div>

              {/* Bottom Section */}
              <div className="jp-course-card-bottom">
                <div className="mb-1 flex justify-center gap-6">
                  <div className="flex flex-col">
                    <span className="text-[9px] font-bold opacity-80 uppercase">BÀI HỌC</span>
                    <span className="text-base font-black">{isLoading ? '...' : (summary?.lessonCount ?? 0)}</span>
                  </div>
                </div>
                
                <div className="text-[9px] font-bold mt-1 opacity-80 uppercase tracking-widest">
                  {item.subtitle.toUpperCase()}
                </div>
              </div>
            </Link>
          );
        })}
      </div>
    </div>
  );
};
