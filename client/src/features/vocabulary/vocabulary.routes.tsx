import { Route } from 'react-router-dom';
import { VocabularyDashboardPage } from './pages/VocabularyDashboardPage';
import { VocabularyCoursePage } from './pages/VocabularyCoursePage';
import { VocabularyLessonPage } from './pages/VocabularyLessonPage';

export const staticVocabularyRoutes = [
  <Route key="static-vocabulary-dashboard" path="vocabulary" element={<VocabularyDashboardPage />} />,
  <Route key="static-vocabulary-course" path="vocabulary/:courseCode" element={<VocabularyCoursePage />} />,
  <Route key="static-vocabulary-lesson" path="vocabulary/:courseCode/lessons/:lessonId" element={<VocabularyLessonPage />} />,
];
