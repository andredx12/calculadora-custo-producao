export interface Producao {
  id: string;
  receitaId: string;
  nomeReceita: string;
  quantidadeProduzida: number;
  dataProducao: string;
  criadoEm: string;
}

export interface CriarProducaoDto {
  receitaId: string;
  quantidadeProduzida: number;
  dataProducao: string;
}
