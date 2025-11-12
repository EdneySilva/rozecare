import { useQuery } from '@tanstack/react-query';
import { api } from '../services/api';

interface AuditLogDto {
  id: string;
  whenUtc: string;
  actorUserId?: string;
  action: string;
  resourceType: string;
  resourceId?: string;
  details: string;
}

export const AuditPage = () => {
  const { data, isLoading } = useQuery({
    queryKey: ['audit'],
    queryFn: async () => {
      const response = await api.get<AuditLogDto[]>(`/api/audit`);
      return response.data;
    }
  });

  return (
    <section className="space-y-4">
      <h2 className="text-xl font-semibold">Auditoria</h2>
      {isLoading ? (
        <p className="text-sm text-slate-300">Carregando auditoria...</p>
      ) : (
        <div className="overflow-hidden rounded border border-slate-800">
          <table className="min-w-full divide-y divide-slate-800 text-sm">
            <thead className="bg-slate-950/40 text-left text-xs uppercase tracking-wide text-slate-400">
              <tr>
                <th className="px-4 py-3">Quando</th>
                <th className="px-4 py-3">Ação</th>
                <th className="px-4 py-3">Recurso</th>
                <th className="px-4 py-3">Detalhes</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-800 bg-slate-950/20 text-slate-200">
              {(data ?? []).map((log) => (
                <tr key={log.id}>
                  <td className="px-4 py-3 text-xs text-slate-400">{new Date(log.whenUtc).toLocaleString()}</td>
                  <td className="px-4 py-3">{log.action}</td>
                  <td className="px-4 py-3 text-xs text-slate-300">{log.resourceType}</td>
                  <td className="px-4 py-3 text-xs text-slate-400">
                    <code className="block whitespace-pre-wrap text-xs">{log.details}</code>
                  </td>
                </tr>
              ))}
              {(data ?? []).length === 0 && (
                <tr>
                  <td colSpan={4} className="px-4 py-6 text-center text-slate-400">
                    Nenhum evento registrado.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      )}
    </section>
  );
};
