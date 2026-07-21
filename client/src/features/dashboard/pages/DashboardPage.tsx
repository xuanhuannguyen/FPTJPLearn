import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { 
  ChevronLeft, 
  ChevronRight, 
  Sparkles, 
  Castle, 
  BookText, 
  Brain, 
  FileQuestion,
  GraduationCap,
  ShieldCheck,
  ShoppingBag,
  Star,
  Users,
} from 'lucide-react';
import { dashboardApi, type DashboardTrustSummary } from '../api/dashboardApi';

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

const TRUST_STAT_CONFIG = [
  { key: 'activeUsers', label: 'Người dùng truy cập', icon: <Users size={20} />, tone: 'bg-blue-50 text-blue-700 border-blue-100' },
  { key: 'paidLearners', label: 'Học viên đã mua khóa', icon: <ShoppingBag size={20} />, tone: 'bg-emerald-50 text-emerald-700 border-emerald-100' },
  { key: 'contentItems', label: 'Nội dung JPD/PE', icon: <GraduationCap size={20} />, tone: 'bg-amber-50 text-amber-700 border-amber-100' },
];

const EMPTY_TRUST_SUMMARY: DashboardTrustSummary = {
  activeUsers: 0,
  paidLearners: 0,
  contentItems: 0,
  recentBuyers: [],
};

const formatCompactNumber = (value: number) => {
  if (value >= 1000) {
    const compact = value / 1000;
    return `${Number.isInteger(compact) ? compact.toFixed(0) : compact.toFixed(1)}k+`;
  }

  return `${value}`;
};

export const DashboardPage = () => {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [trustSummary, setTrustSummary] = useState<DashboardTrustSummary>(EMPTY_TRUST_SUMMARY);
  const [trustLoading, setTrustLoading] = useState(true);

  useEffect(() => {
    const timer = setInterval(() => {
      setCurrentIndex((prev) => (prev + 1) % IMAGES.length);
    }, 5000);
    return () => clearInterval(timer);
  }, []);

  useEffect(() => {
    let ignore = false;

    const loadTrustSummary = async () => {
      try {
        const summary = await dashboardApi.getTrustSummary();
        if (!ignore) {
          setTrustSummary(summary);
        }
      } catch (error) {
        console.error('Failed to load dashboard trust summary:', error);
      } finally {
        if (!ignore) {
          setTrustLoading(false);
        }
      }
    };

    loadTrustSummary();

    return () => {
      ignore = true;
    };
  }, []);

  const next = () => setCurrentIndex((prev) => (prev + 1) % IMAGES.length);
  const prev = () => setCurrentIndex((prev) => (prev - 1 + IMAGES.length) % IMAGES.length);

  return (
    <div className="relative w-full animate-fade-in p-4 md:p-8">
      {/* Social Proof Section */}
      <section className="mb-6 w-full max-w-xs overflow-hidden rounded-[22px] border border-blue-100 bg-white/95 shadow-[3px_3px_0_#bfdbfe] xl:absolute xl:left-4 xl:top-8 xl:mb-0">
          <div className="p-4">
            <div className="flex items-start justify-between gap-3">
              <div>
                <p className="text-[10px] font-black uppercase tracking-wider text-blue-600">Được tin dùng</p>
                <h2 className="mt-1 font-heading text-lg font-black text-slate-950">Cộng đồng JPLearn</h2>
              </div>
              <div className="flex h-9 w-9 items-center justify-center rounded-xl border border-emerald-100 bg-emerald-50 text-emerald-700">
                <ShieldCheck size={18} />
              </div>
            </div>

            <div className="mt-4 grid gap-2">
              {TRUST_STAT_CONFIG.map((stat) => (
                <div key={stat.label} className={`flex items-center justify-between gap-3 rounded-2xl border px-3 py-2.5 ${stat.tone}`}>
                  <div className="flex min-w-0 items-center gap-2">
                    <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-xl bg-white/80">
                      {stat.icon}
                    </div>
                    <p className="truncate text-[10px] font-black uppercase tracking-wider opacity-80">{stat.label}</p>
                  </div>
                  <span className="font-heading text-xl font-black">
                    {trustLoading ? '...' : formatCompactNumber(trustSummary[stat.key])}
                  </span>
                </div>
              ))}
            </div>
          </div>

          <div className="border-t border-blue-100 bg-blue-50/60 p-4">
            <div className="flex items-center gap-2">
              <Star size={16} className="fill-amber-400 text-amber-400" />
              <h3 className="font-heading text-sm font-black text-slate-950">Mua gần đây</h3>
            </div>
            <div className="mt-3 space-y-2">
              {trustSummary.recentBuyers.slice(0, 2).map((buyer) => (
                <div key={`${buyer.buyer}-${buyer.packageName}`} className="rounded-2xl border border-white bg-white/90 p-2.5 shadow-sm">
                  <div className="flex items-center justify-between gap-3">
                    <p className="truncate text-xs font-black text-slate-900">{buyer.buyer}</p>
                    <span className="shrink-0 rounded-lg bg-blue-100 px-1.5 py-0.5 text-[9px] font-black uppercase tracking-wider text-blue-700">
                      {buyer.time}
                    </span>
                  </div>
                  <p className="mt-1 truncate text-[11px] font-bold text-slate-500">{buyer.packageName}</p>
                </div>
              ))}
              {!trustLoading && trustSummary.recentBuyers.length === 0 ? (
                <div className="rounded-2xl border border-white bg-white/90 p-2.5 text-[11px] font-bold text-slate-500 shadow-sm">
                  Chưa có giao dịch hiển thị.
                </div>
              ) : null}
            </div>
          </div>
        </section>

      <div className="mx-auto max-w-5xl space-y-8">
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
    </div>
  );
};
