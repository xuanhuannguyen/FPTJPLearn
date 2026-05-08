import { Route } from 'react-router-dom';
import { VocabularyDetailPage } from './pages/VocabularyDetailPage';
import { VocabularyPage } from './pages/VocabularyPage';

export const vocabularyRoutes = [
  <Route key="vocabulary" path="vocabulary" element={<VocabularyPage />} />,
  <Route key="vocabulary-detail" path="vocabulary/:id" element={<VocabularyDetailPage />} />,
];
