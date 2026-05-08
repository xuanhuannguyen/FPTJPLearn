import { Route } from 'react-router-dom';
import { MemoryDashboardPage } from './pages/MemoryDashboardPage';
import { MemoryGrammarReviewPage } from './pages/MemoryGrammarReviewPage';
import { MemoryPlaceholderReviewPage } from './pages/MemoryPlaceholderReviewPage';

export const memoryRoutes = [
  <Route key="memory" path="memory" element={<MemoryDashboardPage />} />,
  <Route key="memory-grammar-review" path="memory/grammar/review" element={<MemoryGrammarReviewPage />} />,
  <Route key="memory-kanji-review" path="memory/kanji/review" element={<MemoryPlaceholderReviewPage type="kanji" />} />,
  <Route key="memory-vocabulary-review" path="memory/vocabulary/review" element={<MemoryPlaceholderReviewPage type="vocabulary" />} />,
];
