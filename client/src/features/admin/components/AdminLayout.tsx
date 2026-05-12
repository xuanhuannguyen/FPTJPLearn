import { Link, NavLink, Outlet } from 'react-router-dom';
import { BarChart3, FileQuestion, Home, Settings, Users } from 'lucide-react';

const navItems = [
  { label: 'Dashboard', path: '/admin', icon: Home },
  { label: 'Câu hỏi luyện thi', path: '/admin/exam-questions', icon: FileQuestion },
  { label: 'Người dùng', path: '/admin/users', icon: Users, disabled: true },
  { label: 'Thông tin web', path: '/admin/site', icon: Settings, disabled: true },
];

export const AdminLayout = () => (
  <div className="min-h-screen bg-slate-950 text-slate-100">
    <aside className="fixed inset-y-0 left-0 hidden w-72 border-r border-slate-800 bg-slate-950/95 px-5 py-6 lg:block">
      <Link to="/admin" className="flex items-center gap-3">
        <span className="grid h-11 w-11 place-items-center rounded-xl bg-blue-600 font-heading text-xl font-black text-white shadow-[3px_3px_0_#020617]">
          JP
        </span>
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
              end={item.path === '/admin'}
              className={({ isActive }) =>
                `flex h-12 items-center gap-3 rounded-xl px-3 text-sm font-black transition-colors ${
                  isActive ? 'bg-blue-600 text-white' : 'text-slate-300 hover:bg-slate-900 hover:text-white'
                }`
              }
            >
              <Icon size={19} />
              {item.label}
            </NavLink>
          );
        })}
      </nav>
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

      <main className="min-h-[calc(100vh-57px)] bg-slate-100 text-slate-950">
        <Outlet />
      </main>
    </div>
  </div>
);
