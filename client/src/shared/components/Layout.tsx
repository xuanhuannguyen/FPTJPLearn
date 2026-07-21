import { Outlet, useLocation } from 'react-router-dom';
import { Sidebar } from './Sidebar';
import { Navbar } from './Navbar';
import { Footer } from './Footer';

export const Layout = () => {
  const location = useLocation();
  const hideSidebar = location.pathname.startsWith('/notifications');

  return (
    <div className="flex h-screen overflow-hidden bg-transparent text-text-primary">
      {hideSidebar ? null : <Sidebar />}
      <div className="relative flex flex-1 flex-col overflow-hidden">
        <Navbar />
        
        <main className={`relative min-h-0 flex-1 blue-grid ${hideSidebar ? 'overflow-hidden' : 'overflow-y-auto'}`}>
          <div className={`animate-fade-in flex flex-col ${hideSidebar ? 'h-full min-h-0' : 'min-h-full'}`}>
            <div className={`flex-1 ${hideSidebar ? 'h-full min-h-0 p-0' : 'p-4'}`}>
              <Outlet />
            </div>
          </div>
        </main>
        <Footer />
      </div>
    </div>
  );
};
