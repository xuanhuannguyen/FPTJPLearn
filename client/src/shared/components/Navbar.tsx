import { useMemo } from 'react';
import { useLocation } from 'react-router-dom';
import { Bell, Search, User } from 'lucide-react';
import { useSearchStore } from '../stores/searchStore';

export const Navbar = () => {
  const location = useLocation();
  const query = useSearchStore((state) => state.query);
  const setQuery = useSearchStore((state) => state.setQuery);

  const searchConfig = useMemo(() => {
    if (location.pathname === '/vocabulary') {
      return {
        placeholder: 'Search lists or vocabulary...',
      };
    }

    if (location.pathname === '/folders') {
      return {
        placeholder: 'Search folders...',
      };
    }

    return null;
  }, [location.pathname]);

  return (
    <header className="h-16 flex-shrink-0 backdrop-blur-md border-b border-border flex items-center justify-between px-6 z-10 sticky top-0" style={{ backgroundColor: 'rgba(255, 255, 255, 0.85)' }}>
      <div className="flex-1 max-w-xl">
        {searchConfig ? (
          <div className="relative">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-text-muted" size={18} />
            <input
              type="text"
              value={query}
              onChange={(event) => setQuery(event.target.value)}
              placeholder={searchConfig.placeholder}
              className="w-full bg-bg-secondary border border-border rounded-lg pl-10 pr-4 py-2 text-sm focus:outline-none focus:border-accent-primary focus:ring-1 focus:ring-accent-primary transition-colors text-text-primary placeholder:text-text-muted"
            />
          </div>
        ) : null}
      </div>
      
      <div className="flex items-center gap-4 ml-6">
        <button className="p-2 text-text-secondary hover:text-text-primary hover:bg-bg-secondary rounded-lg transition-colors cursor-pointer relative">
          <Bell size={20} />
          <span className="absolute top-1.5 right-1.5 w-2 h-2 bg-accent-danger rounded-full"></span>
        </button>
        
        <div className="h-8 w-8 bg-bg-tertiary rounded-full flex items-center justify-center text-text-secondary cursor-pointer border border-border hover:border-accent-primary transition-colors">
          <User size={16} />
        </div>
      </div>
    </header>
  );
};
