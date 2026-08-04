import { useEffect, useMemo, useState, type ReactNode } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import {
  ArrowLeft,
  BookOpen,
  BookOpenCheck,
  Crown,
  FileQuestion,
  Hash,
  Loader2,
  MessageSquare,
  Play,
  Timer,
} from 'lucide-react';
import { examApi } from '../api/examApi';
import type { ExamCourse, ExamTopic } from '../types/exam.types';

const TOPIC_ICON_CONFIG: Record<string, { icon: ReactNode; tone: string }> = {
  kanji: { icon: <span className="font-jp text-xl font-black leading-none">漢</span>, tone: 'bg-blue-50 text-blue-600 ring-blue-100' },
  vocabulary: { icon: <span className="font-jp text-xl font-black leading-none">あ</span>, tone: 'bg-emerald-50 text-emerald-600 ring-emerald-100' },
  grammar: { icon: <Hash size={18} />, tone: 'bg-violet-50 text-violet-600 ring-violet-100' },
  conversation: { icon: <MessageSquare size={18} />, tone: 'bg-amber-50 text-amber-600 ring-amber-100' },
  reading: { icon: <BookOpen size={18} />, tone: 'bg-rose-50 text-rose-600 ring-rose-100' },
};

const DEFAULT_TOPIC_CONFIG: { icon: ReactNode; tone: string } = {
  icon: <FileQuestion size={18} />,
  tone: 'bg-slate-50 text-slate-600 ring-slate-100',
};

