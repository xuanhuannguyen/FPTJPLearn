import { useMemo } from 'react';
import { useLocation } from 'react-router-dom';
import { Bell, Search, User } from 'lucide-react';
import { useSearchStore } from '../stores/searchStore';

export const Navbar = () => {
  const location = useLocation();
  const query = useSearchStore((state) => state.query);
  const setQuery = useSearchStore((state) => state.setQuery);

  const searchConfig = useMemo(() => {
    if (location.pathname === '/folders') {
      return {
        placeholder: 'Search folders...',
      };
    }

    return null;
  }, [location.pathname]);

  return (
    <header className="sticky top-0 z-10 flex h-12 flex-shrink-0 items-center justify-between border-b-2 border-border bg-white/70 px-4 backdrop-blur-xl sm:px-6">
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
        <button className="relative flex h-8 w-8 cursor-pointer items-center justify-center rounded-lg border-2 border-border bg-white/95 text-text-secondary shadow-pop transition-all hover:-translate-y-0.5 hover:text-text-primary">
          <Bell size={16} />
          <span className="absolute right-1.5 top-1.5 h-1 w-1 rounded-full bg-accent-danger"></span>
        </button>
        
        <div className="flex h-8 w-8 cursor-pointer items-center justify-center rounded-lg border-2 border-border bg-bg-tertiary text-text-secondary shadow-pop transition-all hover:-translate-y-0.5 hover:bg-white">
          <User size={12} />
        </div>
      </div>
    </header>
  );
};
