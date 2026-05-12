import { Navigate, useParams } from 'react-router-dom';

export const ReviewPage = () => {
  const { listId } = useParams();

  if (!listId) {
    return <Navigate to="/active-vocabulary" replace />;
  }

  return <Navigate to={`/active-vocabulary/${listId}?study=1`} replace />;
};
