namespace CalculadoraCusto.Application.DTOs;

// Representa um ingrediente dentro de uma receita, com os calculos ja prontos
public record ReceitaIngredienteDto(
    Guid Id,
    Guid? IngredienteId,
    string NomeIngrediente,
    decimal QuantidadeComprada,
    string UnidadeCompra,
    decimal ValorPago,
    decimal QuantidadeUtilizada,
    string UnidadeUtilizada,
    int Ordem,
    decimal CustoUnitario,
    decimal CustoUtilizado
);

// Dados para adicionar um ingrediente a uma receita (nova ou existente)
public record CriarReceitaIngredienteDto(
    Guid? IngredienteId,
    string NomeIngrediente,
    decimal QuantidadeComprada,
    string UnidadeCompra,
    decimal ValorPago,
    decimal QuantidadeUtilizada,
    string UnidadeUtilizada,
    int Ordem = 0
);

// Receita completa, com ingredientes e resumo financeiro calculado
public record ReceitaDto(
    Guid Id,
    string Nome,
    string? Descricao,
    decimal QuantidadeProduzida,
    string UnidadeProduzida,
    decimal? MargemLucroPadrao,
    bool Ativo,
    DateTime CriadoEm,
    List<ReceitaIngredienteDto> Ingredientes,
    ResumoFinanceiroDto ResumoFinanceiro
);

// O resumo financeiro que voce pediu: total gasto, custo por unidade, margem, lucro, preco final
public record ResumoFinanceiroDto(
    decimal TotalGasto,
    decimal CustoPorUnidade,
    decimal MargemAplicada,
    decimal LucroPorUnidade,
    decimal PrecoFinalSugerido
);

// Dados para criar uma receita nova, ja com os ingredientes juntos
public record CriarReceitaDto(
    string Nome,
    string? Descricao,
    decimal QuantidadeProduzida,
    string UnidadeProduzida,
    decimal? MargemLucroPadrao,
    List<CriarReceitaIngredienteDto> Ingredientes
);

// Dados para atualizar so os campos gerais da receita (sem mexer nos ingredientes)
public record AtualizarReceitaDto(
    string Nome,
    string? Descricao,
    decimal QuantidadeProduzida,
    string UnidadeProduzida,
    decimal? MargemLucroPadrao
);

// Usado no endpoint que simula um preco de venda com outra margem, sem salvar nada
public record SimularMargemDto(
    decimal MargemLucro
);
