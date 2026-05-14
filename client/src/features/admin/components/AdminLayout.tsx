import { Link, NavLink, Outlet, useNavigate } from 'react-router-dom';
import { BarChart3, FileQuestion, Home, Settings, Users, ShoppingCart, LogOut, Loader2 } from 'lucide-react';
import { useEffect, useState } from 'react';
import { Footer } from '../../../shared/components/Footer';
import { apiClient } from '../../../shared/api/axios';
import type { AxiosError } from 'axios';
import { AppLogo } from '../../../shared/components/AppLogo';

const ADMIN_PATH = '/jplearn-manage-xh21';

const navItems = [
  { label: 'Dashboard', path: ADMIN_PATH, icon: Home },
  { label: 'Đơn hàng', path: `${ADMIN_PATH}/orders`, icon: ShoppingCart },
  { label: 'Câu hỏi luyện thi', path: `${ADMIN_PATH}/exam-questions`, icon: FileQuestion },
  { label: 'Người dùng', path: `${ADMIN_PATH}/users`, icon: Users },
  { label: 'Thông tin web', path: `${ADMIN_PATH}/site`, icon: Settings, disabled: true },
];

export const AdminLayout = () => {
  const navigate = useNavigate();
  const [isAuthorized, setIsAuthorized] = useState(false);
  const [isChecking, setIsChecking] = useState(true);

  const handleLogout = () => {
    localStorage.removeItem('jplearn_admin_key');
    navigate('/');
  };

  useEffect(() => {
    const checkAuth = async () => {
      let savedKey = localStorage.getItem('jplearn_admin_key');
      
      if (!savedKey) {
        savedKey = window.prompt('Nhập mã bí mật Admin để tiếp tục:');
        const normalizedKey = savedKey?.trim();
        if (normalizedKey) {
          localStorage.setItem('jplearn_admin_key', normalizedKey);
        } else {
          navigate('/');
          return;
        }
      } else {
        const normalizedKey = savedKey.trim();
        if (normalizedKey !== savedKey) {
          localStorage.setItem('jplearn_admin_key', normalizedKey);
        }
      }

      let attempts = 0;
      const maxAttempts = 3;

      while (attempts < maxAttempts) {
        try {
          setIsChecking(true);
          await apiClient.get('/admin/verify');
          setIsAuthorized(true);
          return; // Thành công thì thoát
        } catch (error: any) {
          attempts++;
          console.error(`Admin verification attempt ${attempts} failed:`, error);

          const status = error?.response?.status;
          if (status === 401) {
            localStorage.removeItem('jplearn_admin_key');
            alert('Mã bí mật không chính xác! Vui lòng kiểm tra lại.');
            navigate('/');
            return;
          }

          if (attempts >= maxAttempts) {
            alert('Lỗi kết nối Server! Vui lòng kiểm tra Tunnel (Pinggy) hoặc Backend có đang chạy không.');
            navigate('/');
          } else {
            // Đợi 5 giây trước khi thử lại để server kịp khởi động
            await new Promise(resolve => setTimeout(resolve, 5000));
          }
        } finally {
          if (attempts >= maxAttempts) {
            setIsChecking(false);
          }
        }
      }
    };
    
    checkAuth();
  }, [navigate]);

  if (isChecking) {
    return (
      <div className="flex min-h-screen flex-col items-center justify-center bg-slate-950 text-white p-6 text-center">
        <Loader2 className="mb-4 animate-spin text-blue-500" size={40} />
        <p className="font-bold tracking-widest text-slate-400 uppercase">Đang xác thực quyền Admin...</p>
        <p className="mt-2 text-xs text-slate-500 max-w-xs">
          Nếu server đang nghỉ, quá trình này có thể mất tới 30 giây. Vui lòng kiên nhẫn một chút nhé!
        </p>
      </div>
    );
  }

  if (!isAuthorized) return null;

  return (
    <div className="min-h-screen bg-slate-950 text-slate-100">
      <aside className="fixed inset-y-0 left-0 hidden w-72 border-r border-slate-800 bg-slate-950/95 px-5 py-6 lg:block">
        <Link to={ADMIN_PATH} className="flex items-center gap-3">
          <AppLogo className="h-11 w-11 rounded-xl border border-slate-700 shadow-[3px_3px_0_#020617]" />
          <div>
            <p className="font-heading text-xl font-black">JPLearn Admin</p>
            <p className="text-xs font-bold text-slate-400">Content operations</p>
          </div>
        </Link>

        <nav className="mt-8 space-y-2">
          {navItems.map((item) => {
            const Icon = item.icon;
            if (item.disabled) {
              return (
                <div key={item.path} className="flex h-12 items-center gap-3 rounded-xl px-3 text-sm font-black text-slate-600">
                  <Icon size={19} />
                  {item.label}
                  <span className="ml-auto rounded-full bg-slate-900 px-2 py-0.5 text-[10px] uppercase text-slate-500">soon</span>
                </div>
              );
            }

            return (
              <NavLink
                key={item.path}
                to={item.path}
                end={item.path === ADMIN_PATH}
                className={({ isActive }) =>
                  `flex h-12 items-center gap-3 rounded-xl px-3 text-sm font-black transition-colors ${isActive ? 'bg-blue-600 text-white' : 'text-slate-300 hover:bg-slate-900 hover:text-white'
                  }`
                }
              >
                <Icon size={19} />
                {item.label}
              </NavLink>
            );
          })}
        </nav>

        <div className="absolute bottom-6 left-0 w-full px-5">
          <button
            onClick={handleLogout}
            className="flex h-12 w-full items-center gap-3 rounded-xl px-3 text-sm font-black text-red-400 transition-colors hover:bg-red-500/10 hover:text-red-300"
          >
            <LogOut size={19} />
            Đăng xuất Admin
          </button>
        </div>
      </aside>

      <div className="lg:pl-72">
        <header className="sticky top-0 z-20 border-b border-slate-800 bg-slate-950/90 px-4 py-3 backdrop-blur lg:px-8">
          <div className="flex items-center justify-between gap-4">
            <div className="flex items-center gap-3">
              <BarChart3 size={22} className="text-blue-400" />
              <span className="font-heading text-lg font-black">Admin Console</span>
            </div>
            <Link to="/exam" className="rounded-lg border border-slate-700 px-3 py-2 text-xs font-black text-slate-300 hover:bg-slate-900">
              Về app học
            </Link>
          </div>
        </header>

        <main className="min-h-[calc(100vh-57px)] flex flex-col bg-slate-100 text-slate-950">
          <div className="flex-1">
            <Outlet />
          </div>
          <Footer />
        </main>
      </div>
    </div>
  );
}
