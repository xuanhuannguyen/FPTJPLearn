import { useNavigate } from 'react-router-dom';
import { XCircle } from 'lucide-react';

export function PaymentCancelPage() {
  const navigate = useNavigate();

  return (
    <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', minHeight: '60vh', gap: '1rem', textAlign: 'center', padding: '2rem' }}>
      <XCircle size={64} color="#ef4444" />
      <h1 style={{ fontSize: '1.5rem', fontWeight: 700, color: '#1e293b', margin: 0 }}>Đã hủy thanh toán</h1>
      <p style={{ color: '#64748b', maxWidth: '400px' }}>Đơn hàng của bạn chưa được thanh toán. Bạn có thể thử lại bất cứ lúc nào.</p>
      <button
        onClick={() => navigate('/pricing')}
        style={{ padding: '0.75rem 2rem', background: '#2563eb', color: 'white', border: 'none', borderRadius: '0.75rem', fontSize: '1rem', fontWeight: 600, cursor: 'pointer' }}
      >
        Quay lại bảng giá
      </button>
    </div>
  );
}
