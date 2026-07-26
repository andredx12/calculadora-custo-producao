namespace CalculadoraCusto.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using CalculadoraCusto.Application.DTOs;
using CalculadoraCusto.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IProducaoRepository _producaoRepository;
    private readonly IVendaRepository _vendaRepository;
    private readonly IReceitaRepository _receitaRepository;

    public DashboardController(
        IProducaoRepository producaoRepository,
        IVendaRepository vendaRepository,
        IReceitaRepository receitaRepository)
    {
        _producaoRepository = producaoRepository;
        _vendaRepository = vendaRepository;
        _receitaRepository = receitaRepository;
    }

    // GET api/dashboard/mensal?mes=7&ano=2026
    // Se nao informar mes/ano, usa o mes atual
    [HttpGet("mensal")]
    public async Task<ActionResult<DashboardMensalDto>> Mensal([FromQuery] int? mes, [FromQuery] int? ano)
    {
        var hoje = DateTime.UtcNow;
        var mesConsulta = mes ?? hoje.Month;
        var anoConsulta = ano ?? hoje.Year;

        var inicio = new DateTime(anoConsulta, mesConsulta, 1, 0, 0, 0, DateTimeKind.Utc);
        var fim = inicio.AddMonths(1).AddTicks(-1);

        var producoesDoMes = await _producaoRepository.ListarPorPeriodoAsync(inicio, fim);
        var vendasDoMes = await _vendaRepository.ListarPorPeriodoAsync(inicio, fim);
        var receitas = await _receitaRepository.ListarTodasAsync();
        var nomesPorId = receitas.ToDictionary(r => r.Id, r => r.Nome);

        var lucroDoMes = vendasDoMes.Sum(v => v.LucroTotal);
        var quantidadeProduzida = producoesDoMes.Sum(p => p.QuantidadeProduzida);
        var quantidadeVendida = vendasDoMes.Sum(v => v.QuantidadeVendida);

        var vendasAgrupadas = vendasDoMes
            .GroupBy(v => v.ReceitaId)
            .Select(g => new RankingReceitaDto(
                g.Key,
                nomesPorId.TryGetValue(g.Key, out var nome) ? nome : "(receita removida)",
                g.Sum(v => v.QuantidadeVendida)
            ))
            .OrderByDescending(r => r.QuantidadeVendida)
            .ToList();

        var maisVendido = vendasAgrupadas.FirstOrDefault();
        var menosVendido = vendasAgrupadas.LastOrDefault();

        var resultado = new DashboardMensalDto(
            mesConsulta,
            anoConsulta,
            lucroDoMes,
            quantidadeProduzida,
            quantidadeVendida,
            maisVendido,
            vendasAgrupadas.Count > 1 ? menosVendido : null
        );

        return Ok(resultado);
    }
}
