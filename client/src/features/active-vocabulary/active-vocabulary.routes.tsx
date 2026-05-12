import { Route } from 'react-router-dom';
import { VocabularyDetailPage } from './pages/VocabularyDetailPage';
import { VocabularyPage } from './pages/VocabularyPage';

export const activeVocabularyRoutes = [
  <Route key="active-vocabulary" path="active-vocabulary" element={<VocabularyPage />} />,
  <Route key="active-vocabulary-detail" path="active-vocabulary/:id" element={<VocabularyDetailPage />} />,
];
