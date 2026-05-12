import { AppRouter } from './Router';
import { initAuthListener } from './shared/stores/authStore';

initAuthListener();

function App() {
  return <AppRouter />;
}

export default App;
