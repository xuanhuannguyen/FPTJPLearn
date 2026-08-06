import { useNavigate, useParams } from 'react-router-dom';
import {
  AlertTriangle,
  Clock,
  Loader2,
  Maximize,
  Send,
  XCircle,
} from 'lucide-react';
import { useExamAttempt } from '../hooks/useExamAttempt';
import { useExamTimer } from '../hooks/useExamTimer';
import { useExamProctor } from '../hooks/useExamProctor';
import { useKeyboardNav } from '../hooks/useKeyboardNav';
import { ExamQuestionView } from '../components/ExamQuestionView';
import { ExamQuestionGrid } from '../components/ExamQuestionGrid';
import { ExamNavBar } from '../components/ExamNavBar';

export const ExamAttemptPage = () => {
  const { attemptId } = useParams<{ attemptId: string }>();
  const navigate = useNavigate();

  const {
    phase, attempt, review, currentIndex, selectedAnswers,
    error, answeredCount,
    setPhase, setCurrentIndex, selectAnswer, handleSubmit,
  } = useExamAttempt(attemptId);

  const { timeLeft, isUrgent, formatTime } = useExamTimer(
    phase === 'exam', attempt, handleSubmit,
  );

  const {
    containerRef, tabWarnings, fullscreenExits, showFullscreenOverlay,
    enterFullscreen, reEnterFullscreen,
  } = useExamProctor(phase === 'exam', handleSubmit);

  // Keyboard nav for both exam and result
  const totalForNav = phase === 'result' && review
    ? review.questions.length
    : attempt?.questions.length ?? 0;
  useKeyboardNav(phase === 'exam' || phase === 'result', totalForNav, setCurrentIndex);

  const startExam = () => {
    setPhase('exam');
    enterFullscreen();
  };

  const navigateBackToCourse = () => {
    const courseCode = review?.courseCode ?? attempt?.courseCode;
    navigate(courseCode ? `/exam/${courseCode}` : '/exam');
  };

  // --- LOADING ---
  if (phase === 'loading') {
    return (
      <div className="flex h-screen flex-col items-center justify-center bg-[#F8FAFC]">
        <Loader2 size={40} className="animate-spin text-[#2563EB]" />
        <p className="mt-4 font-black text-slate-600">Đang tải bài luyện thi...</p>
      </div>
    );
  }

  // --- ERROR ---
  if (error && phase !== 'exam') {
    return (
      <div className="flex h-screen flex-col items-center justify-center bg-[#F8FAFC]">
        <XCircle size={48} className="text-red-500" />
        <p className="mt-4 font-black text-slate-800">{error}</p>
        <button onClick={navigateBackToCourse} className="mt-6 border-2 border-slate-900 bg-white px-6 py-2 font-black shadow-[4px_4px_0_#111827]">
          Quay lại
        </button>
      </div>
    );
  }

  // --- CONFIRM ---
  if (phase === 'confirm' && attempt) {
    return (
      <div ref={containerRef} className="flex min-h-screen flex-col items-center justify-center bg-[#F8FAFC] px-4 py-8">
        <div className="w-full max-w-lg rounded-3xl border border-blue-200/80 bg-white p-7 md:p-8 shadow-[0_12px_36px_rgba(37,99,235,0.10)]">
          <div className="mb-6 flex items-center gap-3.5">
            <div className="flex h-12 w-12 shrink-0 items-center justify-center rounded-2xl border border-blue-100 bg-blue-50 text-[#2563EB] shadow-sm">
              <Clock size={26} />
            </div>
            <div>
              <h1 className="text-2xl font-extrabold text-[#0b2554]">Sẵn sàng luyện thi?</h1>
              <p className="text-xs font-bold text-slate-500 mt-0.5">Kiểm tra thông tin trước khi bắt đầu bài thi</p>
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="rounded-2xl border border-blue-100 bg-[#EFF6FF]/60 p-4 text-center shadow-sm">
              <p className="text-3xl font-black text-[#0b2554]">{attempt.totalQuestions}</p>
              <p className="mt-1 text-xs font-black uppercase tracking-wider text-blue-600/80">Câu hỏi</p>
            </div>
            <div className="rounded-2xl border border-blue-100 bg-[#EFF6FF]/60 p-4 text-center shadow-sm">
              <p className="text-3xl font-black text-[#0b2554]">{attempt.durationMinutes}'</p>
              <p className="mt-1 text-xs font-black uppercase tracking-wider text-blue-600/80">Thời gian</p>
            </div>
          </div>

          <ul className="mt-6 space-y-3 text-sm font-bold text-slate-700">
            <li className="flex items-center gap-3">
              <span className="flex h-7 w-7 shrink-0 items-center justify-center rounded-lg border border-amber-200 bg-amber-50 text-amber-600">
                <AlertTriangle size={15} />
              </span>
              <span>Chuyển tab sẽ bị ghi nhận cảnh báo</span>
            </li>
            <li className="flex items-center gap-3">
              <span className="flex h-7 w-7 shrink-0 items-center justify-center rounded-lg border border-blue-200 bg-blue-50 text-[#2563EB]">
                <Maximize size={15} />
              </span>
              <span>Bài thi sẽ mở toàn màn hình</span>
            </li>
            <li className="flex items-center gap-3">
              <span className="flex h-7 w-7 shrink-0 items-center justify-center rounded-lg border border-sky-200 bg-sky-50 text-sky-700">
                <Clock size={15} />
              </span>
              <span>Hết giờ sẽ tự động nộp bài</span>
            </li>
          </ul>

          <div className="mt-8 flex gap-3">
            <button
              onClick={navigateBackToCourse}
              className="flex-1 rounded-full border border-sky-300 bg-white px-5 py-3 font-extrabold text-[#0b2554] shadow-sm transition-all hover:bg-sky-50 active:scale-95 text-sm"
            >
              Hủy
            </button>
            <button
              onClick={startExam}
              className="flex-1 rounded-full border border-blue-600 bg-gradient-to-r from-blue-600 via-[#2563EB] to-blue-700 px-5 py-3 font-extrabold text-white shadow-md shadow-blue-200 transition-all hover:from-blue-700 hover:to-blue-800 active:scale-95 text-sm uppercase tracking-wider"
            >
              Bắt đầu thi
            </button>
          </div>
        </div>
      </div>
    );
  }

  // --- SUBMITTING ---
  if (phase === 'submitting') {
    return (
      <div className="flex h-screen flex-col items-center justify-center bg-[#F8FAFC]">
        <Loader2 size={40} className="animate-spin text-[#2563EB]" />
        <p className="mt-4 font-black text-slate-600">Đang nộp bài...</p>
      </div>
    );
  }

  // --- RESULT ---
  if (phase === 'result' && review) {
    const passed = review.scorePercent >= 60;
    const rq = review.questions[currentIndex];

    return (
      <div className="flex h-screen flex-col bg-[#F8FAFC]">
        {/* Score bar */}
        <div className="flex shrink-0 items-center justify-between border-b border-sky-200 bg-white px-4 md:px-6 py-3 shadow-sm">
          <div className="flex items-center gap-4">
            <span className={`text-2xl font-black ${passed ? 'text-emerald-600' : 'text-rose-600'}`}>{review.scorePercent}%</span>
            <span className="text-sm font-black text-slate-600">{review.correctCount}/{review.totalQuestions} đúng</span>
            {tabWarnings > 0 && <span className="text-xs font-extrabold text-amber-600">• {tabWarnings} cảnh báo</span>}
          </div>
          <div className="flex items-center gap-3">
            <span className={`rounded-full border px-4 py-1 text-xs font-black uppercase shadow-xs ${
              passed ? 'border-emerald-300 bg-emerald-50 text-emerald-700' : 'border-rose-300 bg-rose-50 text-rose-700'
            }`}>
              {passed ? 'ĐẠT' : 'CHƯA ĐẠT'}
            </span>
            <button
              onClick={navigateBackToCourse}
              className="inline-flex items-center gap-2 rounded-full border border-blue-600 bg-[#2563EB] px-5 py-1.5 text-xs md:text-sm font-extrabold text-white shadow-md shadow-blue-200 hover:bg-blue-700 transition-all active:scale-95"
            >
              Quay lại
            </button>
          </div>
        </div>

        <ExamQuestionGrid
          total={review.questions.length}
          currentIndex={currentIndex}
          onSelect={setCurrentIndex}
          getStatus={(i) => (review.questions[i].isCorrect ? 'correct' : 'wrong')}
        />

        {rq && (
          <div className="flex-1 overflow-y-auto px-4 py-6">
            <ExamQuestionView
              index={currentIndex}
              questionText={rq.questionText}
              passage={rq.passage}
              options={rq.options}
              selectedOptionId={rq.selectedOptionId}
              mode="review"
              correctOptionId={rq.correctOptionId}
              isCorrect={rq.isCorrect}
              explanation={rq.explanation}
              reviewOptions={rq.options}
            />
          </div>
        )}

        <ExamNavBar
          currentIndex={currentIndex}
          total={review.questions.length}
          onPrev={() => setCurrentIndex((p) => p - 1)}
          onNext={() => setCurrentIndex((p) => p + 1)}
        />
      </div>
    );
  }

  // --- EXAM MODE ---
  if (phase !== 'exam' || !attempt) return null;
  const currentQuestion = attempt.questions[currentIndex];
  if (!currentQuestion) return null;

  return (
    <div ref={containerRef} className="flex h-screen flex-col bg-[#F8FAFC]">
      {/* Fullscreen enforcement overlay */}
      {showFullscreenOverlay && (
        <div className="fixed inset-0 z-[9999] flex flex-col items-center justify-center bg-slate-950/90 backdrop-blur-md p-6">
          <div className="w-full max-w-md rounded-3xl border border-amber-300/40 bg-slate-900 p-8 text-center shadow-2xl">
            <AlertTriangle size={56} className="mx-auto text-amber-400" />
            <h2 className="mt-4 text-2xl font-black text-white">Bạn đã thoát toàn màn hình!</h2>
            <p className="mt-2 text-sm font-bold text-slate-300">Bài thi yêu cầu chế độ toàn màn hình. Nhấn nút bên dưới để tiếp tục.</p>
            <p className="mt-3 inline-block rounded-full border border-amber-500/40 bg-amber-500/10 px-4 py-1 text-xs font-black text-amber-400">
              ⚠ Cảnh báo lần {fullscreenExits}/3 — Lần thứ 3 sẽ kết thúc bài thi
            </p>
            <button
              onClick={reEnterFullscreen}
              className="mt-6 w-full rounded-full border border-blue-500 bg-[#2563EB] px-8 py-3 text-base font-extrabold text-white shadow-lg shadow-blue-500/30 transition-all hover:bg-blue-600 active:scale-95"
            >
              Quay lại toàn màn hình
            </button>
          </div>
        </div>
      )}

      {/* Warning bar */}
      {tabWarnings > 0 && !showFullscreenOverlay && (
        <div className="shrink-0 border-b border-amber-200 bg-amber-50 px-4 py-2 text-center text-xs font-black text-amber-800 shadow-xs">
          ⚠ Cảnh báo: Đã thoát toàn màn hình {fullscreenExits}/3 lần. Lần thứ 3 sẽ kết thúc bài thi.
        </div>
      )}

      {/* Top bar */}
      <div className="flex shrink-0 items-center justify-between border-b border-sky-200 bg-white px-4 md:px-6 py-3 shadow-sm">
        <div className="flex items-center gap-4">
          <span className="text-sm font-black text-[#0b2554]">Câu {currentIndex + 1}/{attempt.questions.length}</span>
          <span className="text-xs font-bold text-slate-500 hidden sm:inline">Đã trả lời: {answeredCount}/{attempt.questions.length}</span>
        </div>

        <div className={`flex items-center gap-2 px-4 py-1.5 rounded-full font-black text-base md:text-lg tabular-nums transition-all ${
          isUrgent
            ? 'border-2 border-rose-500 bg-rose-50 text-rose-600 shadow-sm animate-pulse'
            : 'border border-sky-200 bg-sky-50/70 text-[#0b2554] shadow-sm'
        }`}>
          <Clock size={18} className={isUrgent ? 'text-rose-600' : 'text-[#2563EB]'} />
          {formatTime(timeLeft)}
        </div>

        <button
          onClick={() => { if (window.confirm('Bạn có chắc muốn nộp bài?')) handleSubmit(); }}
          className="inline-flex items-center gap-2 rounded-full border border-blue-600 bg-[#2563EB] px-5 py-1.5 text-xs md:text-sm font-extrabold uppercase tracking-wider text-white shadow-md shadow-blue-200 transition-all hover:bg-blue-700 active:scale-95"
        >
          <Send size={14} /> Nộp bài
        </button>
      </div>

      <ExamQuestionGrid
        total={attempt.questions.length}
        currentIndex={currentIndex}
        onSelect={setCurrentIndex}
        getStatus={(i) => {
          if (i === currentIndex) return 'current' as const;
          return selectedAnswers[attempt.questions[i].id] ? 'answered' : 'unanswered';
        }}
      />

      {/* Question content */}
      <div className="flex-1 overflow-y-auto px-4 py-6">
        <ExamQuestionView
          index={currentIndex}
          questionText={currentQuestion.questionText}
          passage={currentQuestion.passage}
          options={currentQuestion.options}
          selectedOptionId={selectedAnswers[currentQuestion.id]}
          mode="exam"
          onSelectOption={(optionId) => selectAnswer(currentQuestion.id, optionId)}
        />
      </div>

      <ExamNavBar
        currentIndex={currentIndex}
        total={attempt.questions.length}
        onPrev={() => setCurrentIndex((p) => p - 1)}
        onNext={() => setCurrentIndex((p) => p + 1)}
      />
    </div>
  );
};
