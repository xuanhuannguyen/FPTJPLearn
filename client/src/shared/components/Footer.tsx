import { Phone, Globe, MessageSquare, GraduationCap, Link2 } from 'lucide-react';
import { FooterCommunityCard } from './FooterCommunityCard';

interface FooterProps {
  showCommunityCard?: boolean;
}

export const Footer = ({ showCommunityCard = true }: FooterProps) => {
  return (
    <footer className="mt-auto border-t border-slate-100 bg-white/80 py-2 backdrop-blur-sm">
      <div className="w-full px-4 lg:px-8">
        <div className={showCommunityCard ? 'grid gap-2 lg:grid-cols-[minmax(0,1fr)_auto_minmax(0,1fr)] lg:items-center' : 'flex flex-col gap-2 md:flex-row md:items-center md:justify-center'}>
          {showCommunityCard ? (
            <div className="min-w-0">
              <FooterCommunityCard />
            </div>
          ) : null}

          {/* Contact Info */}
          <div className={showCommunityCard ? 'flex min-w-max flex-wrap items-center justify-center gap-4 lg:col-start-2' : 'flex flex-wrap items-center justify-center gap-4'}>
              <div className="flex items-center gap-2">
                <img
                  src="/xuanhuan.webp"
                  alt="Nguyễn Xuân Huấn"
                  className="h-8 w-8 rounded-full border-2 border-blue-500/20 object-cover shadow-sm"
                  loading="lazy"
                  decoding="async"
                />
                <div>
                  <p className="font-heading text-sm font-black text-slate-900 underline decoration-blue-500/30 decoration-2 underline-offset-4">Nguyễn Xuân Huấn</p>
                </div>
              </div>

              <div className="flex items-center gap-3">
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
          <div className={showCommunityCard ? 'flex flex-wrap justify-center gap-x-3 gap-y-0.5 text-[10px] font-black uppercase tracking-widest text-slate-400 lg:col-start-3 lg:justify-end' : 'flex flex-wrap justify-center gap-x-3 gap-y-0.5 text-[10px] font-black uppercase tracking-widest text-slate-400'}>
              <span className="flex items-center gap-1.5 px-1.5 py-0.5 transition-colors hover:text-blue-600 cursor-default">
                <Globe size={12} className="text-blue-400" /> Nhận Thiết kế web
              </span>
              <span className="flex items-center gap-1.5 px-1.5 py-0.5 transition-colors hover:text-blue-600 cursor-default">
                <MessageSquare size={12} className="text-blue-400" /> Mentor Nhật Speaking 1 - 1
              </span>
              <span className="flex items-center gap-1.5 px-1.5 py-0.5 transition-colors hover:text-blue-600 cursor-default">
                <GraduationCap size={12} className="text-blue-400" /> Mentor SE Kỳ 1 - 4
              </span>
          </div>
        </div>

        <div className="mt-1 border-t border-slate-50 pt-1 text-center">
          <p className="text-[9px] font-bold uppercase tracking-[0.2em] text-slate-300">
            © {new Date().getFullYear()} JPLearn • Build by Xuan Huan
          </p>
        </div>
      </div>
    </footer>
  );
};
