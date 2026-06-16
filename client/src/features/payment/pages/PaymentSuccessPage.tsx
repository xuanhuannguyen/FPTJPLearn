import { useEffect, useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { CheckCircle, Loader2, XCircle } from 'lucide-react';
import { apiClient } from '../../../shared/api/axios';
import { refreshUserAccessCache } from '../../../shared/hooks/useUserAccess';

export function PaymentSuccessPage() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const orderId = searchParams.get('orderId');
  const [status, setStatus] = useState<'checking' | 'paid' | 'pending' | 'error'>(
    orderId ? 'checking' : 'paid'
  );

  useEffect(() => {
    if (!orderId) return;

    let cancelled = false;
    let attempts = 0;

    const checkStatus = async () => {
      try {
        attempts += 1;
        const response = await apiClient.get(`/orders/${orderId}/status`);
        if (cancelled) return;

        if (response.data.status === 'paid') {
          await refreshUserAccessCache();
          setStatus('paid');
          return;
        }

        if (attempts >= 12) {
          setStatus('pending');
          return;
        }

        window.setTimeout(checkStatus, 10000);
      } catch (error) {
        console.error('Payment status check failed:', error);
        if (!cancelled) setStatus('error');
      }
    };

    void checkStatus();
    return () => {
      cancelled = true;
    };
  }, [orderId]);

  if (status === 'checking') {
    return (
      <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', minHeight: '60vh', gap: '1rem', textAlign: 'center', padding: '2rem' }}>
        <Loader2 size={64} color="#2563eb" className="spin" />
        <h1 style={{ fontSize: '1.5rem', fontWeight: 700, color: '#1e293b', margin: 0 }}>Đang xác nhận thanh toán...</h1>
        <p style={{ color: '#64748b', maxWidth: '420px' }}>Vui lòng chờ webhook từ cổng thanh toán. Hệ thống sẽ tự kiểm tra trong khoảng 2 phút.</p>
      </div>
    );
  }

  if (status === 'pending') {
    return (
      <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', minHeight: '60vh', gap: '1rem', textAlign: 'center', padding: '2rem' }}>
        <Loader2 size={64} color="#f59e0b" />
        <h1 style={{ fontSize: '1.5rem', fontWeight: 700, color: '#1e293b', margin: 0 }}>Thanh toán đang chờ xác nhận</h1>
        <p style={{ color: '#64748b', maxWidth: '420px' }}>Nếu bạn đã thanh toán, vui lòng chờ thêm hoặc kiểm tra lại sau. Không tạo đơn mới ngay để tránh trùng thanh toán.</p>
        <button onClick={() => navigate('/pricing')} style={{ padding: '0.75rem 2rem', background: '#2563eb', color: 'white', border: 'none', borderRadius: '0.75rem', fontSize: '1rem', fontWeight: 600, cursor: 'pointer' }}>
          Về bảng giá
        </button>
      </div>
    );
  }

  if (status === 'error') {
    return (
      <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', minHeight: '60vh', gap: '1rem', textAlign: 'center', padding: '2rem' }}>
        <XCircle size={64} color="#ef4444" />
        <h1 style={{ fontSize: '1.5rem', fontWeight: 700, color: '#1e293b', margin: 0 }}>Không kiểm tra được thanh toán</h1>
        <p style={{ color: '#64748b', maxWidth: '420px' }}>Vui lòng đăng nhập đúng tài khoản đã mua và thử tải lại trang.</p>
        <button onClick={() => window.location.reload()} style={{ padding: '0.75rem 2rem', background: '#2563eb', color: 'white', border: 'none', borderRadius: '0.75rem', fontSize: '1rem', fontWeight: 600, cursor: 'pointer' }}>
          Tải lại
        </button>
      </div>
    );
  }

  return (
    <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', minHeight: '60vh', gap: '1rem', textAlign: 'center', padding: '2rem' }}>
      <CheckCircle size={64} color="#22c55e" />
      <h1 style={{ fontSize: '1.5rem', fontWeight: 700, color: '#1e293b', margin: 0 }}>Thanh toán thành công!</h1>
      <p style={{ color: '#64748b', maxWidth: '400px' }}>Khóa học đã được mở khóa. Bạn có thể bắt đầu học ngay bây giờ.</p>
      <button
        onClick={() => navigate('/dashboard')}
        style={{ padding: '0.75rem 2rem', background: '#2563eb', color: 'white', border: 'none', borderRadius: '0.75rem', fontSize: '1rem', fontWeight: 600, cursor: 'pointer' }}
      >
        Về trang chủ
      </button>
    </div>
  );
}
