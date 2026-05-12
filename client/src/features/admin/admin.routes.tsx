import { Route } from 'react-router-dom';
import { AdminLayout } from './components/AdminLayout';
import { AdminDashboardPage } from './pages/AdminDashboardPage';
import { AdminExamQuestionsPage } from './pages/AdminExamQuestionsPage';

export const adminRoutes = [
  <Route key="admin" path="/admin" element={<AdminLayout />}>
    <Route index element={<AdminDashboardPage />} />
    <Route path="exam-questions" element={<AdminExamQuestionsPage />} />
  </Route>,
];
