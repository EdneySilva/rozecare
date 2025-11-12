import { FormEvent, useMemo } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { api } from '../services/api';
import { toast } from 'react-toastify';

interface PatientProfileDto {
  userId: string;
  bloodType?: string;
  conditions: string[];
  allergies: string[];
  emergencyContacts: string[];
  preferredProviders: string[];
}

export const ProfilePage = () => {
  const queryClient = useQueryClient();
  const { data, isLoading } = useQuery({
    queryKey: ['profile'],
    queryFn: async () => {
      const response = await api.get<PatientProfileDto>('/api/patients/me');
      return response.data;
    }
  });

  const mutation = useMutation({
    mutationFn: async (body: PatientProfileDto) => {
      await api.put(`/api/patients/me`, {
        bloodType: body.bloodType,
        conditions: body.conditions,
        allergies: body.allergies,
        emergencyContacts: body.emergencyContacts,
        preferredProviders: body.preferredProviders
      });
    },
    onSuccess: () => {
      toast.success('Perfil atualizado');
      queryClient.invalidateQueries({ queryKey: ['profile'] });
    },
    onError: () => toast.error('Não foi possível atualizar o perfil')
  });

  const formData = useMemo(() => {
    if (!data) {
      return {
        bloodType: '',
        conditions: '',
        allergies: '',
        emergencyContacts: '',
        preferredProviders: ''
      };
    }
    return {
      bloodType: data.bloodType ?? '',
      conditions: data.conditions.join(', '),
      allergies: data.allergies.join(', '),
      emergencyContacts: data.emergencyContacts.join(', '),
      preferredProviders: data.preferredProviders.join(', ')
    };
  }, [data]);

  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const form = new FormData(event.currentTarget);
    mutation.mutate({
      userId: data?.userId ?? '',
      bloodType: form.get('bloodType')?.toString() ?? undefined,
      conditions: (form.get('conditions')?.toString() ?? '').split(',').map((value) => value.trim()).filter(Boolean),
      allergies: (form.get('allergies')?.toString() ?? '').split(',').map((value) => value.trim()).filter(Boolean),
      emergencyContacts: (form.get('emergencyContacts')?.toString() ?? '').split(',').map((value) => value.trim()).filter(Boolean),
      preferredProviders: (form.get('preferredProviders')?.toString() ?? '').split(',').map((value) => value.trim()).filter(Boolean)
    });
  };

  if (isLoading) {
    return <p className="text-sm text-slate-300">Carregando perfil...</p>;
  }

  return (
    <section className="space-y-4">
      <h2 className="text-xl font-semibold">Meu Perfil</h2>
      <form onSubmit={handleSubmit} className="grid gap-4 md:grid-cols-2">
        <label className="text-sm text-slate-300">
          Tipo sanguíneo
          <input name="bloodType" defaultValue={formData.bloodType} className="mt-1 w-full rounded border border-slate-800 bg-slate-950 px-3 py-2 text-sm" />
        </label>
        <label className="text-sm text-slate-300">
          Condições (separadas por vírgula)
          <input name="conditions" defaultValue={formData.conditions} className="mt-1 w-full rounded border border-slate-800 bg-slate-950 px-3 py-2 text-sm" />
        </label>
        <label className="text-sm text-slate-300">
          Alergias
          <input name="allergies" defaultValue={formData.allergies} className="mt-1 w-full rounded border border-slate-800 bg-slate-950 px-3 py-2 text-sm" />
        </label>
        <label className="text-sm text-slate-300">
          Contatos de emergência
          <input name="emergencyContacts" defaultValue={formData.emergencyContacts} className="mt-1 w-full rounded border border-slate-800 bg-slate-950 px-3 py-2 text-sm" />
        </label>
        <label className="text-sm text-slate-300 md:col-span-2">
          Prestadores preferidos
          <input name="preferredProviders" defaultValue={formData.preferredProviders} className="mt-1 w-full rounded border border-slate-800 bg-slate-950 px-3 py-2 text-sm" />
        </label>
        <button
          type="submit"
          className="md:col-span-2 rounded bg-rose-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-rose-500"
          disabled={mutation.isLoading}
        >
          {mutation.isLoading ? 'Salvando...' : 'Salvar alterações'}
        </button>
      </form>
    </section>
  );
};
