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

    void loadQuestions();

    return () => {
      cancelled = true;
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
    if (!question) {
      setCurrentQuestion(null);
      return;
    }

    void loadQuestionDetail(question.id);
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
    <div className="mx-auto max-w-5xl space-y-5 px-4 pb-20 pt-4 animate-fade-in">
      <header className="flex flex-col gap-4 md:flex-row md:items-end md:justify-between">
        <div>
          <button
            type="button"
            onClick={() => navigate(`/exam/${courseCode}`)}
            className="inline-flex items-center gap-2 text-sm font-extrabold text-text-muted transition hover:text-text-primary"
          >
            <ArrowLeft size={18} />
            Quay lại {courseCode.toUpperCase()}
          </button>
          <p className="mt-4 text-xs font-black uppercase tracking-[0.18em] text-text-muted">Chế độ học</p>
          <h1 className="text-3xl font-black text-text-primary">{activeTopic?.label ?? 'Luyện câu hỏi'}</h1>
          <p className="mt-1 text-xs font-bold text-text-muted">
            Có thể dùng phím ← / → để chuyển câu nhanh.
          </p>
        </div>

        <div className="flex flex-wrap gap-2">
          {topics.map((item) => (
            <button
              key={item.topic}
              type="button"
              onClick={() => navigate(`/exam/study/${item.topic}${courseQuery}`)}
              className={`border-2 px-4 py-2 text-xs font-black transition-all hover:-translate-y-0.5 hover:-translate-x-0.5 ${
                item.topic === topic
                  ? 'border-slate-900 bg-slate-900 text-white shadow-[2px_2px_0_#111827]'
                  : 'border-slate-200 bg-white text-slate-500 hover:border-slate-900 hover:text-slate-900 hover:shadow-[2px_2px_0_#111827]'
              }`}
            >
              {item.label}
            </button>
          ))}
        </div>
      </header>

      {error ? (
        <div className="border-2 border-accent-danger bg-accent-danger/10 p-4 text-sm font-bold text-accent-danger">
          {error}
        </div>
      ) : null}

      {isLoading ? (
        <div className="flex min-h-80 flex-col items-center justify-center border-2 border-slate-900 bg-white shadow-[8px_8px_0_#111827]">
          <Loader2 size={36} className="mb-4 animate-spin text-accent-primary" />
          <p className="font-black text-slate-600">Đang tải câu hỏi...</p>
        </div>
      ) : isCompleted ? (
        <div className="border-2 border-slate-900 bg-white p-8 text-center shadow-[8px_8px_0_#111827]">
          <CheckCircle2 size={56} className="mx-auto text-accent-success" />
          <h2 className="mt-4 text-3xl font-black text-slate-900">Hoàn thành chủ đề</h2>
          <p className="mt-2 font-bold text-slate-500">Bạn đã học hết {questions.length} câu trong chủ đề này.</p>
          <button
            type="button"
            onClick={() => {
              setCurrentIndex(0);
              const storageKey = `exam_progress_${courseCode}_${topic}`;
              localStorage.removeItem(storageKey);
            }}
            className="mt-6 inline-flex h-11 items-center gap-2 border-2 border-slate-900 bg-[#3B82F6] px-5 text-sm font-black text-white shadow-[4px_4px_0_#111827] transition-all hover:translate-x-[2px] hover:translate-y-[2px] hover:shadow-[2px_2px_0_#111827]"
          >
            Học lại
          </button>
        </div>
      ) : questions.length === 0 ? (
        <div className="border-2 border-slate-900 bg-white p-8 text-center shadow-[8px_8px_0_#111827]">
          <h2 className="text-2xl font-black text-slate-900">Chưa có câu hỏi</h2>
          <p className="mt-2 font-bold text-slate-500">Chủ đề này chưa có dữ liệu luyện thi.</p>
        </div>
      ) : (
        <section className="border-2 border-slate-900 bg-white p-5 shadow-[8px_8px_0_#111827] md:p-8">
          <div className="mb-5 flex flex-wrap items-center justify-between gap-3 border-b-2 border-slate-200 pb-4">
            <div>
              <p className="text-xs font-black uppercase tracking-[0.18em] text-slate-500">Question</p>
              <h2 className="text-2xl font-black text-slate-900">
                {Math.min(currentIndex + 1, questions.length)} / {questions.length}
              </h2>
            </div>
            <span className="bg-slate-100 px-3 py-1 text-xs font-black uppercase text-slate-600">
              {currentQuestion?.level ?? 'n5'}
            </span>
          </div>

          {isQuestionLoading || !currentQuestion ? (
            <div className="flex min-h-72 flex-col items-center justify-center">
              <Loader2 size={32} className="mb-3 animate-spin text-accent-primary" />
              <p className="font-bold text-slate-500">Đang tải câu hỏi...</p>
            </div>
          ) : (
            <div className="space-y-6">
              {currentQuestion.passage ? (
                <div className="border-2 border-slate-200 bg-[#F8FAFC] p-5">
                  <p className="mb-2 text-xs font-black uppercase tracking-[0.16em] text-slate-500">{currentQuestion.passage.title}</p>
                  <p className="whitespace-pre-wrap font-jp text-lg font-bold leading-9 text-slate-900">{currentQuestion.passage.content}</p>
                </div>
              ) : null}

              <div>
                <p className="text-xs font-black uppercase tracking-[0.18em] text-text-muted">Câu hỏi</p>
                <h2 className="mt-2 text-2xl font-black leading-snug text-text-primary">{currentQuestion.questionText}</h2>
              </div>

              <div className="grid gap-2">
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
                      className={`flex min-h-[52px] items-center gap-3 border-2 bg-white px-4 py-2.5 text-left transition-all ${
                        isCorrect
                          ? 'border-accent-success bg-[#ECFDF5] text-accent-success shadow-[3px_3px_0_#10B981]'
                          : isWrongSelection
                            ? 'border-accent-danger bg-[#FEF2F2] text-accent-danger shadow-[3px_3px_0_#EF4444]'
                            : isSelected
                              ? 'border-blue-500 bg-[#EFF6FF] text-blue-700 shadow-[3px_3px_0_#3B82F6]'
                              : 'border-slate-200 text-slate-900 hover:-translate-x-[1px] hover:-translate-y-[1px] hover:border-slate-900 hover:shadow-[3px_3px_0_#111827]'
                      } disabled:cursor-default`}
                    >
                      <span className="grid h-7 w-7 shrink-0 place-items-center border-2 border-current text-xs font-black">{option.label}</span>
                      <span className="font-bold text-base">{option.text}</span>
                    </button>
                  );
                })}
              </div>

              {answerResult ? (
                <div className={`mt-4 border-2 p-5 shadow-[4px_4px_0_currentColor] ${
                  answerResult.isCorrect
                    ? 'border-accent-success bg-[#ECFDF5] text-accent-success'
                    : 'border-accent-danger bg-[#FEF2F2] text-accent-danger'
                }`}>
                  <div className="flex items-center gap-2 font-black text-lg">
                    {answerResult.isCorrect ? <CheckCircle2 size={24} /> : <XCircle size={24} />}
                    {answerResult.isCorrect ? 'Chính xác' : 'Chưa đúng'}
                  </div>
                  <p className="mt-3 text-[15px] font-bold leading-relaxed text-slate-800">{answerResult.explanation}</p>
                </div>
              ) : null}

              <div className="flex flex-wrap justify-end gap-3 pt-4">
                <button
                  type="button"
                  disabled={currentIndex === 0 || isQuestionLoading}
                  onClick={goPrevious}
                  className="inline-flex h-11 items-center gap-2 border-2 border-slate-900 bg-white px-4 text-sm font-black text-slate-900 shadow-[4px_4px_0_#111827] transition-all hover:translate-x-[2px] hover:translate-y-[2px] hover:shadow-[2px_2px_0_#111827] disabled:pointer-events-none disabled:opacity-40"
                >
                  <ChevronLeft size={18} />
                  Câu trước
                </button>
                <span className="hidden items-center bg-slate-100 px-3 text-xs font-black text-slate-500 border-2 border-slate-200 md:inline-flex">
                  ← / → chuyển câu
                </span>
                {answerResult ? (
                  <button type="button" onClick={goNext} className="inline-flex h-11 items-center gap-2 border-2 border-slate-900 bg-[#3B82F6] px-5 text-sm font-black text-white shadow-[4px_4px_0_#111827] transition-all hover:translate-x-[2px] hover:translate-y-[2px] hover:shadow-[2px_2px_0_#111827]">
                    {currentIndex + 1 >= questions.length ? 'Hoàn thành' : 'Câu tiếp theo'}
                  </button>
                ) : (
                  <button
                    type="button"
                    disabled={currentIndex + 1 >= questions.length || isQuestionLoading}
                    onClick={goNext}
                    className="inline-flex h-11 items-center gap-2 border-2 border-slate-900 bg-white px-5 text-sm font-black text-slate-900 shadow-[4px_4px_0_#111827] transition-all hover:translate-x-[2px] hover:translate-y-[2px] hover:shadow-[2px_2px_0_#111827] disabled:pointer-events-none disabled:opacity-40"
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
