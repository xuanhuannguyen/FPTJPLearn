import type { RouteObject } from 'react-router-dom';
import { KanjiDashboardPage } from './pages/KanjiDashboardPage';
import { KanjiLevelPage } from './pages/KanjiLevelPage';
import { KanjiLessonPage } from './pages/KanjiLessonPage';
import { KanjiStudyPage } from './pages/KanjiStudyPage';
import { KanjiFlashcardPage } from './pages/KanjiFlashcardPage';
import { KanjiVocabularyFlashcardPage } from './pages/KanjiVocabularyFlashcardPage';

export const kanjiRoutes: RouteObject[] = [
  {
    index: true,
    element: <KanjiDashboardPage />,
  },
  {
    path: ':level',
    element: <KanjiLevelPage />,
  },
  {
    path: ':level/lessons/:lessonId',
    element: <KanjiLessonPage />,
  },
  {
    path: ':level/lessons/:lessonId/study',
    element: <KanjiStudyPage />,
  },
  {
    path: ':level/lessons/:lessonId/flashcards',
    element: <KanjiFlashcardPage />,
  },
  {
    path: ':level/lessons/:lessonId/vocabulary-flashcards',
    element: <KanjiVocabularyFlashcardPage />,
  },
];
