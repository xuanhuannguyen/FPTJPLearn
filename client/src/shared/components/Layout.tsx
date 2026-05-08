import { Outlet } from 'react-router-dom';
import { Sidebar } from './Sidebar';
import { Navbar } from './Navbar';

export const Layout = () => {
  return (
    <div className="flex h-screen overflow-hidden bg-transparent text-text-primary">
      <Sidebar />
      <div className="relative flex flex-1 flex-col overflow-hidden">
        <Navbar />
        
        <main className="relative flex-1 overflow-y-auto blue-grid">
          <div className="animate-fade-in p-4">
            <Outlet />
          </div>
        </main>
      </div>
    </div>
  );
};
