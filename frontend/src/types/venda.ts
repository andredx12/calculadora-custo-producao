export interface Venda {
  id: string;
  receitaId: string;
  nomeReceita: string;
  quantidadeVendida: number;
  precoUnitarioVenda: number;
  custoUnitarioNoMomento: number;
  lucroTotal: number;
  dataVenda: string;
  criadoEm: string;
}

export interface CriarVendaDto {
  receitaId: string;
  quantidadeVendida: number;
  precoUnitarioVenda: number;
  dataVenda: string;
}
