import { apiClient } from '../../../shared/api/axios';
import type { KanjiComponent, KanjiItem, KanjiLesson, KanjiLevel, KanjiLevelStats, KanjiVocabulary } from '../types/kanji.types';

interface KanjiLevelResponse {
  level: KanjiLevel;
  lessonCount: number;
  kanjiCount: number;
  vocabularyCount: number;
  learnedCount: number;
  practicedCount: number;
}

interface KanjiLessonDetailResponse {
  lesson: KanjiLessonResponse;
  kanjiItems: KanjiItemResponse[];
  vocabularyItems: KanjiVocabulary[];
}

interface KanjiLessonResponse extends Omit<KanjiLesson, 'orderIndex'> {
  orderIndex?: number;
  learnedCount?: number;
  practicedCount?: number;
}

interface KanjiItemResponse extends Omit<KanjiItem, 'kunReading' | 'onReading' | 'mnemonic'> {
  kunReading?: string | null;
  onReading?: string | null;
  mnemonic?: string | null;
  orderIndex?: number;
}

const lessonDetailCache = new Map<string, Promise<KanjiLessonDetailResponse>>();

const getLessonDetail = (lessonId: string) => {
  const cached = lessonDetailCache.get(lessonId);
  if (cached) {
    return cached;
  }

  const request = apiClient
    .get<KanjiLessonDetailResponse>(`/kanji/lessons/${lessonId}`)
    .then((response) => response.data);

  lessonDetailCache.set(lessonId, request);
  return request;
};

const mapLesson = (lesson: KanjiLessonResponse): KanjiLesson => ({
  ...lesson,
  orderIndex: lesson.orderIndex ?? lesson.lessonNumber,
});

const mapKanjiItem = (item: KanjiItemResponse): KanjiItem => ({
  ...item,
  kunReading: item.kunReading ?? '',
  onReading: item.onReading ?? '',
  mnemonic: item.mnemonic ?? '',
  components: parseComponents(item.componentMapJson),
});

const parseComponents = (componentMapJson?: string): KanjiComponent[] => {
  if (!componentMapJson) {
    return [];
  }

  try {
    const raw = JSON.parse(componentMapJson);
    if (!Array.isArray(raw)) {
      return [];
    }

    return raw
      .map((component): KanjiComponent | null => {
        const character = component.component ?? component.character;
        if (!character) {
          return null;
        }

        return {
          character,
          name: component.name ?? component.meaning ?? character,
          kanjiId: component.kanjiId,
          reading: component.reading,
        };
      })
      .filter((component): component is KanjiComponent => component !== null);
  } catch {
    return [];
  }
};

export const kanjiApi = {
  getLevelStats: async (): Promise<KanjiLevelStats[]> => {
    const response = await apiClient.get<KanjiLevelResponse[]>('/kanji/levels');
    return response.data.map((level) => ({
      level: level.level,
      totalLessons: level.lessonCount,
      totalKanji: level.kanjiCount,
      totalVocabulary: level.vocabularyCount,
      progressPercentage: level.kanjiCount === 0 ? 0 : Math.round((level.learnedCount / level.kanjiCount) * 100),
    }));
  },

  getLessonsByLevel: async (level: KanjiLevel): Promise<KanjiLesson[]> => {
    const response = await apiClient.get<{ lessons: KanjiLessonResponse[] }>(`/kanji/${level}/lessons`);
    return response.data.lessons.map(mapLesson);
  },

  getLessonById: async (lessonId: string): Promise<KanjiLesson> => {
    const detail = await getLessonDetail(lessonId);
    return mapLesson(detail.lesson);
  },

  getKanjiItemsByLesson: async (lessonId: string): Promise<KanjiItem[]> => {
    const detail = await getLessonDetail(lessonId);
    return detail.kanjiItems.map(mapKanjiItem);
  },

  getVocabularyByLesson: async (lessonId: string): Promise<KanjiVocabulary[]> => {
    const detail = await getLessonDetail(lessonId);
    return detail.vocabularyItems;
  },
};
