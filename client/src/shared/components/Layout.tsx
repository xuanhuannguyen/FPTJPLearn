import { Outlet } from 'react-router-dom';
import { Sidebar } from './Sidebar';
import { Navbar } from './Navbar';
import { Footer } from './Footer';

export const Layout = () => {
  return (
    <div className="flex h-screen overflow-hidden bg-transparent text-text-primary">
      <Sidebar />
      <div className="relative flex flex-1 flex-col overflow-hidden">
        <Navbar />
        
        <main className="relative flex-1 overflow-y-auto blue-grid">
          <div className="animate-fade-in flex flex-col min-h-full">
            <div className="flex-1 p-4">
              <Outlet />
            </div>
            <Footer />
          </div>
        </main>
      </div>
    </div>
  );
};
