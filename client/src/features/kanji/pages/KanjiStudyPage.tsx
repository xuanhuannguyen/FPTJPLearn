import { useState, useEffect, useRef } from 'react';
import { useParams, useNavigate, useSearchParams } from 'react-router-dom';
import { ArrowLeft, ChevronLeft, ChevronRight, BookmarkPlus, BookmarkCheck, RotateCcw, Eye, PenLine, X, Loader2 } from 'lucide-react';
import { kanjiApi } from '../api/kanjiApi';
import { memoryApi } from '../../memory/api/memoryApi';
import type { KanjiLesson, KanjiItem } from '../types/kanji.types';
import type { MemoryGrammarStatus } from '../../memory/types/memory.types';
// @ts-expect-error -- this project doesn't include hanzi-writer types
import HanziWriter from 'hanzi-writer';

type HanziWriterCharData = {
  medians?: number[][][];
  strokes?: string[];
};

type HanziWriterQuizStrokeData = {
  strokeNum: number;
};

type HanziWriterInstance = {
  showCharacter: () => void;
  hideCharacter: () => void;
  animateStroke: (strokeNum: number, options?: { onComplete?: () => void }) => void;
  quiz: (options: { onCorrectStroke?: (data: HanziWriterQuizStrokeData) => void; onComplete?: () => void }) => void;
};

type HanziWriterModule = {
  create: (
    target: HTMLElement,
    character: string,
    options: Record<string, unknown> & { charDataLoader?: (char: string, onComplete: (data: HanziWriterCharData) => void) => void }
  ) => HanziWriterInstance;
};

const HanziWriterTyped = HanziWriter as unknown as HanziWriterModule;

const RADICAL_HANVIET: Record<string, string> = {
  '一': 'Nhất', '二': 'Nhị', '十': 'Thập', '人': 'Nhân', '亻': 'Nhân',
  '日': 'Nhật', '月': 'Nguyệt', '木': 'Mộc', '水': 'Thủy', '氵': 'Thủy',
  '火': 'Hỏa', '灬': 'Hỏa', '土': 'Thổ', '金': 'Kim', '田': 'Điền',
  '力': 'Lực', '口': 'Khẩu', '女': 'Nữ', '門': 'Môn', '言': 'Ngôn',
  '寺': 'Tự', '貝': 'Bối', '肉': 'Nhục', '糸': 'Mịch', '彳': 'Xích',
  '心': 'Tâm', '忄': 'Tâm', '手': 'Thủ', '扌': 'Thủ', '犬': 'Khuyển',
  '犭': 'Khuyển', '辶': 'Sước', '邑': 'Ấp', '阝': 'Phụ/Ấp', '阜': 'Phụ',
  '艹': 'Thảo', '宀': 'Miên', '广': 'Nghiễm', '厂': 'Hán', '冂': 'Quynh',
  '匚': 'Phương', '彐': 'Ký', '彡': 'Sam', '夕': 'Tịch', '大': 'Đại',
  '小': 'Tiểu', '寸': 'Thốn', '山': 'Sơn', '川': 'Xuyên', '巛': 'Xuyên',
  '工': 'Công', '己': 'Kỷ', '巾': 'Cân', '干': 'Can', '幺': 'Yêu',
  '廴': 'Dẫn', '廾': 'Củng', '弋': 'Dặc', '弓': 'Cung', '戈': 'Qua',
  '戶': 'Hộ', '支': 'Chi', '攴': 'Phộc', '攵': 'Phộc', '文': 'Văn',
  '斗': 'Đẩu', '斤': 'Cân', '方': 'Phương', '无': 'Vô', '曰': 'Viết',
  '欠': 'Khiếm', '止': 'Chỉ', '歹': 'Đãi', '殳': 'Thù', '毋': 'Vô',
  '比': 'Tỷ', '毛': 'Mao', '氏': 'Thị', '气': 'Khí', '爪': 'Trảo',
  '爫': 'Trảo', '父': 'Phụ', '爻': 'Hào', '爿': 'Tường', '片': 'Phiến',
  '牙': 'Nha', '牛': 'Ngưu', '牜': 'Ngưu', '玄': 'Huyền', '玉': 'Ngọc',
  '王': 'Vương', '瓜': 'Qua', '瓦': 'Ngõa', '甘': 'Cam', '生': 'Sinh',
  '用': 'Dụng', '疋': 'Thất', '疒': 'Nạch', '癶': 'Bát', '白': 'Bạch',
  '皮': 'Bì', '皿': 'Mãnh', '目': 'Mục', '矛': 'Mâu', '矢': 'Thỉ',
  '石': 'Thạch', '示': 'Thị', '礻': 'Thị', '禸': 'Nhựu', '禾': 'Hòa',
  '穴': 'Huyệt', '立': 'Lập', '竹': 'Trúc', '⺮': 'Trúc', '米': 'Mễ',
  '糹': 'Mịch', '缶': 'Phẫu', '网': 'Võng', '罒': 'Võng', '羊': 'Dương',
  '羽': 'Vũ', '老': 'Lão', '而': 'Nhi', '耒': 'Lỗi', '耳': 'Nhĩ',
  '聿': 'Duật', '臣': 'Thần', '自': 'Tự', '至': 'Chí', '臼': 'Cữu',
  '舌': 'Thiệt', '舛': 'Suyễn', '舟': 'Chu', '艮': 'Cấn', '色': 'Sắc',
  '艸': 'Thảo', '虍': 'Hô', '虫': 'Trùng', '血': 'Huyết', '行': 'Hành',
  '衣': 'Y', '衤': 'Y', '襾': 'Á', '見': 'Kiến', '角': 'Giác',
  '谷': 'Cốc', '豆': 'Đậu', '豕': 'Thỉ', '豸': 'Trãi', '赤': 'Xích',
  '走': 'Tẩu', '足': 'Túc', '身': 'Thân', '車': 'Xa', '辛': 'Tân',
  '辰': 'Thần', '辵': 'Sước', '酉': 'Dậu', '釆': 'Biện', '里': 'Lý',
  '長': 'Trường', '隶': 'Đãi', '隹': 'Chuy', '雨': 'Vũ', '⻗': 'Vũ',
  '青': 'Thanh', '非': 'Phi', '面': 'Diện', '革': 'Cách', '韋': 'Vi',
  '韭': 'Cửu', '音': 'Âm', '頁': 'Hiệt', '風': 'Phong', '飛': 'Phi',
  '食': 'Thực', '首': 'Thủ', '香': 'Hương', '馬': 'Mã', '骨': 'Cốt',
  '高': 'Cao', '髟': 'Bưu', '鬥': 'Đấu', '鬯': 'Sưởng', '鬲': 'Cách',
  '鬼': 'Quỷ', '魚': 'Ngư', '鳥': 'Điểu', '鹵': 'Lỗ', '鹿': 'Lộc',
  '麥': 'Mạch', '麻': 'Ma', '黃': 'Hoàng', '黍': 'Thử', '黑': 'Hắc',
  '黹': 'Chỉ', '黽': 'Mãnh', '鼎': 'Đỉnh', '鼓': 'Cổ', '鼠': 'Thử',
  '鼻': 'Tỵ', '齊': 'Tề', '齒': 'Xỉ', '龍': 'Long', '龜': 'Quy',
  '龠': 'Dược', '㐅': 'Nghệ', '乂': 'Nghệ', '亠': 'Đầu'
};

