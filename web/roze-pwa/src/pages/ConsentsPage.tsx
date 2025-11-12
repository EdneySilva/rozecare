import { FormEvent } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { api } from '../services/api';
import { usePatientId } from '../hooks/usePatientId';
import { toast } from 'react-toastify';

interface ConsentDto {
  id: string;
  patientId: string;
  granteeType: string;
  granteeId: string;
  scopes: string[];
  expiresAtUtc: string;
  status: string;
}

export const ConsentsPage = () => {
  const patientId = usePatientId();
  const queryClient = useQueryClient();

  const { data } = useQuery({
    enabled: !!patientId,
    queryKey: ['consents', patientId],
    queryFn: async () => {
      const response = await api.get<ConsentDto[]>(`/api/patients/${patientId}/consents`);
      return response.data;
    }
  });

  const mutation = useMutation({
    mutationFn: async (body: { granteeId: string; scopes: string[]; expiresAtUtc: string }) => {
      await api.post(`/api/patients/${patientId}/consents`, {
        granteeType: 'Provider',
        granteeId: body.granteeId,
        scopes: body.scopes,
        expiresAtUtc: body.expiresAtUtc
      });
    },
    onSuccess: () => {
      toast.success('Consentimento registrado');
      queryClient.invalidateQueries({ queryKey: ['consents', patientId] });
    },
    onError: () => toast.error('Falha ao registrar consentimento')
  });

  const revokeMutation = useMutation({
    mutationFn: async (consentId: string) => {
      await api.post(`/api/patients/${patientId}/consents/${consentId}/revoke`);
    },
    onSuccess: () => {
      toast.success('Consentimento revogado');
      queryClient.invalidateQueries({ queryKey: ['consents', patientId] });
    }
  });

  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const form = new FormData(event.currentTarget);
    const granteeId = form.get('granteeId')?.toString();
    const scopes = (form.get('scopes')?.toString() ?? '').split(',').map((scope) => scope.trim()).filter(Boolean);
    const expiresAt = form.get('expiresAt')?.toString();
    if (!granteeId || !expiresAt) {
      toast.error('Preencha os campos obrigatórios');
      return;
    }
    mutation.mutate({ granteeId, scopes, expiresAtUtc: expiresAt });
  };

  return (
    <section className="space-y-6">
      <div>
        <h2 className="text-xl font-semibold">Consentimentos ativos</h2>
        <p className="text-sm text-slate-300">Gerencie quem pode acessar seus dados clínicos e com quais escopos.</p>
      </div>
      <form onSubmit={handleSubmit} className="grid gap-4 md:grid-cols-3">
        <label className="text-sm text-slate-300">
          ID do provedor
          <input name="granteeId" className="mt-1 w-full rounded border border-slate-800 bg-slate-950 px-3 py-2 text-sm" />
        </label>
        <label className="text-sm text-slate-300">
          Escopos (ex: obs:read, docs:read)
          <input name="scopes" className="mt-1 w-full rounded border border-slate-800 bg-slate-950 px-3 py-2 text-sm" />
        </label>
        <label className="text-sm text-slate-300">
          Expira em
          <input type="datetime-local" name="expiresAt" className="mt-1 w-full rounded border border-slate-800 bg-slate-950 px-3 py-2 text-sm" />
        </label>
        <button
          type="submit"
          className="md:col-span-3 rounded bg-rose-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-rose-500"
        >
          Conceder consentimento
        </button>
      </form>
      <div className="overflow-hidden rounded border border-slate-800">
        <table className="min-w-full divide-y divide-slate-800 text-sm">
          <thead className="bg-slate-950/40 text-left text-xs uppercase tracking-wide text-slate-400">
            <tr>
              <th className="px-4 py-3">Grantee</th>
              <th className="px-4 py-3">Escopos</th>
              <th className="px-4 py-3">Expira em</th>
              <th className="px-4 py-3">Status</th>
              <th className="px-4 py-3" />
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-800 bg-slate-950/20 text-slate-200">
            {(data ?? []).map((consent) => (
              <tr key={consent.id}>
                <td className="px-4 py-3 font-mono text-xs text-slate-400">{consent.granteeId}</td>
                <td className="px-4 py-3 text-xs text-slate-300">{consent.scopes.join(', ')}</td>
                <td className="px-4 py-3 text-xs text-slate-400">{new Date(consent.expiresAtUtc).toLocaleString()}</td>
                <td className="px-4 py-3 text-xs text-slate-300">{consent.status}</td>
                <td className="px-4 py-3 text-right">
                  {consent.status === 'Active' && (
                    <button className="rounded bg-rose-500 px-3 py-1 text-xs font-semibold text-white" onClick={() => revokeMutation.mutate(consent.id)}>
                      Revogar
                    </button>
                  )}
                </td>
              </tr>
            ))}
            {(data ?? []).length === 0 && (
              <tr>
                <td colSpan={5} className="px-4 py-6 text-center text-slate-400">
                  Nenhum consentimento ativo.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </section>
  );
};
