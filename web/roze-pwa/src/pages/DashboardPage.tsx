import { useQuery } from '@tanstack/react-query';
import { api } from '../services/api';

export const DashboardPage = () => {
  const { data: audit } = useQuery({
    queryKey: ['audit-summary'],
    queryFn: async () => {
      const response = await api.get('/api/audit');
      return response.data as Array<{ id: string; action: string; whenUtc: string }>;
    }
  });

  return (
    <section className="space-y-4">
      <h2 className="text-xl font-semibold">Visão Geral</h2>
      <p className="text-sm text-slate-300">
        Bem-vindo ao RozeCare! Utilize o menu para gerenciar seu prontuário pessoal, compartilhar dados com profissionais e acompanhar acessos recentes.
      </p>
      <div className="rounded border border-slate-800 bg-slate-950/40 p-4">
        <h3 className="mb-2 text-sm font-semibold uppercase tracking-wide text-slate-400">Últimos acessos</h3>
        <ul className="space-y-2 text-sm text-slate-200">
          {(audit ?? []).slice(0, 5).map((entry) => (
            <li key={entry.id} className="flex justify-between text-slate-300">
              <span>{entry.action}</span>
              <span>{new Date(entry.whenUtc).toLocaleString()}</span>
            </li>
          ))}
          {(!audit || audit.length === 0) && <li>Nenhum registro recente.</li>}
        </ul>
      </div>
    </section>
  );
};
