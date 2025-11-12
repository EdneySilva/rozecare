import { useQuery } from '@tanstack/react-query';
import { api } from '../services/api';
import { usePatientId } from '../hooks/usePatientId';

interface MedicationDto {
  id: string;
  name: string;
  dosage: string;
  startDate: string;
  endDate?: string;
  prescribedBy?: string;
}

export const MedicationsPage = () => {
  const patientId = usePatientId();
  const { data, isLoading } = useQuery({
    enabled: !!patientId,
    queryKey: ['medications', patientId],
    queryFn: async () => {
      const response = await api.get<MedicationDto[]>(`/api/patients/${patientId}/medications`);
      return response.data;
    }
  });

  return (
    <section className="space-y-4">
      <h2 className="text-xl font-semibold">Medicamentos ativos</h2>
      {isLoading ? (
        <p className="text-sm text-slate-300">Carregando medicamentos...</p>
      ) : (
        <ul className="space-y-3">
          {(data ?? []).map((item) => (
            <li key={item.id} className="rounded border border-slate-800 bg-slate-950/30 p-4 text-sm text-slate-200">
              <div className="flex items-center justify-between">
                <span className="font-semibold text-white">{item.name}</span>
                <span className="text-xs text-slate-400">{item.dosage}</span>
              </div>
              <p className="text-xs text-slate-400">
                {new Date(item.startDate).toLocaleDateString()} - {item.endDate ? new Date(item.endDate).toLocaleDateString() : 'Em uso'}
              </p>
              {item.prescribedBy && <p className="mt-1 text-xs text-slate-300">Prescrito por {item.prescribedBy}</p>}
            </li>
          ))}
          {(data ?? []).length === 0 && <li className="text-sm text-slate-400">Nenhum medicamento cadastrado.</li>}
        </ul>
      )}
    </section>
  );
};
