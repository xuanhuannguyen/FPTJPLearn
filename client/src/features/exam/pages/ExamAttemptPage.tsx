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
      <div ref={containerRef} className="flex min-h-screen flex-col items-center justify-center bg-[#F8FAFC] px-4">
        <div className="w-full max-w-lg border-2 border-slate-900 bg-white p-8 shadow-[8px_8px_0_#111827]">
          <div className="mb-6 flex items-center gap-3">
            <Clock size={32} className="text-[#2563EB]" />
            <h1 className="text-2xl font-black text-slate-900">Sẵn sàng luyện thi?</h1>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="border-2 border-slate-900 bg-[#EFF6FF] p-4">
              <p className="text-3xl font-black text-slate-900">{attempt.totalQuestions}</p>
              <p className="text-xs font-black uppercase tracking-widest text-slate-500">Câu hỏi</p>
            </div>
            <div className="border-2 border-slate-900 bg-[#EFF6FF] p-4">
              <p className="text-3xl font-black text-slate-900">{attempt.durationMinutes}'</p>
              <p className="text-xs font-black uppercase tracking-widest text-slate-500">Thời gian</p>
            </div>
          </div>

          <ul className="mt-6 space-y-2 text-sm font-bold text-slate-700">
            <li className="flex items-start gap-2"><AlertTriangle size={16} className="mt-0.5 shrink-0 text-amber-500" /> Chuyển tab sẽ bị ghi nhận cảnh báo</li>
            <li className="flex items-start gap-2"><Maximize size={16} className="mt-0.5 shrink-0 text-[#2563EB]" /> Bài thi sẽ mở toàn màn hình</li>
            <li className="flex items-start gap-2"><Clock size={16} className="mt-0.5 shrink-0 text-slate-500" /> Hết giờ sẽ tự động nộp bài</li>
          </ul>

          <div className="mt-8 flex gap-3">
            <button onClick={navigateBackToCourse} className="flex-1 border-2 border-slate-900 bg-white px-4 py-3 font-black text-slate-900 shadow-[4px_4px_0_#111827] transition-all hover:translate-x-[2px] hover:translate-y-[2px] hover:shadow-[2px_2px_0_#111827]">
              Hủy
            </button>
            <button onClick={startExam} className="flex-1 border-2 border-slate-900 bg-[#2563EB] px-4 py-3 font-black text-white shadow-[4px_4px_0_#111827] transition-all hover:translate-x-[2px] hover:translate-y-[2px] hover:shadow-[2px_2px_0_#111827]">
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
        <div className="flex shrink-0 items-center justify-between border-b-2 border-slate-900 bg-white px-4 py-2">
          <div className="flex items-center gap-4">
            <span className={`text-2xl font-black ${passed ? 'text-emerald-600' : 'text-red-500'}`}>{review.scorePercent}%</span>
            <span className="text-sm font-black text-slate-500">{review.correctCount}/{review.totalQuestions} đúng</span>
            {tabWarnings > 0 && <span className="text-xs font-bold text-amber-600">• {tabWarnings} cảnh báo</span>}
          </div>
          <div className="flex items-center gap-3">
            <span className={`border-2 px-3 py-1 text-sm font-black ${passed ? 'border-emerald-600 bg-emerald-50 text-emerald-700' : 'border-red-500 bg-red-50 text-red-600'}`}>
              {passed ? 'ĐẠT' : 'CHƯA ĐẠT'}
            </span>
            <button onClick={navigateBackToCourse} className="inline-flex items-center gap-2 border-2 border-slate-900 bg-[#2563EB] px-4 py-1.5 text-sm font-black text-white shadow-[3px_3px_0_#111827] transition-all hover:translate-x-[1px] hover:translate-y-[1px] hover:shadow-[2px_2px_0_#111827]">
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
        <div className="fixed inset-0 z-[9999] flex flex-col items-center justify-center bg-slate-900/95">
          <AlertTriangle size={56} className="text-amber-400" />
          <h2 className="mt-4 text-2xl font-black text-white">Bạn đã thoát toàn màn hình!</h2>
          <p className="mt-2 text-sm font-bold text-slate-300">Bài thi yêu cầu chế độ toàn màn hình. Nhấn nút bên dưới để tiếp tục.</p>
          <p className="mt-1 text-xs font-bold text-amber-400">⚠ Cảnh báo lần {fullscreenExits}/3 — Lần thứ 3 sẽ kết thúc bài thi</p>
          <button onClick={reEnterFullscreen} className="mt-6 border-2 border-white bg-[#2563EB] px-8 py-3 text-lg font-black text-white shadow-[4px_4px_0_#fff] transition-all hover:translate-x-[2px] hover:translate-y-[2px] hover:shadow-[2px_2px_0_#fff]">
            Quay lại toàn màn hình
          </button>
        </div>
      )}

      {/* Warning bar */}
      {tabWarnings > 0 && !showFullscreenOverlay && (
        <div className="shrink-0 border-b-2 border-amber-600 bg-amber-50 px-4 py-1.5 text-center text-xs font-black text-amber-800">
          ⚠ Cảnh báo: Đã thoát toàn màn hình {fullscreenExits}/3 lần. Lần thứ 3 sẽ kết thúc bài thi.
        </div>
      )}

      {/* Top bar */}
      <div className="flex shrink-0 items-center justify-between border-b-2 border-slate-900 bg-white px-4 py-2">
        <div className="flex items-center gap-4">
          <span className="text-sm font-black text-slate-500">Câu {currentIndex + 1}/{attempt.questions.length}</span>
          <span className="text-xs font-bold text-slate-400">Đã trả lời: {answeredCount}/{attempt.questions.length}</span>
        </div>
        <div className={`flex items-center gap-2 border-2 px-3 py-1 text-lg font-black tabular-nums ${isUrgent ? 'border-red-500 bg-red-50 text-red-600 animate-pulse' : 'border-slate-900 bg-white text-slate-900'}`}>
          <Clock size={18} />
          {formatTime(timeLeft)}
        </div>
        <button
          onClick={() => { if (window.confirm('Bạn có chắc muốn nộp bài?')) handleSubmit(); }}
          className="inline-flex items-center gap-2 border-2 border-slate-900 bg-[#2563EB] px-4 py-1.5 text-sm font-black text-white shadow-[3px_3px_0_#111827] transition-all hover:translate-x-[1px] hover:translate-y-[1px] hover:shadow-[2px_2px_0_#111827]"
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
