import { create } from 'zustand';
import { signInWithPopup, signOut, onAuthStateChanged, type User } from 'firebase/auth';
import { auth, googleProvider } from '../config/firebase';
import { apiClient } from '../api/axios';
import { clearUserAccessCache } from '../hooks/useUserAccess';
import { v4 as uuidv4 } from 'uuid';

interface AuthState {
  user: User | null;
  loading: boolean;
  initialized: boolean;
  deviceToken: string;
  signInWithGoogle: () => Promise<void>;
  logout: () => Promise<void>;
  setUser: (user: User | null) => void;
  setInitialized: (initialized: boolean) => void;
}

// Lấy hoặc tạo Device Token duy nhất cho trình duyệt này
const getDeviceToken = () => {
  let token = localStorage.getItem('jplearn_device_token');
  if (!token) {
    token = uuidv4();
    localStorage.setItem('jplearn_device_token', token);
  }
  return token;
};

export const useAuthStore = create<AuthState>()((set) => ({
  user: null,
  loading: false,
  initialized: false,
  deviceToken: getDeviceToken(),

  signInWithGoogle: async () => {
    set({ loading: true });
    try {
      const result = await signInWithPopup(auth, googleProvider);
      if (result.user) {
        clearUserAccessCache();
        // Đồng bộ user với server ngay khi login
        await apiClient.post('/auth/sync', {
          displayName: result.user.displayName,
          avatarUrl: result.user.photoURL,
          deviceToken: getDeviceToken()
        });
      }
    } catch (error) {
      console.error('Google sign-in failed:', error);
    } finally {
      set({ loading: false });
    }
  },

  logout: async () => {
    set({ loading: true });
    try {
      await apiClient.post('/auth/logout', {
        deviceToken: getDeviceToken()
      }).catch((error) => {
        console.error('Server logout failed:', error);
      });
      clearUserAccessCache();
      await signOut(auth);
      set({ user: null });
    } catch (error) {
      console.error('Logout failed:', error);
    } finally {
      set({ loading: false });
    }
  },

  setUser: (user) => set({ user }),
  setInitialized: (initialized) => set({ initialized }),
}));

export function initAuthListener() {
  const { setUser, setInitialized, deviceToken } = useAuthStore.getState();
  
  onAuthStateChanged(auth, async (user) => {
    setUser(user);
    setInitialized(true);

    if (user) {
      clearUserAccessCache();
      // Sync định kỳ khi refresh trang để đảm bảo Device Token luôn mới nhất
      apiClient.post('/auth/sync', {
        displayName: user.displayName,
        avatarUrl: user.photoURL,
        deviceToken
      }).catch(err => {
        if (err.response?.status === 403) {
          // Nếu server báo token không khớp -> Logout ngay
          signOut(auth);
        }
      });
    }
  });
}
