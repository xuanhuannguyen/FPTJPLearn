import type { MemoryTypeSummary } from '../types/memory.types';

type MemoryStatsBarProps = {
  summary: MemoryTypeSummary;
};

export const MemoryStatsBar = ({ summary }: MemoryStatsBarProps) => {
  const stats = [
    { label: 'Cần ôn', value: summary.due, color: 'text-rose-600', bg: 'bg-rose-50/70 border-rose-100' },
    { label: 'Mới thêm', value: summary.new, color: 'text-sky-600', bg: 'bg-sky-50/70 border-sky-100' },
    { label: 'Đang học (< 6 phút)', value: summary.learning, color: 'text-amber-600', bg: 'bg-amber-50/70 border-amber-100' },
    { label: 'Mới thuộc (ngắn hạn < 21 ngày)', value: summary.shortTerm, color: 'text-orange-600', bg: 'bg-orange-50/70 border-orange-100' },
    { label: 'Đã thuộc (dài hạn)', value: summary.longTerm, color: 'text-emerald-600', bg: 'bg-emerald-50/70 border-emerald-100' },
    { label: 'Tổng đã học qua', value: summary.totalStudied, color: 'text-slate-900', bg: 'bg-slate-50 border-slate-200/80' },
  ];

  return (
    <section className="grid grid-cols-2 gap-3 rounded-2xl border border-slate-200/90 bg-white/95 p-3.5 shadow-sm md:grid-cols-3 xl:grid-cols-6 backdrop-blur-md">
      {stats.map((stat) => (
        <div 
          key={stat.label} 
          className={`flex flex-col items-center justify-center rounded-xl border p-3 transition-all duration-200 hover:-translate-y-0.5 hover:shadow-sm ${stat.bg}`}
        >
          <p className={`text-2xl font-black tracking-tight ${stat.color}`}>{stat.value}</p>
          <p className="mt-1 text-[11px] font-bold text-slate-600 text-center leading-tight">{stat.label}</p>
        </div>
      ))}
    </section>
  );
};
