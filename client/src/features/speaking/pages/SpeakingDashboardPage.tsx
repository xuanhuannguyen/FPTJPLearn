import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Loader2, Lock, Volume2 } from 'lucide-react';
import { speakingApi } from '../api/speakingApi';
import type { SpeakingCourse } from '../types/speaking.types';

export const SpeakingDashboardPage = () => {
  const navigate = useNavigate();
  const [courses, setCourses] = useState<SpeakingCourse[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    let cancelled = false;

    const loadCourses = async () => {
      try {
        setIsLoading(true);
        setError('');
        const data = await speakingApi.getCourses();
        if (!cancelled) setCourses(data);
      } catch (err) {
        console.error(err);
        if (!cancelled) setError('Không tải được khóa luyện nói.');
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
    <div className="mx-auto max-w-7xl px-4 py-2 animate-fade-in">
      <div className="mb-4">
        <h1 className="flex items-center gap-2 font-mono text-[40px] font-black uppercase leading-none tracking-tight text-text-primary">
          Luyện nói <span className="text-blue-600">会話</span>
        </h1>
        <p className="mt-1 text-sm font-bold uppercase tracking-widest text-text-secondary">
          JPD113 / JPD123 SPEAKING PRACTICE
        </p>
      </div>

      {/* Marketing Hook */}
      <div className="mb-6 max-w-3xl overflow-hidden rounded-2xl border border-orange-100 bg-gradient-to-br from-orange-50/50 to-white p-5 shadow-sm">
        <div className="flex items-start gap-4">
          <div className="flex h-12 w-12 shrink-0 items-center justify-center rounded-2xl bg-orange-500 text-white shadow-lg shadow-orange-200">
            <Volume2 size={24} />
          </div>
          <div>
            <h3 className="font-heading text-lg font-black text-slate-900 leading-tight">
              Công cụ hỗ trợ luyện nói thông minh
            </h3>
            <p className="mt-1 text-sm font-bold leading-relaxed text-slate-600">
              Mỗi bài luyện nói đều có chế độ <span className="text-orange-600">Bật tắt nghĩa</span>, và <span className="text-orange-600">Hiragana</span> đối với chữ Kanji giúp bạn luyện nói dễ dàng, tiện lợi hơn.
            </p>
          </div>
        </div>
      </div>

      {error ? (
        <div className="mb-4 border-2 border-accent-danger bg-accent-danger/10 p-4 text-sm font-bold text-accent-danger">
          {error}
        </div>
      ) : null}

      {isLoading ? (
        <div className="flex h-48 max-w-4xl flex-col items-center justify-center text-text-secondary">
          <Loader2 size={36} className="mb-4 animate-spin text-accent-primary" />
          <p className="font-bold">Đang tải khóa luyện nói...</p>
        </div>
      ) : (
        <div className="grid max-w-3xl grid-cols-1 gap-4 md:grid-cols-2">
          {courses.map((course) => {
            const isJpd113 = course.code === 'jpd113';
            const colorTop = isJpd113 ? 'bg-[#f4e8dc]' : 'bg-[#dbeafe]';
            const colorBottom = 'bg-blue-600';

            return (
              <div
                key={course.id}
                onClick={() => course.isLocked ? navigate('/pricing') : navigate(`/speaking/${course.code}`)}
                className={`group relative overflow-hidden rounded-[24px] border-2 border-border/5 shadow-sm transition-all cursor-pointer hover:-translate-y-1 hover:shadow-md ${
                  course.isLocked ? 'grayscale-[0.3]' : ''
                }`}
              >
                <div className={`${colorTop} flex flex-col items-center justify-center px-5 py-6 text-center`}>
                  <span className="mb-1 text-[10px] font-bold uppercase tracking-widest text-text-secondary/70">
                    {course.description}
                  </span>
                  <span className="text-5xl font-black tracking-tighter text-[#0f172a] uppercase">
                    {course.title}
                  </span>
                </div>

                <div className={`${colorBottom} px-5 py-4 text-center text-white`}>
                  <div className="mb-2 flex justify-center gap-6">
                    <div className="flex flex-col">
                      <span className="text-[9px] font-bold uppercase opacity-80">BÀI ĐỌC</span>
                      <span className="text-base font-black">{course.lessonCount}</span>
                    </div>
                    <div className="flex flex-col">
                      <span className="text-[9px] font-bold uppercase opacity-80">CÂU HỎI</span>
                      <span className="text-base font-black">{course.sentenceCount}</span>
                    </div>
                  </div>

                  <div className="mt-1 text-[9px] font-bold uppercase tracking-widest opacity-80">
                    {course.isLocked ? 'Cần kích hoạt gói Premium' : `Đọc và luyện nói ${course.title}`}
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
      )}
    </div>
  );
};
