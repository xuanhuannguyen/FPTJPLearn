import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import { dashboardRoutes } from './features/dashboard/dashboard.routes';
import { grammarRoutes } from './features/grammar/grammar.routes';
import { memoryRoutes } from './features/memory/memory.routes';
import { reviewRoutes } from './features/review/review.routes';
import { vocabularyRoutes } from './features/vocabulary/vocabulary.routes';
import { kanjiRoutes } from './features/kanji/kanji.routes';
import { Layout } from './shared/components/Layout';

export const AppRouter = () => (
  <BrowserRouter>
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<Navigate to="/dashboard" replace />} />
        {dashboardRoutes}
        {vocabularyRoutes}
        {memoryRoutes}
        <Route path="grammar">
          {grammarRoutes.map((route) => (
            <Route key={route.path} path={route.path} element={route.element} />
          ))}
        </Route>
        <Route path="kanji">
          {kanjiRoutes.map((route, i) => (
            route.index 
              ? <Route key="index" index element={route.element} />
              : <Route key={route.path || i} path={route.path} element={route.element} />
          ))}
        </Route>
        {reviewRoutes}
      </Route>
    </Routes>
  </BrowserRouter>
);
