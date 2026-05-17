import { Route } from 'react-router-dom';
import { IntroDashboardPage } from './pages/IntroDashboardPage';
import { IntroModePlaceholderPage } from './pages/IntroModePlaceholderPage';
import { IntroScriptPage } from './pages/IntroScriptPage';

export const introRoutes = [
  <Route key="intro-dashboard" path="intro" element={<IntroDashboardPage />} />,
  <Route key="intro-script" path="intro/:script" element={<IntroScriptPage />} />,
  <Route key="intro-mode" path="intro/:script/:mode" element={<IntroModePlaceholderPage />} />,
];
