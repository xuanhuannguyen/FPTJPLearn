import { NavLink } from 'react-router-dom';
import { BookOpen, LayoutDashboard, Settings } from 'lucide-react';

export const Sidebar = () => {
  const navItems = [
    { name: 'Dashboard', path: '/dashboard', icon: <LayoutDashboard size={20} /> },
    { name: 'Vocabulary', path: '/vocabulary', icon: <BookOpen size={20} /> },
  ];

  return (
    <aside className="w-64 flex-shrink-0 bg-bg-secondary border-r border-border h-full flex flex-col hidden md:flex">
      <div className="h-16 flex items-center px-6 border-b border-border">
        <h1 className="text-xl font-bold text-accent-primary tracking-wide">
          JPLearn
        </h1>
      </div>
      
      <nav className="flex-1 overflow-y-auto py-6 px-3 space-y-1">
        {navItems.map((item) => (
          <NavLink
            key={item.path}
            to={item.path}
            className={({ isActive }) =>
              `flex items-center gap-3 px-3 py-2.5 rounded-lg transition-colors duration-200 cursor-pointer ${
                isActive 
                  ? 'bg-accent-primary/10 text-accent-primary font-medium' 
                  : 'text-text-secondary hover:bg-bg-tertiary hover:text-text-primary'
              }`
            }
          >
            {item.icon}
            <span>{item.name}</span>
          </NavLink>
        ))}
      </nav>

      <div className="p-4 border-t border-border">
        <button className="flex items-center gap-3 px-3 py-2.5 w-full text-left rounded-lg text-text-secondary hover:bg-bg-tertiary hover:text-text-primary transition-colors cursor-pointer">
          <Settings size={20} />
          <span>Settings</span>
        </button>
      </div>
    </aside>
  );
};
