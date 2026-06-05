import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { 
  ChevronLeft, 
  ChevronRight, 
  Sparkles, 
  Castle, 
  BookText, 
  Brain, 
  FileQuestion 
} from 'lucide-react';

const IMAGES = [
  '/scoll_1.webp',
  '/scoll_2.webp',
  '/scoll_3.webp',
  '/scoll_4.webp',
  '/scoll_5.webp',
];

const QUICK_FEATURES = [
  { label: 'Hán tự', path: '/kanji', icon: <Castle size={24} /> },
  { label: 'Từ vựng', path: '/vocabulary', icon: <BookText size={24} /> },
  { label: 'Ngữ pháp', path: '/grammar', icon: <Brain size={24} /> },
  { label: 'Luyện thi', path: '/exam', icon: <FileQuestion size={24} /> },
];

export const DashboardPage = () => {
  const [currentIndex, setCurrentIndex] = useState(0);

  useEffect(() => {
    const timer = setInterval(() => {
      setCurrentIndex((prev) => (prev + 1) % IMAGES.length);
    }, 5000);
    return () => clearInterval(timer);
  }, []);

  const next = () => setCurrentIndex((prev) => (prev + 1) % IMAGES.length);
  const prev = () => setCurrentIndex((prev) => (prev - 1 + IMAGES.length) % IMAGES.length);

  return (
    <div className="mx-auto max-w-5xl space-y-8 p-4 md:p-8 animate-fade-in">
      {/* Hero Welcome Section */}
      <section className="flex flex-col gap-6 lg:flex-row lg:items-center lg:justify-between">
        <div className="space-y-2">
          <div className="inline-flex items-center gap-2 rounded-full bg-blue-50 px-3 py-1 text-xs font-bold text-blue-600">
            <Sparkles size={14} />
            <span>Chào mừng bạn quay trở lại</span>
          </div>
          <h1 className="text-4xl font-black tracking-tight text-slate-900 md:text-5xl">
            Học tiếng Nhật <br />
            <span className="text-blue-600">thông minh hơn</span> cùng JPLearn
          </h1>
          <p className="max-w-xl text-lg font-medium text-slate-500">
            Nền tảng hỗ trợ học tập JPD113 & JPD123 tối ưu nhất dành riêng cho sinh viên FPT University.
          </p>
        </div>
      </section>

      {/* Image Carousel Section (Fade Transition - Auto Fit) */}
      <div className="group relative overflow-hidden rounded-[2rem] border-4 border-white bg-white shadow-2xl">
        <div className="relative w-full">
          <img
            key={IMAGES[currentIndex]}
            src={IMAGES[currentIndex]}
            alt={`Feature ${currentIndex + 1}`}
            className="block h-auto w-full object-cover"
            loading={currentIndex === 0 ? 'eager' : 'lazy'}
            decoding="async"
          />
        </div>

        {/* Navigation Arrows */}
        <button 
          onClick={prev}
          className="absolute left-4 top-1/2 -translate-y-1/2 z-20 rounded-full bg-white/20 p-3 text-white backdrop-blur-md transition-all hover:bg-white/40 group-hover:left-6"
        >
          <ChevronLeft size={24} />
        </button>
        <button 
          onClick={next}
          className="absolute right-4 top-1/2 -translate-y-1/2 z-20 rounded-full bg-white/20 p-3 text-white backdrop-blur-md transition-all hover:bg-white/40 group-hover:right-6"
        >
          <ChevronRight size={24} />
        </button>

        {/* Indicators */}
        <div className="absolute bottom-6 left-1/2 z-20 flex -translate-x-1/2 gap-2">
          {IMAGES.map((_, i) => (
            <button
              key={i}
              onClick={() => setCurrentIndex(i)}
              className={`h-2 transition-all rounded-full ${
                currentIndex === i ? 'w-8 bg-white' : 'w-2 bg-white/50'
              }`}
            />
          ))}
        </div>
      </div>

      {/* Feature Quick Links Grid */}
      <div className="grid grid-cols-2 gap-4 md:grid-cols-4">
         {QUICK_FEATURES.map((item, i) => (
           <Link 
            key={i} 
            to={item.path}
            className="group/card rounded-3xl border-2 border-slate-100 bg-white p-6 transition-all hover:-translate-y-1 hover:border-blue-200 hover:shadow-xl"
           >
             <div className="mb-4 h-12 w-12 rounded-2xl bg-blue-50 text-blue-600 flex items-center justify-center transition-colors group-hover/card:bg-blue-600 group-hover/card:text-white">
               {item.icon}
             </div>
             <h3 className="font-bold text-slate-900">{item.label}</h3>
             <p className="text-sm text-slate-500">Bắt đầu học ngay</p>
           </Link>
         ))}
      </div>
    </div>
  );
};
