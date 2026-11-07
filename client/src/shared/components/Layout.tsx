import { useEffect, useState } from 'react';
import { Outlet, useLocation } from 'react-router-dom';
import { Sidebar } from './Sidebar';
import { Navbar } from './Navbar';
import { Footer } from './Footer';

export const Layout = () => {
  const location = useLocation();
  const hideSidebar = location.pathname.startsWith('/notifications');
  const [mobileNavOpen, setMobileNavOpen] = useState(false);

  useEffect(() => {
    if (!mobileNavOpen) return;
    const previousOverflow = document.body.style.overflow;
    document.body.style.overflow = 'hidden';
    const closeOnEscape = (event: KeyboardEvent) => {
      if (event.key === 'Escape') setMobileNavOpen(false);
    };
    window.addEventListener('keydown', closeOnEscape);
    return () => {
      document.body.style.overflow = previousOverflow;
      window.removeEventListener('keydown', closeOnEscape);
    };
  }, [mobileNavOpen]);

  return (
    <div className="app-layout flex h-screen overflow-hidden text-text-primary">
      {hideSidebar ? null : <Sidebar />}
      {mobileNavOpen && (
        <div className="mobile-nav-layer md:hidden">
          <button type="button" className="mobile-nav-backdrop" onClick={() => setMobileNavOpen(false)} aria-label="Đóng menu điều hướng" />
          <Sidebar mobile onNavigate={() => setMobileNavOpen(false)} onClose={() => setMobileNavOpen(false)} />
        </div>
      )}
      <div className="relative flex flex-1 flex-col overflow-hidden">
        <Navbar onMenuClick={() => setMobileNavOpen(true)} mobileNavOpen={mobileNavOpen} />
        
        <main className={`relative min-h-0 flex-1 blue-grid ${hideSidebar ? 'overflow-hidden' : 'overflow-y-auto'}`}>
          <div className={`animate-fade-in flex flex-col ${hideSidebar ? 'h-full min-h-0' : 'min-h-full'}`}>
            <div className={`flex-1 ${hideSidebar ? 'h-full min-h-0 p-0' : 'app-content p-4 sm:p-6'}`}>
              <Outlet />
            </div>
          </div>
        </main>
        <Footer />
      </div>
    </div>
  );
};
