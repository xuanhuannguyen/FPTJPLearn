import { Route } from 'react-router-dom';
import { DashboardPage } from './pages/DashboardPage';

export const dashboardRoutes = [
  <Route key="dashboard" path="dashboard" element={<DashboardPage />} />,
];
