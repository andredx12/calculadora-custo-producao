import { api } from "./apiClient";
import type { Producao, CriarProducaoDto } from "../types/producao";

export const producaoService = {
  listar: () => api.get<Producao[]>("/producoes"),
  registrar: (dto: CriarProducaoDto) => api.post<Producao>("/producoes", dto),
};
