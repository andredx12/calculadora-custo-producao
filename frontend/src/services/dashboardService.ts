import { api } from "./apiClient";
import type { DashboardMensal } from "../types/dashboard";

export const dashboardService = {
  mensal: (mes?: number, ano?: number) => {
    const params = new URLSearchParams();
    if (mes) params.set("mes", mes.toString());
    if (ano) params.set("ano", ano.toString());
    const query = params.toString();
    return api.get<DashboardMensal>(`/dashboard/mensal${query ? `?${query}` : ""}`);
  },
};
