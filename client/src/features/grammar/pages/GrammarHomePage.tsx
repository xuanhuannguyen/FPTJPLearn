import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { grammarApi } from '../api/grammarApi';
import type { GrammarLevelSummary, GrammarLevel } from '../types/grammar.types';

const levels: { level: GrammarLevel; title: string; jpTitle: string; subtitle: string; colorTop: string; colorBottom: string }[] = [
  { 
    level: 'N5', 
    jpTitle: 'みんなの日本語 初級I 第2版',
    title: 'N5', 
    subtitle: 'Minna no Nihongo Sơ cấp 1 – Ngữ pháp',
    colorTop: 'bg-[#e5e1da]',
    colorBottom: 'bg-[#2d9a56]'
  },
  { 
    level: 'N4', 
    jpTitle: 'みんなの日本語 初級II 第2版',
    title: 'N4', 
    subtitle: 'Minna no Nihongo Sơ cấp 2 – Ngữ pháp',
    colorTop: 'bg-[#94bfa7]',
    colorBottom: 'bg-[#2d9a56]'
  },
  { 
    level: 'N3', 
    jpTitle: 'みんなの日本語 中級I 第2版',
    title: 'N3', 
    subtitle: 'Minna no Nihongo Trung cấp 1 – Ngữ pháp',
    colorTop: 'bg-[#b8d4e3]',
    colorBottom: 'bg-[#2d9a56]'
  },
  { 
    level: 'N2', 
    jpTitle: 'みんなの日本語 中級II 第2版',
    title: 'N2', 
    subtitle: 'Minna no Nihongo Trung cấp 2 – Ngữ pháp',
    colorTop: 'bg-[#d6cbd3]',
    colorBottom: 'bg-[#2d9a56]'
  },
  { 
    level: 'N1', 
    jpTitle: '上級で学ぶ日本語',
    title: 'N1', 
    subtitle: 'Tiếng Nhật Thượng cấp – Ngữ pháp',
    colorTop: 'bg-[#cbd5e1]',
    colorBottom: 'bg-[#2d9a56]'
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
    <div className="max-w-7xl mx-auto px-4 py-8 animate-fade-in">
      {/* Header Section Simplified */}
      <div className="mb-10">
        <h1 className="text-3xl font-black text-text-primary mb-2">
          Ngữ pháp tiếng Nhật
        </h1>
        <p className="text-sm text-text-muted font-bold">
          Duyệt bài theo JLPT, mở từng mẫu ngữ pháp, làm bài tập và thêm mẫu cần học vào Ghi nhớ.
        </p>
      </div>

      {/* Legend / Abbreviations */}
      <section className="mb-10 rounded-2xl border-2 border-border/10 bg-white/50 p-6 shadow-sm">
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
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-3">
        {levels.map((item) => {
          const summary = summaries.find(s => s.level === item.level);

          return (
            <Link 
              key={item.level}
              to={`/grammar/${item.level}`}
              className="group overflow-hidden rounded-xl border-2 border-border/10 shadow-sm transition-all hover:-translate-y-0.5 hover:shadow-md"
            >
              {/* Top Section */}
              <div className={`${item.colorTop} py-4 px-3 flex flex-col items-center justify-center text-center`}>
                <span className="text-[9px] font-bold text-text-secondary/70 mb-0.5 font-jp">
                  {item.jpTitle}
                </span>
                <span className="text-3xl font-black text-text-primary tracking-tighter">
                  {item.level}
                </span>
              </div>

              {/* Bottom Section */}
              <div className={`${item.colorBottom} py-2 px-3 text-center text-white`}>
                <div className="text-sm font-black">
                  {isLoading ? '...' : (summary?.lessonCount ?? 0)} bài học
                </div>
                <div className="text-[9px] font-bold opacity-90 truncate">
                  {item.subtitle}
                </div>
              </div>
            </Link>
          );
        })}
      </div>
    </div>
  );
};