const getTopicConfig = (code: string) => TOPIC_ICON_CONFIG[code] ?? DEFAULT_TOPIC_CONFIG;

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
  const isLocked = course?.isLocked ?? true;

  const startExam = async () => {
    if (!courseCode || isLocked) return;
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
    <div className="relative mx-auto max-w-6xl px-4 pb-20 pt-4 animate-fade-in">
      {/* Soft decor backdrop */}
      <div aria-hidden="true" className="pointer-events-none absolute inset-x-0 -top-10 h-44 bg-gradient-to-b from-blue-50/90 to-transparent" />
      <div aria-hidden="true" className="pointer-events-none absolute -right-16 top-24 select-none font-jp text-[120px] font-black leading-none text-blue-100/50">🌸</div>

      <div className="relative space-y-6">
        {/* Back + Header */}
        <header className="space-y-4">
          <button
            onClick={() => navigate('/exam')}
            className="group inline-flex items-center gap-2 text-sm font-black text-blue-600 transition-colors hover:text-blue-800"
          >
            <ArrowLeft size={16} className="transition-transform group-hover:-translate-x-1" />
            Quay lại
          </button>

          <div className="flex flex-col gap-4 md:flex-row md:items-end md:justify-between">
            <div>
              <span className="inline-flex items-center rounded-full bg-blue-100 px-3 py-1 text-[10px] font-black uppercase tracking-[0.18em] text-blue-700 ring-1 ring-blue-200">
                Exam Practice
              </span>
              <h1 className="mt-3 text-3xl font-black tracking-tight text-slate-900 md:text-4xl">
                Luyện thi {courseTitle}
              </h1>
              <p className="mt-2 max-w-2xl text-sm font-medium leading-6 text-slate-500">
                {courseDescription} — Học theo chủ đề hoặc làm bài luyện thi 30 câu / 30 phút.
              </p>
            </div>

            <div className="inline-flex items-center gap-4 rounded-3xl border border-blue-100 bg-white px-6 py-4 shadow-lg shadow-blue-100/60">
              <div className="grid h-12 w-12 place-items-center rounded-2xl bg-blue-50 text-blue-600 ring-1 ring-blue-100">
                <FileQuestion size={22} />
              </div>
              <div>
                <p className="text-[10px] font-black uppercase tracking-[0.16em] text-slate-400">Ngân hàng {courseTitle}</p>
                <p className="text-3xl font-black leading-none text-slate-900">{isLoading ? '—' : totalQuestions}</p>
              </div>
            </div>
          </div>
        </header>

        {error ? (
          <div className="rounded-2xl border border-red-100 bg-red-50/80 p-4 text-sm font-bold text-red-600">
            {error}
          </div>
        ) : null}

        {isLoading ? (
          <div className="flex h-56 flex-col items-center justify-center rounded-3xl border border-blue-100 bg-white/80 shadow-lg shadow-blue-100/40">
            <div className="grid h-14 w-14 place-items-center rounded-2xl bg-blue-50 text-blue-600 ring-1 ring-blue-100">
              <Loader2 size={28} className="animate-spin" />
            </div>
            <p className="mt-4 text-sm font-bold text-slate-500">Đang tải dữ liệu {courseTitle}...</p>
          </div>
        ) : isLocked ? (
          <section className="relative overflow-hidden rounded-3xl border border-blue-100 bg-gradient-to-br from-white to-blue-50 p-8 text-center shadow-lg shadow-blue-100/40">
            <div aria-hidden="true" className="pointer-events-none absolute -right-8 -top-8 h-28 w-28 rounded-full bg-blue-100/60 blur-2xl" />
            <div className="mx-auto grid h-16 w-16 place-items-center rounded-2xl bg-gradient-to-br from-blue-500 to-indigo-500 text-white shadow-lg shadow-blue-200">
              <Crown size={30} />
            </div>
            <h2 className="mt-5 text-2xl font-black text-slate-900">Cần kích hoạt gói {courseCode?.toUpperCase()}</h2>
            <p className="mx-auto mt-2 max-w-xl text-sm font-medium text-slate-500">
              Tài khoản hiện tại chưa có quyền truy cập luyện thi {courseCode?.toUpperCase()}.
            </p>
            <button
              type="button"
              onClick={() => navigate('/pricing')}
              className="mt-6 inline-flex h-12 items-center gap-2 rounded-2xl bg-gradient-to-r from-blue-500 to-indigo-500 px-6 text-sm font-black text-white shadow-lg shadow-blue-200 transition-all hover:-translate-y-0.5 hover:shadow-xl"
            >
              <Crown size={17} />
              Nâng cấp ngay
            </button>
          </section>
        ) : (
          <section className="grid gap-6 lg:grid-cols-2">
            {/* Study Mode */}
            <div className="flex flex-col rounded-3xl border border-slate-100 bg-white p-6 shadow-lg shadow-slate-100/70">
              <div className="mb-6 flex items-center gap-3">
                <span className="grid h-12 w-12 place-items-center rounded-2xl bg-blue-50 text-blue-600 ring-1 ring-blue-100">
                  <BookOpenCheck size={24} />
                </span>
                <div>
                  <p className="text-[10px] font-black uppercase tracking-[0.16em] text-slate-400">Chế độ học</p>
                  <h2 className="text-2xl font-black text-slate-900">Học theo chủ đề</h2>
                </div>
              </div>

              <div className="grid flex-1 content-start gap-3 sm:grid-cols-2">
                {topics.map((topic) => {
                  const config = getTopicConfig(topic.topic);
                  return (
                    <Link
                      key={topic.topic}
                      to={`/exam/study/${topic.topic}?course=${courseCode}`}
                      className="group flex items-center gap-3 rounded-2xl border border-slate-100 bg-slate-50/80 p-4 transition-all hover:-translate-y-0.5 hover:border-blue-200 hover:bg-white hover:shadow-md hover:shadow-blue-100/60"
                    >
                      <span className={`grid h-10 w-10 shrink-0 place-items-center rounded-xl ring-1 ${config.tone}`}>
                        {config.icon}
                      </span>
                      <div className="min-w-0">
                        <p className="truncate text-sm font-black text-slate-900">{topic.label}</p>
                        <p className="mt-0.5 text-xs font-bold text-slate-400">{topic.questionCount} câu hỏi</p>
                      </div>
                    </Link>
                  );
                })}
              </div>

              <button
                type="button"
                disabled={!firstTopic}
                onClick={() => firstTopic && navigate(`/exam/study/${firstTopic}?course=${courseCode}`)}
                className="mt-6 inline-flex h-12 w-full items-center justify-center gap-2 rounded-2xl border border-slate-200 bg-white px-5 text-sm font-black text-slate-900 shadow-sm transition-all hover:-translate-y-0.5 hover:border-blue-300 hover:bg-blue-50/50 hover:text-blue-700 hover:shadow-md disabled:pointer-events-none disabled:opacity-50"
              >
                <Play size={17} className="fill-current" />
                Bắt đầu học
              </button>
            </div>

            {/* Exam Mode */}
            <div className="relative overflow-hidden rounded-3xl border border-blue-100 bg-gradient-to-br from-blue-50 to-sky-50 p-6 shadow-lg shadow-blue-100/60">
              <div aria-hidden="true" className="pointer-events-none absolute -right-6 -top-6 h-24 w-24 rounded-full bg-blue-100/70 blur-2xl" />

              <div className="mb-6 flex items-center gap-3">
                <span className="grid h-12 w-12 place-items-center rounded-2xl bg-gradient-to-br from-blue-500 to-indigo-500 text-white shadow-lg shadow-blue-200">
                  <Timer size={24} />
                </span>
                <div>
                  <p className="text-[10px] font-black uppercase tracking-[0.16em] text-blue-500">Luyện thi {courseTitle}</p>
                  <h2 className="text-2xl font-black text-slate-900">30 câu / 30 phút</h2>
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div className="rounded-2xl border border-blue-100 bg-white p-4 shadow-sm">
                  <p className="text-4xl font-black text-slate-900">30</p>
                  <p className="mt-1 text-[10px] font-black uppercase tracking-[0.12em] text-slate-400">Câu hỏi</p>
                </div>
                <div className="rounded-2xl border border-blue-100 bg-white p-4 shadow-sm">
                  <p className="text-4xl font-black text-slate-900">30'</p>
                  <p className="mt-1 text-[10px] font-black uppercase tracking-[0.12em] text-slate-400">Thời gian</p>
                </div>
              </div>

              <button
                type="button"
                disabled={isStartingAttempt || totalQuestions === 0}
                onClick={startExam}
                className="mt-6 inline-flex h-12 w-full items-center justify-center gap-2 rounded-2xl bg-gradient-to-r from-blue-500 to-indigo-500 px-5 text-sm font-black text-white shadow-lg shadow-blue-200 transition-all hover:-translate-y-0.5 hover:shadow-xl disabled:pointer-events-none disabled:opacity-60"
              >
                {isStartingAttempt ? <Loader2 size={17} className="animate-spin" /> : <FileQuestion size={17} />}
                {isStartingAttempt ? 'Đang tạo bài' : `Thi ${courseTitle}`}
              </button>
              <p className="mt-4 text-xs font-bold text-slate-500">
                30 câu ngẫu nhiên từ {courseTitle}, fullscreen, chống chuyển tab.
              </p>
            </div>
          </section>
        )}
      </div>
    </div>
  );
};
