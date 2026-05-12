import { useState, useEffect } from 'react';
import { apiClient } from '../../../shared/api/axios';
import { CreditCard, CheckCircle, XCircle, Clock, ExternalLink } from 'lucide-react';

export function AdminOrderManagerPage() {
  const [orders, setOrders] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  const fetchOrders = async () => {
    setLoading(true);
    try {
      const res = await apiClient.get('/admin/orders');
      setOrders(res.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchOrders();
  }, []);

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

      <div className="border border-black bg-white overflow-hidden shadow-[4px_4px_0px_#000]">
        <table className="w-full text-left">
          <thead className="bg-slate-50 border-b border-black">
            <tr>
              <th className="p-4 text-xs font-black uppercase">Đơn hàng</th>
              <th className="p-4 text-xs font-black uppercase">Khách hàng</th>
              <th className="p-4 text-xs font-black uppercase">Gói</th>
              <th className="p-4 text-xs font-black uppercase">Số tiền</th>
              <th className="p-4 text-xs font-black uppercase">Provider</th>
              <th className="p-4 text-xs font-black uppercase">Trạng thái</th>
            </tr>
          </thead>
          <tbody>
            {orders.map(o => (
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
