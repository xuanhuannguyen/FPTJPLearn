import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { apiClient } from '../../../shared/api/axios';
import { useAuthStore } from '../../../shared/stores/authStore';
import { Crown, Zap, Star, Check, Loader2 } from 'lucide-react';
import './PricingPage.css';

interface Package {
  code: string;
  name: string;
  price: number;
  originalPrice?: number;
  duration: string;
  discount?: string;
}

const packages: Package[] = [
  { code: 'jpd113', name: 'JPD113', price: 80000, duration: '6 tháng' },
  { code: 'jpd123', name: 'JPD123', price: 100000, duration: '6 tháng' },
  { code: 'combo', name: 'Combo', price: 150000, originalPrice: 180000, duration: '6 tháng', discount: 'Tiết kiệm 17%' },
];

const features: Record<string, string[]> = {
  jpd113: ['Toàn bộ Kanji JPD113', 'Toàn bộ Từ vựng JPD113', 'Toàn bộ Ngữ pháp JPD113', 'Luyện thi JPD113', 'Luyện nói JPD113', 'Sử dụng 6 tháng'],
  jpd123: ['Toàn bộ Kanji JPD123', 'Toàn bộ Từ vựng JPD123', 'Toàn bộ Ngữ pháp JPD123', 'Luyện thi JPD123', 'Luyện nói JPD123', 'Sử dụng 6 tháng'],
  combo: ['Tất cả nội dung JPD113', 'Tất cả nội dung JPD123', 'Luyện thi cả 2 khóa', 'Luyện nói cả 2 khóa', 'Tiết kiệm 30,000đ', 'Sử dụng 6 tháng'],
};

const icons: Record<string, React.ReactNode> = {
  jpd113: <Zap size={28} />,
  jpd123: <Star size={28} />,
  combo: <Crown size={28} />,
};

export function PricingPage() {
  const [loading, setLoading] = useState<string | null>(null);
  const { user } = useAuthStore();
  const navigate = useNavigate();

  const handleBuy = async (packageCode: string) => {
    if (!user) {
      navigate('/login');
      return;
    }

    setLoading(packageCode);
    try {
      const res = await apiClient.post('/orders', { packageCode });
      const { paymentUrl } = res.data;
      if (paymentUrl) {
        window.location.href = paymentUrl;
      }
    } catch (err) {
      console.error('Payment error:', err);
      alert('Có lỗi xảy ra khi tạo đơn hàng. Vui lòng thử lại.');
    } finally {
      setLoading(null);
    }
  };

  const formatPrice = (price: number) =>
    new Intl.NumberFormat('vi-VN').format(price) + 'đ';

  return (
    <div className="pricing-page">
      <div className="pricing-header">
        <h1 className="pricing-title">Mở khóa toàn bộ nội dung</h1>
        <p className="pricing-subtitle">Chọn gói phù hợp với bạn để truy cập đầy đủ bài học</p>
      </div>

      <div className="pricing-grid">
        {packages.map((pkg) => (
          <div
            key={pkg.code}
            className={`pricing-card ${pkg.code === 'combo' ? 'pricing-card--featured' : ''}`}
          >
            {pkg.discount && (
              <div className="pricing-badge">{pkg.discount}</div>
            )}

            <div className="pricing-card-icon">{icons[pkg.code]}</div>
            <h2 className="pricing-card-name">{pkg.name}</h2>

            <div className="pricing-card-price">
              {pkg.originalPrice && (
                <span className="pricing-card-original">{formatPrice(pkg.originalPrice)}</span>
              )}
              <span className="pricing-card-amount">{formatPrice(pkg.price)}</span>
              <span className="pricing-card-period">/ {pkg.duration}</span>
            </div>

            <ul className="pricing-card-features">
              {features[pkg.code]?.map((f, i) => (
                <li key={i}>
                  <Check size={16} className="pricing-check" />
                  {f}
                </li>
              ))}
            </ul>

            <button
              className={`pricing-card-btn ${pkg.code === 'combo' ? 'pricing-card-btn--featured' : ''}`}
              onClick={() => handleBuy(pkg.code)}
              disabled={loading !== null}
            >
              {loading === pkg.code ? (
                <><Loader2 size={16} className="spin" /> Đang xử lý...</>
              ) : (
                'Mua ngay'
              )}
            </button>
          </div>
        ))}
      </div>

      <p className="pricing-note">
        Thanh toán qua chuyển khoản ngân hàng • Mở khóa tự động trong vài giây
      </p>
    </div>
  );
}
