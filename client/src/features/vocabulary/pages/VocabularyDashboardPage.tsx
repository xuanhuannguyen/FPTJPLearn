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
      {/* Header */}
      <div className="mb-4">
        <h1 className="text-[40px] font-black uppercase leading-none tracking-tight text-text-primary font-mono flex items-center gap-2">
          Từ vựng <span className="text-blue-600">語彙</span>
        </h1>
        <p className="text-sm font-bold text-text-secondary uppercase tracking-widest mt-1">
          JPD113 / JPD123 VOCABULARY PRACTICE
        </p>
      </div>

      {/* Grid of Courses */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 max-w-3xl">
        {courses.map((course) => {
          const isJpd113 = course.code.includes('113');
          const displayLevel = course.code;
          const description = course.title;
          const cardTone = isJpd113 ? 'jp-course-card--113' : 'jp-course-card--123';

          return (
            <Link
              key={course.id}
              to={`/vocabulary/${course.code}`}
              className={`jp-course-card group ${cardTone}`}
            >
              {/* Top Section */}
              <div className="jp-course-card-top">
                <span className="jp-course-card-kicker mb-1 text-[10px] font-bold uppercase tracking-widest text-text-secondary/70">
                  {description.toUpperCase()}
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
                    <span className="text-base font-black">{course.lessonCount}</span>
                  </div>
                  <div className="flex flex-col">
                    <span className="text-[9px] font-bold opacity-80 uppercase">TỪ MỚI</span>
                    <span className="text-base font-black">{course.wordCount}</span>
                  </div>
                </div>

                <div className="text-[9px] font-bold mt-1 opacity-80 uppercase tracking-widest">
                  KHÁM PHÁ LỘ TRÌNH TỪ VỰNG {course.code}
                </div>
              </div>
            </Link>
          );
        })}
      </div>
    </div>
  );
};
