import { Navigate, useParams } from 'react-router-dom';

export const ReviewPage = () => {
  const { listId } = useParams();

  if (!listId) {
    return <Navigate to="/vocabulary" replace />;
  }

  return <Navigate to={`/vocabulary/${listId}?study=1`} replace />;
};
