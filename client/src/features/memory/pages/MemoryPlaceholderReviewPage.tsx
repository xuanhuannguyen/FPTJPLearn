import { useNavigate } from 'react-router-dom';
import { ArrowLeft, Clock3 } from 'lucide-react';
import type { MemoryItemType } from '../types/memory.types';

type MemoryPlaceholderReviewPageProps = {
  type: Extract<MemoryItemType, 'kanji' | 'vocabulary'>;
};

const copy = {
  kanji: {
    title: 'Kanji trong Ghi nhớ',
    body: 'Kanji trong Ghi nhớ sẽ được thêm sau khi Kanji module sẵn sàng.',
  },
  vocabulary: {
    title: 'Từ vựng trong Ghi nhớ',
    body: 'Từ vựng trong Ghi nhớ sẽ dùng source riêng, không dùng Vocabulary hiện tại.',
  },
};

export const MemoryPlaceholderReviewPage = ({ type }: MemoryPlaceholderReviewPageProps) => {
  const navigate = useNavigate();
  const content = copy[type];

  return (
    <div className="mx-auto max-w-3xl space-y-5 rounded-2xl bg-white p-8 text-center shadow-card">
      <Clock3 size={48} className="mx-auto text-blue-500" />
      <h1 className="text-3xl font-black text-text-primary">{content.title}</h1>
      <p className="font-bold text-text-secondary">{content.body}</p>
      <button type="button" onClick={() => navigate('/memory')} className="btn-secondary">
        <ArrowLeft size={18} />
        Quay lại Ghi nhớ
      </button>
    </div>
  );
};
