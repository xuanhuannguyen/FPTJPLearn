import { NavLink } from 'react-router-dom';
import { 
  Castle, 
  Crown, 
  Brain, 
  BrainCircuit,
  BookText,
  FileQuestion,
  Mic2
} from 'lucide-react';

export const Sidebar = () => {
  const navItems = [
    { name: 'Hán tự', path: '/kanji', icon: <Castle size={24} /> },
    { name: 'Từ vựng', path: '/vocabulary', icon: <BookText size={24} /> },
    { name: 'Ngữ pháp', path: '/grammar', icon: <Brain size={24} /> },
    { name: 'Ghi nhớ', path: '/memory', icon: <BrainCircuit size={24} /> },
    { name: 'Từ vựng\nChủ động', path: '/active-vocabulary', icon: <Crown size={24} /> },
    { name: 'Luyện nói', path: '/speaking', icon: <Mic2 size={24} /> },
    { name: 'Luyện thi', path: '/exam', icon: <FileQuestion size={24} /> },
  ];

  return (
    <aside className="hidden h-screen w-[78px] flex-shrink-0 sticky top-0 bg-white md:flex">
      <div className="flex h-full w-full flex-col items-center border-r border-slate-100 bg-white overflow-y-auto scrollbar-hide">
      <div className="sticky top-0 z-10 flex h-16 w-full shrink-0 items-center justify-center border-b border-slate-100 bg-white">
        <div className="flex h-11 w-11 items-center justify-center rounded-xl bg-accent-primary text-white shadow-pop">
          <span className="font-heading text-xl font-black leading-none">JP</span>
        </div>
      </div>

      <nav className="w-full space-y-2 px-1 py-4">
        {navItems.map((item) => (
          <NavLink
            key={item.path}
            to={item.path}
            className={({ isActive }) =>
              `mx-auto flex h-[68px] w-[68px] cursor-pointer flex-col items-center justify-center gap-1.5 rounded-lg text-center transition-colors duration-200 ${
                isActive 
                  ? 'bg-slate-100 text-text-primary' 
                  : 'text-slate-500 hover:bg-slate-50 hover:text-text-primary'
              }`
            }
          >
            <div className="flex h-7 w-7 items-center justify-center">
              {item.icon}
            </div>
            <span className="whitespace-pre-line text-[11px] font-extrabold leading-tight">{item.name}</span>
          </NavLink>
        ))}
      </nav>

      </div>
    </aside>
  );
};
