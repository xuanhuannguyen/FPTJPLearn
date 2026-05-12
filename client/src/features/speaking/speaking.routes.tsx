import { Route } from 'react-router-dom';
import { SpeakingCoursePage } from './pages/SpeakingCoursePage';
import { SpeakingDashboardPage } from './pages/SpeakingDashboardPage';
import { SpeakingLessonPage } from './pages/SpeakingLessonPage';

export const speakingRoutes = [
  <Route key="speaking-dashboard" path="speaking" element={<SpeakingDashboardPage />} />,
  <Route key="speaking-course" path="speaking/:courseCode" element={<SpeakingCoursePage />} />,
  <Route key="speaking-lesson" path="speaking/:courseCode/lessons/:lessonId" element={<SpeakingLessonPage />} />,
];
