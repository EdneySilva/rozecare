import { create } from 'zustand';

interface AuthState {
  token: string | null;
  refreshToken: string | null;
  isAuthenticated: boolean;
  setCredentials: (token: string, refresh: string) => void;
  logout: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  token: null,
  refreshToken: null,
  isAuthenticated: false,
  setCredentials: (token, refresh) => set({ token, refreshToken: refresh, isAuthenticated: true }),
  logout: () => set({ token: null, refreshToken: null, isAuthenticated: false })
}));
