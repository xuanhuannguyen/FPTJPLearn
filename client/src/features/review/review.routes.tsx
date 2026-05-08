import { Route } from 'react-router-dom';
import { ReviewHomePage } from './pages/ReviewHomePage';
import { ReviewPage } from './pages/ReviewPage';

export const reviewRoutes = [
  <Route key="review" path="review" element={<ReviewHomePage />} />,
  <Route key="review-list" path="review/:listId" element={<ReviewPage />} />,
];
