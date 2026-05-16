import { useMemo, useState, useRef, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import { Bell, Search, LogOut, Sparkles } from 'lucide-react';
import { Link } from 'react-router-dom';
import { useSearchStore } from '../stores/searchStore';
import { useAuthStore } from '../stores/authStore';
import { isFreeExperienceEnabled } from '../config/features';

export const Navbar = () => {
  const location = useLocation();
  const query = useSearchStore((state) => state.query);
  const setQuery = useSearchStore((state) => state.setQuery);
  const { user, logout } = useAuthStore();
  const [showMenu, setShowMenu] = useState(false);
  const menuRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (menuRef.current && !menuRef.current.contains(e.target as Node)) {
        setShowMenu(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const searchConfig = useMemo(() => {
    if (location.pathname === '/folders') {
      return {
        placeholder: 'Search folders...',
      };
    }

    return null;
  }, [location.pathname]);

  return (
    <header className="sticky top-0 z-40 flex h-12 flex-shrink-0 items-center justify-between border-b-2 border-border bg-white/70 px-4 backdrop-blur-xl sm:px-6">
      <div className="flex-1 max-w-2xl">
        {searchConfig ? (
          <div className="relative">
            <label htmlFor="global-search" className="sr-only">
              Global search
            </label>
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-text-muted" size={14} />
            <input
              id="global-search"
              type="text"
              value={query}
              onChange={(event) => setQuery(event.target.value)}
              placeholder={searchConfig.placeholder}
              className="form-control form-control-with-icon h-8 py-0 text-[10px]"
            />
          </div>
        ) : null}
      </div>
      
      <div className="ml-4 flex items-center gap-2">
        {!isFreeExperienceEnabled ? (
          <Link
            to="/pricing"
            className="mr-2 flex h-8 items-center gap-2 rounded-lg border-2 border-border bg-gradient-to-r from-blue-500 to-blue-600 px-3 text-white shadow-pop transition-all hover:-translate-y-0.5"
          >
            <Sparkles size={14} className="fill-current" />
            <span className="text-[10px] font-black uppercase tracking-wider">Nâng cấp</span>
          </Link>
        ) : null}

        <button className="relative flex h-8 w-8 cursor-pointer items-center justify-center rounded-lg border-2 border-border bg-white/95 text-text-secondary shadow-pop transition-all hover:-translate-y-0.5 hover:text-text-primary">
          <Bell size={16} />
          <span className="absolute right-1.5 top-1.5 h-1 w-1 rounded-full bg-accent-danger"></span>
        </button>
        
        <div className="relative" ref={menuRef}>
          <button
            onClick={() => setShowMenu(!showMenu)}
            className="flex h-8 w-8 cursor-pointer items-center justify-center rounded-lg border-2 border-border overflow-hidden shadow-pop transition-all hover:-translate-y-0.5"
          >
            {user?.photoURL ? (
              <img src={user.photoURL} alt="" className="h-full w-full object-cover" referrerPolicy="no-referrer" />
            ) : (
              <span className="text-xs font-bold text-text-secondary">
                {user?.displayName?.[0] || '?'}
              </span>
            )}
          </button>
          
          {showMenu && (
            <div className="absolute right-0 top-full mt-2 w-56 rounded-xl border-2 border-border bg-white shadow-lg overflow-hidden z-50">
              <div className="px-4 py-3 border-b border-border">
                <p className="text-sm font-semibold text-text-primary truncate">{user?.displayName}</p>
                <p className="text-xs text-text-muted truncate">{user?.email}</p>
              </div>
              <button
                onClick={() => { setShowMenu(false); logout(); }}
                className="flex w-full items-center gap-2 px-4 py-2.5 text-sm text-accent-danger hover:bg-red-50 transition-colors cursor-pointer"
              >
                <LogOut size={14} />
                Đăng xuất
              </button>
            </div>
          )}
        </div>
      </div>
    </header>
  );
};
