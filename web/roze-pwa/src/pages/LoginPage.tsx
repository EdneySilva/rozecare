import { FormEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { api } from '../services/api';
import { useAuthStore } from '../store/auth';
import { toast } from 'react-toastify';

export const LoginPage = () => {
  const navigate = useNavigate();
  const { setCredentials } = useAuthStore();
  const [email, setEmail] = useState('patient@rozecare.test');
  const [password, setPassword] = useState('P@ssword123!');
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault();
    try {
      setIsLoading(true);
      const response = await api.post('/api/auth/login', { email, password });
      setCredentials(response.data.accessToken ?? response.data.access_token, response.data.refreshToken ?? response.data.refresh_token);
      toast.success('Login realizado com sucesso');
      navigate('/');
    } catch (error: any) {
      toast.error(error?.response?.data?.message ?? 'Não foi possível autenticar');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-slate-900 px-6">
      <form onSubmit={handleSubmit} className="w-full max-w-md space-y-4 rounded-xl border border-slate-800 bg-slate-950/60 p-8 shadow-xl">
        <h1 className="text-2xl font-semibold text-white">Entrar no RozeCare</h1>
        <label className="block text-sm text-slate-300">
          Email
          <input
            value={email}
            onChange={(event) => setEmail(event.target.value)}
            type="email"
            className="mt-1 w-full rounded border border-slate-700 bg-slate-900 px-3 py-2 text-sm text-white"
          />
        </label>
        <label className="block text-sm text-slate-300">
          Senha
          <input
            value={password}
            onChange={(event) => setPassword(event.target.value)}
            type="password"
            className="mt-1 w-full rounded border border-slate-700 bg-slate-900 px-3 py-2 text-sm text-white"
          />
        </label>
        <button
          type="submit"
          disabled={isLoading}
          className="w-full rounded bg-rose-600 py-2 text-sm font-semibold text-white transition hover:bg-rose-500 disabled:opacity-60"
        >
          {isLoading ? 'Entrando...' : 'Entrar'}
        </button>
      </form>
    </div>
  );
};
