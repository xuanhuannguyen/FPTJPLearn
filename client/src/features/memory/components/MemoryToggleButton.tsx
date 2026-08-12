import { useEffect, useState } from 'react';
import { Brain, Check, Loader2 } from 'lucide-react';
import { memoryApi } from '../api/memoryApi';
import type { MemoryItemType } from '../types/memory.types';

type Props = {
  type: MemoryItemType;
  itemId: string;
  initialInMemory?: boolean;
  className?: string;
};

export const MemoryToggleButton = ({ type, itemId, initialInMemory = false, className = '' }: Props) => {
  const [isInMemory, setIsInMemory] = useState(initialInMemory);
  const [memoryItemId, setMemoryItemId] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    let cancelled = false;
    const loadStatus = async () => {
      try {
        const status = await memoryApi.getItemStatus(type, itemId);
        if (!cancelled) {
          setIsInMemory(status.isInMemory && status.isActive);
          setMemoryItemId(status.memoryItemId ?? null);
        }
      } catch {
        if (!cancelled) setError('Không tải được trạng thái Memory.');
      }
    };
    void loadStatus();
    return () => { cancelled = true; };
  }, [itemId, type]);

  const toggle = async () => {
    try {
      setError('');
      setIsLoading(true);
      if (isInMemory && memoryItemId) {
        await memoryApi.removeItem(type, memoryItemId);
        setIsInMemory(false);
        setMemoryItemId(null);
      } else {
        const result = await memoryApi.addItem(type, itemId);
        setIsInMemory(true);
        setMemoryItemId(result.memoryItemId);
      }
    } catch {
      setError('Không thể cập nhật Memory. Nội dung bị khóa hoặc bạn chưa đăng nhập.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <span className="inline-flex flex-col items-end gap-1">
      <button
        type="button"
        onClick={(event) => {
          event.preventDefault();
          event.stopPropagation();
          void toggle();
        }}
        disabled={isLoading}
        aria-pressed={isInMemory}
        className={`inline-flex h-9 items-center gap-1.5 rounded-lg border-2 px-2.5 text-xs font-black transition-all disabled:cursor-wait disabled:opacity-70 ${
          isInMemory ? 'border-emerald-300 bg-emerald-50 text-emerald-700' : 'border-slate-600 bg-accent-cta text-white shadow-pop hover:-translate-y-0.5'
        } ${className}`}
      >
        {isLoading ? <Loader2 size={14} className="animate-spin" /> : isInMemory ? <Check size={14} /> : <Brain size={14} />}
        {isInMemory ? 'Đã lưu' : 'Ghi nhớ'}
      </button>
      {error ? <span className="max-w-40 text-right text-[10px] font-bold text-rose-600">{error}</span> : null}
    </span>
  );
};
