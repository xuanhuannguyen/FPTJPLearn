import { useEffect, useState, useCallback } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import {
  ArrowLeft,
  Loader2,
  Lock,
  Play,
  Volume2,
  VolumeX,
  BookOpen,
  MessageSquare,
  Image as ImageIcon,
  HelpCircle,
  ChevronLeft,
  ChevronRight,
  AlertCircle,
  Lightbulb,
  Undo2,
  Shuffle,
  Sparkles
} from 'lucide-react';
import { speakingApi } from '../api/speakingApi';
import type { SpeakingLesson, QaLessonDetail, QaQuestion } from '../types/speaking.types';
import { useUserAccess } from '../../../shared/hooks/useUserAccess';
import { jpd113QaLessons, jpd123QaLessons } from '../data/qaData';

export const SpeakingCoursePage = () => {
  const { courseCode } = useParams<{ courseCode: string }>();
  const navigate = useNavigate();
  const [lessons, setLessons] = useState<SpeakingLesson[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');
  const { isContentLocked } = useUserAccess();

  // Navigation states
  const [selectedType, setSelectedType] = useState<'reading' | 'qa' | null>(null);
  const [selectedQaLesson, setSelectedQaLesson] = useState<number | null>(null);
  const [selectedQaMode, setSelectedQaMode] = useState<'no_image' | 'with_image' | null>(null);
  const [selectedPictureId, setSelectedPictureId] = useState<string | null>(null);

  // Q&A Dynamic Data states
  const [qaLessonDetail, setQaLessonDetail] = useState<QaLessonDetail | null>(null);
  const [isLoadingQaDetail, setIsLoadingQaDetail] = useState(false);
  const [qaDetailError, setQaDetailError] = useState('');

  // Interactive practice states
  const [isStudyingQa, setIsStudyingQa] = useState(false);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [showQaGuide, setShowQaGuide] = useState(false);
  const [showQaMeaning, setShowQaMeaning] = useState(false);

  // Shuffle states
  const [isShuffled, setIsShuffled] = useState(false);
  const [shuffledQuestions, setShuffledQuestions] = useState<QaQuestion[]>([]);

  // TTS Speech Synthesis states
  const [voices, setVoices] = useState<SpeechSynthesisVoice[]>([]);
  const [selectedVoiceName, setSelectedVoiceName] = useState<string>('');
  const [isSpeakingQuestion, setIsSpeakingQuestion] = useState(false);
  const [speakingSampleId, setSpeakingSampleId] = useState<string | null>(null);
  const [speakingVocabWord, setSpeakingVocabWord] = useState<string | null>(null);

  // Load standard speaking reading lessons
  useEffect(() => {
    if (!courseCode) return;
    let cancelled = false;

    const loadLessons = async () => {
      try {
        setIsLoading(true);
        setError('');
        const data = await speakingApi.getLessonsByCourse(courseCode);
        if (!cancelled) setLessons(data);
      } catch (err) {
        console.error(err);
        if (!cancelled) setError('Không tải được bài học.');
      } finally {
        if (!cancelled) setIsLoading(false);
      }
    };

    void loadLessons();
    return () => {
      cancelled = true;
    };
  }, [courseCode]);

  // Load Q&A lesson details dynamically based on lesson index and image mode
  useEffect(() => {
    if (!selectedQaMode || !selectedQaLesson || !courseCode) {
      return;
    }

    let cancelled = false;
    const loadQaDetail = async () => {
      try {
        setIsLoadingQaDetail(true);
        setQaDetailError('');
        const response = await fetch(`/data/speaking/${courseCode}/qa/lesson${selectedQaLesson}_${selectedQaMode}.json`);
        if (!response.ok) {
          throw new Error('Dữ liệu đang được biên soạn.');
        }
        const data = (await response.json()) as QaLessonDetail;
        if (!cancelled) {
          setQaLessonDetail(data);
        }
      } catch (err) {
        console.error(err);
        if (!cancelled) {
          setQaDetailError('Dữ liệu phần vấn đáp này đang được cập nhật. Vui lòng quay lại sau!');
        }
      } finally {
        if (!cancelled) {
          setIsLoadingQaDetail(false);
        }
      }
    };

    void loadQaDetail();
    return () => {
      cancelled = true;
    };
  }, [selectedQaMode, selectedQaLesson, courseCode]);

  // Fetch TTS Voices
  useEffect(() => {
    if (typeof window === 'undefined' || !('speechSynthesis' in window)) return;

    const updateVoices = () => {
      const allVoices = window.speechSynthesis.getVoices();
      const jaVoices = allVoices.filter((v) => v.lang.startsWith('ja'));
      setVoices(jaVoices);
      if (jaVoices.length > 0 && !selectedVoiceName) {
        const defaultJaVoice = jaVoices.find((v) => v.name.includes('Google') || v.name.includes('Haruka') || v.name.includes('Kyoko')) || jaVoices[0];
        setSelectedVoiceName(defaultJaVoice.name);
      }
    };

    updateVoices();
    window.speechSynthesis.onvoiceschanged = updateVoices;
    return () => {
      window.speechSynthesis.onvoiceschanged = null;
    };
  }, [selectedVoiceName]);

  // Speech Helper functions
  const speakText = useCallback((text: string, onStart?: () => void, onEnd?: () => void) => {
    if (typeof window === 'undefined' || !('speechSynthesis' in window)) return;
    window.speechSynthesis.cancel();

    const utterance = new SpeechSynthesisUtterance(text);
    utterance.lang = 'ja-JP';
    utterance.rate = 0.82;
    utterance.pitch = 1;

    if (selectedVoiceName) {
      const voice = voices.find((v) => v.name === selectedVoiceName);
      if (voice) utterance.voice = voice;
    }

    if (onStart) utterance.onstart = onStart;
    if (onEnd) {
      utterance.onend = onEnd;
      utterance.onerror = onEnd;
    } else {
      utterance.onend = () => {};
      utterance.onerror = () => {};
    }

    window.speechSynthesis.speak(utterance);
  }, [selectedVoiceName, voices]);

  const stopSpeaking = useCallback(() => {
    if (typeof window === 'undefined' || !('speechSynthesis' in window)) return;
    window.speechSynthesis.cancel();
    setIsSpeakingQuestion(false);
    setSpeakingSampleId(null);
    setSpeakingVocabWord(null);
  }, []);

  // Cleanup speech synthesis on component unmount
  useEffect(() => {
    return () => {
      if (typeof window !== 'undefined' && 'speechSynthesis' in window) {
        window.speechSynthesis.cancel();
      }
    };
  }, []);

  const readingLessons = lessons.filter((l) => l.lessonType === 'reading');
  const activeQaLessons = courseCode === 'jpd113' ? jpd113QaLessons : jpd123QaLessons;
  const selectedQaLessonData = activeQaLessons.find((l) => l.id === selectedQaLesson);

  // Compute original questions for study
  let originalQuestions: QaQuestion[] = [];
  if (selectedQaMode === 'no_image' && qaLessonDetail?.sections) {
    originalQuestions = qaLessonDetail.sections.flatMap((s) => s.questionList);
  } else if (selectedQaMode === 'with_image' && selectedPictureId && qaLessonDetail?.pictureSets) {
    const activePictureSet = qaLessonDetail.pictureSets.find((p) => p.pictureId === selectedPictureId);
    originalQuestions = activePictureSet ? activePictureSet.questions : [];
  }

  const allQuestions = isShuffled ? shuffledQuestions : originalQuestions;
  const currentQuestion = allQuestions[currentQuestionIndex];
  const shouldShowQuestionText = showQaGuide;
  const shouldShowQuestionAudio = selectedQaMode !== null;

  // Picture Set Details for visual Practice Cards
  const activePictureSet = selectedQaMode === 'with_image' && qaLessonDetail?.pictureSets
    ? qaLessonDetail.pictureSets.find((p) => p.pictureId === selectedPictureId)
    : null;
  const activeImageUrl = activePictureSet ? activePictureSet.imageUrl : null;

  // Toggle Shuffle mode
  const handleToggleShuffle = () => {
    stopSpeaking();
    if (!isShuffled) {
      const arr = [...originalQuestions];
      for (let i = arr.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [arr[i], arr[j]] = [arr[j], arr[i]];
      }
      setShuffledQuestions(arr);
      setIsShuffled(true);
      setCurrentQuestionIndex(0);
      setShowQaGuide(false);
      setShowQaMeaning(false);
    } else {
      setIsShuffled(false);
      setCurrentQuestionIndex(0);
      setShowQaGuide(false);
      setShowQaMeaning(false);
    }
  };

  const handlePrevQuestion = () => {
    if (currentQuestionIndex > 0) {
      stopSpeaking();
      setCurrentQuestionIndex((prev) => prev - 1);
      setShowQaGuide(false);
      setShowQaMeaning(false);
    }
  };

  const handleNextQuestion = () => {
    if (currentQuestionIndex < allQuestions.length - 1) {
      stopSpeaking();
      setCurrentQuestionIndex((prev) => prev + 1);
      setShowQaGuide(false);
      setShowQaMeaning(false);
    }
  };

  // Keyboard navigation for Q&A practice
  useEffect(() => {
    if (!isStudyingQa || allQuestions.length === 0) return;

    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.target instanceof HTMLInputElement || e.target instanceof HTMLTextAreaElement) return;

      if (e.key === 'ArrowLeft') {
        if (currentQuestionIndex > 0) {
          stopSpeaking();
          setCurrentQuestionIndex((prev) => prev - 1);
          setShowQaGuide(false);
          setShowQaMeaning(false);
        }
      } else if (e.key === 'ArrowRight') {
        if (currentQuestionIndex < allQuestions.length - 1) {
          stopSpeaking();
          setCurrentQuestionIndex((prev) => prev + 1);
          setShowQaGuide(false);
          setShowQaMeaning(false);
        }
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [isStudyingQa, allQuestions.length, currentQuestionIndex, stopSpeaking]);

  const handleBack = () => {
    stopSpeaking();
    if (isStudyingQa) {
      if (selectedQaMode === 'with_image') {
        setIsStudyingQa(false);
        setSelectedPictureId(null);
        setIsShuffled(false);
      } else {
        setIsStudyingQa(false);
        setIsShuffled(false);
      }
      setCurrentQuestionIndex(0);
      setShowQaGuide(false);
      setShowQaMeaning(false);
    } else if (selectedQaMode !== null) {
      setSelectedQaMode(null);
      setQaLessonDetail(null);
    } else if (selectedQaLesson !== null) {
      setSelectedQaLesson(null);
      setQaLessonDetail(null);
    } else if (selectedType !== null) {
      setSelectedType(null);
    } else {
      navigate('/speaking');
    }
  };


  const getBackBtnText = () => {
    if (isStudyingQa) {
      return selectedQaMode === 'with_image' ? 'Quay lại chọn tranh' : 'Quay lại danh sách câu hỏi';
    }
    if (selectedQaMode !== null) return 'Quay lại chọn chế độ';
    if (selectedQaLesson !== null) return 'Quay lại danh sách bài học';
    if (selectedType !== null) return 'Quay lại chọn phần học';
    return 'Back to Speaking';
  };

  return (
    <div className="mx-auto max-w-[1295px] space-y-6 px-4 pb-14 animate-fade-in md:px-6">
      <header className="space-y-4">
        <button
          onClick={handleBack}
          className="group inline-flex items-center gap-2 text-sm font-black text-accent-primary transition-colors hover:text-accent-hover"
        >
          <ArrowLeft size={16} className="transition-transform group-hover:-translate-x-1" />
          {getBackBtnText()}
        </button>

        <div className="py-2">
          <h1 className="text-[32px] font-black leading-none tracking-tight text-text-primary">
            Luyện nói {courseCode?.toUpperCase()}
          </h1>
          <p className="mt-1 text-sm font-bold uppercase tracking-widest text-text-secondary">
            {isStudyingQa
              ? `Luyện phản xạ Q&A - Câu ${currentQuestionIndex + 1}/${allQuestions.length}`
              : selectedQaMode === 'no_image'
              ? `Vấn đáp không tranh - Bài học ${selectedQaLesson} - ${selectedQaLessonData?.title}`
              : selectedQaMode === 'with_image'
              ? `Vấn đáp có tranh - Bài học ${selectedQaLesson} - ${selectedQaLessonData?.title}`
              : selectedQaLesson !== null
              ? `Bài học ${selectedQaLesson} - ${selectedQaLessonData?.title}`
              : selectedType === 'reading'
              ? 'Danh sách bài đọc luyện nói'
              : selectedType === 'qa'
              ? 'Danh sách bài vấn đáp'
              : 'Chọn phần học để bắt đầu'}
          </p>
        </div>
      </header>

      {error ? (
        <div className="border-2 border-accent-danger bg-accent-danger/10 p-4 text-sm font-bold text-accent-danger">
          {error}
        </div>
      ) : null}

      {isLoading ? (
        <div className="flex h-64 flex-col items-center justify-center text-text-secondary">
          <Loader2 size={36} className="mb-4 animate-spin text-accent-primary" />
          <p className="font-bold">Đang tải bài đọc...</p>
        </div>
      ) : (
        <>
          {/* Trạng thái 1: Chưa chọn Phần (Đọc vs Vấn đáp) */}
          {selectedType === null && (
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6 max-w-4xl mx-auto pt-6">
              <button
                onClick={() => setSelectedType('reading')}
                className="interactive-surface text-left flex flex-col p-6 rounded-[24px] border-2 border-slate-900 bg-[#FFF7ED] shadow-[6px_6px_0_#111827] hover:-translate-y-1 transition-all"
              >
                <div className="flex h-14 w-14 items-center justify-center rounded-2xl border-2 border-slate-900 bg-[#8B3A22] text-[#FFF7ED] shadow-[3px_3px_0_#111827] mb-6">
                  <BookOpen size={28} />
                </div>
                <h3 className="font-heading text-2xl font-black text-slate-900">
                  Phần 1: Luyện Đọc Thành Tiếng
                </h3>
                <p className="mt-3 text-sm font-bold text-slate-600 leading-relaxed flex-grow">
                  Luyện đọc các đoạn văn tiếng Nhật theo từng câu. Tích hợp âm thanh phát âm chuẩn, cách đọc Kanji và dịch nghĩa đầy đủ.
                </p>
                <div className="mt-6 inline-flex items-center gap-1.5 text-xs font-black uppercase tracking-wider text-[#8B3A22]">
                  <span>{readingLessons.length} bài đọc</span>
                  <span>•</span>
                  <span>Bắt đầu →</span>
                </div>
              </button>

              <button
                onClick={() => setSelectedType('qa')}
                className="interactive-surface text-left flex flex-col p-6 rounded-[24px] border-2 border-slate-900 bg-[#EFF6FF] shadow-[6px_6px_0_#111827] hover:-translate-y-1 transition-all"
              >
                <div className="flex h-14 w-14 items-center justify-center rounded-2xl border-2 border-slate-900 bg-blue-600 text-white shadow-[3px_3px_0_#111827] mb-6">
                  <MessageSquare size={28} />
                </div>
                <h3 className="font-heading text-2xl font-black text-slate-900">
                  Phần 2: Luyện Phản Xạ Vấn Đáp (Q&A)
                </h3>
                <p className="mt-3 text-sm font-bold text-slate-600 leading-relaxed flex-grow">
                  Luyện phản xạ nghe câu hỏi ngắn của giám khảo (có/không tranh) và tự đưa ra câu trả lời nhanh chóng dựa trên từ khóa.
                </p>
                <div className="mt-6 inline-flex items-center gap-1.5 text-xs font-black uppercase tracking-wider text-blue-600">
                  <span>{activeQaLessons.length} bài phản xạ</span>
                  <span>•</span>
                  <span>Bắt đầu →</span>
                </div>
              </button>
            </div>
          )}

          {/* Trạng thái 2: Xem danh sách bài Đọc */}
          {selectedType === 'reading' && (
            <div className="grid grid-cols-1 gap-5 md:grid-cols-2 lg:grid-cols-3">
              {readingLessons.map((lesson) => {
                const isLocked = isContentLocked(lesson);
                return (
                  <Link
                    key={lesson.id}
                    to={isLocked ? '/pricing' : `/speaking/${courseCode}/lessons/${lesson.id}`}
                    className={`interactive-surface group relative flex min-h-[118px] cursor-pointer gap-4 overflow-hidden rounded-[18px] p-3.5 ${
                      isLocked ? 'grayscale-[0.35]' : ''
                    }`}
                  >
                    <div className="absolute left-0 top-0 h-full w-2 bg-[#8B3A22]" />

                    <div className="relative flex h-24 w-[68px] shrink-0 items-center justify-center rounded-xl border-2 border-slate-900 bg-[#FFF7ED] text-[#8B3A22] shadow-[3px_3px_0_#111827]">
                      <Volume2 size={28} />
                    </div>

                    <div className="relative flex min-w-0 flex-1 flex-col justify-center">
                      <div className="flex items-start justify-between gap-3">
                        <div className="min-w-0 flex-1">
                          <p className="text-[11px] font-black uppercase tracking-[0.16em] text-text-muted">
                            Lesson {lesson.lessonNumber}
                          </p>
                          <h3 className="mt-0.5 line-clamp-1 font-heading text-xl font-black leading-none tracking-tight text-text-primary transition-colors group-hover:text-accent-primary">
                            {lesson.title}
                          </h3>

                          {lesson.description ? (
                            <p className="mt-1.5 line-clamp-2 text-xs font-bold leading-5 text-text-secondary">
                              {lesson.description}
                            </p>
                          ) : null}

                          <p className="mt-2 text-[11px] font-black uppercase tracking-[0.14em] text-[#8B3A22]">
                            {isLocked ? 'Cần kích hoạt gói Premium' : `${lesson.sentenceCount} câu đọc`}
                          </p>
                        </div>

                        {isLocked ? (
                          <span className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg border-2 border-border bg-bg-tertiary text-text-muted">
                            <Lock size={16} />
                          </span>
                        ) : (
                          <span className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg border-2 border-border bg-accent-cta text-white shadow-pop transition-all group-hover:-translate-y-0.5">
                            <Play size={16} className="ml-0.5 fill-current" />
                          </span>
                        )}
                      </div>
                    </div>
                  </Link>
                );
              })}
            </div>
          )}

          {/* Trạng thái 3: Xem danh sách bài Vấn Đáp (Q&A) */}
          {selectedType === 'qa' && selectedQaLesson === null && (
            <div className="grid grid-cols-1 gap-5 md:grid-cols-2 lg:grid-cols-3">
              {activeQaLessons.map((lesson) => {
                const isLocked = !lesson.overview;
                return (
                  <button
                    key={lesson.id}
                    onClick={() => !isLocked && setSelectedQaLesson(lesson.id)}
                    disabled={isLocked}
                    className={`interactive-surface text-left flex gap-4 overflow-hidden rounded-[18px] p-3.5 border-2 border-slate-900 bg-white shadow-[4px_4px_0_#111827] transition-all ${
                      isLocked ? 'opacity-60 cursor-not-allowed bg-slate-50' : 'hover:-translate-y-0.5'
                    }`}
                  >
                    <div className="relative flex h-20 w-16 shrink-0 items-center justify-center rounded-xl border-2 border-slate-900 bg-[#E0F2FE] text-blue-600 shadow-[2px_2px_0_#111827]">
                      <MessageSquare size={24} />
                    </div>

                    <div className="min-w-0 flex-grow flex flex-col justify-center">
                      <p className="text-[10px] font-black uppercase tracking-[0.16em] text-slate-400">
                        Lesson {lesson.id}
                      </p>
                      <h3 className="mt-0.5 font-heading text-lg font-black leading-tight text-slate-900">
                        {lesson.title}
                      </h3>
                      <p className="mt-1 line-clamp-2 text-xs font-bold leading-relaxed text-slate-500">
                        {lesson.description}
                      </p>
                      <p className="mt-1.5 text-[10px] font-black uppercase tracking-[0.14em] text-blue-600">
                        {isLocked ? 'Đang cập nhật' : 'Sẵn sàng học'}
                      </p>
                    </div>
                  </button>
                );
              })}
            </div>
          )}

          {/* Trạng thái 4: Đã chọn bài vấn đáp, hiển thị thông tin bài học và lựa chọn chế độ Có/Không tranh */}
          {selectedType === 'qa' && selectedQaLesson !== null && (
            <div className="space-y-6 animate-fade-in max-w-5xl">
              {/* Trạng thái 4.1: Chưa chọn chế độ vấn đáp */}
              {selectedQaMode === null && (
                <>
                  <div className="rounded-[24px] border-2 border-slate-900 bg-white p-6 shadow-[5px_5px_0_#111827]">
                    <div className="flex items-center gap-3 mb-4">
                      <span className="rounded-full bg-blue-100 px-3 py-1 text-xs font-black text-blue-700 border-2 border-slate-900">
                        Vấn đáp - Bài {selectedQaLesson}
                      </span>
                      <h2 className="font-heading text-2xl font-black text-slate-900">
                        {selectedQaLessonData?.title}
                      </h2>
                    </div>

                    {selectedQaLessonData?.overview && (
                      <div className="space-y-5">
                        <div className="border-l-4 border-blue-600 pl-4 py-1">
                          <h4 className="text-sm font-black uppercase tracking-wider text-blue-600">Tổng quan bài học</h4>
                          <p className="mt-1 text-sm font-bold text-slate-700 leading-relaxed">
                            {selectedQaLessonData.overview.shortSummary}
                          </p>
                        </div>

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4 pt-2">
                          <div className="rounded-xl border border-slate-200 bg-slate-50/50 p-4">
                            <h5 className="text-xs font-black uppercase tracking-wider text-slate-500 mb-2">Sinh viên có thể</h5>
                            <ul className="list-disc pl-5 space-y-1">
                              {selectedQaLessonData.overview.studentCanDo.map((item, idx) => (
                                <li key={idx} className="text-xs font-bold text-slate-600 leading-relaxed">{item}</li>
                              ))}
                            </ul>
                          </div>

                          <div className="rounded-xl border border-slate-200 bg-slate-50/50 p-4">
                            <h5 className="text-xs font-black uppercase tracking-wider text-slate-500 mb-2">Trọng tâm ngữ pháp</h5>
                            <div className="flex flex-wrap gap-1.5 mt-2">
                              {selectedQaLessonData.overview.mainGrammarFocus.map((grammar, idx) => (
                                <span key={idx} className="rounded-md border border-slate-200 bg-white px-2 py-0.5 text-xs font-bold text-slate-700">
                                  {grammar}
                                </span>
                              ))}
                            </div>
                          </div>
                        </div>

                        <div className="rounded-xl border border-amber-200 bg-amber-50/30 p-4 text-xs font-bold text-amber-900 leading-relaxed">
                          <span className="text-amber-700 font-black uppercase tracking-wider block mb-1">Mẹo thi vấn đáp:</span>
                          {selectedQaLessonData.overview.examTipSummary}
                        </div>
                      </div>
                    )}
                  </div>

                  <div className="grid grid-cols-1 sm:grid-cols-2 gap-6 mt-6">
                    <button
                      onClick={() => setSelectedQaMode('no_image')}
                      className="interactive-surface text-left flex flex-col p-5 rounded-[20px] border-2 border-slate-900 bg-white shadow-[4px_4px_0_#111827] hover:-translate-y-0.5 transition-all"
                    >
                      <div className="flex h-12 w-12 items-center justify-center rounded-xl border-2 border-slate-900 bg-emerald-100 text-emerald-600 shadow-[2px_2px_0_#111827] mb-4">
                        <HelpCircle size={24} />
                      </div>
                      <h4 className="font-heading text-xl font-black text-slate-900">
                        Chế độ 1: Vấn đáp không tranh
                      </h4>
                      <p className="mt-2 text-xs font-bold text-slate-500 leading-relaxed flex-grow">
                        {selectedQaLessonData?.noImageDesc || "Luyện phản xạ nói trực tiếp thông qua các câu hỏi giao tiếp."}
                      </p>
                      <span className="mt-4 inline-flex items-center text-xs font-black uppercase tracking-wider text-emerald-600">
                        {selectedQaLessonData?.noImageQuestionCount || 0} câu hỏi • Bắt đầu →
                      </span>
                    </button>

                    <button
                      onClick={() => setSelectedQaMode('with_image')}
                      className="interactive-surface text-left flex flex-col p-5 rounded-[20px] border-2 border-slate-900 bg-white shadow-[4px_4px_0_#111827] hover:-translate-y-0.5 transition-all"
                    >
                      <div className="flex h-12 w-12 items-center justify-center rounded-xl border-2 border-slate-900 bg-indigo-100 text-indigo-600 shadow-[2px_2px_0_#111827] mb-4">
                        <ImageIcon size={24} />
                      </div>
                      <h4 className="font-heading text-xl font-black text-slate-900">
                        Chế độ 2: Vấn đáp có tranh
                      </h4>
                      <p className="mt-2 text-xs font-bold text-slate-500 leading-relaxed flex-grow">
                        {selectedQaLessonData?.withImageDesc || "Quan sát hình vẽ và trả lời các câu hỏi vấn đáp xoay quanh thông tin trong tranh."}
                      </p>
                      <span className="mt-4 inline-flex items-center text-xs font-black uppercase tracking-wider text-indigo-600">
                        {selectedQaLessonData?.withImagePictureCount || 0} bức tranh ({selectedQaLessonData?.withImageQuestionCount || 0} câu) • Bắt đầu →
                      </span>
                    </button>
                  </div>
                </>
              )}

              {/* Loader hoặc báo lỗi trong quá trình tải Q&A Detail */}
              {selectedQaMode !== null && isLoadingQaDetail && (
                <div className="flex h-64 flex-col items-center justify-center text-text-secondary bg-white rounded-[24px] border-2 border-slate-900 shadow-[4px_4px_0_#111827]">
                  <Loader2 size={36} className="mb-4 animate-spin text-accent-primary" />
                  <p className="font-bold">Đang tải dữ liệu vấn đáp...</p>
                </div>
              )}

              {selectedQaMode !== null && !isLoadingQaDetail && qaDetailError && (
                <div className="rounded-[24px] border-2 border-slate-900 bg-white p-8 text-center shadow-[4px_4px_0_#111827]">
                  <div className="flex h-16 w-16 items-center justify-center rounded-full border-2 border-slate-900 bg-rose-50 text-rose-600 mx-auto mb-4">
                    <AlertCircle size={28} />
                  </div>
                  <h3 className="font-heading text-xl font-black text-slate-900">{qaDetailError}</h3>
                  <button
                    onClick={() => {
                      setSelectedQaMode(null);
                      setQaLessonDetail(null);
                    }}
                    className="mt-6 rounded-xl border-2 border-slate-900 bg-white px-5 py-2 text-xs font-black text-slate-900 shadow-[3px_3px_0_#111827] transition-all hover:-translate-y-0.5"
                  >
                    Quay lại chọn chế độ
                  </button>
                </div>
              )}

              {/* Trạng thái 4.2: VẤN ĐÁP KHÔNG TRANH - Xem danh sách câu hỏi */}
              {selectedQaMode === 'no_image' && !isLoadingQaDetail && !qaDetailError && !isStudyingQa && qaLessonDetail && (
                <div className="space-y-6">
                  <div className="rounded-[24px] border-2 border-slate-900 bg-white p-5 shadow-[4px_4px_0_#111827] flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
                    <div>
                      <h3 className="font-heading text-xl font-black text-slate-900">
                        Danh sách câu hỏi phản xạ
                      </h3>
                      <p className="mt-1 text-xs font-bold text-slate-500">
                        Bài học có tổng cộng {originalQuestions.length} câu hỏi vấn đáp không tranh.
                      </p>
                    </div>

                    <button
                      onClick={() => {
                        setIsStudyingQa(true);
                        setCurrentQuestionIndex(0);
                        setIsShuffled(false);
                        setShowQaGuide(false);
                        setShowQaMeaning(false);
                      }}
                      className="inline-flex h-12 items-center justify-center gap-2 rounded-full border-2 border-slate-900 bg-[#2b160f] px-6 text-sm font-black text-white shadow-[3px_3px_0_#111827] transition-all hover:translate-x-[1px] hover:translate-y-[1px] hover:shadow-[2px_2px_0_#111827]"
                    >
                      <Play size={16} className="fill-current" />
                      Luyện phản xạ ngay
                    </button>
                  </div>

                  {qaLessonDetail.sections?.map((section) => (
                    <div key={section.sectionId} className="space-y-3">
                      <div className="border-b border-slate-200 pb-1">
                        <h4 className="font-heading text-lg font-black text-slate-800">
                          {section.sectionTitle} - {section.sectionViTitle}
                        </h4>
                        <p className="text-[11px] font-bold text-slate-400">Mục tiêu: {section.sectionGoal}</p>
                      </div>

                      <div className="space-y-3">
                        {section.questionList.map((question) => (
                          <div
                            key={question.questionId}
                            className="flex items-start gap-4 p-4 rounded-xl border border-slate-200 bg-white shadow-sm"
                          >
                            <span className="flex h-7 w-7 shrink-0 items-center justify-center rounded-full border border-slate-300 bg-slate-50 text-xs font-black text-slate-500">
                              {question.order}
                            </span>
                            <div className="min-w-0 flex-1">
                              <p className="text-sm font-extrabold text-slate-900 leading-normal">{question.question.ja}</p>
                              <p className="mt-1 text-xs font-bold text-slate-500">{question.question.vi}</p>
                            </div>
                          </div>
                        ))}
                      </div>
                    </div>
                  ))}
                </div>
              )}

              {/* Trạng thái 4.3: VẤN ĐÁP CÓ TRANH - Hiển thị danh sách 2 bức tranh nhân vật */}
              {selectedQaMode === 'with_image' && !isLoadingQaDetail && !qaDetailError && !isStudyingQa && qaLessonDetail && (
                <div className="space-y-6">
                  <div className="rounded-[20px] border-2 border-slate-900 bg-slate-50/50 p-5 shadow-[4px_4px_0_#111827]">
                    <h3 className="font-heading text-xl font-black text-slate-900">
                      Chọn bức tranh để bắt đầu luyện tập
                    </h3>
                    <p className="mt-1 text-xs font-bold text-slate-500">
                      Bản {courseCode?.toUpperCase()} Bài {selectedQaLesson} có {qaLessonDetail.pictureSets?.length || 0} tranh thông tin. Vui lòng bấm chọn bức tranh để luyện phản xạ hỏi - đáp.
                    </p>
                  </div>

                  <div className="grid grid-cols-2 md:grid-cols-3 gap-4">
                    {qaLessonDetail.pictureSets?.map((picture) => (
                      <button
                        key={picture.pictureId}
                        onClick={() => {
                          setSelectedPictureId(picture.pictureId);
                          setIsStudyingQa(true);
                          setCurrentQuestionIndex(0);
                          setIsShuffled(false);
                          setShowQaGuide(false);
                          setShowQaMeaning(false);
                        }}
                        className="interactive-surface text-left flex flex-col p-3 rounded-2xl border-2 border-slate-900 bg-white shadow-[3px_3px_0_#111827] hover:-translate-y-1 transition-all"
                      >
                        {/* Bức tranh thu nhỏ bo góc */}
                        <div className="relative aspect-[4/3] w-full rounded-lg border border-slate-900 overflow-hidden bg-slate-100 shadow-[1.5px_1.5px_0_#111827] mb-3">
                          <img
                            src={picture.imageUrl}
                            alt={picture.pictureTitle}
                            className="w-full h-full object-cover"
                            loading="lazy"
                            decoding="async"
                          />
                        </div>

                        <h4 className="font-heading text-sm font-black text-slate-900 line-clamp-1">
                          {picture.pictureTitle}
                        </h4>
                        <p className="mt-0.5 text-[11px] font-bold text-slate-500">
                          Số câu hỏi: {picture.questions.length} câu hỏi vấn đáp
                        </p>

                        <span className="mt-3 w-full text-center py-1.5 rounded-lg border-2 border-slate-900 bg-indigo-600 text-white font-black text-xs shadow-[1.5px_1.5px_0_#111827] transition-all hover:bg-indigo-700">
                          Luyện tập tranh này
                        </span>
                      </button>
                    ))}
                  </div>
                </div>
              )}

              {/* Trạng thái 4.4: ĐANG HỌC VẤN ĐÁP TƯƠNG TÁC (Study Flow) */}
              {selectedQaMode !== null && !isLoadingQaDetail && !qaDetailError && isStudyingQa && currentQuestion && (
                <div className="space-y-6 animate-fade-in max-w-4xl mx-auto">
                  {/* Progress Bar & Header */}
                  <div className="space-y-2">
                    <div className="flex justify-between items-center text-xs font-black uppercase text-slate-500">
                      <span>Tiến trình phản xạ nói</span>
                      <span>{currentQuestionIndex + 1} / {allQuestions.length} câu</span>
                    </div>
                    <div className="w-full h-3.5 bg-white border-2 border-slate-900 rounded-full overflow-hidden shadow-[2px_2px_0_#111827]">
                      <div
                        className="h-full bg-indigo-600 transition-all duration-300 border-r border-slate-900"
                        style={{ width: `${((currentQuestionIndex + 1) / allQuestions.length) * 100}%` }}
                      />
                    </div>
                  </div>

                  {/* Speech & Tool Selection Settings Panel */}
                  <div className="flex flex-wrap items-center justify-between gap-3 p-4 rounded-[20px] border-2 border-slate-900 bg-[#F8FAFC] shadow-[3px_3px_0_#111827]">
                    <div className="flex items-center gap-2">
                      <span className="text-xs font-black text-slate-700 uppercase">Giọng đọc (TTS):</span>
                      <select
                        value={selectedVoiceName}
                        onChange={(e) => {
                          stopSpeaking();
                          setSelectedVoiceName(e.target.value);
                        }}
                        className="rounded-lg border border-slate-300 bg-white px-2 py-1 text-xs font-extrabold text-slate-700 focus:outline-none focus:ring-1 focus:ring-indigo-500"
                      >
                        {voices.map((voice) => (
                          <option key={voice.name} value={voice.name}>
                            {voice.name} ({voice.lang})
                          </option>
                        ))}
                        {voices.length === 0 && <option value="">Giọng mặc định hệ thống</option>}
                      </select>
                    </div>

                    <button
                      onClick={handleToggleShuffle}
                      className={`inline-flex items-center gap-1.5 h-8 px-3 rounded-lg border-2 text-xs font-black shadow-[2px_2px_0_#111827] transition-all active:translate-y-0.5 active:shadow-[1px_1px_0_#111827] ${
                        isShuffled
                          ? 'border-slate-900 bg-indigo-50 text-indigo-750'
                          : 'border-slate-900 bg-white text-slate-700 hover:bg-slate-50'
                      }`}
                    >
                      <Shuffle size={13} />
                      {isShuffled ? 'Bỏ trộn câu' : 'Trộn câu hỏi'}
                    </button>
                  </div>

                  {/* THẺ CÂU HỎI CHÍNH (MAIN FLASHCARD) */}
                  <div className="rounded-[24px] border-2 border-slate-900 bg-white shadow-[6px_6px_0_#111827] overflow-hidden flex flex-col items-center">
                    {/* Bức tranh TO ở phía trên câu hỏi (chỉ khi học vấn đáp có tranh) */}
                    {selectedQaMode === 'with_image' && activeImageUrl && (
                      <div className="w-full border-b-2 border-slate-900 bg-slate-50/50 p-4 flex justify-center items-center">
                        <div className="relative max-w-lg w-full rounded-xl border-2 border-slate-900 overflow-hidden bg-white shadow-[4px_4px_0_#111827]">
                          <img
                            src={activeImageUrl}
                            alt="Bức tranh luyện tập"
                            className="w-full max-h-[280px] object-contain mx-auto"
                            decoding="async"
                          />
                        </div>
                      </div>
                    )}

                    <div className="p-5 md:p-6 w-full flex flex-col items-center">
                      {shouldShowQuestionAudio && (
                        <>
                          {shouldShowQuestionText && (
                            <div className="flex items-center justify-between w-full mb-3">
                              <span className="text-xs font-black uppercase tracking-[0.16em] text-slate-400">
                                Câu hỏi {currentQuestionIndex + 1}
                              </span>
                              <button
                                onClick={() => setShowQaMeaning((v) => !v)}
                                className="text-xs font-black text-blue-600 hover:text-blue-800 transition"
                              >
                                {showQaMeaning ? 'Ẩn nghĩa câu hỏi' : 'Hiện nghĩa câu hỏi'}
                              </button>
                            </div>
                          )}

                          <div className="flex flex-col items-center justify-center text-center space-y-4 py-2 w-full">
                            {/* Loa đọc câu hỏi - Kích thước thu gọn */}
                            <button
                              type="button"
                              onClick={() => {
                                if (isSpeakingQuestion) {
                                  stopSpeaking();
                                } else {
                                  setIsSpeakingQuestion(true);
                                  speakText(
                                    currentQuestion.question.ja,
                                    () => setIsSpeakingQuestion(true),
                                    () => setIsSpeakingQuestion(false)
                                  );
                                }
                              }}
                              className={`flex h-16 w-16 shrink-0 items-center justify-center rounded-full border-2 border-slate-900 shadow-[3px_3px_0_#111827] transition-all hover:-translate-y-0.5 hover:shadow-[4px_4px_0_#111827] active:translate-y-0 active:shadow-[2px_2px_0_#111827] ${
                                isSpeakingQuestion ? 'bg-rose-50 text-rose-600' : 'bg-blue-50 text-blue-600'
                              }`}
                            >
                              {isSpeakingQuestion ? <VolumeX size={28} /> : <Volume2 size={28} />}
                            </button>

                            {shouldShowQuestionText && (
                              <div className="space-y-2">
                                <h2 className="text-xl md:text-2xl font-black text-slate-900 tracking-wide select-all">
                                  {currentQuestion.question.ja}
                                </h2>
                                {showQaMeaning && (
                                  <p className="text-sm font-bold text-slate-500 leading-normal">
                                    {currentQuestion.question.vi}
                                  </p>
                                )}
                              </div>
                            )}
                          </div>
                        </>
                      )}

                      {/* Nút Hướng Dẫn/Ẩn Hướng Dẫn - Kích thước nhỏ gọn */}
                      <div className={`${shouldShowQuestionText ? 'mt-4 border-t border-slate-100 pt-4' : ''} w-full flex justify-center`}>
                        <button
                          onClick={() => setShowQaGuide((g) => !g)}
                          className={`inline-flex items-center gap-1.5 px-4 py-2 rounded-full border-2 text-xs font-black shadow-[2.5px_2.5px_0_#111827] transition-all active:translate-y-0.5 active:shadow-[1.5px_1.5px_0_#111827] ${
                            showQaGuide
                              ? 'border-slate-900 bg-[#2b160f] text-white'
                              : 'border-slate-900 bg-white text-[#2b160f] hover:bg-slate-50'
                          }`}
                        >
                          <Sparkles size={14} />
                          {showQaGuide ? 'Ẩn Hướng Dẫn' : 'Hiện Hướng Dẫn'}
                        </button>
                      </div>
                    </div>
                  </div>

                  {/* KHU VỰC HƯỚNG DẪN HỌC (CHỈ HIỂN THỊ KHI BẬT) */}
                  {showQaGuide && (
                    <div className="grid grid-cols-1 lg:grid-cols-12 gap-6 items-stretch animate-fade-in">
                      {/* Cột trái: Câu trả lời gợi ý + Mẹo & Lỗi (7/12) */}
                      <div className="lg:col-span-7 space-y-6 flex flex-col justify-between">
                        {/* Thẻ Gợi ý câu trả lời */}
                        <div className="rounded-[24px] border-2 border-slate-900 bg-emerald-50/20 p-5 shadow-[4px_4px_0_#111827] space-y-4 flex-grow flex flex-col">
                          <h3 className="font-heading text-lg font-black text-emerald-800 flex items-center gap-2 border-b border-emerald-200 pb-2 shrink-0">
                            Gợi ý câu trả lời
                          </h3>
                          <div className="space-y-3 flex-grow overflow-y-auto">
                            {currentQuestion.sampleAnswers.map((answer, index) => {
                              const isSpeakingSample = speakingSampleId === `sample_${index}`;
                              return (
                                <div
                                  key={index}
                                  className="flex items-start justify-between gap-4 p-3 rounded-xl border border-slate-900 bg-white"
                                >
                                  <div className="min-w-0 flex-1">
                                    <p className="text-sm font-black text-slate-800 select-all leading-normal">{answer.ja}</p>
                                    <p className="mt-1 text-xs font-bold text-slate-500 leading-normal">{answer.vi}</p>
                                  </div>
                                  <button
                                    type="button"
                                    onClick={() => {
                                      if (isSpeakingSample) {
                                        stopSpeaking();
                                      } else {
                                        setSpeakingSampleId(`sample_${index}`);
                                        speakText(
                                          answer.ja,
                                          () => setSpeakingSampleId(`sample_${index}`),
                                          () => setSpeakingSampleId(null)
                                        );
                                      }
                                    }}
                                    className={`flex h-8 w-8 shrink-0 items-center justify-center rounded-lg border-2 border-slate-900 shadow-[2px_2px_0_#111827] transition-all hover:-translate-y-0.5 hover:shadow-[3px_3px_0_#111827] active:translate-y-0 active:shadow-[1px_1px_0_#111827] ${
                                      isSpeakingSample ? 'bg-rose-50 text-rose-600' : 'bg-emerald-50 text-emerald-600'
                                    }`}
                                  >
                                    {isSpeakingSample ? <VolumeX size={14} /> : <Volume2 size={14} />}
                                  </button>
                                </div>
                              );
                            })}
                          </div>
                        </div>

                        {/* Thẻ mẹo thi & lỗi sai (ghép phía dưới) */}
                        <div className="space-y-4 pt-2 shrink-0">
                          {currentQuestion.tips && currentQuestion.tips.length > 0 && (
                            <div className="rounded-[24px] border-2 border-slate-900 bg-amber-50/20 p-5 shadow-[4px_4px_0_#111827] space-y-2">
                              <h3 className="font-heading text-sm font-black text-amber-800 flex items-center gap-1.5 border-b border-amber-200 pb-1.5">
                                <Lightbulb size={16} />
                                Mẹo làm bài thi
                              </h3>
                              <ul className="space-y-1">
                                {currentQuestion.tips.map((tip, idx) => (
                                  <li key={idx} className="flex items-start gap-2 text-xs font-bold text-amber-950 leading-relaxed">
                                    <span className="mt-1.5 shrink-0 h-1.5 w-1.5 rounded-full bg-amber-600" />
                                    <span>{tip}</span>
                                  </li>
                                ))}
                              </ul>
                            </div>
                          )}

                          {currentQuestion.commonMistakes && currentQuestion.commonMistakes.length > 0 && (
                            <div className="rounded-[24px] border-2 border-slate-900 bg-rose-50/20 p-5 shadow-[4px_4px_0_#111827] space-y-2">
                              <h3 className="font-heading text-sm font-black text-rose-800 flex items-center gap-1.5 border-b border-rose-200 pb-1.5">
                                <AlertCircle size={16} />
                                Lỗi thường gặp
                              </h3>
                              <ul className="space-y-1">
                                {currentQuestion.commonMistakes.map((mistake, idx) => (
                                  <li key={idx} className="flex items-start gap-2 text-xs font-bold text-rose-955 leading-relaxed">
                                    <span className="mt-1.5 shrink-0 h-1.5 w-1.5 rounded-full bg-rose-600" />
                                    <span>{mistake}</span>
                                  </li>
                                ))}
                              </ul>
                            </div>
                          )}
                        </div>
                      </div>

                      {/* Cột phải: Từ vựng liên quan (5/12) */}
                      <div className="lg:col-span-5 flex flex-col">
                        <div className="rounded-[24px] border-2 border-slate-900 bg-blue-50/20 p-5 shadow-[4px_4px_0_#111827] space-y-4 flex-grow flex flex-col min-h-[300px]">
                          <h3 className="font-heading text-lg font-black text-blue-800 flex items-center gap-2 border-b border-blue-200 pb-2 shrink-0">
                            Từ vựng liên quan
                          </h3>

                          {currentQuestion.relatedVocabulary && currentQuestion.relatedVocabulary.length > 0 ? (
                            /* Grid 2 cột phủ hết chiều ngang box ngoài */
                            <div className="grid w-full grid-cols-2 content-start gap-2 overflow-y-auto pr-1 flex-grow">
                              {currentQuestion.relatedVocabulary.map((vocab, index) => {
                                const isSpeakingVocab = speakingVocabWord === vocab.word;
                                return (
                                  <div
                                    key={index}
                                    className="min-w-0 rounded-xl border border-slate-900 bg-white p-2 shadow-[2px_2px_0_#111827] text-left"
                                  >
                                    <div className="flex min-w-0 items-start justify-between gap-2">
                                      <div className="min-w-0">
                                        <span className="block truncate text-xs font-black text-indigo-600" title={vocab.word}>
                                          {vocab.word}
                                        </span>
                                        {vocab.reading && (
                                          <span className="block truncate text-[9px] font-bold text-slate-400" title={vocab.reading}>
                                            [{vocab.reading}]
                                          </span>
                                        )}
                                      </div>
                                      <button
                                        type="button"
                                        onClick={() => {
                                          if (isSpeakingVocab) {
                                            stopSpeaking();
                                          } else {
                                            setSpeakingVocabWord(vocab.word);
                                            speakText(
                                              vocab.word,
                                              () => setSpeakingVocabWord(vocab.word),
                                              () => setSpeakingVocabWord(null)
                                            );
                                          }
                                        }}
                                        className={`flex h-6 w-6 shrink-0 items-center justify-center rounded-md border border-slate-900 shadow-[1px_1px_0_#111827] transition-all hover:-translate-y-0.5 active:translate-y-0 active:shadow-none ${
                                          isSpeakingVocab
                                            ? 'bg-rose-50 text-rose-600'
                                            : 'bg-indigo-50 text-indigo-600 hover:bg-indigo-100'
                                        }`}
                                      >
                                        {isSpeakingVocab ? <VolumeX size={11} /> : <Volume2 size={11} />}
                                      </button>
                                    </div>
                                    <span className="mt-1.5 block border-t border-slate-100 pt-1.5 text-[11px] font-bold leading-normal text-slate-600 line-clamp-2" title={vocab.meaning}>
                                      {vocab.meaning}
                                    </span>
                                  </div>
                                );
                              })}
                            </div>
                          ) : (
                            <div className="flex flex-col items-center justify-center p-8 text-center text-slate-400 flex-grow bg-white border border-slate-200 border-dashed rounded-xl">
                              <p className="text-xs font-bold">Không có từ vựng liên quan.</p>
                            </div>
                          )}
                        </div>
                      </div>
                    </div>
                  )}

                  {/* FOOTER ĐIỀU HƯỚNG CÂU HỎI */}
                  <div className="flex items-center justify-between border-t border-slate-200 pt-6">
                    <button
                      type="button"
                      disabled={currentQuestionIndex === 0}
                      onClick={handlePrevQuestion}
                      className="inline-flex h-11 items-center justify-center gap-1.5 rounded-full border-2 border-slate-900 bg-white px-4 text-xs font-black text-slate-900 shadow-[3px_3px_0_#111827] transition-all hover:translate-x-[1px] hover:translate-y-[1px] hover:shadow-[1.5px_1.5px_0_#111827] disabled:pointer-events-none disabled:opacity-40"
                    >
                      <ChevronLeft size={16} />
                      Câu trước
                    </button>

                    <button
                      type="button"
                      onClick={handleBack}
                      className="inline-flex h-11 items-center justify-center gap-1.5 rounded-full border-2 border-slate-900 bg-slate-100 px-4 text-xs font-black text-slate-600 shadow-[3px_3px_0_#111827] transition-all hover:translate-x-[1px] hover:translate-y-[1px] hover:shadow-[1.5px_1.5px_0_#111827]"
                    >
                      <Undo2 size={15} />
                      Dừng luyện tập
                    </button>

                    <button
                      type="button"
                      disabled={currentQuestionIndex === allQuestions.length - 1}
                      onClick={handleNextQuestion}
                      className="inline-flex h-11 items-center justify-center gap-1.5 rounded-full border-2 border-slate-900 bg-[#2b160f] px-4 text-xs font-black text-white shadow-[3px_3px_0_#111827] transition-all hover:translate-x-[1px] hover:translate-y-[1px] hover:shadow-[1.5px_1.5px_0_#111827] disabled:pointer-events-none disabled:opacity-40"
                    >
                      Câu sau
                      <ChevronRight size={16} />
                    </button>
                  </div>
                </div>
              )}
            </div>
          )}
        </>
      )}
    </div>
  );
};
