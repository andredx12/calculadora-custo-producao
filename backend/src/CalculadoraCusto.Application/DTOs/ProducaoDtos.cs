namespace CalculadoraCusto.Application.DTOs;

public record ProducaoDto(
    Guid Id,
    Guid ReceitaId,
    string NomeReceita,
    decimal QuantidadeProduzida,
    DateTime DataProducao,
    DateTime CriadoEm
);

public record CriarProducaoDto(
    Guid ReceitaId,
    decimal QuantidadeProduzida,
    DateTime DataProducao
);
