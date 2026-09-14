import { NavLink, Link } from 'react-router-dom';
import {
  AudioLines,
  BookOpenCheck,
  BrainCircuit,
  Compass,
  Languages,
  Layers3,
  PencilLine,
  Trophy,
} from 'lucide-react';
import { AppLogo } from './AppLogo';

export const Sidebar = () => {
  const navItems = [
    { 
      name: 'Nhập\nMôn', 
      path: '/intro', 
      icon: <Compass size={18} strokeWidth={2.3} />,
      activeGrad: 'from-sky-500 to-blue-600',
      iconColor: 'text-sky-600',
      bgLight: 'bg-sky-50/90',
      borderLight: 'border-sky-200/80'
    },
    { 
      name: 'Hán tự', 
      path: '/kanji', 
      icon: <Languages size={18} strokeWidth={2.3} />,
      activeGrad: 'from-emerald-500 to-teal-600',
      iconColor: 'text-emerald-600',
      bgLight: 'bg-emerald-50/90',
      borderLight: 'border-emerald-200/80'
    },
    { 
      name: 'Từ vựng', 
      path: '/vocabulary', 
      icon: <BookOpenCheck size={18} strokeWidth={2.3} />,
      activeGrad: 'from-amber-500 to-orange-600',
      iconColor: 'text-amber-600',
      bgLight: 'bg-amber-50/90',
      borderLight: 'border-amber-200/80'
    },
    { 
      name: 'Ngữ pháp', 
      path: '/grammar', 
      icon: <Layers3 size={18} strokeWidth={2.3} />,
      activeGrad: 'from-teal-600 to-cyan-600',
      iconColor: 'text-teal-700',
      bgLight: 'bg-teal-50/90',
      borderLight: 'border-teal-200/80'
    },
    { 
      name: 'Ghi nhớ', 
      path: '/memory', 
      icon: <BrainCircuit size={18} strokeWidth={2.3} />,
      activeGrad: 'from-rose-500 to-pink-600',
      iconColor: 'text-rose-600',
      bgLight: 'bg-rose-50/90',
      borderLight: 'border-rose-200/80'
    },
    { 
      name: 'Từ vựng\nChủ động', 
      path: '/active-vocabulary', 
      icon: <PencilLine size={18} strokeWidth={2.3} />,
      activeGrad: 'from-orange-500 to-amber-600',
      iconColor: 'text-orange-600',
      bgLight: 'bg-orange-50/90',
      borderLight: 'border-orange-200/80'
    },
    { 
      name: 'Luyện nói', 
      path: '/speaking', 
      icon: <AudioLines size={18} strokeWidth={2.3} />,
      activeGrad: 'from-cyan-600 to-teal-600',
      iconColor: 'text-cyan-700',
      bgLight: 'bg-cyan-50/90',
      borderLight: 'border-cyan-200/80'
    },
    { 
      name: 'Luyện thi', 
      path: '/exam', 
      icon: <Trophy size={18} strokeWidth={2.3} />,
      activeGrad: 'from-amber-400 to-yellow-600',
      iconColor: 'text-amber-600',
      bgLight: 'bg-amber-50/90',
      borderLight: 'border-amber-200/80'
    },
  ];

  return (
    <aside className="sidebar-shell hidden h-screen w-[88px] flex-shrink-0 sticky top-0 md:flex flex-col select-none">
      <div className="sidebar-brand sticky top-0 z-10 flex h-[72px] w-full shrink-0 items-center justify-center">
        <Link
          to="/dashboard"
          aria-label="Về trang tổng quan"
          className="sidebar-logo p-1.5 transition-all duration-200 hover:scale-105 active:scale-95"
        >
          <AppLogo className="h-10 w-10 rounded-xl" imageClassName="p-0.5" />
        </Link>
      </div>

      <nav aria-label="Điều hướng học tập" className="sidebar-nav scrollbar-hide flex-1 w-full overflow-y-auto space-y-2 px-2 py-3">
        {navItems.map((item) => (
          <NavLink
            key={item.path}
            to={item.path}
            title={item.name.replace('\n', ' ')}
            className={({ isActive }) =>
              `sidebar-nav-link mx-auto group ${
                isActive ? 'sidebar-nav-link--active' : ''
              }`
            }
          >
            {({ isActive }) => (
              <>
                <div
                  className={`sidebar-nav-icon transition-all duration-200 ${
                    isActive
                      ? `bg-gradient-to-br ${item.activeGrad} text-white border-transparent shadow-md shadow-slate-300`
                      : `${item.bgLight} ${item.iconColor} ${item.borderLight} group-hover:scale-110 group-hover:shadow-sm`
                  }`}
                >
                  {item.icon}
                </div>
                <span className={`sidebar-nav-label whitespace-pre-line text-center transition-colors ${
                  isActive ? 'text-slate-900 font-black' : 'text-slate-600 group-hover:text-slate-900 font-bold'
                }`}>
                  {item.name}
                </span>
              </>
            )}
          </NavLink>
        ))}
      </nav>
    </aside>
  );
};
