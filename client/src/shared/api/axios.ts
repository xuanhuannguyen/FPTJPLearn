import axios from 'axios';
import { auth } from '../config/firebase';

export const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5178/api',


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
