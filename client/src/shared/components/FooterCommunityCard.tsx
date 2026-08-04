import { useState, useEffect } from 'react';
import { ShieldCheck, ShoppingBag, Star, Users, GraduationCap } from 'lucide-react';
import { dashboardApi, type DashboardTrustSummary } from '../../features/dashboard/api/dashboardApi';

const TRUST_STAT_CONFIG = [
  { key: 'activeUsers', label: 'Truy cập', icon: <Users size={14} />, tone: 'bg-blue-50 text-blue-700 border-blue-100' },
  { key: 'paidLearners', label: 'Đã mua', icon: <ShoppingBag size={14} />, tone: 'bg-emerald-50 text-emerald-700 border-emerald-100' },
  { key: 'contentItems', label: 'Nội dung', icon: <GraduationCap size={14} />, tone: 'bg-amber-50 text-amber-700 border-amber-100' },
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

export const FooterCommunityCard = () => {
  const [trustSummary, setTrustSummary] = useState<DashboardTrustSummary>(EMPTY_TRUST_SUMMARY);
  const [trustLoading, setTrustLoading] = useState(true);

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

  return (
    <div className="flex min-w-0 flex-col gap-2">
      <div className="flex flex-wrap items-center gap-x-3 gap-y-1.5">
        <div className="flex min-w-0 items-center gap-2 pr-1">
          <span className="flex h-8 w-8 shrink-0 items-center justify-center rounded-xl border border-emerald-100 bg-emerald-50 text-emerald-700">
            <ShieldCheck size={16} />
          </span>
          <div className="min-w-0">
            <p className="text-[9px] font-black uppercase tracking-[0.18em] text-blue-600">Được tin dùng</p>
            <h2 className="truncate font-heading text-sm font-black text-slate-950">Cộng đồng JPLearn</h2>
          </div>
        </div>

        {TRUST_STAT_CONFIG.map((stat) => (
          <div key={stat.label} className={`flex h-7 items-center gap-1.5 rounded-full border px-2.5 ${stat.tone}`}>
            {stat.icon}
            <span className="font-heading text-sm font-black">
              {trustLoading ? '...' : formatCompactNumber(trustSummary[stat.key])}
            </span>
            <span className="text-[9px] font-black uppercase tracking-wider opacity-75">{stat.label}</span>
          </div>
        ))}
      </div>

      <div className="flex min-w-0 flex-wrap items-center gap-x-2 gap-y-1 text-[10px] font-bold text-slate-500">
        <span className="flex items-center gap-1 font-black text-slate-700">
          <Star size={13} className="fill-amber-400 text-amber-400" />
          Mua gần đây:
        </span>
        {trustSummary.recentBuyers.slice(0, 2).map((buyer) => (
          <span key={`${buyer.buyer}-${buyer.packageName}`} className="min-w-0 max-w-[230px] truncate rounded-full bg-slate-50 px-2 py-0.5">
            <span className="font-black text-slate-800">{buyer.buyer}</span>
            <span className="text-slate-400"> · {buyer.packageName} · {buyer.time}</span>
          </span>
        ))}
        {!trustLoading && trustSummary.recentBuyers.length === 0 ? (
          <span className="rounded-full bg-slate-50 px-2 py-0.5">Chưa có giao dịch hiển thị.</span>
        ) : null}
      </div>
    </div>
  );
};
