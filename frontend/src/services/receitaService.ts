import { api } from "./apiClient";
import type {
  Receita,
  CriarReceitaDto,
  AtualizarReceitaDto,
  CriarReceitaIngredienteDto,
  SimularMargemDto,
  ResumoFinanceiro,
} from "../types/receita";

export const receitaService = {
  listar: (apenasAtivas = false) =>
    api.get<Receita[]>(`/receitas?apenasAtivas=${apenasAtivas}`),

  buscar: (termo: string) =>
    api.get<Receita[]>(`/receitas/busca?termo=${encodeURIComponent(termo)}`),

  obterPorId: (id: string) =>
    api.get<Receita>(`/receitas/${id}`),

  criar: (dto: CriarReceitaDto) =>
    api.post<Receita>("/receitas", dto),

  atualizar: (id: string, dto: AtualizarReceitaDto) =>
    api.put<Receita>(`/receitas/${id}`, dto),

  desativar: (id: string) =>
    api.delete<void>(`/receitas/${id}`),

  adicionarIngrediente: (id: string, dto: CriarReceitaIngredienteDto) =>
    api.post<Receita>(`/receitas/${id}/ingredientes`, dto),

  removerIngrediente: (id: string, receitaIngredienteId: string) =>
    api.delete<Receita>(`/receitas/${id}/ingredientes/${receitaIngredienteId}`),

  duplicar: (id: string) =>
    api.post<Receita>(`/receitas/${id}/duplicar`),

  simularMargem: (id: string, dto: SimularMargemDto) =>
    api.post<ResumoFinanceiro>(`/receitas/${id}/simular-margem`, dto),
};
