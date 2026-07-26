import { api } from "./apiClient";
import type { Venda, CriarVendaDto } from "../types/venda";

export const vendaService = {
  listar: () => api.get<Venda[]>("/vendas"),
  registrar: (dto: CriarVendaDto) => api.post<Venda>("/vendas", dto),
};