export const KanjiStudyPage = () => {
  const { level, lessonId } = useParams<{ level: string; lessonId: string }>();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const [lesson, setLesson] = useState<KanjiLesson | null>(null);
  const [kanjis, setKanjis] = useState<KanjiItem[]>([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [isLoading, setIsLoading] = useState(true);
  const [memoryStatus, setMemoryStatus] = useState<MemoryGrammarStatus>({ isInMemory: false, isActive: false, memoryItemId: null });
  const [isSavingStudy, setIsSavingStudy] = useState(false);
  const [showStrokeOrder, setShowStrokeOrder] = useState(false);
  const [strokeAnimIndex, setStrokeAnimIndex] = useState(-1);
  const [isAnimating, setIsAnimating] = useState(false);
  const [kanjiMedians, setKanjiMedians] = useState<number[][][]>([]);
  const [kanjiStrokes, setKanjiStrokes] = useState<string[]>([]);
  const writerContainerRef = useRef<HTMLDivElement>(null);
  const hwRef = useRef<HanziWriterInstance | null>(null);

  // Practice Quiz State
  const quizContainerRef = useRef<HTMLDivElement>(null);
  const quizWriterRef = useRef<HanziWriterInstance | null>(null);
  const [quizStrokeNum, setQuizStrokeNum] = useState(1);
  const [isQuizComplete, setIsQuizComplete] = useState(false);
  const [freehandMode, setFreehandMode] = useState(false);
  const [hoveredComponent, setHoveredComponent] = useState<number | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      if (!lessonId) return;
      try {
        const [lessonData, kanjiData] = await Promise.all([
          kanjiApi.getLessonById(lessonId),
          kanjiApi.getKanjiItemsByLesson(lessonId),
        ]);
        setLesson(lessonData);
        setKanjis(kanjiData);

        // Jump to the kanji specified by ?index= or ?kanjiId= query param
        const indexParam = searchParams.get('index');
        const kanjiIdParam = searchParams.get('kanjiId');
        if (indexParam !== null) {
          const idx = parseInt(indexParam, 10);
          if (!isNaN(idx) && idx >= 0 && idx < kanjiData.length) {
            setCurrentIndex(idx);
          }
        } else if (kanjiIdParam) {
          const idx = kanjiData.findIndex(k => k.id === kanjiIdParam);
          if (idx !== -1) {
            setCurrentIndex(idx);
          }
        }
      } catch (error) {
        console.error('Failed to fetch study data:', error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchData();
  }, [lessonId]);

  useEffect(() => {
    const timer = window.setTimeout(() => {
      setShowStrokeOrder(false);
      setStrokeAnimIndex(-1);
      setIsAnimating(false);
    }, 0);

    return () => window.clearTimeout(timer);
  }, [currentIndex]);

  const currentKanji = kanjis[currentIndex];

  useEffect(() => {
    if (currentKanji) {
      memoryApi.getKanjiItemStatus(currentKanji.id)
        .then((status) => setMemoryStatus(status))
        .catch(console.error);
    }
  }, [currentKanji]);

  const toggleMemory = async () => {
    if (!currentKanji) return;
    try {
      setIsSavingStudy(true);
      if (memoryStatus.isInMemory && memoryStatus.memoryItemId) {
        await memoryApi.removeKanjiItem(memoryStatus.memoryItemId);
      } else {
        await memoryApi.addKanjiFromItem(currentKanji.id);
      }
      const newStatus = await memoryApi.getKanjiItemStatus(currentKanji.id);
      setMemoryStatus(newStatus);
    } catch (e) {
      console.error('Failed to toggle memory:', e);
    } finally {
      setIsSavingStudy(false);
    }
  };

  useEffect(() => {
    if (showStrokeOrder && writerContainerRef.current && currentKanji) {
      writerContainerRef.current.innerHTML = '';
      hwRef.current = HanziWriterTyped.create(writerContainerRef.current, currentKanji.character, {
        width: 180,
        height: 180,
        padding: 5,
        strokeColor: '#0F172A',
        showOutline: false,
        strokeAnimationSpeed: 1,
        delayBetweenStrokes: 400,
        charDataLoader: (char: string, onComplete: (data: HanziWriterCharData) => void) => {
          fetch(`https://cdn.jsdelivr.net/npm/hanzi-writer-data-jp@0.1.x/data/${char}.json`)
            .then((res) => {
              if (!res.ok) throw new Error();
              return res.json();
            })
            .then((data: HanziWriterCharData) => {
              setKanjiMedians(data.medians || []);
              setKanjiStrokes(data.strokes || []);
              onComplete(data);
            })
            .catch(() => {
              fetch(`https://cdn.jsdelivr.net/npm/hanzi-writer-data@2.0/${char}.json`)
                .then((res) => res.json())
                .then((data: HanziWriterCharData) => {
                  setKanjiMedians(data.medians || []);
                  setKanjiStrokes(data.strokes || []);
                  onComplete(data);
                });
            });
        },
      });
      hwRef.current.showCharacter();
    } else {
      hwRef.current = null;
    }
  }, [showStrokeOrder, currentKanji]);

  const initQuiz = () => {
    if (!quizWriterRef.current) return;
    setIsQuizComplete(false);
    setQuizStrokeNum(1);
    quizWriterRef.current.quiz({
      onCorrectStroke: (strokeData: HanziWriterQuizStrokeData) => {
        setQuizStrokeNum(strokeData.strokeNum + 2);
      },
      onComplete: () => {
        setIsQuizComplete(true);
      }
    });
  };

  // Initialize Practice Quiz
  useEffect(() => {
    if (!currentKanji) return;

    const resetTimer = window.setTimeout(() => {
      setIsQuizComplete(false);
      setQuizStrokeNum(1);
    }, 0);

    // Small delay to ensure DOM is ready after re-render
    const timer = window.setTimeout(() => {
      if (!quizContainerRef.current) return;

      quizContainerRef.current.innerHTML = '';
      quizWriterRef.current = HanziWriterTyped.create(quizContainerRef.current, currentKanji.character, {
        width: 260,
        height: 260,
        padding: 10,
        showCharacter: false,
        strokeColor: '#22c55e', // green-500 for completed strokes
        highlightColor: '#60a5fa', // blue-400 for hint stroke guide
        outlineColor: '#e2e8f0',
        showOutline: false, // We render our own custom outline layer
        showHintAfterMisses: false, // Turn off native hints to use our persistent SVG hint
        drawingColor: '#3b82f6', // blue-500 while drawing
        highlightOnComplete: false,
        strokeHighlightSpeed: 2,
        charDataLoader: (char: string, onComplete: (data: HanziWriterCharData) => void) => {
          fetch(`https://cdn.jsdelivr.net/npm/hanzi-writer-data-jp@0.1.x/data/${char}.json`)
            .then((res) => {
              if (!res.ok) throw new Error();
              return res.json();
            })
            .then((data: HanziWriterCharData) => {
              setKanjiMedians(data.medians || []);
              setKanjiStrokes(data.strokes || []);
              onComplete(data);
            })
            .catch(() => {
              fetch(`https://cdn.jsdelivr.net/npm/hanzi-writer-data@2.0/${char}.json`)
                .then((res) => res.json())
                .then((data: HanziWriterCharData) => {
                  setKanjiMedians(data.medians || []);
                  setKanjiStrokes(data.strokes || []);
                  onComplete(data);
                });
            });
        },
      });

      initQuiz();
    }, 50);

    return () => {
      window.clearTimeout(resetTimer);
      window.clearTimeout(timer);
    };
  }, [currentKanji, currentIndex, freehandMode, kanjis.length]);

  const startQuiz = () => {
    if (!quizWriterRef.current || !quizContainerRef.current || !currentKanji) return;
    // Rebuild the writer entirely to get a clean state
    quizContainerRef.current.innerHTML = '';
    quizWriterRef.current = HanziWriterTyped.create(quizContainerRef.current, currentKanji.character, {
      width: 260,
      height: 260,
      padding: 10,
      showCharacter: false,
      strokeColor: '#22c55e',
      highlightColor: '#60a5fa',
      outlineColor: '#e2e8f0',
      showOutline: false, // We render our own custom outline layer
      showHintAfterMisses: false, // Turn off native hints to use our persistent SVG hint
      drawingColor: '#3b82f6',
      highlightOnComplete: false,
      strokeHighlightSpeed: 2,
      charDataLoader: (char: string, onComplete: (data: HanziWriterCharData) => void) => {
        fetch(`https://cdn.jsdelivr.net/npm/hanzi-writer-data-jp@0.1.x/data/${char}.json`)
          .then((res) => {
            if (!res.ok) throw new Error();
            return res.json();
          })
          .then((data: HanziWriterCharData) => {
            setKanjiMedians(data.medians || []);
            setKanjiStrokes(data.strokes || []);
            onComplete(data);
          })
          .catch(() => {
            fetch(`https://cdn.jsdelivr.net/npm/hanzi-writer-data@2.0/${char}.json`)
              .then((res) => res.json())
              .then((data: HanziWriterCharData) => {
                setKanjiMedians(data.medians || []);
                setKanjiStrokes(data.strokes || []);
                onComplete(data);
              });
          });
      },
    });
    initQuiz();
  };

  const toggleFreehand = () => {
    setFreehandMode(prev => !prev);
  };

  const handleNext = () => {
    if (currentIndex < kanjis.length - 1) {
      setCurrentIndex(prev => prev + 1);
    }
  };

  const handlePrev = () => {
    if (currentIndex > 0) {
      setCurrentIndex(prev => prev - 1);
    }
  };

  const handleStrokeAnimate = async () => {
    if (isAnimating || !hwRef.current) return;
    setIsAnimating(true);
    setStrokeAnimIndex(-2); // -2 indicates we are starting
    
    hwRef.current.hideCharacter();
    
    const total = currentKanji?.strokeCount || 1;
    
    // Add small delay to show the blank paper clearly before drawing
    await new Promise(r => setTimeout(r, 400));

    for (let i = 0; i < total; i++) {
      if (!hwRef.current) break; // if modal closed
      setStrokeAnimIndex(i);
      await new Promise(resolve => {
        hwRef.current.animateStroke(i, {
          onComplete: () => setTimeout(resolve, 400)
        });
      });
    }
    
    setIsAnimating(false);
    setStrokeAnimIndex(-1); // Reset
  };

  if (isLoading) {
    return (
      <div className="flex h-screen items-center justify-center bg-slate-50">
        <div className="h-10 w-10 animate-spin border-4 border-black border-t-transparent"></div>
      </div>
    );
  }

  if (!lesson || kanjis.length === 0) {
    return <div className="p-8 text-center font-mono">No kanji to study</div>;
  }

  const renderStrokeNumbers = () => {
    if (kanjiMedians.length === 0 || strokeAnimIndex === -2) return null;

    const size = 180;
    const padding = 5;
    const scale = (size - 2 * padding) / 1024;

    return kanjiMedians.map((points, index) => {
      // Only show number if this stroke has been drawn or is currently drawing
      if (strokeAnimIndex !== -1 && index > strokeAnimIndex) return null;

      if (!points || points.length === 0) return null;
      const [rawX, rawY] = points[0]; // Start point of the stroke
      const x = padding + rawX * scale;
      const y = padding + (1024 - rawY) * scale;

      return (
        <span
          key={index}
          className="absolute text-[11px] font-black text-[#94a3b8] select-none animate-fade-in pointer-events-none"
          style={{
            left: `${x}px`,
            top: `${y}px`,
            transform: 'translate(-110%, -50%)',
            fontFamily: 'monospace'
          }}
        >
          {index + 1}
        </span>
      );
    });
  };

  return (
    <div className="min-h-screen blue-grid flex flex-col font-sans">
      {/* Top Navbar */}
      <header className="flex h-14 shrink-0 items-center justify-between border-b-2 border-black bg-white px-4">
        <div className="flex items-center gap-3">
          <button 
            onClick={() => navigate(`/kanji/${level}/lessons/${lessonId}`)}
            className="flex h-8 w-8 items-center justify-center border border-black hover:bg-black hover:text-white transition-colors"
          >
            <ArrowLeft size={16} />
          </button>
          <div className="flex flex-col">
            <span className="text-[10px] font-black uppercase tracking-widest text-text-tertiary">
              {level} • L{lesson.lessonNumber}
            </span>
            <span className="text-sm font-bold leading-none text-text-primary">
              Study Mode
            </span>
          </div>
        </div>
        
        <div className="flex items-center gap-2">
          <span className="font-mono text-sm font-bold">
            {currentIndex + 1} <span className="text-text-tertiary">/ {kanjis.length}</span>
          </span>
        </div>
      </header>

      {/* Main Content: Bento Box / Split Pane */}
      <main className="flex-1 overflow-y-auto p-3 md:p-4 lg:p-6">
        <div className="max-w-5xl mx-auto grid grid-cols-1 lg:grid-cols-12 gap-4">
          
          {/* Left Pane: Visual & Practice (8 cols) */}
          <div className="lg:col-span-8 flex flex-col gap-3">
            
            {/* Main Kanji Display */}
            <div className="flex flex-col items-center justify-center border-2 border-black bg-white py-3 px-4 shadow-[2px_2px_0px_#0F172A] relative overflow-hidden">
              <span className="absolute top-2 left-2 text-[10px] font-mono font-bold text-text-tertiary">
                Stroke: {currentKanji.strokeCount}
              </span>
              <button
                onClick={() => setShowStrokeOrder(prev => !prev)}
                className={`absolute top-2 right-2 flex items-center gap-1 px-2 py-1 text-[10px] uppercase font-bold transition-all border ${
                  showStrokeOrder
                    ? 'bg-accent-primary text-white border-accent-primary'
                    : 'bg-black text-white border-black hover:bg-accent-primary hover:border-accent-primary'
                }`}
              >
                <Eye size={12} />
                Stroke
              </button>

              {/* Normal kanji display */}
              <span 
                className={`text-[72px] md:text-[80px] leading-none text-text-primary select-none transition-opacity duration-300 ${showStrokeOrder ? 'opacity-0' : 'opacity-100'}`}
                style={{ fontFamily: '"Noto Serif JP", serif' }}
              >
                {currentKanji.character}
              </span>

              {/* Stroke order modal */}
              {showStrokeOrder && (
                <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 animate-fade-in" onClick={() => { setShowStrokeOrder(false); setStrokeAnimIndex(-1); setIsAnimating(false); }}>
                  <div className="bg-white rounded-2xl shadow-xl w-[90vw] max-w-[360px] aspect-square relative flex items-center justify-center border border-slate-200" onClick={e => e.stopPropagation()}>
                    
                    {/* Close button */}
                    <button
                      onClick={() => { setShowStrokeOrder(false); setStrokeAnimIndex(-1); setIsAnimating(false); }}
                      className="absolute top-3 right-3 text-slate-400 hover:text-slate-700 transition-colors p-1 z-10"
                    >
                      <X size={20} />
                    </button>

                    {/* Central Kanji Display (HanziWriter Target) */}
                    <div className="relative w-[180px] h-[180px]">
                      {renderStrokeNumbers()}
                      <div ref={writerContainerRef} className="w-full h-full"></div>
                    </div>

                    {/* Red Floating Action Button (Animate) */}
                    <button
                      onClick={handleStrokeAnimate}
                      disabled={isAnimating}
                      className={`absolute bottom-6 right-6 flex items-center justify-center w-14 h-14 rounded-full text-white shadow-[0_4px_14px_rgba(225,29,72,0.4)] transition-all ${
                        isAnimating ? 'bg-slate-300 cursor-not-allowed scale-95 shadow-none' : 'bg-[#EF4444] hover:bg-red-600 hover:scale-105 active:scale-95'
                      }`}
                    >
                      <PenLine size={24} />
                    </button>

                  </div>
                </div>
              )}
            </div>

            {/* Practice Bento Box */}
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 flex-1">
              {/* Component Graph Box */}
              <div className="border-2 border-black bg-white p-3 shadow-[4px_4px_0px_#0F172A] flex flex-col min-h-[260px]">
                <div className="flex items-center justify-between mb-3 border-b border-black pb-2">
                  <span className="text-xs font-black uppercase tracking-wider">Bộ thủ / Thành phần</span>
                </div>
                <div className="flex-1 flex items-center justify-center">
                  {currentKanji.components && currentKanji.components.length > 0 ? (
                    <svg width="100%" height="200" viewBox="0 0 280 200" className="overflow-visible">
                      <defs>
                        <marker id="arrowhead" markerWidth="8" markerHeight="6" refX="8" refY="3" orient="auto">
                          <polygon points="0 0, 8 3, 0 6" fill="#94a3b8" />
                        </marker>
                        <filter id="nodeShadow" x="-20%" y="-20%" width="140%" height="140%">
                          <feDropShadow dx="1" dy="2" stdDeviation="2" floodOpacity="0.15" />
                        </filter>
                      </defs>

                      {/* Dynamically compute component positions around the center */}
                      {(() => {
                        const centerX = 140;
                        const centerY = 100;
                        const radius = 85;
                        const components = currentKanji.components;
                        const positions = components.map((_, i) => {
                          const angle = (-Math.PI / 2) + (i * (2 * Math.PI / components.length)) + (components.length === 1 ? Math.PI / 4 : 0);
                          return {
                            x: centerX + radius * Math.cos(angle),
                            y: centerY + radius * Math.sin(angle),
                          };
                        });

                        return (
                          <>
                            {/* Arrows from components to center */}
                            {positions.map((pos, i) => {
                              const dx = centerX - pos.x;
                              const dy = centerY - pos.y;
                              const dist = Math.sqrt(dx * dx + dy * dy);
                              const nx = dx / dist;
                              const ny = dy / dist;
                              const startX = pos.x + nx * 26;
                              const startY = pos.y + ny * 26;
                              const endX = centerX - nx * 30;
                              const endY = centerY - ny * 30;

                              return (
                                <g key={`arrow-${i}`}>
                                  <line
                                    x1={startX} y1={startY} x2={endX} y2={endY}
                                    stroke="#94a3b8" strokeWidth="1.5" markerEnd="url(#arrowhead)"
                                  />
                                  {components[i].reading && (
                                    <text
                                      x={(startX + endX) / 2 - 10}
                                      y={(startY + endY) / 2 - 6}
                                      fill="#64748b" fontSize="10" fontWeight="600"
                                      fontFamily="'Comic Neue', cursive"
                                    >
                                      {components[i].reading}
                                    </text>
                                  )}
                                </g>
                              );
                            })}

                            {/* Component nodes (Orange) - clickable */}
                            {components.map((comp, i) => {
                              const hanVietLabel = RADICAL_HANVIET[comp.character] || comp.name.split(' ')[0];
                              const labelWidth = hanVietLabel.length * 8 + 16;
                              return (
                              <g
                                key={`comp-${i}`}
                                filter="url(#nodeShadow)"
                                className="cursor-pointer transition-transform hover:scale-[1.03]"
                                style={{ transformOrigin: `${positions[i].x}px ${positions[i].y}px` }}
                                onClick={() => {
                                  if (comp.kanjiId) {
                                    window.open(`/kanji/${currentKanji.level}/lessons/${currentKanji.lessonId}/study?kanjiId=${comp.kanjiId}`, '_blank');
                                  }
                                }}
                                onMouseEnter={() => setHoveredComponent(i)}
                                onMouseLeave={() => setHoveredComponent(null)}
                              >
                                <circle cx={positions[i].x} cy={positions[i].y} r="24" fill="#F97316" stroke="#C2410C" strokeWidth="2.5" />
                                <text x={positions[i].x} y={positions[i].y + 6} textAnchor="middle" fill="#fff" fontSize="18" fontWeight="bold" style={{ fontFamily: '"Noto Serif JP", serif' }}>
                                  {comp.character}
                                </text>
                                
                                {hoveredComponent === i && (
                                  <g className="animate-fade-in pointer-events-none">
                                    <rect 
                                      x={positions[i].x - labelWidth / 2} 
                                      y={positions[i].y + 32} 
                                      width={labelWidth} 
                                      height="24" 
                                      rx="6" 
                                      fill="#1E293B" 
                                      opacity="0.95" 
                                    />
                                    <text 
                                      x={positions[i].x} 
                                      y={positions[i].y + 48} 
                                      textAnchor="middle" 
                                      fill="#F8FAFC" 
                                      fontSize="12" 
                                      fontWeight="bold" 
                                      fontFamily="'Comic Neue', cursive"
                                    >
                                      {hanVietLabel}
                                    </text>
                                  </g>
                                )}
                              </g>
                            )})}

                            {/* Main Kanji node (center, Blue) */}
                            <g filter="url(#nodeShadow)">
                              <circle cx={centerX} cy={centerY} r="28" fill="#3B82F6" stroke="#1D4ED8" strokeWidth="2.5" />
                              <text x={centerX} y={centerY + 8} textAnchor="middle" fill="#fff" fontSize="22" fontWeight="bold" style={{ fontFamily: '"Noto Serif JP", serif' }}>
                                {currentKanji.character}
                              </text>
                            </g>
                          </>
                        );
                      })()}
                    </svg>
                  ) : (
                    <span className="text-sm text-slate-400 font-mono">Không có dữ liệu bộ thủ</span>
                  )}
                </div>
              </div>

              {/* Writing Canvas Box */}
              <div className="border-2 border-black bg-white p-3 shadow-[4px_4px_0px_#0F172A] flex flex-col min-h-[260px]">
                <div className="flex items-center justify-between mb-3 border-b border-black pb-2">
                  <span className="text-xs font-black uppercase tracking-wider">
                    {isQuizComplete ? 'Practice' : freehandMode ? `Tự viết ${Math.min(quizStrokeNum, currentKanji.strokeCount)}/${currentKanji.strokeCount}` : `Nét ${Math.min(quizStrokeNum, currentKanji.strokeCount)}/${currentKanji.strokeCount}`}
                  </span>
                  <div className="flex items-center gap-1">
                    <button
                      onClick={toggleFreehand}
                      className={`flex items-center gap-1 border px-2 py-1 text-[10px] uppercase font-bold transition-colors ${
                        freehandMode 
                          ? 'border-black hover:bg-slate-100' 
                          : 'bg-black text-white border-black hover:bg-slate-800'
                      }`}
                      title={freehandMode ? 'Bật hướng dẫn nét vẽ' : 'Tắt hướng dẫn, tự viết tay'}
                    >
                      <PenLine size={12} />
                      {freehandMode ? 'Guide' : 'Free'}
                    </button>
                    <button 
                      onClick={startQuiz}
                      className="flex items-center gap-1 border border-black px-2 py-1 text-[10px] uppercase font-bold hover:bg-slate-100 transition-colors"
                    >
                      <RotateCcw size={12} />
                      Clear
                    </button>
                  </div>
                </div>
                <div className="flex-1 bg-slate-50 border border-slate-200 relative aspect-square mx-auto w-full max-w-[260px] flex items-center justify-center rounded-lg overflow-hidden">
                  {/* Grid background */}
                  <div className="absolute inset-0 grid grid-cols-2 grid-rows-2 pointer-events-none">
                    <div className="border-r border-b border-dashed border-slate-300"></div>
                    <div className="border-b border-dashed border-slate-300"></div>
                    <div className="border-r border-dashed border-slate-300"></div>
                    <div></div>
                  </div>
                  
                  {/* Quiz Canvas (Always mounted so startQuiz can find the ref) */}
                  <div ref={quizContainerRef} className="absolute inset-0 w-full h-full cursor-crosshair touch-none flex items-center justify-center"></div>
                  
                  {isQuizComplete && (
                    <div className="absolute inset-0 flex flex-col items-center justify-center animate-fade-in space-y-3 bg-[#f0fdf4] w-full h-full border-2 border-green-500 rounded-lg z-20">
                      <div className="w-16 h-16 rounded-full bg-green-100 flex items-center justify-center text-green-500 shadow-sm border border-green-200">
                        <svg className="w-10 h-10" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M5 13l4 4L19 7"></path></svg>
                      </div>
                      <div className="text-center">
                        <h3 className="font-bold text-green-600 text-lg">Đỉnh nha</h3>
                        <p className="text-slate-500 text-xs mt-1">Có năng khiếu đấy :))</p>
                      </div>
                    </div>
                  )}

                  {!isQuizComplete && (
                    <>
                      {/* Custom Outline Layer: gray for future, blue for current (Guide mode only) */}
                      {!freehandMode && kanjiStrokes.length > 0 && !isQuizComplete && (
                        <svg 
                          className="absolute inset-0 w-full h-full pointer-events-none z-[5]" 
                          width="260" 
                          height="260"
                        >
                          {/* Match HanziWriter's internal Positioner transform exactly */}
                          {(() => {
                            const w = 260, h = 260, pad = 10;
                            const VBX = 0, VBY = -124, VBW = 1024, VBH = 1024;
                            const drawW = w - 2 * pad;
                            const drawH = h - 2 * pad;
                            const sc = Math.min(drawW / VBW, drawH / VBH);
                            const xOff = -VBX * sc + pad + (drawW - sc * VBW) / 2;
                            const yOff = -VBY * sc + pad + (drawH - sc * VBH) / 2;
                            return (
                              <g transform={`translate(${xOff}, ${h - yOff}) scale(${sc}, ${-sc})`}>
                            {kanjiStrokes.map((strokePath, idx) => {
                              // Completed strokes: skip (HanziWriter renders them green)
                              if (idx < quizStrokeNum - 1) return null;
                              
                              const isCurrent = idx === quizStrokeNum - 1;
                              return (
                                <path 
                                  key={idx}
                                  d={strokePath} 
                                  fill={isCurrent ? '#93c5fd' : '#e2e8f0'}
                                  opacity={isCurrent ? 0.85 : 0.6}
                                />
                              );
                            })}
                            {/* Start Dot on current stroke */}
                            {quizStrokeNum <= kanjiStrokes.length && (() => {
                              const points = kanjiMedians[quizStrokeNum - 1];
                              if (!points || points.length === 0) return null;
                              const [rawX, rawY] = points[0];
                              return (
                                <circle 
                                  cx={rawX} 
                                  cy={rawY} 
                                  r="40" 
                                  fill="#3b82f6" 
                                />
                              );
                            })()}
                          </g>
                            );
                          })()}
                        </svg>
                      )}
                    </>
                  )}
                </div>
              </div>
            </div>
            
          </div>

          {/* Right Pane: Info (4 cols) */}
          <div className="lg:col-span-4 flex flex-col gap-3" style={{ fontFamily: '"Comic Neue", cursive' }}>
            
            <div className="border-2 border-black bg-white p-5 shadow-[4px_4px_0px_#0F172A] flex-1 flex flex-col rounded-xl">
              <div className="mb-4 text-orange-500 font-bold text-sm tracking-wide">
                → Quy tắc chuyển âm
              </div>

              <div className="space-y-3 text-[17px] text-slate-800 flex-1">
                <div className="flex gap-1">
                  <span className="font-bold min-w-[110px]">Hán Việt:</span>
                  <span className="text-[#F05152] font-bold uppercase">{currentKanji.hanViet}</span>
                </div>
                
                <div className="flex gap-1">
                  <span className="font-bold min-w-[110px]">Ý nghĩa:</span>
                  <span className="text-[#F05152] font-bold">{currentKanji.meaning}</span>
                </div>

                <div className="flex gap-1">
                  <span className="font-bold min-w-[110px]">Trình độ JLPT:</span>
                  <span className="text-[#F05152] font-bold">{level?.toUpperCase() || 'N5'}</span>
                </div>

                <div className="flex gap-1">
                  <span className="font-bold min-w-[110px]">Số nét:</span>
                  <span className="text-[#F05152] font-bold">{currentKanji.strokeCount}</span>
                </div>

                <div className="flex gap-1">
                  <span className="font-bold min-w-[110px]">Âm Kun:</span>
                  <span className="text-[#F05152] font-bold tracking-wide">
                    {currentKanji.kunReading || "-"}
                  </span>
                </div>

                <div className="flex gap-1">
                  <span className="font-bold min-w-[110px]">Âm On:</span>
                  <span className="text-[#F05152] font-bold tracking-wide">
                    {currentKanji.onReading || "-"}
                  </span>
                </div>

                <div className="flex gap-1 mt-3">
                  <span className="font-bold min-w-[110px]">Gợi ý cách nhớ:</span>
                  <span className="text-[#F05152] font-bold flex-1">
                    {currentKanji.mnemonic || "Chưa có gợi ý cách nhớ cho từ này."}
                  </span>
                </div>
              </div>

            </div>

          </div>
        </div>
      </main>

      {/* Bottom Bar: Controls */}
      <footer className="shrink-0 border-t-2 border-black bg-white px-4 py-3 sticky bottom-0">
        <div className="max-w-5xl mx-auto flex items-center justify-between">
          
          <div className="flex items-center gap-2">
            <button 
              onClick={handlePrev}
              disabled={currentIndex === 0}
              className={`flex items-center gap-1 border-2 border-black px-3 py-2 text-xs font-black uppercase transition-all ${
                currentIndex === 0 
                  ? 'opacity-50 cursor-not-allowed bg-slate-100' 
                  : 'bg-white hover:bg-black hover:text-white shadow-[2px_2px_0px_#0F172A] active:translate-y-0.5 active:shadow-none'
              }`}
            >
              <ChevronLeft size={16} />
              Prev
            </button>
            <button 
              onClick={handleNext}
              disabled={currentIndex === kanjis.length - 1}
              className={`flex items-center gap-1 border-2 border-black px-3 py-2 text-xs font-black uppercase transition-all ${
                currentIndex === kanjis.length - 1
                  ? 'opacity-50 cursor-not-allowed bg-slate-100' 
                  : 'bg-white hover:bg-black hover:text-white shadow-[2px_2px_0px_#0F172A] active:translate-y-0.5 active:shadow-none'
              }`}
            >
              Next
              <ChevronRight size={16} />
            </button>
          </div>

          <button 
            onClick={toggleMemory}
            disabled={isSavingStudy}
            className={`flex items-center gap-2 border-2 border-black px-4 py-2 text-xs md:text-sm font-black uppercase transition-all shadow-[2px_2px_0px_#0F172A] active:translate-y-0.5 active:shadow-none ${
              memoryStatus.isInMemory 
                ? 'bg-emerald-500 text-white hover:bg-emerald-600'
                : 'bg-accent-primary text-white hover:bg-black'
            }`}
          >
            {isSavingStudy ? (
              <Loader2 size={16} className="animate-spin" />
            ) : memoryStatus.isInMemory ? (
              <BookmarkCheck size={16} />
            ) : (
              <BookmarkPlus size={16} />
            )}
            <span className="hidden sm:inline">
              {memoryStatus.isInMemory ? 'Saved to Memory' : 'Add to Memory'}
            </span>
            <span className="sm:hidden">
              {memoryStatus.isInMemory ? 'Saved' : 'Add'}
            </span>
          </button>
          
        </div>
      </footer>
    </div>
  );
};
