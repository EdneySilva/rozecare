import { useQuery } from '@tanstack/react-query';
import { api } from '../services/api';
import { usePatientId } from '../hooks/usePatientId';

interface AllergyDto {
  id: string;
  substance: string;
  reaction: string;
  severity: string;
}

export const AllergiesPage = () => {
  const patientId = usePatientId();
  const { data, isLoading } = useQuery({
    enabled: !!patientId,
    queryKey: ['allergies', patientId],
    queryFn: async () => {
      const response = await api.get<AllergyDto[]>(`/api/patients/${patientId}/allergies`);
      return response.data;
    }
  });

  return (
    <section className="space-y-4">
      <h2 className="text-xl font-semibold">Alergias</h2>
      {isLoading ? (
        <p className="text-sm text-slate-300">Carregando alergias...</p>
      ) : (
        <ul className="space-y-3">
          {(data ?? []).map((item) => (
            <li key={item.id} className="rounded border border-slate-800 bg-slate-950/30 p-4">
              <p className="text-sm font-semibold text-white">{item.substance}</p>
              <p className="text-xs text-slate-300">Reação: {item.reaction}</p>
              <p className="text-xs text-slate-400">Gravidade: {item.severity}</p>
            </li>
          ))}
          {(data ?? []).length === 0 && <li className="text-sm text-slate-400">Nenhuma alergia registrada.</li>}
        </ul>
      )}
    </section>
  );
};
