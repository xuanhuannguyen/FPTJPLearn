import { fetchStatic } from '../../../shared/services/staticDataService';
import type {
  ExamAnswerResult,
  ExamAttempt,
  ExamAttemptAnswer,
  ExamAttemptQuestion,
  ExamAttemptReview,
  ExamAttemptReviewQuestion,
  ExamCourse,
  ExamQuestion,
  ExamQuestionDetail,
  ExamReviewOption,
  ExamTopic,
  StartExamAttemptRequest,
} from '../types/exam.types';

type StaticExamQuestionDetail = ExamQuestionDetail & {
  correctOptionId: string;
  explanation: string;
};

type StoredAttempt = ExamAttempt & {
  selectedAnswers: Record<string, string>;
};

const attemptKey = (attemptId: string) => `exam_attempt_${attemptId}`;

const loadQuestionDetails = async (courseCode: string) =>
  fetchStatic<StaticExamQuestionDetail[]>(`exam/${courseCode}/question-details.json`);

const toAttemptQuestion = (question: StaticExamQuestionDetail, sequenceNumber: number): ExamAttemptQuestion => ({
  ...question,
  attemptAnswerId: `${question.id}-attempt-answer`,
  selectedOptionId: null,
  isCorrect: null,
  sequenceNumber,
});

const saveAttempt = (attempt: StoredAttempt) => {
  localStorage.setItem(attemptKey(attempt.id), JSON.stringify(attempt));
};

const loadAttempt = (attemptId: string): StoredAttempt => {
  const raw = localStorage.getItem(attemptKey(attemptId));
  if (!raw) throw new Error(`Exam attempt not found: ${attemptId}`);
  return JSON.parse(raw) as StoredAttempt;
};

const buildReview = async (attempt: StoredAttempt): Promise<ExamAttemptReview> => {
  const details = await loadQuestionDetails(attempt.courseCode);
  const detailById = new Map(details.map((question) => [question.id, question]));

  const questions: ExamAttemptReviewQuestion[] = attempt.questions.map((question) => {
    const detail = detailById.get(question.id);
    const selectedOptionId = attempt.selectedAnswers[question.id] ?? null;
    const correctOptionId = detail?.correctOptionId ?? '';
    const isCorrect = Boolean(selectedOptionId && selectedOptionId === correctOptionId);
    const options: ExamReviewOption[] = question.options.map((option) => ({
      ...option,
      isCorrect: option.id === correctOptionId,
    }));

    return {
      ...question,
      options,
      selectedOptionId,
      correctOptionId,
      isCorrect,
      explanation: detail?.explanation ?? '',
      sequenceNumber: question.sequenceNumber,
    };
  });

  const correctCount = questions.filter((question) => question.isCorrect).length;
  return {
    id: attempt.id,
    courseCode: attempt.courseCode,
    mode: attempt.mode,
    status: 'submitted',
    startedAt: attempt.startedAt,
    submittedAt: attempt.submittedAt ?? new Date().toISOString(),
    totalQuestions: questions.length,
    correctCount,
    scorePercent: questions.length === 0 ? 0 : Math.round((correctCount / questions.length) * 100),
    questions,
  };
};

