import axios, { AxiosError, InternalAxiosRequestConfig } from "axios";
import { API_URL } from "./endpoints";
import { RefreshToken } from "./authService";

interface FailedRequest {
  resolve: (token: string | null) => void;
  reject: (error: AxiosError) => void;
}

export const client = axios.create({
  baseURL: API_URL,
});

client.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

let isRefreshing = false;
let failedQueue: FailedRequest[] = [];

const processQueue = (
  error: AxiosError | null,
  token: string | null = null
) => {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });
  failedQueue = [];
};

client.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const originalRequest = error.config as InternalAxiosRequestConfig & {
      _retry?: boolean;
    };

    if (error.response?.status === 401 && !originalRequest._retry) {
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({
            resolve: (token: string | null) => {
              originalRequest.headers.Authorization = `Bearer ${token}`;
              resolve(client(originalRequest));
            },
            reject: (err: AxiosError) => reject(err),
          });
        });
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        const response = await RefreshToken();
        const newToken = response?.accessToken;

        if (newToken) {
          localStorage.setItem("token", newToken);
          client.defaults.headers.common.Authorization = `Bearer ${newToken}`;
          processQueue(null, newToken);
          return client(originalRequest);
        }
      } catch (refreshErr) {
        processQueue(refreshErr as AxiosError, null);
        localStorage.removeItem("token");
        localStorage.removeItem("refreshToken");
        localStorage.removeItem("userId");
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);
