import { BrainCircuit, Castle, Crown, type LucideIcon } from 'lucide-react';
import type { MemoryItemType } from './types/memory.types';

export type MemoryTypeConfig = {
  type: MemoryItemType;
  label: string;
  summaryLabel: string;
  emptyText: string;
  reviewPath: string;
  listPath: string;
  icon: LucideIcon;
  canResetProgress: boolean;
  accentClassName: string;
  tabActiveClassName: string;
  btnTextClassName: string;
};

export const memoryTypeConfigs: Record<MemoryItemType, MemoryTypeConfig> = {
  kanji: {
    type: 'kanji',
    label: 'Kanji',
    summaryLabel: 'Kanji',
    emptyText: 'Kanji trong Ghi nhớ sẽ được thêm sau khi bạn bấm "Add to memory" ở màn hình học Kanji.',
    reviewPath: '/memory/kanji/review',
    listPath: '/memory/kanji/list',
    icon: Castle,
    canResetProgress: true,
    accentClassName: 'bg-gradient-to-br from-emerald-600 via-teal-600 to-emerald-700',
    tabActiveClassName: 'bg-gradient-to-r from-emerald-600 to-teal-600 text-white shadow-md shadow-emerald-600/25',
    btnTextClassName: 'text-emerald-800 hover:text-emerald-900',
  },
  vocabulary: {
    type: 'vocabulary',
    label: 'Từ vựng',
    summaryLabel: 'Từ vựng',
    emptyText: 'Hãy mở một bài từ vựng và bấm "Ghi nhớ" ở từ bạn muốn học.',
    reviewPath: '/memory/vocabulary/review',
    listPath: '/memory/vocabulary/list',
    icon: Crown,
    canResetProgress: true,
    accentClassName: 'bg-gradient-to-br from-amber-500 via-orange-500 to-amber-600',
    tabActiveClassName: 'bg-gradient-to-r from-amber-500 to-orange-500 text-white shadow-md shadow-amber-500/25',
    btnTextClassName: 'text-amber-800 hover:text-amber-900',
  },
  grammar: {
    type: 'grammar',
    label: 'Ngữ pháp',
    summaryLabel: 'Thẻ ngữ pháp',
    emptyText: 'Hãy mở một mẫu ngữ pháp và bấm "Thêm vào ghi nhớ".',
    reviewPath: '/memory/grammar/review',
    listPath: '/memory/grammar/list',
    icon: BrainCircuit,
    canResetProgress: true,
    accentClassName: 'bg-gradient-to-br from-teal-700 via-cyan-800 to-slate-800',
    tabActiveClassName: 'bg-gradient-to-r from-teal-700 to-cyan-700 text-white shadow-md shadow-teal-700/25',
    btnTextClassName: 'text-teal-900 hover:text-teal-950',
  },
};

export const memoryTypeOrder: MemoryItemType[] = ['kanji', 'vocabulary', 'grammar'];

export const getMemoryTypeConfig = (type: MemoryItemType) => memoryTypeConfigs[type];
