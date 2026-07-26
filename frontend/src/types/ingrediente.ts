export interface Ingrediente {
  id: string;
  nome: string;
  unidadePadrao: string;
  ativo: boolean;
  criadoEm: string;
}

export interface CriarIngredienteDto {
  nome: string;
  unidadePadrao: string;
}

export interface AtualizarIngredienteDto {
  nome: string;
}
