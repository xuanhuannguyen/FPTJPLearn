import { Lock, Crown } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import './PremiumLock.css';

interface PremiumLockProps {
  isLocked: boolean;
  packageCode?: string;
  children: React.ReactNode;
}

export function PremiumLock({ isLocked, packageCode, children }: PremiumLockProps) {
  const navigate = useNavigate();

  if (!isLocked) return <>{children}</>;

  return (
    <div className="premium-lock-container">
      <div className="premium-lock-blur">
        {children}
      </div>
      <div className="premium-lock-overlay">
        <div className="premium-lock-card">
          <div className="premium-lock-icon">
            <Lock size={32} />
          </div>
          <h3 className="premium-lock-title">Nội dung Premium</h3>
          <p className="premium-lock-text">
            Bài học này thuộc gói <strong>{packageCode?.toUpperCase() || 'Premium'}</strong>. 
            Vui lòng nâng cấp để tiếp tục học.
          </p>
          <button 
            className="premium-lock-btn"
            onClick={() => navigate('/pricing')}
          >
            <Crown size={18} />
            Nâng cấp ngay
          </button>
        </div>
      </div>
    </div>
  );
}
