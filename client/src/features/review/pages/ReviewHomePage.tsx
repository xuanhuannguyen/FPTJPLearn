import { ArrowRight, BookOpen } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

export const ReviewHomePage = () => {
  const navigate = useNavigate();

  return (
    <div className="glass-card min-h-[60vh] px-8 py-12 flex flex-col items-center justify-center text-center">
      <div className="p-4 rounded-full bg-accent-primary/10 text-accent-primary mb-5">
        <BookOpen size={34} />
      </div>
      <h1 className="text-3xl font-bold text-text-primary mb-3">Ôn tập Từ vựng chủ động</h1>
      <p className="text-text-secondary max-w-xl mb-8">
        Chọn một bộ từ vựng chủ động trước. Review session sẽ ở trong bộ hiện tại và cập nhật level riêng cho từng từ.
      </p>
      <button
        onClick={() => navigate('/active-vocabulary')}
        className="inline-flex items-center gap-2 rounded-xl bg-accent-primary px-5 py-3 font-semibold text-white shadow-glow transition-all hover:-translate-y-0.5 hover:bg-accent-hover"
      >
        <span>Mở Từ vựng chủ động</span>
        <ArrowRight size={18} />
      </button>
    </div>
  );
};
