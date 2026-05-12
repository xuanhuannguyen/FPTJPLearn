import { useNavigate } from 'react-router-dom';
import { CheckCircle } from 'lucide-react';

export function PaymentSuccessPage() {
  const navigate = useNavigate();

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
