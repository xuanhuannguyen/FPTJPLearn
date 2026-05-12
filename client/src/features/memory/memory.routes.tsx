import { Route } from 'react-router-dom';
import { MemoryDashboardPage } from './pages/MemoryDashboardPage';
import { MemoryGrammarReviewPage } from './pages/MemoryGrammarReviewPage';
import { MemoryKanjiReviewPage } from './pages/MemoryKanjiReviewPage';
import { MemoryListPage } from './pages/MemoryListPage';
import { MemoryVocabularyReviewPage } from './pages/MemoryVocabularyReviewPage';
import { getMemoryTypeConfig } from './memory.config';

export const memoryRoutes = [
  <Route key="memory" path="memory" element={<MemoryDashboardPage />} />,
  <Route key="memory-list" path="memory/:type/list" element={<MemoryListPage />} />,
  <Route key="memory-grammar-review" path={getMemoryTypeConfig('grammar').reviewPath.replace(/^\//, '')} element={<MemoryGrammarReviewPage />} />,
  <Route key="memory-kanji-review" path={getMemoryTypeConfig('kanji').reviewPath.replace(/^\//, '')} element={<MemoryKanjiReviewPage />} />,
  <Route key="memory-vocabulary-review" path={getMemoryTypeConfig('vocabulary').reviewPath.replace(/^\//, '')} element={<MemoryVocabularyReviewPage />} />,
];
