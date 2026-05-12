import axios from 'axios';

export const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? '/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// We can add interceptors here later for JWT tokens
apiClient.interceptors.request.use(
  (config) => {
    // const token = localStorage.getItem('token');
    // if (token) {
    //   config.headers.Authorization = `Bearer ${token}`;
    // }
    const adminKey = localStorage.getItem('jplearn_admin_key');
    if (adminKey) {
      config.headers['X-Admin-Key'] = adminKey;
    }
    return config;
  },
  (error) => Promise.reject(error)
);
