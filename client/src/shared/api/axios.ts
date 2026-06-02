import axios from 'axios';
import { signOut } from 'firebase/auth';
import { auth } from '../config/firebase';

const getBaseURL = () => {
  if (import.meta.env.VITE_API_BASE_URL) return import.meta.env.VITE_API_BASE_URL;
  
  // Nếu đang chạy trên tunnel (không phải localhost), ưu tiên dùng origin hiện tại
  if (typeof window !== 'undefined' && !window.location.hostname.includes('localhost')) {
    return `${window.location.origin}/api`;
  }
  
  return 'http://localhost:5175/api';
};

export const apiClient = axios.create({
  baseURL: getBaseURL(),
  headers: {
    'Content-Type': 'application/json',
  },
});

apiClient.interceptors.request.use(
  async (config) => {
    const currentUser = auth.currentUser;
    if (currentUser) {
      const token = await currentUser.getIdToken();
      config.headers.Authorization = `Bearer ${token}`;
    }

    const adminKey = localStorage.getItem('jplearn_admin_key');
    if (adminKey) {
      config.headers['X-Admin-Key'] = adminKey;
    }

    const deviceToken = localStorage.getItem('jplearn_device_token');
    if (deviceToken) {
      config.headers['X-Device-Token'] = deviceToken;
    }

    return config;

  },
  (error) => Promise.reject(error)
);

let isHandlingStaleDevice = false;

apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    const status = error.response?.status;
    const errorCode = error.response?.data?.error;

    if (status === 403 && errorCode === 'ACCOUNT_LOGGED_IN_ELSEWHERE' && !isHandlingStaleDevice) {
      isHandlingStaleDevice = true;
      sessionStorage.setItem(
        'jplearn_login_notice',
        error.response?.data?.message || 'Tài khoản đã đăng nhập ở thiết bị khác.'
      );

      try {
        await signOut(auth);
      } finally {
        window.location.assign('/login');
      }
    }

    return Promise.reject(error);
  }
);
