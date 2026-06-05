import { Phone, Globe, MessageSquare, GraduationCap, Link2 } from 'lucide-react';

export const Footer = () => {
  return (
    <footer className="mt-auto border-t border-slate-100 bg-white/80 py-4 backdrop-blur-sm">
      <div className="mx-auto max-w-7xl px-4 lg:px-8">
        <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
          {/* Contact Info */}
          <div className="flex flex-wrap items-center gap-6">
            <div className="flex items-center gap-2">
              <img
                src="/xuanhuan.webp"
                alt="Nguyễn Xuân Huấn"
                className="h-9 w-9 rounded-full border-2 border-blue-500/20 object-cover shadow-sm"
                loading="lazy"
                decoding="async"
              />
              <div>
                <p className="font-heading text-base font-black text-slate-900 underline decoration-blue-500/30 decoration-2 underline-offset-4">Nguyễn Xuân Huấn</p>
              </div>
            </div>

            <div className="flex items-center gap-4">
              <a
                href="https://zalo.me/0833283840"
                target="_blank"
                rel="noopener noreferrer"
                className="flex items-center gap-1.5 text-xs font-bold text-slate-600 transition-colors hover:text-blue-600"
              >
                <Phone size={14} className="text-blue-500" />
                Zalo: 0833283840
              </a>
              <a
                href="https://www.facebook.com/xunhuns/"
                target="_blank"
                rel="noopener noreferrer"
                className="flex items-center gap-1.5 text-xs font-bold text-slate-600 transition-colors hover:text-blue-600"
              >
                <Link2 size={14} className="text-blue-500" />
                FB: xunhuns
              </a>
            </div>
          </div>

          {/* Services - Compact Light */}
          <div className="flex flex-wrap gap-4 text-[10px] font-black uppercase tracking-widest text-slate-400">
            <span className="flex items-center gap-1.5 px-2 py-1 transition-colors hover:text-blue-600 cursor-default">
              <Globe size={12} className="text-blue-400" /> Nhận Thiết kế web
            </span>
            <span className="flex items-center gap-1.5 px-2 py-1 transition-colors hover:text-blue-600 cursor-default">
              <MessageSquare size={12} className="text-blue-400" /> Mentor Nhật Speaking 1 - 1
            </span>
            <span className="flex items-center gap-1.5 px-2 py-1 transition-colors hover:text-blue-600 cursor-default">
              <GraduationCap size={12} className="text-blue-400" /> Mentor SE Kỳ 1 - 4
            </span>
          </div>
        </div>

        <div className="mt-4 border-t border-slate-50 pt-3 text-center">
          <p className="text-[9px] font-bold uppercase tracking-[0.2em] text-slate-300">
            © {new Date().getFullYear()} JPLearn • Build by Xuan Huan
          </p>
        </div>
      </div>
    </footer>
  );
};
