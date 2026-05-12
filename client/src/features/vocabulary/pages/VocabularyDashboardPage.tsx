import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { staticVocabularyApi } from '../api/vocabularyApi';
import type { VocabularyCourse } from '../types/vocabulary.types';

export const VocabularyDashboardPage = () => {
  const [courses, setCourses] = useState<VocabularyCourse[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchCourses = async () => {
      try {
        const data = await staticVocabularyApi.getCourses();
        setCourses(data);
      } catch (error) {
        console.error('Failed to fetch vocabulary courses:', error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchCourses();
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
          Từ vựng <span className="text-accent-primary">語彙</span>
        </h1>
        <p className="text-sm font-bold text-text-secondary uppercase tracking-widest mt-1">
          JPD113 / JPD123 static lessons
        </p>
      </div>

      {/* Grid of Courses */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 max-w-3xl">
        {courses.map((course) => {
          // Hardcode for FPT layout similar to Kanji
          const isJpd113 = course.code.includes('113');
          const displayLevel = course.code;
          const description = course.title;
          const colorTop = isJpd113 ? 'bg-[#e5e1da]' : 'bg-[#b8d4e3]';
          const colorBottom = 'bg-[#2d9a56]';

          return (
            <Link
              key={course.id}
              to={`/vocabulary/${course.code}`}
              className="group overflow-hidden rounded-[24px] border-2 border-border/5 shadow-sm transition-all hover:-translate-y-1 hover:shadow-md"
            >
              {/* Top Section */}
              <div className={`${colorTop} py-6 px-5 flex flex-col items-center justify-center text-center`}>
                <span className="text-[10px] font-bold text-text-secondary/70 mb-1 uppercase tracking-widest">
                  {description}
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
                    <span className="text-base font-black">{course.lessonCount}</span>
                  </div>
                  <div className="flex flex-col">
                    <span className="text-[9px] font-bold opacity-80 uppercase">Words</span>
                    <span className="text-base font-black">{course.wordCount}</span>
                  </div>
                </div>

                <div className="text-[9px] font-bold mt-1 opacity-80 uppercase tracking-widest">
                  Khám phá lộ trình Từ vựng {course.code}
                </div>
              </div>
            </Link>
          );
        })}
      </div>
    </div>
  );
};
