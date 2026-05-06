import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { Layout } from './shared/components/Layout';
import { VocabularyPage } from './features/vocabulary/VocabularyPage';
import { VocabularyDetailPage } from './features/vocabulary/VocabularyDetailPage';
import { ReviewPage } from './features/review/ReviewPage';

const PlaceholderPage = ({ title }: { title: string }) => (
  <div className="glass-card p-8 min-h-[60vh] flex flex-col items-center justify-center text-center">
    <h2 className="text-3xl font-bold mb-4">{title}</h2>
    <p className="text-text-secondary">This module is under development.</p>
  </div>
);

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<Navigate to="/dashboard" replace />} />
          <Route path="dashboard" element={<PlaceholderPage title="Dashboard" />} />
          <Route path="vocabulary" element={<VocabularyPage />} />
          <Route path="vocabulary/:id" element={<VocabularyDetailPage />} />
          <Route path="review" element={<Navigate to="/vocabulary" replace />} />
          <Route path="review/:listId" element={<ReviewPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
