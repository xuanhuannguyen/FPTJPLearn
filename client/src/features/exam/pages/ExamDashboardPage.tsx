import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { Loader2 } from 'lucide-react';
import { examApi } from '../api/examApi';
import type { ExamCourse } from '../types/exam.types';

export const ExamDashboardPage = () => {
  const [courses, setCourses] = useState<ExamCourse[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    let cancelled = false;

    const loadCourses = async () => {
      try {
        setIsLoading(true);
        setError('');
        const data = await examApi.getCourses();
        if (!cancelled) setCourses(data);
      } catch (err) {
        console.error(err);
        if (!cancelled) setError('Không tải được danh sách khóa luyện thi.');
      } finally {
        if (!cancelled) setIsLoading(false);
      }
    };

    void loadCourses();
    return () => {
      cancelled = true;
    };
  }, []);

  return (
    <div className="max-w-7xl mx-auto px-4 py-2 animate-fade-in">
      {/* Header */}
      <div className="mb-4">
        <h1 className="text-[40px] font-black uppercase leading-none tracking-tight text-text-primary font-mono flex items-center gap-2">
          Luyện thi <span className="text-accent-primary">試験</span>
        </h1>
        <p className="text-sm font-bold text-text-secondary uppercase tracking-widest mt-1">
          JPD113 / JPD123 exam practice
        </p>
      </div>

      {/* Course Grid */}
      {error ? (
        <div className="mb-4 border-2 border-accent-danger bg-accent-danger/10 p-4 text-sm font-bold text-accent-danger">
          {error}
        </div>
      ) : null}

      {isLoading ? (
        <div className="flex h-48 max-w-4xl flex-col items-center justify-center text-text-secondary">
          <Loader2 size={36} className="mb-4 animate-spin text-accent-primary" />
          <p className="font-bold">Đang tải khóa luyện thi...</p>
        </div>
      ) : null}

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 max-w-3xl">
        {courses.map((course) => {
          const isJpd113 = course.code === 'jpd113';
          const colorTop = isJpd113 ? 'bg-[#e5e1da]' : 'bg-[#b8d4e3]';
          const colorBottom = 'bg-[#2563EB]';

          return (
            <Link
              key={course.code}
              to={`/exam/${course.code}`}
              className="group overflow-hidden rounded-[24px] border-2 border-border/5 shadow-sm transition-all hover:-translate-y-1 hover:shadow-md"
            >
              {/* Top */}
              <div className={`${colorTop} py-6 px-5 flex flex-col items-center justify-center text-center`}>
                <span className="text-[10px] font-bold text-text-secondary/70 mb-1 uppercase tracking-widest">
                  {course.description}
                </span>
                <span className="text-5xl font-black text-[#0f172a] tracking-tighter">
                  {course.title}
                </span>
              </div>

              {/* Bottom */}
              <div className={`${colorBottom} py-4 px-5 text-center text-white`}>
                <div className="flex justify-center gap-6 mb-2">
                  <div className="flex flex-col">
                    <span className="text-[9px] font-bold opacity-80 uppercase">Câu hỏi</span>
                    <span className="text-base font-black">{course.questionCount}</span>
                  </div>
                  <div className="flex flex-col">
                    <span className="text-[9px] font-bold opacity-80 uppercase">Thời gian</span>
                    <span className="text-base font-black">30'</span>
                  </div>
                </div>

                <div className="text-[9px] font-bold mt-1 opacity-80 uppercase tracking-widest">
                  Học & Luyện thi {course.title}
                </div>
              </div>
            </Link>
          );
        })}
      </div>
    </div>
  );
};
