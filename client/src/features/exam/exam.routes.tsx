import { Route } from 'react-router-dom';
import { ExamAttemptPage } from './pages/ExamAttemptPage';
import { ExamCoursePage } from './pages/ExamCoursePage';
import { ExamDashboardPage } from './pages/ExamDashboardPage';
import { ExamStudyPage } from './pages/ExamStudyPage';

export const examRoutes = [
  <Route key="exam-dashboard" path="exam" element={<ExamDashboardPage />} />,
  <Route key="exam-course" path="exam/:courseCode" element={<ExamCoursePage />} />,
  <Route key="exam-study" path="exam/study" element={<ExamStudyPage />} />,
  <Route key="exam-study-topic" path="exam/study/:topic" element={<ExamStudyPage />} />,
  <Route key="exam-test-attempt" path="exam/test/:attemptId" element={<ExamAttemptPage />} />,
];
