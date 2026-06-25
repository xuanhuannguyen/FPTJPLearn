import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Loader2, Lock, Volume2 } from 'lucide-react';
import { speakingApi } from '../api/speakingApi';
import type { SpeakingCourse } from '../types/speaking.types';
import { useUserAccess } from '../../../shared/hooks/useUserAccess';

export const SpeakingDashboardPage = () => {
  const navigate = useNavigate();
  const [courses, setCourses] = useState<SpeakingCourse[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');
  const { isContentLocked } = useUserAccess();

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
        <div className="grid max-w-2xl grid-cols-1 gap-3 md:grid-cols-2">
          {courses.map((course) => {
            const isLocked = isContentLocked(course);
            const isJpd113 = course.code === 'jpd113';
            const cardTone = isJpd113 ? 'jp-course-card--113' : 'jp-course-card--123';

            return (
              <div
                key={course.id}
                onClick={() => isLocked ? navigate('/pricing') : navigate(`/speaking/${course.code}`)}
                className={`jp-course-card jp-course-card--compact group cursor-pointer ${cardTone} ${
                  isLocked ? 'grayscale-[0.3]' : ''
                }`}
              >
                <div className="jp-course-card-top">
                  <span className="jp-course-card-kicker mb-1 text-[10px] font-bold uppercase tracking-widest text-text-secondary/70">
                    {course.description}
                  </span>
                  <span className="jp-course-card-title text-5xl font-black uppercase tracking-tighter text-[#061452]">
                    {course.title}
                  </span>
                </div>

                <div className="jp-course-card-bottom">
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
                    {isLocked ? 'Cần kích hoạt gói Premium' : `Đọc và luyện nói ${course.title}`}
                  </div>
                </div>

                {isLocked && (
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
