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
      {/* Header */}
      <div className="mb-4">
        <h1 className="text-[40px] font-black uppercase leading-none tracking-tight text-text-primary font-mono flex items-center gap-2">
          Hán tự <span className="text-blue-600">漢字</span>
        </h1>
        <p className="text-sm font-bold text-text-secondary uppercase tracking-widest mt-1">
          JPD113 / JPD123 KANJI MASTERY
        </p>
      </div>

      {/* Grid of Levels */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 max-w-3xl">
        {stats
          .map((levelStat) => {
            const courseCode = levelStat.courseCode ?? (levelStat.level === 'N5' ? 'jpd113' : 'jpd123');
            const isJpd113 = courseCode === 'jpd113';
            const displayLevel = courseCode.toUpperCase();
            const description = isJpd113 ? 'Japanese 1' : 'Japanese 2';
            const cardTone = isJpd113 ? 'jp-course-card--113' : 'jp-course-card--123';
            
            return (
              <Link
                key={courseCode}
                to={`/kanji/${courseCode}`}
                className={`jp-course-card group ${cardTone}`}
              >
                {/* Top Section */}
                <div className="jp-course-card-top">
                  <span className="jp-course-card-kicker mb-1 text-[10px] font-bold uppercase tracking-widest text-text-secondary/70">
                    {displayLevel} - {description.toUpperCase()}
                  </span>
                  <span className="jp-course-card-title text-5xl font-black uppercase tracking-tighter text-[#061452]">
                    {displayLevel}
                  </span>
                </div>

                {/* Bottom Section */}
                <div className="jp-course-card-bottom">
                  <div className="flex justify-center gap-6 mb-2">
                    <div className="flex flex-col">
                      <span className="text-[9px] font-bold opacity-80 uppercase">BÀI HỌC</span>
                      <span className="text-base font-black">{levelStat.totalLessons}</span>
                    </div>
                    <div className="flex flex-col">
                      <span className="text-[9px] font-bold opacity-80 uppercase">HÁN TỰ</span>
                      <span className="text-base font-black">{levelStat.totalKanji}</span>
                    </div>
                    <div className="flex flex-col">
                      <span className="text-[9px] font-bold opacity-80 uppercase">TỪ VỰNG</span>
                      <span className="text-base font-black">{levelStat.totalVocabulary}</span>
                    </div>
                  </div>
                  
                  <div className="text-[9px] font-bold mt-1 opacity-80 uppercase tracking-widest">
                    KHÁM PHÁ LỘ TRÌNH HÁN TỰ {displayLevel}
                  </div>
                </div>
              </Link>
            )})}
      </div>
    </div>
  );
};
