export interface ReceitaIngrediente {
  id: string;
  ingredienteId: string | null;
  nomeIngrediente: string;
  quantidadeComprada: number;
  unidadeCompra: string;
  valorPago: number;
  quantidadeUtilizada: number;
  unidadeUtilizada: string;
  ordem: number;
  custoUnitario: number;
  custoUtilizado: number;
}

export interface CriarReceitaIngredienteDto {
  ingredienteId: string | null;
  nomeIngrediente: string;
  quantidadeComprada: number;
  unidadeCompra: string;
  valorPago: number;
  quantidadeUtilizada: number;
  unidadeUtilizada: string;
  ordem?: number;
}

export interface ResumoFinanceiro {
  totalGasto: number;
  custoPorUnidade: number;
  margemAplicada: number;
  lucroPorUnidade: number;
  precoFinalSugerido: number;
}

export interface Receita {
  id: string;
  nome: string;
  descricao: string | null;
  quantidadeProduzida: number;
  unidadeProduzida: string;
  margemLucroPadrao: number | null;
  ativo: boolean;
  criadoEm: string;
  ingredientes: ReceitaIngrediente[];
  resumoFinanceiro: ResumoFinanceiro;
}

export interface CriarReceitaDto {
  nome: string;
  descricao: string | null;
  quantidadeProduzida: number;
  unidadeProduzida: string;
  margemLucroPadrao: number | null;
  ingredientes: CriarReceitaIngredienteDto[];
}

export interface AtualizarReceitaDto {
  nome: string;
  descricao: string | null;
  quantidadeProduzida: number;
  unidadeProduzida: string;
  margemLucroPadrao: number | null;
}

export interface SimularMargemDto {
  margemLucro: number;
}
