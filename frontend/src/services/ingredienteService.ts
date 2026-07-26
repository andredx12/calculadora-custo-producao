import { api } from "./apiClient";
import type { Ingrediente, CriarIngredienteDto, AtualizarIngredienteDto } from "../types/ingrediente";

export const ingredienteService = {
  listar: (apenasAtivos = false) =>
    api.get<Ingrediente[]>(`/ingredientes?apenasAtivos=${apenasAtivos}`),

  buscar: (termo: string) =>
    api.get<Ingrediente[]>(`/ingredientes/busca?termo=${encodeURIComponent(termo)}`),

  obterPorId: (id: string) =>
    api.get<Ingrediente>(`/ingredientes/${id}`),

  criar: (dto: CriarIngredienteDto) =>
    api.post<Ingrediente>("/ingredientes", dto),

  atualizar: (id: string, dto: AtualizarIngredienteDto) =>
    api.put<Ingrediente>(`/ingredientes/${id}`, dto),

  desativar: (id: string) =>
    api.delete<void>(`/ingredientes/${id}`),
};
