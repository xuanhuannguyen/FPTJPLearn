import { useNavigate } from 'react-router-dom';
import { ArrowLeft, Clock3 } from 'lucide-react';
import { getMemoryTypeConfig } from '../memory.config';
import type { MemoryItemType } from '../types/memory.types';

type MemoryPlaceholderReviewPageProps = {
  type: Extract<MemoryItemType, 'kanji' | 'vocabulary'>;
};

export const MemoryPlaceholderReviewPage = ({ type }: MemoryPlaceholderReviewPageProps) => {
  const navigate = useNavigate();
  const config = getMemoryTypeConfig(type);

  return (
    <div className="mx-auto max-w-3xl space-y-5 rounded-2xl bg-white p-8 text-center shadow-card">
      <Clock3 size={48} className="mx-auto text-blue-500" />
      <h1 className="text-3xl font-black text-text-primary">{config.label} trong Ghi nhớ</h1>
      <p className="font-bold text-text-secondary">{config.emptyText}</p>
      <button type="button" onClick={() => navigate('/memory')} className="btn-secondary">
        <ArrowLeft size={18} />
        Quay lại Ghi nhớ
      </button>
    </div>
  );
};
