import type { RouteObject } from 'react-router-dom';
import { GrammarHomePage } from './pages/GrammarHomePage';
import { GrammarLessonPage } from './pages/GrammarLessonPage';
import { GrammarLevelPage } from './pages/GrammarLevelPage';
import { GrammarPatternDetailPage } from './pages/GrammarPatternDetailPage';
import { GrammarPatternPracticePage } from './pages/GrammarPatternPracticePage';
import { GrammarReviewPage } from './pages/GrammarReviewPage';

export const grammarRoutes: RouteObject[] = [
  {
    path: '',
    element: <GrammarHomePage />,
  },
  {
    path: 'review',
    element: <GrammarReviewPage />,
  },
  {
    path: 'patterns/:patternId/practice',
    element: <GrammarPatternPracticePage />,
  },
  {
    path: 'patterns/:patternId',
    element: <GrammarPatternDetailPage />,
  },
  {
    path: ':level',
    element: <GrammarLevelPage />,
  },
  {
    path: ':level/lessons/:lessonId',
    element: <GrammarLessonPage />,
  }
];
