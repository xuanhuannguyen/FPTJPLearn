import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { apiClient } from '../../../shared/api/axios';
import { useAuthStore } from '../../../shared/stores/authStore';
import { refreshUserAccessCache, useUserAccess } from '../../../shared/hooks/useUserAccess';
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
  { code: 'jpd113', name: 'JPD113', price: 30000, duration: '6 tháng' },
  { code: 'jpd123', name: 'JPD123', price: 30000, duration: '6 tháng' },
  { code: 'combo', name: 'Combo', price: 50000, originalPrice: 60000, duration: '6 tháng', discount: 'Tiết kiệm 10,000đ' },
];

const features: Record<string, string[]> = {
  jpd113: ['Toàn bộ Kanji JPD113', 'Toàn bộ Từ vựng JPD113', 'Toàn bộ Ngữ pháp JPD113', 'Luyện thi JPD113', 'Luyện nói JPD113', 'Sử dụng 6 tháng'],
  jpd123: ['Toàn bộ Kanji JPD123', 'Toàn bộ Từ vựng JPD123', 'Toàn bộ Ngữ pháp JPD123', 'Luyện thi JPD123', 'Luyện nói JPD123', 'Sử dụng 6 tháng'],
  combo: ['Tất cả nội dung JPD113', 'Tất cả nội dung JPD123', 'Luyện thi cả 2 khóa', 'Luyện nói cả 2 khóa', 'Tiết kiệm 10,000đ', 'Sử dụng 6 tháng'],
};

const icons: Record<string, React.ReactNode> = {
  jpd113: <Zap size={28} />,
  jpd123: <Star size={28} />,
  combo: <Crown size={28} />,
};