export const examApi = {
  getCourses: async (): Promise<ExamCourse[]> => {
    return fetchStatic<ExamCourse[]>('exam/courses.json');
  },

  getTopics: async (courseCode?: string, level?: string): Promise<ExamTopic[]> => {
    const code = courseCode || 'jpd113';
    const topics = await fetchStatic<ExamTopic[]>(`exam/${code}/topics.json`);
    if (!level) return topics;

    const questions = await loadQuestionDetails(code);
    const allowedTopics = new Set(questions.filter((question) => question.level === level).map((question) => question.topic));
    return topics.filter((topic) => allowedTopics.has(topic.topic));
  },

  getQuestions: async (params?: { courseCode?: string; topic?: string; level?: string }): Promise<ExamQuestion[]> => {
    const code = params?.courseCode || 'jpd113';
    const questions = await fetchStatic<ExamQuestion[]>(`exam/${code}/questions.json`);
    return questions.filter((question) => (
      (!params?.topic || question.topic === params.topic) &&
      (!params?.level || question.level === params.level)
    ));
  },

  getQuestion: async (questionId: string): Promise<ExamQuestionDetail> => {
    const courseCode = questionId.includes('jpd123') ? 'jpd123' : 'jpd113';
    const questions = await loadQuestionDetails(courseCode);
    const question = questions.find((candidate) => candidate.id === questionId);
    if (!question) throw new Error(`Exam question not found: ${questionId}`);
    return question;
  },

  answerQuestion: async (questionId: string, selectedOptionId: string): Promise<ExamAnswerResult> => {
    const courseCode = questionId.includes('jpd123') ? 'jpd123' : 'jpd113';
    const questions = await loadQuestionDetails(courseCode);
    const question = questions.find((candidate) => candidate.id === questionId);
    if (!question) throw new Error(`Exam question not found: ${questionId}`);

    return {
      questionId,
      selectedOptionId,
      correctOptionId: question.correctOptionId,
      isCorrect: selectedOptionId === question.correctOptionId,
      explanation: question.explanation,
    };
  },

  startAttempt: async (payload: StartExamAttemptRequest = {}): Promise<ExamAttempt> => {
    const courseCode = payload.courseCode || 'jpd113';
    const durationMinutes = payload.durationMinutes ?? 30;
    const questionCount = payload.questionCount ?? 30;
    const allQuestions = await loadQuestionDetails(courseCode);
    const filtered = allQuestions.filter((question) => (
      (!payload.level || question.level === payload.level) &&
      (!payload.topics?.length || payload.topics.includes(question.topic))
    ));
    const selected = filtered.slice(0, Math.min(questionCount, filtered.length));
    const now = new Date();
    const expiresAt = new Date(now.getTime() + durationMinutes * 60 * 1000);
    const id = `static-${courseCode}-${Date.now()}`;
    const attempt: StoredAttempt = {
      id,
      courseCode,
      mode: payload.mode || 'exam',
      status: 'in_progress',
      startedAt: now.toISOString(),
      expiresAt: expiresAt.toISOString(),
      submittedAt: null,
      durationMinutes,
      totalQuestions: selected.length,
      correctCount: 0,
      scorePercent: 0,
      questions: selected.map(toAttemptQuestion),
      selectedAnswers: {},
    };

    saveAttempt(attempt);
    return attempt;
  },

  getAttempt: async (attemptId: string): Promise<ExamAttempt> => {
    return loadAttempt(attemptId);
  },

  saveAttemptAnswer: async (attemptId: string, questionId: string, selectedOptionId: string): Promise<ExamAttemptAnswer> => {
    const attempt = loadAttempt(attemptId);
    attempt.selectedAnswers[questionId] = selectedOptionId;
    attempt.questions = attempt.questions.map((question) => (
      question.id === questionId ? { ...question, selectedOptionId } : question
    ));
    saveAttempt(attempt);

    const question = attempt.questions.find((item) => item.id === questionId);
    return {
      id: question?.attemptAnswerId ?? `${attemptId}-${questionId}`,
      questionId,
      selectedOptionId,
      isCorrect: null,
      answeredAt: new Date().toISOString(),
      sequenceNumber: question?.sequenceNumber ?? 0,
    };
  },

  submitAttempt: async (attemptId: string): Promise<ExamAttempt> => {
    const attempt = loadAttempt(attemptId);
    const review = await buildReview(attempt);
    const submitted: StoredAttempt = {
      ...attempt,
      status: 'submitted',
      submittedAt: review.submittedAt,
      correctCount: review.correctCount,
      scorePercent: review.scorePercent,
      questions: attempt.questions.map((question) => {
        const reviewed = review.questions.find((item) => item.id === question.id);
        return {
          ...question,
          selectedOptionId: reviewed?.selectedOptionId ?? null,
          isCorrect: reviewed?.isCorrect ?? false,
        };
      }),
    };
    saveAttempt(submitted);
    return submitted;
  },

  getAttemptReview: async (attemptId: string): Promise<ExamAttemptReview> => {
    return buildReview(loadAttempt(attemptId));
  },
};
