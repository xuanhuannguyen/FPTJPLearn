import { useEffect, useState } from 'react';
import { apiClient } from '../../../shared/api/axios';
import { Shield, ShieldCheck, User as UserIcon, Calendar, Mail, Smartphone } from 'lucide-react';

interface UserSubscription {
  courseCode: string;
  expiresAt: string;
  isActive: boolean;
}

interface AdminUser {
  id: string;
  email: string;
  displayName: string;
  avatarUrl: string;
  createdAt: string;
  activeDeviceToken: string | null;
  subscriptions: UserSubscription[];
}

export const AdminUserManagerPage = () => {
  const [users, setUsers] = useState<AdminUser[]>([]);
  const [loading, setLoading] = useState(true);

  const fetchUsers = async () => {
    try {
      const res = await apiClient.get('/admin/users');
      setUsers(res.data);
    } catch (err) {
      console.error('Fetch users failed:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  const toggleSubscription = async (userId: string, courseCode: string, currentStatus: boolean) => {
    const isAdding = !currentStatus;
    const actionText = isAdding ? 'CẤP QUYỀN' : 'HỦY QUYỀN';
    if (!window.confirm(`Bạn có chắc muốn ${actionText} gói ${courseCode.toUpperCase()} cho người dùng này?`)) return;

    // Nếu thêm thì mặc định 6 tháng, nếu bỏ thì cho về quá khứ
    const expiresAt = isAdding 
      ? new Date(Date.now() + 180 * 24 * 60 * 60 * 1000).toISOString()
      : new Date(0).toISOString();

    try {
      await apiClient.post(`/admin/users/${userId}/subscriptions`, {
        courseCode,
        expiresAt
      });
      alert('Cập nhật thành công!');
      fetchUsers(); // Refresh data
    } catch (err) {
      console.error('Toggle subscription failed:', err);
      alert('Có lỗi xảy ra khi cập nhật gói premium.');
    }
  };

  const resetDevice = async (userId: string) => {
    if (!window.confirm('Bạn có chắc muốn reset thiết bị cho người dùng này?')) return;
    try {
      await apiClient.post(`/admin/users/${userId}/reset-device`);
      alert('Reset thiết bị thành công.');
      fetchUsers();
    } catch (err) {
      console.error('Reset device failed:', err);
    }
  };

  if (loading) return <div className="p-8">Đang tải danh sách người dùng...</div>;

  return (
    <div className="mx-auto max-w-7xl space-y-6 px-4 py-8 lg:px-8">
      <header>
        <h1 className="font-heading text-3xl font-black text-slate-950">Quản lý người dùng</h1>
        <p className="mt-2 text-sm font-bold text-slate-600">Xem thông tin và cấp quyền Premium thủ công.</p>
      </header>

      <div className="grid gap-4">
        {users.map((user) => {
          const hasJPD113 = user.subscriptions.some(s => s.courseCode === 'jpd113' && s.isActive);
          const hasJPD123 = user.subscriptions.some(s => s.courseCode === 'jpd123' && s.isActive);

          return (
            <div key={user.id} className="flex flex-col gap-6 rounded-2xl border border-slate-200 bg-white p-6 shadow-sm md:flex-row md:items-center">
              <div className="flex flex-1 items-center gap-4">
                <div className="grid h-12 w-12 place-items-center rounded-full bg-slate-100">
                  {user.avatarUrl ? (
                    <img src={user.avatarUrl} alt={user.displayName} className="h-full w-full rounded-full object-cover" />
                  ) : (
                    <UserIcon className="text-slate-400" />
                  )}
                </div>
                <div>
                  <h3 className="font-heading text-lg font-black text-slate-950">{user.displayName}</h3>
                  <div className="mt-1 flex flex-wrap gap-x-4 gap-y-1 text-xs font-bold text-slate-500">
                    <span className="flex items-center gap-1.5"><Mail size={14} /> {user.email}</span>
                    <span className="flex items-center gap-1.5"><Calendar size={14} /> Tham gia: {new Date(user.createdAt).toLocaleDateString('vi-VN')}</span>
                  </div>
                </div>
              </div>

              <div className="flex flex-wrap items-center gap-3 border-t border-slate-100 pt-4 md:border-t-0 md:pt-0">
                {/* JPD113 */}
                <button
                  onClick={() => toggleSubscription(user.id, 'jpd113', hasJPD113)}
                  className={`flex h-10 items-center gap-2 rounded-xl border px-4 text-xs font-black transition-all ${
                    hasJPD113 
                      ? 'border-green-200 bg-green-50 text-green-700' 
                      : 'border-slate-200 bg-white text-slate-600 hover:border-slate-300'
                  }`}
                >
                  {hasJPD113 ? <ShieldCheck size={16} /> : <Shield size={16} />}
                  JPD113
                </button>

                {/* JPD123 */}
                <button
                  onClick={() => toggleSubscription(user.id, 'jpd123', hasJPD123)}
                  className={`flex h-10 items-center gap-2 rounded-xl border px-4 text-xs font-black transition-all ${
                    hasJPD123 
                      ? 'border-green-200 bg-green-50 text-green-700' 
                      : 'border-slate-200 bg-white text-slate-600 hover:border-slate-300'
                  }`}
                >
                  {hasJPD123 ? <ShieldCheck size={16} /> : <Shield size={16} />}
                  JPD123
                </button>

                {/* Reset Device */}
                <button
                  onClick={() => resetDevice(user.id)}
                  title="Reset thiết bị (cho phép đăng nhập máy mới)"
                  className="flex h-10 w-10 items-center justify-center rounded-xl border border-orange-200 bg-orange-50 text-orange-600 hover:bg-orange-100"
                >
                  <Smartphone size={18} />
                </button>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};
