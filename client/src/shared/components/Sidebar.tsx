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
  X,
} from 'lucide-react';
import { AppLogo } from './AppLogo';

const navItems = [
  { name: 'Nhập môn', path: '/intro', icon: <Compass size={18} strokeWidth={2.2} /> },
  { name: 'Hán tự', path: '/kanji', icon: <Languages size={18} strokeWidth={2.2} /> },
  { name: 'Từ vựng', path: '/vocabulary', icon: <BookOpenCheck size={18} strokeWidth={2.2} /> },
  { name: 'Ngữ pháp', path: '/grammar', icon: <Layers3 size={18} strokeWidth={2.2} /> },
  { name: 'Ghi nhớ', path: '/memory', icon: <BrainCircuit size={18} strokeWidth={2.2} /> },
  { name: 'Từ vựng chủ động', path: '/active-vocabulary', icon: <PencilLine size={18} strokeWidth={2.2} /> },
  { name: 'Luyện nói', path: '/speaking', icon: <AudioLines size={18} strokeWidth={2.2} /> },
  { name: 'Luyện thi', path: '/exam', icon: <Trophy size={18} strokeWidth={2.2} /> },
];

type SidebarProps = {
  mobile?: boolean;
  onNavigate?: () => void;
  onClose?: () => void;
};

export const Sidebar = ({ mobile = false, onNavigate, onClose }: SidebarProps) => (
  <aside
    className={`sidebar-shell ${mobile ? 'sidebar-shell--mobile flex' : 'hidden md:flex'} h-screen shrink-0 flex-col select-none`}
    aria-label="Điều hướng học tập"
  >
    <div className={`sidebar-brand flex shrink-0 items-center justify-center ${mobile ? 'sidebar-brand--mobile' : ''}`}>
      <Link
        to="/dashboard"
        onClick={onNavigate}
        aria-label="JPLearn – về trang tổng quan"
        className="sidebar-logo transition-transform duration-200 hover:scale-105 active:scale-95"
      >
        <AppLogo className="h-10 w-10 rounded-xl" imageClassName="p-0.5" />
      </Link>
      {mobile && (
        <div className="sidebar-mobile-brand">
          <strong>JPLearn</strong>
          <span>Học tiếng Nhật mỗi ngày</span>
        </div>
      )}
      {mobile && (
        <button type="button" onClick={onClose} className="sidebar-close" aria-label="Đóng menu">
          <X size={19} />
        </button>
      )}
    </div>

    <nav aria-label="Các mục học" className={`sidebar-nav scrollbar-hide flex-1 overflow-y-auto ${mobile ? 'sidebar-nav--mobile' : ''}`}>
      {mobile && <span className="sidebar-section-label">KHÁM PHÁ</span>}
      {navItems.map((item) => (
        <NavLink
          key={item.path}
          to={item.path}
          onClick={onNavigate}
          title={item.name}
          className={({ isActive }) => `sidebar-nav-link group ${item.path === '/active-vocabulary' ? 'sidebar-nav-link--long' : ''} ${isActive ? 'sidebar-nav-link--active' : ''}`}
        >
          <span className="sidebar-nav-icon" aria-hidden="true">{item.icon}</span>
          <span className="sidebar-nav-label">{item.name}</span>
        </NavLink>
      ))}
    </nav>
    {mobile && <div className="sidebar-mobile-footnote">JPD113 · JPD123 · JPD133</div>}
  </aside>
);
