import { useCallback, useEffect, useState } from 'react';
import { apiClient } from '../../../shared/api/axios';
import { CreditCard, CheckCircle, Clock, Loader2, Calendar } from 'lucide-react';

type OrderStatus = 'pending' | 'paid' | 'failed' | 'cancelled' | string;

interface AdminOrder {
  id: string;
  userDisplayName: string;
  userEmail: string;
  packageCode: string;
  amount: number;
  provider: string;
  status: OrderStatus;
  createdAt: string;
  paidAt?: string | null;
}

export function AdminOrderManagerPage() {
  const [orders, setOrders] = useState<AdminOrder[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [statusFilter, setStatusFilter] = useState<'all' | 'paid' | 'pending'>('all');
  const [startDate, setStartDate] = useState<string>('');
  const [endDate, setEndDate] = useState<string>('');

  const filteredOrders = orders.filter((o) => {
    if (statusFilter === 'paid' && o.status !== 'paid') return false;
    if (statusFilter === 'pending' && o.status === 'paid') return false;

    if (startDate) {
      const orderDate = new Date(o.createdAt);
      const start = new Date(startDate);
      start.setHours(0, 0, 0, 0);
      if (orderDate < start) return false;
    }

    if (endDate) {
      const orderDate = new Date(o.createdAt);
      const end = new Date(endDate);
      end.setHours(23, 59, 59, 999);
      if (orderDate > end) return false;
    }

    return true;
  });

  const totalPaidAll = orders
    .filter((o) => o.status === 'paid')
    .reduce((sum, o) => sum + o.amount, 0);

  const totalPaidFiltered = filteredOrders
    .filter((o) => o.status === 'paid')
    .reduce((sum, o) => sum + o.amount, 0);

  const fetchOrders = useCallback(async () => {
    setIsLoading(true);
    try {
      const res = await apiClient.get<AdminOrder[]>('/admin/orders');
      setOrders(res.data);
    } catch (err) {
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    const timer = window.setTimeout(() => {
      void fetchOrders();
    }, 0);

    return () => window.clearTimeout(timer);
  }, [fetchOrders]);

  return (
    <div className="p-8 max-w-6xl mx-auto space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-black uppercase flex items-center gap-2">
            <CreditCard className="text-orange-600" /> Quản lý Đơn hàng
          </h1>
          <p className="text-slate-500 text-sm font-bold">Lịch sử giao dịch SePay và PayOS.</p>
        </div>
      </div>

      {/* Doanh thu stats cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
        <div className="border-2 border-black p-4 bg-emerald-50 shadow-[4px_4px_0px_#000]">
          <div className="text-xs font-black uppercase text-emerald-800">Tổng doanh thu (Tất cả)</div>
          <div className="text-2xl font-black mt-1 text-slate-900">
            {new Intl.NumberFormat('vi-VN').format(totalPaidAll)}đ
          </div>
        </div>
        <div className="border-2 border-black p-4 bg-blue-50 shadow-[4px_4px_0px_#000]">
          <div className="text-xs font-black uppercase text-blue-800">Doanh thu bộ lọc</div>
          <div className="text-2xl font-black mt-1 text-slate-900">
            {new Intl.NumberFormat('vi-VN').format(totalPaidFiltered)}đ
          </div>
        </div>
      </div>

      {/* Filters bar */}
      <div className="flex flex-col lg:flex-row lg:items-center justify-between gap-4 bg-white border-2 border-black p-4 shadow-[4px_4px_0px_#000]">
        {/* Status filters */}
        <div className="flex flex-wrap gap-2">
          <button
            onClick={() => setStatusFilter('all')}
            className={`px-3 py-1.5 text-xs font-black uppercase border-2 border-black shadow-[2px_2px_0px_#000] hover:-translate-y-0.5 active:translate-y-0.5 active:shadow-[1px_1px_0px_#000] transition-all cursor-pointer ${
              statusFilter === 'all'
                ? 'bg-slate-950 text-white'
                : 'bg-white text-slate-900 hover:bg-slate-50'
            }`}
          >
            Tất cả ({orders.length})
          </button>
          <button
            onClick={() => setStatusFilter('paid')}
            className={`px-3 py-1.5 text-xs font-black uppercase border-2 border-black shadow-[2px_2px_0px_#000] hover:-translate-y-0.5 active:translate-y-0.5 active:shadow-[1px_1px_0px_#000] transition-all cursor-pointer ${
              statusFilter === 'paid'
                ? 'bg-emerald-500 text-white'
                : 'bg-white text-slate-900 hover:bg-slate-50'
            }`}
          >
            Thành công ({orders.filter((o) => o.status === 'paid').length})
          </button>
          <button
            onClick={() => setStatusFilter('pending')}
            className={`px-3 py-1.5 text-xs font-black uppercase border-2 border-black shadow-[2px_2px_0px_#000] hover:-translate-y-0.5 active:translate-y-0.5 active:shadow-[1px_1px_0px_#000] transition-all cursor-pointer ${
              statusFilter === 'pending'
                ? 'bg-amber-500 text-white'
                : 'bg-white text-slate-900 hover:bg-slate-50'
            }`}
          >
            Chờ thanh toán ({orders.filter((o) => o.status !== 'paid').length})
          </button>
        </div>

        {/* Date filters */}
        <div className="flex flex-wrap items-center gap-3">
          <div className="flex items-center gap-2">
            <span className="text-[10px] font-black uppercase flex items-center gap-1 text-slate-600">
              <Calendar size={12} /> Từ ngày:
            </span>
            <input
              type="date"
              value={startDate}
              onChange={(e) => setStartDate(e.target.value)}
              className="border-2 border-black px-2 py-1 text-xs font-bold focus:outline-none bg-white"
            />
          </div>
          <div className="flex items-center gap-2">
            <span className="text-[10px] font-black uppercase flex items-center gap-1 text-slate-600">
              <Calendar size={12} /> Đến ngày:
            </span>
            <input
              type="date"
              value={endDate}
              onChange={(e) => setEndDate(e.target.value)}
              className="border-2 border-black px-2 py-1 text-xs font-bold focus:outline-none bg-white"
            />
          </div>
          {(startDate || endDate) && (
            <button
              onClick={() => {
                setStartDate('');
                setEndDate('');
              }}
              className="px-3 py-1.5 bg-rose-500 text-white text-[10px] font-black uppercase border-2 border-black shadow-[2px_2px_0px_#000] hover:-translate-y-0.5 active:translate-y-0.5 active:shadow-[1px_1px_0px_#000] transition-all cursor-pointer"
            >
              Xóa ngày
            </button>
          )}
        </div>
      </div>

      <div className="border border-black bg-white overflow-hidden shadow-[4px_4px_0px_#000]">
        <table className="w-full text-left">
          <thead className="bg-slate-50 border-b border-black">
            <tr>
              <th className="p-4 text-xs font-black uppercase">Đơn hàng</th>
              <th className="p-4 text-xs font-black uppercase">Khách hàng</th>
              <th className="p-4 text-xs font-black uppercase">Gói</th>
              <th className="p-4 text-xs font-black uppercase">Số tiền</th>
              <th className="p-4 text-xs font-black uppercase">Thời gian tạo</th>
              <th className="p-4 text-xs font-black uppercase">Thanh toán lúc</th>
              <th className="p-4 text-xs font-black uppercase">Provider</th>
              <th className="p-4 text-xs font-black uppercase">Trạng thái</th>
            </tr>
          </thead>
          <tbody>
            {isLoading ? (
              <tr>
                <td className="p-6 text-center text-sm font-bold text-slate-500" colSpan={8}>
                  <span className="inline-flex items-center gap-2">
                    <Loader2 size={18} className="animate-spin" /> Đang tải đơn hàng...
                  </span>
                </td>
              </tr>
            ) : null}
            {!isLoading && filteredOrders.length === 0 ? (
              <tr>
                <td className="p-8 text-center text-sm font-bold text-slate-500" colSpan={8}>
                  Không có đơn hàng nào khớp với bộ lọc.
                </td>
              </tr>
            ) : null}
            {filteredOrders.map(o => (
              <tr key={o.id} className="border-b border-slate-100 last:border-0 hover:bg-slate-50">
                <td className="p-4">
                  <span className="font-mono text-[10px] text-slate-500">{o.id.slice(0, 8)}</span>
                </td>
                <td className="p-4">
                  <div className="text-xs font-bold uppercase">{o.userDisplayName}</div>
                  <div className="text-[10px] text-slate-400">{o.userEmail}</div>
                </td>
                <td className="p-4">
                  <span className="bg-slate-100 px-2 py-0.5 border border-slate-200 text-[10px] font-black uppercase">
                    {o.packageCode}
                  </span>
                </td>
                <td className="p-4">
                  <div className="text-sm font-black">{new Intl.NumberFormat('vi-VN').format(o.amount)}đ</div>
                </td>
                <td className="p-4">
                  <div className="text-xs text-slate-600 font-medium">
                    {new Date(o.createdAt).toLocaleString('vi-VN', {
                      year: 'numeric',
                      month: '2-digit',
                      day: '2-digit',
                      hour: '2-digit',
                      minute: '2-digit'
                    })}
                  </div>
                </td>
                <td className="p-4">
                  <div className="text-xs font-medium text-slate-600">
                    {o.status === 'paid' && o.paidAt ? (
                      new Date(o.paidAt).toLocaleString('vi-VN', {
                        year: 'numeric',
                        month: '2-digit',
                        day: '2-digit',
                        hour: '2-digit',
                        minute: '2-digit'
                      })
                    ) : (
                      <span className="text-slate-300 font-bold">-</span>
                    )}
                  </div>
                </td>
                <td className="p-4">
                  <div className="flex items-center gap-1 text-[10px] font-bold uppercase">
                    {o.provider}
                  </div>
                </td>
                <td className="p-4">
                  {o.status === 'paid' ? (
                    <span className="inline-flex items-center gap-1 text-[10px] font-black uppercase text-green-600">
                      <CheckCircle size={12} /> Thành công
                    </span>
                  ) : (
                    <span className="inline-flex items-center gap-1 text-[10px] font-black uppercase text-slate-400">
                      <Clock size={12} /> Chờ thanh toán
                    </span>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
