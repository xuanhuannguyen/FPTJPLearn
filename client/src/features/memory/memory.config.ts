import { BrainCircuit, Castle, Crown, type LucideIcon } from 'lucide-react';
import type { MemoryItemType } from './types/memory.types';

export type MemoryTypeConfig = {
  type: MemoryItemType;
  label: string;
  summaryLabel: string;
  emptyText: string;
  reviewPath: string;
<<<<<<< HEAD
  listPath: string;
=======
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
  icon: LucideIcon;
  canResetProgress: boolean;
  accentClassName: string;
};

export const memoryTypeConfigs: Record<MemoryItemType, MemoryTypeConfig> = {
  kanji: {
    type: 'kanji',
    label: 'Kanji',
    summaryLabel: 'Kanji',
    emptyText: 'Kanji trong Ghi nhớ sẽ được thêm sau khi bạn bấm "Add to memory" ở màn hình học Kanji.',
    reviewPath: '/memory/kanji/review',
<<<<<<< HEAD
    listPath: '/memory/kanji/list',
=======
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
    icon: Castle,
    canResetProgress: true,
    accentClassName: 'bg-blue-500',
  },
  vocabulary: {
    type: 'vocabulary',
    label: 'Từ vựng',
    summaryLabel: 'Từ vựng',
<<<<<<< HEAD
    emptyText: 'Hãy mở một bài từ vựng và bấm "Ghi nhớ" ở từ bạn muốn học.',
    reviewPath: '/memory/vocabulary/review',
    listPath: '/memory/vocabulary/list',
    icon: Crown,
    canResetProgress: true,
=======
    emptyText: 'Từ vựng trong Ghi nhớ sẽ dùng source riêng, không dùng Vocabulary hiện tại.',
    reviewPath: '/memory/vocabulary/review',
    icon: Crown,
    canResetProgress: false,
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
    accentClassName: 'bg-emerald-500',
  },
  grammar: {
    type: 'grammar',
    label: 'Ngữ pháp',
    summaryLabel: 'Thẻ ngữ pháp',
    emptyText: 'Hãy mở một mẫu ngữ pháp và bấm "Thêm vào ghi nhớ".',
    reviewPath: '/memory/grammar/review',
<<<<<<< HEAD
    listPath: '/memory/grammar/list',
=======
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
    icon: BrainCircuit,
    canResetProgress: true,
    accentClassName: 'bg-violet-500',
  },
};

export const memoryTypeOrder: MemoryItemType[] = ['kanji', 'vocabulary', 'grammar'];

export const getMemoryTypeConfig = (type: MemoryItemType) => memoryTypeConfigs[type];
