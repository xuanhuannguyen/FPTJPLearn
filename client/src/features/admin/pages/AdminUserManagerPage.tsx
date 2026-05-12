import { useState, useEffect } from 'react';
import { apiClient } from '../../../shared/api/axios';
import { RefreshCw, Search, ShieldCheck, Mail, Smartphone } from 'lucide-react';

export function AdminUserManagerPage() {
  const [users, setUsers] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');

  const fetchUsers = async () => {
    setLoading(true);
    try {
      const res = await apiClient.get('/admin/users');
      setUsers(res.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  const resetDevice = async (userId: string) => {
    if (!confirm('Xác nhận Reset thiết bị? Người dùng sẽ phải login lại.')) return;
    try {
      await apiClient.post(`/admin/users/${userId}/reset-device`);
      fetchUsers();
    } catch (err) {
      alert('Lỗi khi reset');
    }
  };

  const filteredUsers = users.filter(u => 
    u.email?.toLowerCase().includes(searchTerm.toLowerCase()) || 
    u.displayName?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="p-8 max-w-6xl mx-auto space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-black uppercase flex items-center gap-2">
            <ShieldCheck className="text-blue-600" /> Quản lý Người dùng
          </h1>
          <p className="text-slate-500 text-sm font-bold">Quản lý phiên đăng nhập và thiết bị.</p>
        </div>
        <button onClick={fetchUsers} className="p-2 border border-black hover:bg-slate-50">
          <RefreshCw size={18} className={loading ? 'animate-spin' : ''} />
        </button>
      </div>

      <div className="relative">
        <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400" size={18} />
        <input 
          type="text" 
          placeholder="Tìm theo tên hoặc email..."
          className="w-full pl-10 pr-4 py-3 border border-black focus:shadow-[4px_4px_0px_#000] outline-none"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {filteredUsers.map(u => (
          <div key={u.id} className="border border-black bg-white p-4 hover:shadow-[4px_4px_0px_#000] transition-all">
            <div className="flex items-center gap-4 mb-4">
              <img src={u.avatarUrl || `https://ui-avatars.com/api/?name=${u.displayName}`} className="w-12 h-12 rounded-full border-2 border-black" />
              <div className="min-w-0">
                <h3 className="font-black text-sm uppercase truncate">{u.displayName}</h3>
                <div className="flex items-center gap-1 text-slate-500 text-xs">
                  <Mail size={10} /> {u.email}
                </div>
              </div>
            </div>
            
            <div className="bg-slate-50 p-3 border border-slate-200 space-y-2 mb-4">
              <div className="flex items-center justify-between text-[10px] font-bold uppercase">
                <span className="text-slate-400 flex items-center gap-1"><Smartphone size={10} /> Thiết bị</span>
                {u.activeDeviceToken ? (
                   <span className="text-blue-600">Đang Online</span>
                ) : (
                   <span className="text-slate-400">Ngoại tuyến</span>
                )}
              </div>
              <div className="font-mono text-[10px] break-all text-slate-500">
                {u.activeDeviceToken || 'Chưa có thiết bị đăng ký'}
              </div>
            </div>

            <button 
              onClick={() => resetDevice(u.id)}
              disabled={!u.activeDeviceToken}
              className="w-full py-2 bg-white border border-black text-xs font-black uppercase hover:bg-black hover:text-white transition-colors disabled:opacity-30 disabled:hover:bg-white disabled:hover:text-black"
            >
              Reset Session
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}
