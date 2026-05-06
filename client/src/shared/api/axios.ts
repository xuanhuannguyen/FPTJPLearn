import axios from 'axios';

export const apiClient = axios.create({
  baseURL: 'http://localhost:5000/api', // .NET API URL
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
    return config;
  },
  (error) => Promise.reject(error)
);
