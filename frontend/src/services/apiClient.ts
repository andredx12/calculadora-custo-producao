const API_URL = import.meta.env.VITE_API_URL || "http://localhost:5238/api";

interface ApiErrorBody {
  mensagem?: string;
}

export class ApiError extends Error {
  status: number;

  constructor(status: number, mensagem: string) {
    super(mensagem);
    this.status = status;
  }
}

async function tratarResposta<T>(resposta: Response): Promise<T> {
  if (resposta.status === 204) {
    return undefined as T;
  }

  const texto = await resposta.text();
  const dados = texto ? JSON.parse(texto) : null;

  if (!resposta.ok) {
    const corpo = dados as ApiErrorBody;
    throw new ApiError(resposta.status, corpo?.mensagem || "Erro ao comunicar com o servidor.");
  }

  return dados as T;
}

export const api = {
  async get<T>(caminho: string): Promise<T> {
    const resposta = await fetch(`${API_URL}${caminho}`);
    return tratarResposta<T>(resposta);
  },

  async post<T>(caminho: string, corpo?: unknown): Promise<T> {
    const resposta = await fetch(`${API_URL}${caminho}`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: corpo !== undefined ? JSON.stringify(corpo) : undefined,
    });
    return tratarResposta<T>(resposta);
  },

  async put<T>(caminho: string, corpo: unknown): Promise<T> {
    const resposta = await fetch(`${API_URL}${caminho}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(corpo),
    });
    return tratarResposta<T>(resposta);
  },

  async delete<T>(caminho: string): Promise<T> {
    const resposta = await fetch(`${API_URL}${caminho}`, {
      method: "DELETE",
    });
    return tratarResposta<T>(resposta);
  },
};
