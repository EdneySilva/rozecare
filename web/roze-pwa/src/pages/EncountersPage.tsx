import { useQuery } from '@tanstack/react-query';
import { api } from '../services/api';
import { usePatientId } from '../hooks/usePatientId';

interface EncounterDto {
  id: string;
  providerId: string;
  date: string;
  type: string;
  notes: string;
  diagnoses: string[];
  prescriptions: string[];
}

export const EncountersPage = () => {
  const patientId = usePatientId();
  const { data, isLoading } = useQuery({
    enabled: !!patientId,
    queryKey: ['encounters', patientId],
    queryFn: async () => {
      const response = await api.get<EncounterDto[]>(`/api/patients/${patientId}/encounters`);
      return response.data;
    }
  });

  return (
    <section className="space-y-4">
      <h2 className="text-xl font-semibold">Encontros clínicos</h2>
      {isLoading ? (
        <p className="text-sm text-slate-300">Carregando encontros...</p>
      ) : (
        <div className="space-y-3">
          {(data ?? []).map((item) => (
            <article key={item.id} className="rounded border border-slate-800 bg-slate-950/30 p-4 text-sm text-slate-200">
              <header className="mb-2 flex items-center justify-between text-xs text-slate-400">
                <span>{new Date(item.date).toLocaleString()}</span>
                <span>{item.type}</span>
              </header>
              <p className="mb-2 text-slate-200">{item.notes}</p>
              <div className="grid gap-2 text-xs text-slate-300 md:grid-cols-2">
                <div>
                  <h4 className="font-semibold uppercase tracking-wide text-slate-400">Diagnósticos</h4>
                  <ul className="list-disc pl-4">
                    {item.diagnoses.map((diag) => (
                      <li key={diag}>{diag}</li>
                    ))}
                  </ul>
                </div>
                <div>
                  <h4 className="font-semibold uppercase tracking-wide text-slate-400">Prescrições</h4>
                  <ul className="list-disc pl-4">
                    {item.prescriptions.map((presc) => (
                      <li key={presc}>{presc}</li>
                    ))}
                  </ul>
                </div>
              </div>
            </article>
          ))}
          {(data ?? []).length === 0 && <p className="text-sm text-slate-400">Nenhum encontro registrado.</p>}
        </div>
      )}
    </section>
  );
};
