import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { kanjiApi } from '../api/kanjiApi';
import type { KanjiLevelStats } from '../types/kanji.types';

export const KanjiDashboardPage = () => {
  const [stats, setStats] = useState<KanjiLevelStats[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const data = await kanjiApi.getLevelStats();
        setStats(data);
      } catch (error) {
        console.error('Failed to fetch kanji stats:', error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchStats();
  }, []);

  if (isLoading) {
    return (
      <div className="flex h-64 items-center justify-center">
        <div className="h-10 w-10 animate-spin rounded-full border-4 border-accent-primary border-t-transparent"></div>
      </div>
    );
  }

  return (
    <div className="max-w-7xl mx-auto px-4 py-2 animate-fade-in">
      {/* Brutalist Header */}
      <div className="mb-4">
        <h1 className="text-[40px] font-black uppercase leading-none tracking-tight text-text-primary font-mono flex items-center gap-2">
          Kanji <span className="text-accent-primary">漢字</span>
        </h1>
        <p className="text-sm font-bold text-text-secondary uppercase tracking-widest mt-1">
          Master the characters
        </p>
      </div>

      {/* Grid of Levels */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 max-w-3xl">
        {stats
          .filter(s => s.level === 'N5' || s.level === 'N3')
          .map((levelStat) => {
            const isN5 = levelStat.level === 'N5';
            const displayLevel = isN5 ? 'JPD113' : 'JPD123';
            const description = isN5 ? 'Japanese 1' : 'Japanese 2';
            const colorTop = isN5 ? 'bg-[#e5e1da]' : 'bg-[#b8d4e3]';
            const colorBottom = 'bg-[#2d9a56]';
            
            return (
              <Link
                key={levelStat.level}
                to={`/kanji/${levelStat.level}`}
                className="group overflow-hidden rounded-[24px] border-2 border-border/5 shadow-sm transition-all hover:-translate-y-1 hover:shadow-md"
              >
                {/* Top Section */}
                <div className={`${colorTop} py-6 px-5 flex flex-col items-center justify-center text-center`}>
                  <span className="text-[10px] font-bold text-text-secondary/70 mb-1 uppercase tracking-widest">
                    {displayLevel} - {description}
                  </span>
                  <span className="text-5xl font-black text-[#0f172a] tracking-tighter">
                    {displayLevel}
                  </span>
                </div>

                {/* Bottom Section */}
                <div className={`${colorBottom} py-4 px-5 text-center text-white`}>
                  <div className="flex justify-center gap-6 mb-2">
                    <div className="flex flex-col">
                      <span className="text-[9px] font-bold opacity-80 uppercase">Lessons</span>
                      <span className="text-base font-black">{levelStat.totalLessons}</span>
                    </div>
                    <div className="flex flex-col">
                      <span className="text-[9px] font-bold opacity-80 uppercase">Kanji</span>
                      <span className="text-base font-black">{levelStat.totalKanji}</span>
                    </div>
                    <div className="flex flex-col">
                      <span className="text-[9px] font-bold opacity-80 uppercase">Vocab</span>
                      <span className="text-base font-black">{levelStat.totalVocabulary}</span>
                    </div>
                  </div>
                  
                  <div className="text-[9px] font-bold mt-1 opacity-80 uppercase tracking-widest">
                    Khám phá lộ trình Hán tự {levelStat.level}
                  </div>
                </div>
              </Link>
            )})}
      </div>
    </div>
  );
};
