namespace CalculadoraCusto.Application.DTOs;

public record VendaDto(
    Guid Id,
    Guid ReceitaId,
    string NomeReceita,
    decimal QuantidadeVendida,
    decimal PrecoUnitarioVenda,
    decimal CustoUnitarioNoMomento,
    decimal LucroTotal,
    DateTime DataVenda,
    DateTime CriadoEm
);

public record CriarVendaDto(
    Guid ReceitaId,
    decimal QuantidadeVendida,
    decimal PrecoUnitarioVenda,
    DateTime DataVenda
);
