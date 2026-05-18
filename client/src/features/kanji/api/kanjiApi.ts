import { fetchStatic } from '../../../shared/services/staticDataService';
import type { KanjiComponent, KanjiItem, KanjiLesson, KanjiLevel, KanjiLevelStats, KanjiVocabulary } from '../types/kanji.types';

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

const getLessonDetail = (lessonId: string) => {
  const courseCode = lessonId.includes('jpd123') ? 'jpd123' : 'jpd113';
  return fetchStatic<KanjiLessonDetailResponse>(`kanji/${courseCode}/lessons/${lessonId}.json`);
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
    return fetchStatic<KanjiLevelStats[]>('kanji/levels.json');
  },

  getLessonsByLevel: async (level: KanjiLevel | string): Promise<KanjiLesson[]> => {
    const key = level.toLowerCase().startsWith('jpd') ? level.toLowerCase() : level;
    const lessons = await fetchStatic<KanjiLessonResponse[]>(`kanji/${key}/lessons.json`);
    return lessons.map(mapLesson);
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
