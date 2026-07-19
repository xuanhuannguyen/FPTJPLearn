import { useMemo, useState, useRef, useEffect } from 'react';
import { useLocation, Link, useNavigate } from 'react-router-dom';
import { Bell, Search, LogOut, Sparkles, MessageSquare } from 'lucide-react';
import { useSearchStore } from '../stores/searchStore';
import { useAuthStore } from '../stores/authStore';
import { useUserAccess } from '../hooks/useUserAccess';

const NOTIFICATIONS = [
  {
    id: 3,
    icon: <MessageSquare size={16} className="text-emerald-600" />,
    iconBg: 'bg-emerald-50/80',
    title: 'Mentor & Mẹo thi SE',
    message: 'Mentor các môn SE từ kỳ 1 - 4, Mẹo Tips Thi Ib Facebook zalo',
    link: 'https://zalo.me/0833283840',
    isExternal: true,
    time: 'Mới',
  },
  {
    id: 1,
    icon: <MessageSquare size={16} className="text-blue-600" />,
    iconBg: 'bg-blue-50/80',
    title: 'Hỗ trợ Speaking',
    message: 'Sắp tới bạn nào yếu phần speaking thì có thể ib zalo facebook mình nhận mentor hỗ trợ',
    link: 'https://zalo.me/0833283840',
    isExternal: true,
    time: 'Mới',
  },
  {
    id: 2,
    icon: <Sparkles size={16} className="text-amber-600" />,
    iconBg: 'bg-amber-50/80',
    title: 'Tài liệu & Khóa học',
    message: 'Phần khóa học đã bao gồm source, các bài thi nói, tips luyện nói .... đừng ngần ngại mua :)',
    link: '/pricing',
    isExternal: false,
    time: 'Mới',
  }
];

export const Navbar = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const query = useSearchStore((state) => state.query);
  const setQuery = useSearchStore((state) => state.setQuery);
  const { user, logout } = useAuthStore();
  const { licensingEnabled } = useUserAccess();
  const [showMenu, setShowMenu] = useState(false);
  const [showNotifications, setShowNotifications] = useState(false);
  const [readIds, setReadIds] = useState<number[]>(() => {
    try {
      const savedStr = localStorage.getItem('jplearn_read_notifications');
      if (savedStr) {
        const saved = JSON.parse(savedStr);
        const todayStr = new Date().toDateString();
        if (saved.date === todayStr) {
          return saved.readIds || [];
        }
      }
      return [];
    } catch {
      return [];
    }
  });
  const menuRef = useRef<HTMLDivElement>(null);
  const bellRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const todayStr = new Date().toDateString();
    localStorage.setItem(
      'jplearn_read_notifications',
      JSON.stringify({ date: todayStr, readIds })
    );
  }, [readIds]);

  const unreadCount = NOTIFICATIONS.filter(n => !readIds.includes(n.id)).length;

  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (menuRef.current && !menuRef.current.contains(e.target as Node)) {
        setShowMenu(false);
      }
      if (bellRef.current && !bellRef.current.contains(e.target as Node)) {
        setShowNotifications(false);
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
        {licensingEnabled ? (
          <Link
            to="/pricing"
            className="mr-2 flex h-8 items-center gap-2 rounded-lg border-2 border-border bg-gradient-to-r from-blue-500 to-blue-600 px-3 text-white shadow-pop transition-all hover:-translate-y-0.5"
          >
            <Sparkles size={14} className="fill-current" />
            <span className="text-[10px] font-black uppercase tracking-wider">Nâng cấp</span>
          </Link>
        ) : null}

        <div className="relative" ref={bellRef}>
          <button
            onClick={() => {
              setShowNotifications(!showNotifications);
            }}
            className={`relative flex h-8 w-8 cursor-pointer items-center justify-center rounded-lg border-2 border-border bg-white/95 text-text-secondary shadow-pop transition-all hover:-translate-y-0.5 hover:text-text-primary ${
              showNotifications ? 'bg-slate-50 border-slate-900 text-text-primary' : ''
            }`}
          >
            <Bell size={16} />
            {unreadCount > 0 && (
              <span className="absolute -top-1.5 -right-1.5 flex h-[18px] w-[18px] items-center justify-center rounded-full bg-accent-danger text-[9px] font-black text-white ring-2 ring-white shadow-sm animate-pulse">
                {unreadCount}
              </span>
            )}
          </button>

          {showNotifications && (
            <div className="absolute right-0 top-full mt-2 w-[320px] sm:w-[380px] rounded-2xl border-2 border-border bg-white shadow-2xl overflow-hidden z-50 animate-fade-in">
              <div className="flex items-center justify-between border-b border-border px-4 py-3 bg-slate-50/50">
                <h3 className="font-heading text-sm font-black text-slate-900">Thông báo</h3>
                {unreadCount > 0 && (
                  <button
                    onClick={() => setReadIds(NOTIFICATIONS.map((n) => n.id))}
                    className="text-[10px] font-black uppercase tracking-wider text-blue-600 hover:underline cursor-pointer"
                  >
                    Đánh dấu tất cả đã đọc
                  </button>
                )}
              </div>
              <div className="max-h-[360px] overflow-y-auto divide-y divide-border scrollbar-hide">
                {NOTIFICATIONS.map((notif) => {
                  const isRead = readIds.includes(notif.id);
                  return (
                    <a
                      key={notif.id}
                      href={notif.link}
                      target={notif.isExternal ? "_blank" : undefined}
                      rel={notif.isExternal ? "noopener noreferrer" : undefined}
                      onClick={(e) => {
                        if (!notif.isExternal) {
                          e.preventDefault();
                          navigate(notif.link);
                        }
                        setShowNotifications(false);
                        if (!isRead) {
                          setReadIds((prev) => [...prev, notif.id]);
                        }
                      }}
                      className={`flex gap-3 p-4 transition-all hover:bg-slate-50/80 group cursor-pointer ${
                        isRead ? 'opacity-60 hover:opacity-100' : ''
                      }`}
                    >
                      <div className={`flex h-9 w-9 shrink-0 items-center justify-center rounded-xl ${notif.iconBg} border border-slate-100 group-hover:scale-105 transition-transform`}>
                        {notif.icon}
                      </div>
                      <div className="flex-1 space-y-1">
                        <div className="flex items-center justify-between">
                          <h4 className={`text-xs font-black transition-colors ${
                            isRead ? 'text-slate-700' : 'text-slate-950 group-hover:text-blue-600'
                          }`}>
                            {notif.title}
                          </h4>
                          {isRead ? (
                            <span className="text-[9px] font-bold uppercase tracking-wider text-slate-400">
                              Đã đọc
                            </span>
                          ) : (
                            <span className="text-[9px] font-black uppercase tracking-wider text-blue-600 bg-blue-50 px-1.5 py-0.5 rounded-md animate-pulse">
                              Mới
                            </span>
                          )}
                        </div>
                        <p className={`text-xs font-medium leading-relaxed ${
                          isRead ? 'text-slate-500' : 'text-slate-600'
                        }`}>
                          {notif.message}
                        </p>
                      </div>
                    </a>
                  );
                })}
              </div>
            </div>
          )}
        </div>
        
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
