import { useState } from 'react';
import { Link, Navigate, useParams } from 'react-router-dom';
import { ArrowLeft, BadgePercent, CheckCircle2, ChevronLeft, ChevronRight, CreditCard, ExternalLink, Gift, Link2, MessageSquare, Phone, Sparkles } from 'lucide-react';
import { FIRST_NOTIFICATION_ID, NOTIFICATIONS, type AppNotification } from '../../../shared/data/notifications';
import { useNotificationReads } from '../../../shared/hooks/useNotificationReads';

const isExternalLink = (link: string) => /^https?:\/\//.test(link);

const NotificationIcon = ({ notification, size = 20 }: { notification: AppNotification; size?: number }) => {
  if (notification.icon === 'sparkles') {
    return <Sparkles size={size} className={notification.iconColor} />;
  }

  return <MessageSquare size={size} className={notification.iconColor} />;
};

export const NotificationsPage = () => {
  const { notificationId } = useParams();
  const [galleryState, setGalleryState] = useState({ notificationId: FIRST_NOTIFICATION_ID, index: 0 });
  const { readIds, markAsRead } = useNotificationReads();
  const selectedId = Number(notificationId || FIRST_NOTIFICATION_ID);
  const selectedNotification = NOTIFICATIONS.find((notification) => notification.id === selectedId);

  if (!selectedNotification) {
    return <Navigate to={`/notifications/${FIRST_NOTIFICATION_ID}`} replace />;
  }

  const gallery = selectedNotification.gallery || [];
  const galleryIndex = galleryState.notificationId === selectedId ? galleryState.index : 0;
  const activeGalleryImage = gallery[galleryIndex];
  const hasMultipleGalleryImages = gallery.length > 1;
  const selectedIsRead = readIds.includes(selectedNotification.id);

  const showPreviousImage = () => {
    setGalleryState((current) => {
      const currentIndex = current.notificationId === selectedId ? current.index : 0;
      return {
        notificationId: selectedId,
        index: currentIndex === 0 ? gallery.length - 1 : currentIndex - 1,
      };
    });
  };

  const showNextImage = () => {
    setGalleryState((current) => {
      const currentIndex = current.notificationId === selectedId ? current.index : 0;
      return {
        notificationId: selectedId,
        index: (currentIndex + 1) % gallery.length,
      };
    });
  };

  return (
    <div className="flex h-full min-h-0 w-full">
      <section className="grid h-full min-h-0 w-full overflow-hidden bg-white lg:grid-cols-[360px_minmax(0,1fr)]">
        <aside className="flex min-h-0 flex-col border-b border-border bg-gradient-to-b from-blue-50/80 via-slate-50 to-white lg:border-b-0 lg:border-r">
          <div className="min-h-0 flex-1 space-y-2 overflow-y-auto p-3">
            {NOTIFICATIONS.map((notification) => {
              const isActive = notification.id === selectedNotification.id;
              const isRead = readIds.includes(notification.id);

              return (
                <Link
                  key={notification.id}
                  to={`/notifications/${notification.id}`}
                  onClick={() => markAsRead(notification.id)}
                  className={`flex gap-3 rounded-[16px] border p-3.5 transition-all ${
                    isActive
                      ? 'border-blue-700 bg-blue-600 text-white shadow-card'
                      : 'border-slate-200 bg-white/85 text-slate-600 hover:-translate-y-0.5 hover:border-border-hover hover:bg-white hover:text-slate-950 hover:shadow-lift'
                  }`}
                >
                  <div
                    className={`flex h-10 w-10 shrink-0 items-center justify-center rounded-xl border ${
                      isActive ? 'border-white/40 bg-white' : `${notification.iconBg} border-slate-100`
                    }`}
                  >
                    <NotificationIcon notification={notification} size={18} />
                  </div>
                  <div className="min-w-0 flex-1">
                    <div className="flex items-center justify-between gap-2">
                      <h3 className="truncate text-sm font-black">{notification.title}</h3>
                      <span
                        className={`shrink-0 rounded-md px-1.5 py-0.5 text-[9px] font-black uppercase tracking-wider ${
                          isActive
                            ? 'bg-white/20 text-white'
                            : isRead
                              ? 'bg-slate-100 text-slate-400'
                              : 'bg-blue-50 text-blue-600'
                        }`}
                      >
                        {isRead ? 'Đã đọc' : notification.time}
                      </span>
                    </div>
                    <p className={`mt-1 line-clamp-2 text-xs font-medium leading-relaxed ${isActive ? 'text-blue-50' : 'text-slate-500'}`}>
                      {notification.message}
                    </p>
                  </div>
                </Link>
              );
            })}
          </div>
          <div className="shrink-0 bg-white/95 p-3">
            <Link
              to="/dashboard"
              className="inline-flex h-10 w-full items-center justify-center gap-2 rounded-lg border border-red-800 bg-red-600 px-3 text-[10px] font-black uppercase tracking-wider text-white shadow-pop transition-all hover:-translate-y-0.5 hover:bg-red-700"
            >
              <ArrowLeft size={14} />
              Quay lại app học bài
            </Link>
          </div>
        </aside>

        <article className="min-h-0 overflow-y-auto p-4 md:p-6">
          <div className="flex flex-col gap-5">
          <div className="overflow-hidden rounded-[18px] border border-blue-200 bg-white text-slate-950 shadow-card">
            <div className="bg-[radial-gradient(circle_at_top_left,#dbeafe,transparent_36%),linear-gradient(135deg,#ffffff,#eff6ff_58%,#f8fafc)] p-4 md:p-5">
              <div className="flex items-start gap-4">
                <div className={`flex h-10 w-10 shrink-0 items-center justify-center rounded-xl ${selectedNotification.iconBg} border border-blue-100`}>
                  <NotificationIcon notification={selectedNotification} size={20} />
                </div>
                <div className="min-w-0">
                  <div className="flex flex-wrap items-center gap-2">
                    <h2 className="font-heading text-xl font-black text-slate-950 md:text-2xl">{selectedNotification.title}</h2>
                    <span className={`rounded-lg px-2 py-1 text-[10px] font-black uppercase tracking-wider ${
                      selectedIsRead ? 'bg-slate-100 text-slate-500' : 'bg-blue-100 text-blue-700'
                    }`}>
                      {selectedIsRead ? 'Đã đọc' : selectedNotification.time}
                    </span>
                  </div>
                  <p className="mt-2 text-sm font-bold leading-relaxed text-slate-600">{selectedNotification.message}</p>
                  {selectedNotification.badges?.length ? (
                    <div className="mt-3 flex flex-wrap gap-2">
                      {selectedNotification.badges.map((badge) => (
                        <span
                          key={badge}
                          className="rounded-lg border border-blue-100 bg-white px-2.5 py-1 text-[10px] font-black uppercase tracking-wider text-blue-700"
                        >
                          {badge}
                        </span>
                      ))}
                    </div>
                  ) : null}
                </div>
              </div>
            </div>
          </div>

          <div className="rounded-[18px] border border-slate-200 bg-slate-50/70 p-5">
            <h3 className="text-sm font-black uppercase tracking-wider text-slate-900">Nội dung</h3>
            <p className="mt-3 text-sm font-medium leading-7 text-slate-700">{selectedNotification.description}</p>
          </div>

          {selectedNotification.pricing?.length ? (
            <div className="grid gap-3 md:grid-cols-2">
              {selectedNotification.pricing.map((item, index) => {
                const PriceIcon = index === 0 ? BadgePercent : Gift;

                return (
                <div
                  key={item.label}
                  className={`flex items-center gap-3 rounded-[14px] border px-3.5 py-3 shadow-pop ${
                    item.tone === 'emerald'
                      ? 'border-emerald-700 bg-emerald-50 text-emerald-950'
                      : 'border-blue-700 bg-blue-50 text-blue-950'
                  }`}
                >
                  <div
                    className={`flex h-10 w-10 shrink-0 items-center justify-center rounded-xl ${
                      item.tone === 'emerald' ? 'bg-emerald-100 text-emerald-700' : 'bg-blue-100 text-blue-700'
                    }`}
                  >
                    <PriceIcon size={20} />
                  </div>
                  <div className="min-w-0">
                    <p className="text-[11px] font-black uppercase tracking-wider opacity-75">{item.label}</p>
                    <p className="font-heading text-3xl font-black leading-none">{item.price}</p>
                  </div>
                </div>
              );
              })}
            </div>
          ) : null}

          {selectedNotification.highlights?.length ? (
            <div className="rounded-[18px] border border-slate-200 bg-white p-5">
              <h3 className="text-sm font-black uppercase tracking-wider text-slate-900">Source bao gồm</h3>
              <div className="mt-4 grid gap-3">
                {selectedNotification.highlights.map((highlight) => (
                  <div key={highlight} className="flex gap-3 rounded-xl bg-slate-50 p-3">
                    <CheckCircle2 size={18} className="mt-0.5 shrink-0 text-emerald-600" />
                    <p className="text-sm font-semibold leading-6 text-slate-700">{highlight}</p>
                  </div>
                ))}
              </div>
            </div>
          ) : null}

          {activeGalleryImage ? (
            <div className="rounded-[18px] border border-slate-200 bg-slate-50/70 p-5">
              <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
                <h3 className="text-sm font-black uppercase tracking-wider text-slate-900">Ảnh preview source</h3>
                {hasMultipleGalleryImages ? (
                  <div className="flex items-center gap-2">
                    <button
                      type="button"
                      onClick={showPreviousImage}
                      className="flex h-9 w-9 items-center justify-center rounded-lg border border-border bg-white text-slate-900 shadow-pop transition-all hover:-translate-y-0.5"
                      aria-label="Ảnh trước"
                    >
                      <ChevronLeft size={18} />
                    </button>
                    <span className="min-w-12 text-center text-xs font-black text-slate-500">
                      {galleryIndex + 1}/{gallery.length}
                    </span>
                    <button
                      type="button"
                      onClick={showNextImage}
                      className="flex h-9 w-9 items-center justify-center rounded-lg border border-border bg-white text-slate-900 shadow-pop transition-all hover:-translate-y-0.5"
                      aria-label="Ảnh sau"
                    >
                      <ChevronRight size={18} />
                    </button>
                  </div>
                ) : null}
              </div>
              <figure className="mx-auto mt-4 max-w-4xl overflow-hidden rounded-[18px] border border-slate-500 bg-white shadow-card">
                <img
                  src={activeGalleryImage.src}
                  alt={activeGalleryImage.alt}
                  className="aspect-[16/9] w-full bg-white object-contain"
                  loading="lazy"
                />
              </figure>
            </div>
          ) : null}

          {selectedNotification.purchaseSteps?.length || selectedNotification.paymentImage ? (
            <div className="grid gap-4 lg:grid-cols-[1fr_400px]">
              {selectedNotification.purchaseSteps?.length ? (
                <div className="rounded-[18px] border border-slate-200 bg-white p-5">
                  <div className="flex items-center gap-2">
                    <CreditCard size={18} className="text-blue-600" />
                    <h3 className="text-sm font-black uppercase tracking-wider text-slate-900">Hướng dẫn mua</h3>
                  </div>
                  <ol className="mt-4 space-y-3">
                    {selectedNotification.purchaseSteps.map((step, index) => (
                      <li key={step} className="flex gap-3">
                        <span className="flex h-7 w-7 shrink-0 items-center justify-center rounded-lg bg-blue-600 text-xs font-black text-white">
                          {index + 1}
                        </span>
                        <p className="pt-1 text-sm font-semibold leading-6 text-slate-700">{step}</p>
                      </li>
                    ))}
                  </ol>
                </div>
              ) : null}

              {selectedNotification.paymentImage ? (
                <div className="rounded-[18px] border border-blue-200 bg-blue-50 p-3 shadow-pop">
                  <img
                    src={selectedNotification.paymentImage}
                    alt="QR chuyển khoản mua source SWT301"
                    className="w-full rounded-[14px] bg-white object-contain"
                    loading="lazy"
                  />
                </div>
              ) : null}
            </div>
          ) : null}

          {selectedNotification.contactLinks?.length ? (
            <div className="flex flex-wrap gap-3">
              {selectedNotification.contactLinks.map((contact) => (
                <a
                  key={contact.href}
                  href={contact.href}
                  target="_blank"
                  rel="noopener noreferrer"
                  className={`inline-flex h-11 items-center justify-center gap-2 rounded-lg border border-border px-4 text-xs font-black uppercase tracking-wider text-white shadow-pop transition-all hover:-translate-y-0.5 ${
                    contact.tone === 'slate' ? 'bg-slate-900' : 'bg-blue-600'
                  }`}
                >
                  {contact.tone === 'slate' ? <Link2 size={16} /> : <Phone size={16} />}
                  {contact.label}
                </a>
              ))}
            </div>
          ) : null}

          {selectedNotification.relatedLink ? (
            <div className="mt-auto flex justify-start">
              <Link
                to={selectedNotification.relatedLink}
                target={isExternalLink(selectedNotification.relatedLink) ? '_blank' : undefined}
                rel={isExternalLink(selectedNotification.relatedLink) ? 'noopener noreferrer' : undefined}
                className="inline-flex h-11 items-center justify-center gap-2 rounded-lg border border-border bg-blue-600 px-4 text-xs font-black uppercase tracking-wider text-white shadow-pop transition-all hover:-translate-y-0.5"
              >
                {selectedNotification.relatedLinkLabel || 'Mở liên kết'}
                <ExternalLink size={16} />
              </Link>
            </div>
          ) : null}
          </div>
        </article>
      </section>
    </div>
  );
};
