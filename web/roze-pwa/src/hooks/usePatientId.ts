import { useQuery } from '@tanstack/react-query';
import { api } from '../services/api';

export const usePatientId = () => {
  const { data } = useQuery({
    queryKey: ['patient-id'],
    queryFn: async () => {
      const response = await api.get<{ userId: string }>('/api/patients/me');
      return response.data.userId;
    }
  });
  return data;
};
