namespace CalculadoraCusto.Application.DTOs;

public record RankingReceitaDto(
    Guid ReceitaId,
    string NomeReceita,
    decimal QuantidadeVendida
);

public record DashboardMensalDto(
    int Mes,
    int Ano,
    decimal LucroDoMes,
    decimal QuantidadeProduzidaNoMes,
    decimal QuantidadeVendidaNoMes,
    RankingReceitaDto? BoloMaisVendido,
    RankingReceitaDto? BoloMenosVendido
);
