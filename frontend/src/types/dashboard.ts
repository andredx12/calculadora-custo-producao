export interface RankingReceita {
  receitaId: string;
  nomeReceita: string;
  quantidadeVendida: number;
}

export interface DashboardMensal {
  mes: number;
  ano: number;
  lucroDoMes: number;
  quantidadeProduzidaNoMes: number;
  quantidadeVendidaNoMes: number;
  boloMaisVendido: RankingReceita | null;
  boloMenosVendido: RankingReceita | null;
}
