import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Loader2, Lock, Sparkles } from 'lucide-react';
import { examApi } from '../api/examApi';
import type { ExamCourse } from '../types/exam.types';

export const ExamDashboardPage = () => {
  const navigate = useNavigate();
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
          Luyện thi <span className="text-blue-600">試験</span>
        </h1>
        <p className="text-sm font-bold text-text-secondary uppercase tracking-widest mt-1">
          JPD113 / JPD123 EXAM PRACTICE
        </p>
      </div>

      {/* Marketing Hook */}
      <div className="mb-6 max-w-3xl overflow-hidden rounded-2xl border border-blue-100 bg-gradient-to-br from-blue-50/50 to-white p-5 shadow-sm">
        <div className="flex items-start gap-4">
          <div className="flex h-12 w-12 shrink-0 items-center justify-center rounded-2xl bg-blue-600 text-white shadow-lg shadow-blue-200">
            <Sparkles size={24} />
          </div>
          <div>
            <h3 className="font-heading text-lg font-black text-slate-900 leading-tight">
              Ôn tập chất lượng - Kết quả tối ưu
            </h3>
            <p className="mt-1 text-sm font-bold leading-relaxed text-slate-600">
              Các câu hỏi được tổng hợp kĩ càng và có <span className="text-blue-600">giải thích chi tiết</span> mỗi câu hỏi, giúp các bạn ôn tập một cách hiệu quả nhất.
            </p>
          </div>
        </div>
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
            <div
              key={course.code}
              onClick={() => course.isLocked ? navigate('/pricing') : navigate(`/exam/${course.code}`)}
              className={`group relative overflow-hidden rounded-[24px] border-2 border-border/5 shadow-sm transition-all cursor-pointer hover:-translate-y-1 hover:shadow-md ${
                course.isLocked ? 'grayscale-[0.3]' : ''
              }`}
            >
              {/* Top */}
              <div className={`${colorTop} py-6 px-5 flex flex-col items-center justify-center text-center`}>
                <span className="text-[10px] font-bold text-text-secondary/70 mb-1 uppercase tracking-widest">
                  {course.description}
                </span>
                <span className="text-5xl font-black text-[#0f172a] tracking-tighter uppercase">
                  {course.title}
                </span>
              </div>

              {/* Bottom */}
              <div className={`${colorBottom} py-4 px-5 text-center text-white`}>
                <div className="flex justify-center gap-6 mb-2">
                  <div className="flex flex-col">
                    <span className="text-[9px] font-bold opacity-80 uppercase">CÂU HỎI</span>
                    <span className="text-base font-black">{course.questionCount}</span>
                  </div>
                  <div className="flex flex-col">
                    <span className="text-[9px] font-bold opacity-80 uppercase">THỜI GIAN</span>
                    <span className="text-base font-black">30'</span>
                  </div>
                </div>

                <div className="text-[9px] font-bold mt-1 opacity-80 uppercase tracking-widest">
                  {course.isLocked ? 'Cần kích hoạt gói Premium' : `Học & Luyện thi ${course.title}`}
                </div>
              </div>

              {course.isLocked && (
                <div className="absolute inset-0 z-10 flex items-center justify-center bg-slate-900/10 backdrop-blur-[1px]">
                  <div className="flex h-12 w-12 items-center justify-center rounded-full border-2 border-white bg-slate-900/80 text-white shadow-xl">
                    <Lock size={20} />
                  </div>
                </div>
              )}
            </div>
          );
        })}
      </div>
    </div>
  );
};
