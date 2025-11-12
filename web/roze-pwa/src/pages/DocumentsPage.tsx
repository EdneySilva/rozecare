import { FormEvent, useRef } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { api } from '../services/api';
import { usePatientId } from '../hooks/usePatientId';
import { toast } from 'react-toastify';

interface DocumentDto {
  id: string;
  fileName: string;
  contentType: string;
  size: number;
  uploadedAt: string;
  tags: string[];
  description?: string;
  hash: string;
}

export const DocumentsPage = () => {
  const fileInputRef = useRef<HTMLInputElement>(null);
  const patientId = usePatientId();
  const queryClient = useQueryClient();

  const { data } = useQuery({
    enabled: !!patientId,
    queryKey: ['documents', patientId],
    queryFn: async () => {
      const response = await api.get<DocumentDto[]>(`/api/patients/${patientId}/documents`);
      return response.data;
    }
  });

  const uploadMutation = useMutation({
    mutationFn: async (formData: FormData) => {
      await api.post(`/api/patients/${patientId}/documents`, formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      });
    },
    onSuccess: () => {
      toast.success('Documento enviado');
      queryClient.invalidateQueries({ queryKey: ['documents', patientId] });
      if (fileInputRef.current) {
        fileInputRef.current.value = '';
      }
    },
    onError: () => toast.error('Falha ao enviar documento')
  });

  const handleUpload = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (!patientId) return;
    const form = new FormData(event.currentTarget);
    if (!form.get('file')) {
      toast.error('Selecione um arquivo');
      return;
    }
    uploadMutation.mutate(form);
  };

  const handleDownload = async (docId: string) => {
    try {
      const response = await api.get(`/api/patients/${patientId}/documents/${docId}/download`);
      window.open(response.data.url, '_blank');
    } catch (error) {
      toast.error('Não foi possível gerar link de download');
    }
  };

  return (
    <section className="space-y-6">
      <div>
        <h2 className="text-xl font-semibold">Documentos clínicos</h2>
        <p className="text-sm text-slate-300">Faça upload seguro dos seus exames e compartilhe com quem você confia.</p>
      </div>
      <form onSubmit={handleUpload} className="flex flex-col gap-4 rounded border border-dashed border-rose-500/40 p-4">
        <input ref={fileInputRef} type="file" name="file" className="text-sm text-slate-200" />
        <input name="tags" placeholder="Tags (separadas por vírgula)" className="rounded border border-slate-800 bg-slate-950 px-3 py-2 text-sm" />
        <textarea name="description" placeholder="Descrição" className="rounded border border-slate-800 bg-slate-950 px-3 py-2 text-sm" />
        <button
          type="submit"
          className="self-start rounded bg-rose-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-rose-500"
          disabled={uploadMutation.isLoading}
        >
          {uploadMutation.isLoading ? 'Enviando...' : 'Enviar documento'}
        </button>
      </form>
      <div className="overflow-hidden rounded border border-slate-800">
        <table className="min-w-full divide-y divide-slate-800 text-sm">
          <thead className="bg-slate-950/40 text-left text-xs uppercase tracking-wide text-slate-400">
            <tr>
              <th className="px-4 py-3">Arquivo</th>
              <th className="px-4 py-3">Descrição</th>
              <th className="px-4 py-3">Tags</th>
              <th className="px-4 py-3">Enviado em</th>
              <th className="px-4 py-3" />
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-800 bg-slate-950/20 text-slate-200">
            {(data ?? []).map((doc) => (
              <tr key={doc.id}>
                <td className="px-4 py-3">{doc.fileName}</td>
                <td className="px-4 py-3 text-xs text-slate-300">{doc.description ?? '—'}</td>
                <td className="px-4 py-3 text-xs text-slate-300">{doc.tags.join(', ')}</td>
                <td className="px-4 py-3 text-xs text-slate-400">{new Date(doc.uploadedAt).toLocaleString()}</td>
                <td className="px-4 py-3 text-right">
                  <button className="rounded bg-rose-500 px-3 py-1 text-xs font-semibold text-white" onClick={() => handleDownload(doc.id)}>
                    Download
                  </button>
                </td>
              </tr>
            ))}
            {(data ?? []).length === 0 && (
              <tr>
                <td colSpan={5} className="px-4 py-6 text-center text-slate-400">
                  Nenhum documento cadastrado ainda.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </section>
  );
};
