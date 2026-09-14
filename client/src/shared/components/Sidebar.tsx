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
    { name: 'Nhập\nMôn', path: '/intro', icon: <Compass size={19} strokeWidth={2.2} /> },
    { name: 'Hán tự', path: '/kanji', icon: <Languages size={19} strokeWidth={2.2} /> },
    { name: 'Từ vựng', path: '/vocabulary', icon: <BookOpenCheck size={19} strokeWidth={2.2} /> },
    { name: 'Ngữ pháp', path: '/grammar', icon: <Layers3 size={19} strokeWidth={2.2} /> },
    { name: 'Ghi nhớ', path: '/memory', icon: <BrainCircuit size={19} strokeWidth={2.2} /> },
    { name: 'Từ vựng\nChủ động', path: '/active-vocabulary', icon: <PencilLine size={19} strokeWidth={2.2} /> },
    { name: 'Luyện nói', path: '/speaking', icon: <AudioLines size={19} strokeWidth={2.2} /> },
    { name: 'Luyện thi', path: '/exam', icon: <Trophy size={19} strokeWidth={2.2} /> },
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

      <nav aria-label="Điều hướng học tập" className="sidebar-nav scrollbar-hide flex-1 w-full overflow-y-auto space-y-2 px-2.5 py-3">
        {navItems.map((item) => (
          <NavLink
            key={item.path}
            to={item.path}
            title={item.name.replace('\n', ' ')}
            className={({ isActive }) =>
              `sidebar-nav-link mx-auto ${
                isActive ? 'sidebar-nav-link--active' : ''
              }`
            }
          >
            <div className="sidebar-nav-icon">
              {item.icon}
            </div>
            <span className="sidebar-nav-label whitespace-pre-line text-center">{item.name}</span>
          </NavLink>
        ))}
      </nav>
    </aside>
  );
};
