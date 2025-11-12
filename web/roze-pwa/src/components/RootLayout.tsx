import { NavLink, Outlet, useNavigate } from 'react-router-dom';
import { useEffect } from 'react';
import { useAuthStore } from '../store/auth';
import { clsx } from 'clsx';

const links = [
  { to: '/', label: 'Dashboard' },
  { to: '/profile', label: 'Meu Perfil' },
  { to: '/observations', label: 'Observações' },
  { to: '/medications', label: 'Medicamentos' },
  { to: '/allergies', label: 'Alergias' },
  { to: '/encounters', label: 'Encontros' },
  { to: '/documents', label: 'Documentos' },
  { to: '/consents', label: 'Consents' },
  { to: '/audit', label: 'Auditoria' }
];

export const RootLayout = () => {
  const navigate = useNavigate();
  const { isAuthenticated, logout } = useAuthStore();

  useEffect(() => {
    if (!isAuthenticated) {
      navigate('/login');
    }
  }, [isAuthenticated, navigate]);

  return (
    <div className="min-h-screen bg-slate-900 text-slate-100">
      <header className="border-b border-slate-800 bg-slate-950/60 backdrop-blur">
        <div className="mx-auto flex max-w-6xl items-center justify-between px-6 py-4">
          <h1 className="text-xl font-semibold">RozeCare</h1>
          <nav className="flex gap-4 text-sm">
            {links.map((link) => (
              <NavLink
                key={link.to}
                to={link.to}
                className={({ isActive }) =>
                  clsx('transition hover:text-rose-300', isActive && 'text-rose-400 font-medium')
                }
              >
                {link.label}
              </NavLink>
            ))}
          </nav>
          <button
            className="rounded bg-rose-600 px-3 py-1 text-sm font-semibold text-white shadow"
            onClick={logout}
          >
            Sair
          </button>
        </div>
      </header>
      <main className="mx-auto flex max-w-6xl flex-1 flex-col gap-6 px-6 py-8">
        <Outlet />
      </main>
    </div>
  );
};
