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
    <section className="grid gap-4 rounded-2xl bg-white p-5 shadow-sm md:grid-cols-3 xl:grid-cols-6">
      {stats.map((stat) => (
        <div key={stat.label} className="text-center">
          <p className={`text-3xl font-black ${stat.color}`}>{stat.value}</p>
          <p className="mt-1 text-sm font-bold text-text-secondary">{stat.label}</p>
        </div>
      ))}
    </section>
  );
};
