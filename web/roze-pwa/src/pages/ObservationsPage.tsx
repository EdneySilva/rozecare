import { useQuery } from '@tanstack/react-query';
import { api } from '../services/api';
import { usePatientId } from '../hooks/usePatientId';

interface ObservationDto {
  id: string;
  code: string;
  display: string;
  valueString?: string;
  valueQuantity?: number;
  unit?: string;
  effectiveDate: string;
}

export const ObservationsPage = () => {
  const patientId = usePatientId();

  const { data, isLoading } = useQuery({
    enabled: !!patientId,
    queryKey: ['observations', patientId],
    queryFn: async () => {
      const response = await api.get<ObservationDto[]>(`/api/patients/${patientId}/observations`);
      return response.data;
    }
  });

  return (
    <section className="space-y-4">
      <div className="flex items-center justify-between">
        <h2 className="text-xl font-semibold">Observações clínicas</h2>
      </div>
      {isLoading ? (
        <p className="text-sm text-slate-300">Carregando observações...</p>
      ) : (
        <div className="overflow-hidden rounded border border-slate-800">
          <table className="min-w-full divide-y divide-slate-800 text-sm">
            <thead className="bg-slate-950/50 text-left text-xs uppercase tracking-wide text-slate-400">
              <tr>
                <th className="px-4 py-3">Código</th>
                <th className="px-4 py-3">Descrição</th>
                <th className="px-4 py-3">Valor</th>
                <th className="px-4 py-3">Data</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-800 bg-slate-950/20 text-slate-200">
              {(data ?? []).map((item) => (
                <tr key={item.id}>
                  <td className="px-4 py-3 font-mono text-xs text-slate-400">{item.code}</td>
                  <td className="px-4 py-3">{item.display}</td>
                  <td className="px-4 py-3">
                    {item.valueQuantity ?? item.valueString ?? '—'} {item.unit}
                  </td>
                  <td className="px-4 py-3">{new Date(item.effectiveDate).toLocaleString()}</td>
                </tr>
              ))}
              {(data ?? []).length === 0 && (
                <tr>
                  <td colSpan={4} className="px-4 py-6 text-center text-slate-400">
                    Nenhum registro disponível.
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
