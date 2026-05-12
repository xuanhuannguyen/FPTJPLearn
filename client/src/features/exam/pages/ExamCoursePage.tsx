import { useEffect, useMemo, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { ArrowLeft, BookOpenCheck, FileQuestion, Loader2, Play, Timer } from 'lucide-react';
import { examApi } from '../api/examApi';
import type { ExamCourse, ExamTopic } from '../types/exam.types';

export const ExamCoursePage = () => {
  const { courseCode } = useParams<{ courseCode: string }>();
  const navigate = useNavigate();
  const [course, setCourse] = useState<ExamCourse | null>(null);
  const [topics, setTopics] = useState<ExamTopic[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isStartingAttempt, setIsStartingAttempt] = useState(false);
  const [error, setError] = useState('');

  const courseTitle = course?.title ?? (courseCode ?? '').toUpperCase();
  const courseDescription = course?.description ?? '';

  useEffect(() => {
    if (!courseCode) return;
    let cancelled = false;

    const load = async () => {
      try {
        setIsLoading(true);
        setError('');
        const [courseData, topicData] = await Promise.all([
          examApi.getCourses(),
          examApi.getTopics(courseCode),
        ]);

        if (!cancelled) {
          setCourse(courseData.find((item) => item.code === courseCode) ?? null);
          setTopics(topicData);
        }
      } catch (err) {
        console.error(err);
        if (!cancelled) setError('Không tải được dữ liệu luyện thi.');
      } finally {
        if (!cancelled) setIsLoading(false);
      }
    };

    void load();
    return () => { cancelled = true; };
  }, [courseCode]);

  const totalQuestions = useMemo(() => topics.reduce((sum, t) => sum + t.questionCount, 0), [topics]);
  const firstTopic = topics.find((t) => t.questionCount > 0)?.topic;

  const startExam = async () => {
    if (!courseCode) return;
    try {
      setIsStartingAttempt(true);
      setError('');
      const attempt = await examApi.startAttempt({
        courseCode,
        questionCount: 30,
        durationMinutes: 30,
        mode: 'exam',
      });
      navigate(`/exam/test/${attempt.id}`);
    } catch (err) {
      console.error(err);
      setError('Không thể bắt đầu bài luyện thi. Hãy thử lại.');
    } finally {
      setIsStartingAttempt(false);
    }
  };

  return (
    <div className="mx-auto max-w-6xl space-y-6 px-4 pb-20 pt-4 animate-fade-in">
      {/* Back + Header */}
      <header className="space-y-4">
        <button
          onClick={() => navigate('/exam')}
          className="group inline-flex items-center gap-2 text-sm font-black text-accent-primary transition-colors hover:text-accent-hover"
        >
          <ArrowLeft size={16} className="transition-transform group-hover:-translate-x-1" />
          Quay lại
        </button>

        <div className="flex flex-col gap-4 md:flex-row md:items-end md:justify-between">
          <div>
            <p className="text-xs font-black uppercase tracking-[0.18em] text-text-muted">Exam Practice</p>
            <h1 className="mt-1 text-4xl font-black tracking-normal text-slate-900">
              Luyện thi {courseTitle}
            </h1>
            <p className="mt-2 max-w-2xl text-sm font-bold leading-6 text-slate-600">
              {courseDescription} — Học theo chủ đề hoặc làm bài luyện thi 30 câu / 30 phút.
            </p>
          </div>

          <div className="border-2 border-slate-900 bg-white px-5 py-3 shadow-[4px_4px_0_#111827]">
            <p className="text-xs font-black uppercase tracking-[0.16em] text-slate-500">Ngân hàng {courseTitle}</p>
            <p className="text-3xl font-black text-slate-900">{isLoading ? '—' : totalQuestions}</p>
          </div>
        </div>
      </header>

      {error ? (
        <div className="border-2 border-accent-danger bg-accent-danger/10 p-4 text-sm font-bold text-accent-danger">
          {error}
        </div>
      ) : null}

      {isLoading ? (
        <div className="flex h-48 flex-col items-center justify-center text-text-secondary">
          <Loader2 size={36} className="mb-4 animate-spin text-accent-primary" />
          <p className="font-bold">Đang tải dữ liệu {courseTitle}...</p>
        </div>
      ) : (
        <section className="grid gap-6 lg:grid-cols-2">
          {/* Study Mode */}
          <div className="border-2 border-slate-900 bg-white p-6 shadow-[8px_8px_0_#111827]">
            <div className="mb-6 flex items-center gap-3">
              <span className="grid h-12 w-12 place-items-center border-2 border-slate-900 bg-[#EFF6FF] text-[#2563EB]">
                <BookOpenCheck size={24} />
              </span>
              <div>
                <p className="text-xs font-black uppercase tracking-[0.16em] text-slate-500">Chế độ học</p>
                <h2 className="text-2xl font-black text-slate-900">Học theo chủ đề</h2>
              </div>
            </div>

            <div className="grid gap-4 sm:grid-cols-2">
              {topics.map((topic) => (
                <Link
                  key={topic.topic}
                  to={`/exam/study/${topic.topic}?course=${courseCode}`}
                  className="border-2 border-slate-200 bg-slate-50 p-4 transition-all hover:-translate-y-1 hover:-translate-x-1 hover:border-slate-900 hover:bg-white hover:shadow-[4px_4px_0_#111827]"
                >
                  <p className="text-lg font-black text-slate-900">{topic.label}</p>
                  <p className="mt-1 text-sm font-bold text-slate-500">{topic.questionCount} câu hỏi</p>
                </Link>
              ))}
            </div>

            <button
              type="button"
              disabled={!firstTopic}
              onClick={() => firstTopic && navigate(`/exam/study/${firstTopic}?course=${courseCode}`)}
              className="mt-6 inline-flex h-11 items-center gap-2 border-2 border-slate-900 bg-[#F8FAFC] px-5 text-sm font-black text-slate-900 shadow-[4px_4px_0_#111827] transition-all hover:translate-x-[2px] hover:translate-y-[2px] hover:shadow-[2px_2px_0_#111827] disabled:pointer-events-none disabled:opacity-50"
            >
              <Play size={17} />
              Bắt đầu học
            </button>
          </div>

          {/* Exam Mode */}
          <div className="border-2 border-slate-900 bg-[#E0F2FE] p-6 text-slate-900 shadow-[8px_8px_0_#111827]">
            <div className="mb-6 flex items-center gap-3">
              <span className="grid h-12 w-12 place-items-center border-2 border-slate-900 bg-white text-slate-900">
                <Timer size={24} />
              </span>
              <div>
                <p className="text-xs font-black uppercase tracking-[0.16em] text-slate-600">Luyện thi {courseTitle}</p>
                <h2 className="text-2xl font-black text-slate-900">30 câu / 30 phút</h2>
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="border-2 border-slate-900 bg-white p-4">
                <p className="text-4xl font-black text-slate-900">30</p>
                <p className="mt-1 text-xs font-black uppercase tracking-[0.12em] text-slate-500">Câu hỏi</p>
              </div>
              <div className="border-2 border-slate-900 bg-white p-4">
                <p className="text-4xl font-black text-slate-900">30'</p>
                <p className="mt-1 text-xs font-black uppercase tracking-[0.12em] text-slate-500">Thời gian</p>
              </div>
            </div>

            <button
              type="button"
              disabled={isStartingAttempt || totalQuestions === 0}
              onClick={startExam}
              className="mt-6 inline-flex h-11 items-center gap-2 border-2 border-slate-900 bg-[#2563EB] px-5 text-sm font-black text-white shadow-[4px_4px_0_#111827] transition-all hover:translate-x-[2px] hover:translate-y-[2px] hover:shadow-[2px_2px_0_#111827] disabled:pointer-events-none disabled:opacity-60"
            >
              {isStartingAttempt ? <Loader2 size={17} className="animate-spin" /> : <FileQuestion size={17} />}
              {isStartingAttempt ? 'Đang tạo bài' : `Thi ${courseTitle}`}
            </button>
            <p className="mt-4 text-xs font-bold text-slate-600">
              30 câu ngẫu nhiên từ {courseTitle}, fullscreen, chống chuyển tab.
            </p>
          </div>
        </section>
      )}
    </div>
  );
};