export function PricingPage() {
  const [loading, setLoading] = useState<string | null>(null);
  const [paymentData, setPaymentData] = useState<{
    orderId: string;
    qrUrl: string;
    amount: number;
    description: string;
    provider: string;
  } | null>(null);
  const [timeLeft, setTimeLeft] = useState(300); // 5 minutes in seconds
  
  const { user } = useAuthStore();
  const { freeExperienceEnabled, isLoading: isAccessLoading, subscriptions } = useUserAccess();
  const navigate = useNavigate();

  const activeCourses = subscriptions
    .filter((subscription) => subscription.isActive)
    .map((subscription) => subscription.courseCode.toLowerCase());

  // Countdown timer for payment session
  useEffect(() => {
    let timer: ReturnType<typeof window.setInterval> | undefined;
    if (paymentData && timeLeft > 0) {
      timer = setInterval(() => {
        setTimeLeft(prev => prev - 1);
      }, 1000);
    }
    return () => clearInterval(timer);
  }, [paymentData, timeLeft]);

  useEffect(() => {
    if (!paymentData) return;
    if (timeLeft !== 0) return;

    const timer = window.setTimeout(() => {
      alert('Đã hết thời gian thanh toán. Vui lòng tạo đơn hàng mới.');
      setPaymentData(null);
    }, 0);

    return () => window.clearTimeout(timer);
  }, [paymentData, timeLeft]);

  // Polling for payment status
  useEffect(() => {
    let interval: ReturnType<typeof window.setInterval> | undefined;
    if (paymentData && paymentData.provider === 'SePay') {
      interval = setInterval(async () => {
        try {
          const res = await apiClient.get(`/orders/${paymentData.orderId}/status`);
          if (res.data.status === 'paid') {
            clearInterval(interval);
            await refreshUserAccessCache();
            setPaymentData(null);
            alert('Thanh toán thành công! Khóa học đã được mở.');
            navigate('/dashboard');
          }
        } catch (err) {
          console.error('Polling error:', err);
        }
      }, 10000); // Poll every 10s to reduce database wakeups.
    }
    return () => clearInterval(interval);
  }, [paymentData, navigate]);

  const isPackageLocked = (packageCode: string) => {
    const code = packageCode.toLowerCase();
    const has113 = activeCourses.includes('jpd113');
    const has123 = activeCourses.includes('jpd123');

    if (code === 'jpd113') return has113;
    if (code === 'jpd123') return has123;
    if (code === 'combo') return has113 || has123;
    
    return false;
  };

  const getButtonText = (pkg: Package) => {
    if (loading === pkg.code) return <><Loader2 size={16} className="spin" /> Đang xử lý...</>;
    
    const code = pkg.code.toLowerCase();
    if (activeCourses.includes(code)) return 'Đã sở hữu';
    
    if (isPackageLocked(pkg.code)) {
        if (code === 'combo' && (activeCourses.includes('jpd113') || activeCourses.includes('jpd123'))) {
            return 'Không khả dụng';
        }
        return 'Đã sở hữu';
    }

    return 'Mua ngay';
  };

  const handleBuy = async (packageCode: string) => {
    if (freeExperienceEnabled) return;

    if (!user) {
      navigate('/login');
      return;
    }

    setLoading(packageCode);
    try {
      const returnUrl = `${window.location.origin}/payment/success`;
      const cancelUrl = `${window.location.origin}/payment/cancel`;
      const res = await apiClient.post('/orders', { packageCode, returnUrl, cancelUrl });
      const { paymentUrl, provider, amount, orderId } = res.data;
      
      if (provider === 'SePay') {
        const shortId = orderId.split('-')[0].toUpperCase();
        setTimeLeft(300); // Reset countdown
        setPaymentData({
          orderId,
          qrUrl: paymentUrl,
          amount,
          description: `JP ${shortId}`,
          provider
        });
      } else {
        window.location.assign(paymentUrl);
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
        <h1 className="pricing-title">
          {freeExperienceEnabled ? 'Toàn bộ nội dung đang mở miễn phí' : 'Mở khóa toàn bộ nội dung'}
        </h1>
        <p className="pricing-subtitle">
          {freeExperienceEnabled
            ? 'Tính năng mua Premium đang tạm tắt trong thời gian trải nghiệm.'
            : 'Chọn gói phù hợp với bạn để truy cập đầy đủ bài học'}
        </p>
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
              className={`pricing-card-btn ${pkg.code === 'combo' ? 'pricing-card-btn--featured' : ''} ${isPackageLocked(pkg.code) ? 'pricing-card-btn--locked' : ''}`}
              onClick={() => handleBuy(pkg.code)}
              disabled={freeExperienceEnabled || loading !== null || isAccessLoading || isPackageLocked(pkg.code)}
            >
              {freeExperienceEnabled ? 'Đang mở miễn phí' : getButtonText(pkg)}
            </button>
          </div>
        ))}
      </div>

      <p className="pricing-note">
        {freeExperienceEnabled
          ? 'Bạn có thể học toàn bộ nội dung mà không cần mua gói Premium.'
          : 'Thanh toán qua chuyển khoản ngân hàng • Mở khóa tự động trong vài giây'}
      </p>

      {/* Payment Modal */}
      {paymentData && (
        <div className="payment-modal-overlay" onClick={() => setPaymentData(null)}>
          <div className="payment-modal" onClick={e => e.stopPropagation()}>
            <div className="payment-modal-header">
              <h3>Thanh toán qua mã QR</h3>
              <button className="payment-modal-close" onClick={() => setPaymentData(null)}>×</button>
            </div>
            
            <div className="payment-modal-content">
              <div className="qr-container">
                <img src={paymentData.qrUrl} alt="Payment QR" className="qr-image" />
                <div className="qr-scan-line"></div>
              </div>

              <div className="payment-instructions">
                <div className="instruction-item">
                  <span className="instruction-label">Số tiền:</span>
                  <span className="instruction-value highlight">{formatPrice(paymentData.amount)}</span>
                </div>
                <div className="instruction-item">
                  <span className="instruction-label">Nội dung chuyển khoản:</span>
                  <div className="copy-wrapper">
                    <span className="instruction-value font-mono">{paymentData.description}</span>
                    <button 
                      className="copy-btn"
                      onClick={() => {
                        navigator.clipboard.writeText(paymentData.description);
                        alert('Đã sao chép nội dung!');
                      }}
                    >
                      Sao chép
                    </button>
                  </div>
                </div>
              </div>

              <div className="payment-steps">
                <p>1. Mở ứng dụng Ngân hàng/Ví điện tử</p>
                <p>2. Quét mã QR trên hoặc chuyển khoản thủ công</p>
                <p>3. <strong>Bắt buộc</strong> nhập đúng nội dung chuyển khoản</p>
                <p>4. Hệ thống sẽ tự động kích hoạt sau khi nhận được tiền (30s - 2p)</p>
              </div>
            </div>

            <div className="payment-modal-footer">
              <div className="waiting-status">
                <Loader2 size={16} className="spin text-accent-primary" />
                <span>Đang chờ thanh toán... ({Math.floor(timeLeft / 60)}:{(timeLeft % 60).toString().padStart(2, '0')})</span>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
