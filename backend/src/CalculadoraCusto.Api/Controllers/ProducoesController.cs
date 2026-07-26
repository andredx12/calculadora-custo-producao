namespace CalculadoraCusto.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using CalculadoraCusto.Application.DTOs;
using CalculadoraCusto.Application.Interfaces;
using CalculadoraCusto.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class ProducoesController : ControllerBase
{
    private readonly IProducaoRepository _producaoRepository;
    private readonly IReceitaRepository _receitaRepository;

    public ProducoesController(IProducaoRepository producaoRepository, IReceitaRepository receitaRepository)
    {
        _producaoRepository = producaoRepository;
        _receitaRepository = receitaRepository;
    }

    // GET api/producoes
    [HttpGet]
    public async Task<ActionResult<List<ProducaoDto>>> Listar()
    {
        var producoes = await _producaoRepository.ListarTodasAsync();
        var receitas = await _receitaRepository.ListarTodasAsync();
        var nomesPorId = receitas.ToDictionary(r => r.Id, r => r.Nome);

        var dto = producoes.Select(p => ParaDto(p, nomesPorId)).ToList();
        return Ok(dto);
    }

    // POST api/producoes
    [HttpPost]
    public async Task<ActionResult<ProducaoDto>> Registrar(CriarProducaoDto dto)
    {
        var receita = await _receitaRepository.ObterPorIdAsync(dto.ReceitaId);
        if (receita is null)
            return NotFound(new { mensagem = "Receita não encontrada." });

        try
        {
            var producao = new Producao(dto.ReceitaId, dto.QuantidadeProduzida, dto.DataProducao);
            await _producaoRepository.AdicionarAsync(producao);
            await _producaoRepository.SalvarAlteracoesAsync();

            var nomesPorId = new Dictionary<Guid, string> { [receita.Id] = receita.Nome };
            return Ok(ParaDto(producao, nomesPorId));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    private static ProducaoDto ParaDto(Producao p, Dictionary<Guid, string> nomesPorId)
    {
        var nome = nomesPorId.TryGetValue(p.ReceitaId, out var n) ? n : "(receita removida)";
        return new ProducaoDto(p.Id, p.ReceitaId, nome, p.QuantidadeProduzida, p.DataProducao, p.CriadoEm);
    }
}
