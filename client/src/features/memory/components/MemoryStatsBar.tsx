import type { MemoryTypeSummary } from '../types/memory.types';

type MemoryStatsBarProps = {
  summary: MemoryTypeSummary;
};

export const MemoryStatsBar = ({ summary }: MemoryStatsBarProps) => {
  const stats = [
    { label: 'Cần ôn', value: summary.due, color: 'text-orange-500' },
    { label: 'Mới thêm', value: summary.new, color: 'text-blue-500' },
    { label: 'Đang học (< 6 phút)', value: summary.learning, color: 'text-amber-500' },
    { label: 'Mới thuộc (ngắn hạn < 21 ngày)', value: summary.shortTerm, color: 'text-orange-600' },
    { label: 'Đã thuộc (dài hạn)', value: summary.longTerm, color: 'text-emerald-500' },
    { label: 'Tổng đã học qua', value: summary.totalStudied, color: 'text-slate-900' },
  ];

  return (
    <section className="grid grid-cols-2 gap-4 rounded-2xl border border-slate-200 bg-white p-6 shadow-md md:grid-cols-3 xl:grid-cols-6">
      {stats.map((stat) => (
        <div key={stat.label} className="text-center">
          <p className={`text-2xl font-black ${stat.color}`}>{stat.value}</p>
          <p className="mt-1 text-[11px] font-bold text-text-secondary leading-tight">{stat.label}</p>
        </div>
      ))}
    </section>
  );
};
