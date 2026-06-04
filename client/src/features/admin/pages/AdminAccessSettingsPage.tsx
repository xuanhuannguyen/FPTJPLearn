import { useEffect, useState } from 'react';
import { AlertTriangle, Loader2, LockKeyhole, Power, ShieldCheck, Unlock } from 'lucide-react';
import { apiClient } from '../../../shared/api/axios';
import { clearUserAccessCache, setUserAccessCache } from '../../../shared/hooks/useUserAccess';

type AccessSettings = {
  licensingEnabled: boolean;
  freeExperienceEnabled: boolean;
};

export const AdminAccessSettingsPage = () => {
  const [settings, setSettings] = useState<AccessSettings | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [error, setError] = useState('');

  const loadSettings = async () => {
    try {
      setIsLoading(true);
      setError('');
      const response = await apiClient.get<AccessSettings>('/admin/access-settings');
      setSettings(response.data);
    } catch (err) {
      console.error('Load access settings error:', err);
      setError('Không tải được cấu hình bản quyền.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    loadSettings();
  }, []);

  const updateLicensing = async (licensingEnabled: boolean) => {
    if (!settings || settings.licensingEnabled === licensingEnabled) return;

    const confirmed = window.confirm(
      licensingEnabled
        ? 'Bật bản quyền sẽ khóa nội dung Premium với người dùng chưa mua gói. Tiếp tục?'
        : 'Tắt bản quyền sẽ mở miễn phí toàn bộ nội dung và tạm tắt mua Premium. Tiếp tục?'
    );
    if (!confirmed) return;

    try {
      setIsSaving(true);
      setError('');
      const response = await apiClient.put<AccessSettings>('/admin/access-settings', { licensingEnabled });
      setSettings(response.data);
      if (response.data.freeExperienceEnabled) {
        setUserAccessCache({
          licensingEnabled: response.data.licensingEnabled,
          freeExperienceEnabled: response.data.freeExperienceEnabled,
          activeCourseCodes: [],
          subscriptions: [],
        });
      } else {
        clearUserAccessCache();
      }
    } catch (err) {
      console.error('Update access settings error:', err);
      setError('Cập nhật cấu hình bản quyền thất bại.');
    } finally {
      setIsSaving(false);
    }
  };

  const isLicensingEnabled = settings?.licensingEnabled ?? false;

  return (
    <div className="mx-auto max-w-6xl space-y-6 px-4 py-8 lg:px-8">
      <section className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm">
        <div className="flex flex-col gap-5 lg:flex-row lg:items-end lg:justify-between">
          <div>
            <p className="text-xs font-black uppercase tracking-[0.18em] text-blue-600">Access control</p>
            <h1 className="mt-2 font-heading text-4xl font-black text-slate-950">Bản quyền & khóa nội dung</h1>
            <p className="mt-3 max-w-3xl text-sm font-bold leading-6 text-slate-600">
              Điều khiển trạng thái bán thật của JPLearn. Khi bật bản quyền, người dùng free chỉ học bài miễn phí và cần mua JPD113/JPD123 để mở nội dung premium.
            </p>
          </div>

          <div className={`inline-flex items-center gap-2 rounded-full px-4 py-2 text-sm font-black ${
            isLicensingEnabled ? 'bg-emerald-100 text-emerald-700' : 'bg-amber-100 text-amber-700'
          }`}>
            {isLicensingEnabled ? <ShieldCheck size={18} /> : <Unlock size={18} />}
            {isLicensingEnabled ? 'Đang bật bản quyền' : 'Đang mở miễn phí'}
          </div>
        </div>
      </section>

      {error ? (
        <div className="flex items-center gap-3 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm font-bold text-red-700">
          <AlertTriangle size={18} />
          {error}
        </div>
      ) : null}

      <section className="grid gap-4 lg:grid-cols-2">
        <button
          type="button"
          disabled={isLoading || isSaving}
          onClick={() => updateLicensing(false)}
          className={`rounded-2xl border p-6 text-left shadow-sm transition ${
            !isLicensingEnabled
              ? 'border-amber-300 bg-amber-50 ring-2 ring-amber-200'
              : 'border-slate-200 bg-white hover:-translate-y-0.5 hover:shadow-md'
          } disabled:cursor-wait disabled:opacity-70`}
        >
          <div className="flex items-center justify-between">
            <span className="grid h-12 w-12 place-items-center rounded-xl bg-amber-100 text-amber-700">
              <Unlock size={24} />
            </span>
            {!isLicensingEnabled ? <span className="rounded-full bg-amber-200 px-3 py-1 text-xs font-black text-amber-800">Đang chọn</span> : null}
          </div>
          <h2 className="mt-5 font-heading text-2xl font-black text-slate-950">Mở miễn phí toàn bộ</h2>
          <p className="mt-3 text-sm font-bold leading-6 text-slate-600">
            Tất cả bài học được mở. Người dùng có quota premium cho từ vựng chủ động. Chức năng mua Premium tạm tắt.
          </p>
        </button>

        <button
          type="button"
          disabled={isLoading || isSaving}
          onClick={() => updateLicensing(true)}
          className={`rounded-2xl border p-6 text-left shadow-sm transition ${
            isLicensingEnabled
              ? 'border-emerald-300 bg-emerald-50 ring-2 ring-emerald-200'
              : 'border-slate-200 bg-white hover:-translate-y-0.5 hover:shadow-md'
          } disabled:cursor-wait disabled:opacity-70`}
        >
          <div className="flex items-center justify-between">
            <span className="grid h-12 w-12 place-items-center rounded-xl bg-emerald-100 text-emerald-700">
              <LockKeyhole size={24} />
            </span>
            {isLicensingEnabled ? <span className="rounded-full bg-emerald-200 px-3 py-1 text-xs font-black text-emerald-800">Đang chọn</span> : null}
          </div>
          <h2 className="mt-5 font-heading text-2xl font-black text-slate-950">Bật bản quyền bán thật</h2>
          <p className="mt-3 text-sm font-bold leading-6 text-slate-600">
            Khóa nội dung premium với user free, mở nút mua gói, áp quota free 2 list và premium 10 lượt/ngày.
          </p>
        </button>
      </section>

      <section className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
        <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
          <div>
            <h2 className="font-heading text-xl font-black text-slate-950">Trạng thái hệ thống</h2>
            <p className="mt-1 text-sm font-bold text-slate-600">
              Thay đổi có hiệu lực ngay với API khóa bài, pricing và quota từ vựng chủ động.
            </p>
          </div>
          <button
            type="button"
            onClick={loadSettings}
            disabled={isLoading || isSaving}
            className="inline-flex h-11 items-center justify-center gap-2 rounded-xl bg-slate-950 px-4 text-sm font-black text-white hover:bg-slate-800 disabled:cursor-wait disabled:opacity-70"
          >
            {isLoading || isSaving ? <Loader2 size={16} className="animate-spin" /> : <Power size={16} />}
            {isSaving ? 'Đang lưu...' : 'Tải lại'}
          </button>
        </div>
      </section>
    </div>
  );
};
