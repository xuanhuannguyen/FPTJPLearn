import axios from 'axios';
import { auth } from '../config/firebase';

export const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? '/api',
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
    return config;
  },
  (error) => Promise.reject(error)
);
