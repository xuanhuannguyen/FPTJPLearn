import { useCallback, useEffect, useMemo, useState } from 'react';
import { useNavigate, useParams, useSearchParams } from 'react-router-dom';
import { ArrowLeft, CheckCircle2, ChevronLeft, ChevronRight, Loader2, XCircle } from 'lucide-react';
import { examApi } from '../api/examApi';
import type { ExamAnswerResult, ExamQuestion, ExamQuestionDetail, ExamTopic } from '../types/exam.types';

export const ExamStudyPage = () => {
  const { topic } = useParams<{ topic: string }>();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const courseCode = searchParams.get('course') ?? 'jpd113';
  const courseQuery = `?course=${encodeURIComponent(courseCode)}`;
  const [topics, setTopics] = useState<ExamTopic[]>([]);
  const [questions, setQuestions] = useState<ExamQuestion[]>([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [currentQuestion, setCurrentQuestion] = useState<ExamQuestionDetail | null>(null);
  const [answerResult, setAnswerResult] = useState<ExamAnswerResult | null>(null);
  const [selectedOptionId, setSelectedOptionId] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isQuestionLoading, setIsQuestionLoading] = useState(false);
  const [isAnswering, setIsAnswering] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    let cancelled = false;

    const loadTopics = async () => {
      try {
        const data = await examApi.getTopics(courseCode);
        if (!cancelled) {
          setTopics(data);
          if (!topic) {
            const firstTopic = data.find((item) => item.questionCount > 0)?.topic;
            if (firstTopic) {
              navigate(`/exam/study/${firstTopic}${courseQuery}`, { replace: true });
            }
          }
        }
      } catch (err) {
        console.error(err);
        if (!cancelled) {
          setError('Không tải được chủ đề luyện thi.');
        }
      }
    };

    void loadTopics();

    return () => {
      cancelled = true;
    };
  }, [courseCode, courseQuery, navigate, topic]);

  useEffect(() => {
    if (!topic) return;

    let cancelled = false;

    const loadQuestions = async () => {
      try {
        setIsLoading(true);
        setError('');
        setCurrentIndex(0);
        setAnswerResult(null);
        setSelectedOptionId(null);
        const data = await examApi.getQuestions({ courseCode, topic });
        if (!cancelled) {
          setQuestions(data);
          
          // Load progress from localStorage
          const storageKey = `exam_progress_${courseCode}_${topic}`;
          const savedIndex = localStorage.getItem(storageKey);
          if (savedIndex) {
            const index = parseInt(savedIndex, 10);
            if (index >= 0 && index < data.length) {
              setCurrentIndex(index);
            }
          }
        }
      } catch (err) {
        console.error(err);
        if (!cancelled) {
          setError('Không tải được câu hỏi luyện thi.');
        }
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    };

    const timer = window.setTimeout(() => {
      void loadQuestions();
    }, 0);

    return () => {
      cancelled = true;
      window.clearTimeout(timer);
    };
  }, [courseCode, topic]);

  const loadQuestionDetail = useCallback(async (questionId: string) => {
    try {
      setIsQuestionLoading(true);
      setError('');
      setAnswerResult(null);
      setSelectedOptionId(null);
      const data = await examApi.getQuestion(questionId);
      setCurrentQuestion(data);
    } catch (err) {
      console.error(err);
      setError('Không tải được chi tiết câu hỏi.');
    } finally {
      setIsQuestionLoading(false);
    }
  }, []);

  useEffect(() => {
    const question = questions[currentIndex];
    const timer = window.setTimeout(() => {
      if (!question) {
        setCurrentQuestion(null);
        return;
      }

      void loadQuestionDetail(question.id);
    }, 0);

    return () => window.clearTimeout(timer);
  }, [currentIndex, loadQuestionDetail, questions]);

  const activeTopic = useMemo(() => topics.find((item) => item.topic === topic), [topic, topics]);
  const isCompleted = !isLoading && questions.length > 0 && currentIndex >= questions.length;

  const answerQuestion = async (optionId: string) => {
    if (!currentQuestion || answerResult || isAnswering) return;

    try {
      setIsAnswering(true);
      setSelectedOptionId(optionId);
      const result = await examApi.answerQuestion(currentQuestion.id, optionId);
      setAnswerResult(result);
    } catch (err) {
      console.error(err);
      setError('Không chấm được câu hỏi. Hãy thử lại.');
    } finally {
      setIsAnswering(false);
    }
  };

  const goNext = () => {
    setCurrentIndex((prev) => {
      const next = prev + 1;
      const storageKey = `exam_progress_${courseCode}_${topic}`;
      localStorage.setItem(storageKey, next.toString());
      return next;
    });
  };

  const goPrevious = () => {
    setCurrentIndex((prev) => {
      const next = Math.max(0, prev - 1);
      const storageKey = `exam_progress_${courseCode}_${topic}`;
      localStorage.setItem(storageKey, next.toString());
      return next;
    });
  };

  useEffect(() => {
    const handleKeyDown = (event: KeyboardEvent) => {
      const storageKey = `exam_progress_${courseCode}_${topic}`;
      if (event.key === 'ArrowLeft') {
        setCurrentIndex((prev) => {
          const next = Math.max(0, prev - 1);
          localStorage.setItem(storageKey, next.toString());
          return next;
        });
      }

      if (event.key === 'ArrowRight') {
        setCurrentIndex((prev) => {
          const next = Math.min(questions.length, prev + 1);
          localStorage.setItem(storageKey, next.toString());
          return next;
        });
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [questions.length]);

  return (
    <div className="mx-auto w-full max-w-[1000px] space-y-[24px] px-4 pb-20 pt-4 animate-fade-in">
      <header className="flex flex-col gap-4 md:flex-row md:items-end md:justify-between">
        <div>
          <button
            type="button"
            onClick={() => navigate(`/exam/${courseCode}`)}
            className="inline-flex items-center gap-2 text-[15px] font-extrabold text-text-muted transition hover:text-text-primary"
          >
            <ArrowLeft size={18} />
            Quay lại {courseCode.toUpperCase()}
          </button>
          <p className="mt-4 text-[13px] font-black uppercase tracking-[0.16em] text-slate-500">Chế độ học</p>
          <h1 className="mt-1 text-[34px] font-black tracking-tight text-text-primary">{activeTopic?.label ?? 'Luyện câu hỏi'}</h1>
          <p className="mt-1 text-[14px] font-semibold text-slate-500">
            Có thể dùng phím ← / → để chuyển câu nhanh.
          </p>
        </div>

        <div className="flex flex-wrap gap-2">
          {topics.map((item) => (
            <button
              key={item.topic}
              type="button"
              onClick={() => navigate(`/exam/study/${item.topic}${courseQuery}`)}
              aria-pressed={item.topic === topic}
              className={`min-h-[42px] rounded-xl border-2 px-[16px] py-[8px] text-[14px] font-black transition-all hover:-translate-y-0.5 hover:-translate-x-0.5 focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-blue-100 ${
                item.topic === topic
                  ? 'border-blue-600 bg-blue-600 text-white shadow-md shadow-blue-200'
                  : 'border-slate-200 bg-white text-slate-500 hover:border-border-hover hover:text-slate-900 hover:shadow-lift'
              }`}
            >
              {item.label}
            </button>
          ))}
        </div>
      </header>

      {error ? (
        <div className="rounded-2xl border-2 border-accent-danger bg-accent-danger/10 p-4 text-[15px] font-bold text-accent-danger">
          {error}
        </div>
      ) : null}

      {isLoading ? (
        <div className="flex min-h-80 flex-col items-center justify-center border-2 border-border bg-white shadow-card">
          <Loader2 size={36} className="mb-4 animate-spin text-accent-primary" />
          <p className="text-[16px] font-black text-slate-600">Đang tải câu hỏi...</p>
        </div>
      ) : isCompleted ? (
        <div className="rounded-3xl border border-slate-200 bg-white p-8 text-center shadow-card">
          <CheckCircle2 size={56} className="mx-auto text-accent-success" />
          <h2 className="mt-4 text-[32px] font-black text-slate-900">Hoàn thành chủ đề</h2>
          <p className="mt-2 text-[16px] font-semibold text-slate-600">Bạn đã học hết {questions.length} câu trong chủ đề này.</p>
          <button
            type="button"
            onClick={() => {
              setCurrentIndex(0);
              const storageKey = `exam_progress_${courseCode}_${topic}`;
              localStorage.removeItem(storageKey);
            }}
            className="mt-6 inline-flex h-[48px] items-center gap-2 rounded-xl border-2 border-border bg-[#3B82F6] px-5 text-[15px] font-black text-white shadow-card transition-all hover:-translate-y-0.5 hover:shadow-lift"
          >
            Học lại
          </button>
        </div>
      ) : questions.length === 0 ? (
        <div className="rounded-3xl border border-slate-200 bg-white p-8 text-center shadow-card">
          <h2 className="text-[28px] font-black text-slate-900">Chưa có câu hỏi</h2>
          <p className="mt-2 text-[16px] font-semibold text-slate-600">Chủ đề này chưa có dữ liệu luyện thi.</p>
        </div>
      ) : (
        <section className="rounded-[28px] border border-slate-200 bg-white p-5 shadow-[0_12px_36px_rgba(15,23,42,0.07)] md:p-6">
          <div className="mb-4 flex flex-wrap items-center justify-between gap-3 border-b border-slate-200 pb-4">
            <div>
              <p className="text-[13px] font-black uppercase tracking-[0.16em] text-slate-500">Câu hỏi</p>
              <h2 className="mt-1 text-[28px] font-black leading-tight text-slate-900">
                {Math.min(currentIndex + 1, questions.length)} / {questions.length}
              </h2>
            </div>
            <span className="rounded-lg bg-slate-100 px-3 py-2 text-[13px] font-black uppercase text-slate-600">
              {currentQuestion?.level ?? 'n5'}
            </span>
          </div>

          {isQuestionLoading || !currentQuestion ? (
            <div className="flex min-h-72 flex-col items-center justify-center">
              <Loader2 size={32} className="mb-3 animate-spin text-accent-primary" />
              <p className="font-bold text-slate-500">Đang tải câu hỏi...</p>
            </div>
          ) : (
            <div className="space-y-4">
              {currentQuestion.passage ? (
                <div className="rounded-2xl border border-slate-200 bg-[#F8FAFC] p-4 md:p-5">
                  <p className="mb-3 text-[14px] font-black uppercase tracking-[0.14em] text-slate-500">{currentQuestion.passage.title}</p>
                  <p className="whitespace-pre-wrap font-jp text-[20px] font-medium leading-[1.9] tracking-[0.01em] text-slate-900 md:text-[22px]">{currentQuestion.passage.content}</p>
                </div>
              ) : null}

              <div>
                <p className="text-[14px] font-black uppercase tracking-[0.14em] text-blue-600">Câu hỏi</p>
                <h2 className="mt-1 font-jp text-[25px] font-bold leading-[1.8] tracking-[0.01em] text-slate-900 md:text-[30px]">{currentQuestion.questionText}</h2>
              </div>

              <div className="grid gap-2.5">
                {currentQuestion.options.map((option) => {
                  const isSelected = selectedOptionId === option.id;
                  const isCorrect = answerResult?.correctOptionId === option.id;
                  const isWrongSelection = answerResult && isSelected && !isCorrect;

                  return (
                    <button
                      key={option.id}
                      type="button"
                      disabled={Boolean(answerResult) || isAnswering}
                      onClick={() => answerQuestion(option.id)}
                      className={`flex min-h-[60px] items-center gap-3 rounded-2xl border-2 bg-white px-4 py-3 text-left transition-all focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-blue-100 ${
                        isCorrect
                          ? 'border-accent-success bg-[#ECFDF5] text-accent-success shadow-[3px_3px_0_#10B981]'
                          : isWrongSelection
                            ? 'border-accent-danger bg-[#FEF2F2] text-accent-danger shadow-[3px_3px_0_#EF4444]'
                            : isSelected
                              ? 'border-blue-500 bg-[#EFF6FF] text-blue-700 shadow-[3px_3px_0_#3B82F6]'
                              : 'border-slate-200 text-slate-900 hover:-translate-y-0.5 hover:border-border-hover hover:shadow-lift'
                      } disabled:cursor-default`}
                    >
                      <span className="grid h-8 w-8 shrink-0 place-items-center rounded-xl border-2 border-current text-[14px] font-black">{option.label}</span>
                      <span className="font-jp text-[20px] font-semibold leading-relaxed tracking-[0.01em]">{option.text}</span>
                    </button>
                  );
                })}
              </div>

              {answerResult ? (
                <div className={`mt-4 rounded-2xl border-2 p-5 shadow-sm md:p-6 ${
                  answerResult.isCorrect
                    ? 'border-accent-success bg-[#ECFDF5] text-accent-success'
                    : 'border-accent-danger bg-[#FEF2F2] text-accent-danger'
                }`}>
                  <div className="flex items-center gap-2 font-black text-[20px]">
                    {answerResult.isCorrect ? <CheckCircle2 size={24} /> : <XCircle size={24} />}
                    {answerResult.isCorrect ? 'Chính xác' : 'Chưa đúng'}
                  </div>
                  <p className="mt-3 text-[17px] font-semibold leading-relaxed text-slate-800">{answerResult.explanation}</p>
                </div>
              ) : null}

              <div className="flex flex-wrap justify-end gap-2.5 border-t border-slate-100 pt-4">
                <button
                  type="button"
                  disabled={currentIndex === 0 || isQuestionLoading}
                  onClick={goPrevious}
                  className="inline-flex h-11 items-center gap-2 rounded-xl border-2 border-border bg-white px-4 text-[15px] font-black text-slate-900 shadow-card transition-all hover:-translate-y-0.5 hover:shadow-lift disabled:pointer-events-none disabled:opacity-40"
                >
                  <ChevronLeft size={18} />
                  Câu trước
                </button>
                <span className="hidden items-center rounded-xl border border-slate-200 bg-slate-50 px-3 text-[13px] font-black text-slate-500 md:inline-flex">
                  ← / → chuyển câu
                </span>
                {answerResult ? (
                  <button type="button" onClick={goNext} className="inline-flex h-11 items-center gap-2 rounded-xl border-2 border-border bg-[#3B82F6] px-4 text-[15px] font-black text-white shadow-card transition-all hover:-translate-y-0.5 hover:shadow-lift">
                    {currentIndex + 1 >= questions.length ? 'Hoàn thành' : 'Câu tiếp theo'}
                  </button>
                ) : (
                  <button
                    type="button"
                    disabled={currentIndex + 1 >= questions.length || isQuestionLoading}
                    onClick={goNext}
                    className="inline-flex h-11 items-center gap-2 rounded-xl border-2 border-border bg-white px-4 text-[15px] font-black text-slate-900 shadow-card transition-all hover:-translate-y-0.5 hover:shadow-lift disabled:pointer-events-none disabled:opacity-40"
                  >
                    Câu sau
                    <ChevronRight size={18} />
                  </button>
                )}
              </div>
            </div>
          )}
        </section>
      )}
    </div>
  );
};
