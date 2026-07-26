namespace CalculadoraCusto.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using CalculadoraCusto.Application.DTOs;
using CalculadoraCusto.Application.Interfaces;
using CalculadoraCusto.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class VendasController : ControllerBase
{
    private readonly IVendaRepository _vendaRepository;
    private readonly IReceitaRepository _receitaRepository;

    public VendasController(IVendaRepository vendaRepository, IReceitaRepository receitaRepository)
    {
        _vendaRepository = vendaRepository;
        _receitaRepository = receitaRepository;
    }

    // GET api/vendas
    [HttpGet]
    public async Task<ActionResult<List<VendaDto>>> Listar()
    {
        var vendas = await _vendaRepository.ListarTodasAsync();
        var receitas = await _receitaRepository.ListarTodasAsync();
        var nomesPorId = receitas.ToDictionary(r => r.Id, r => r.Nome);

        var dto = vendas.Select(v => ParaDto(v, nomesPorId)).ToList();
        return Ok(dto);
    }

    // POST api/vendas
    [HttpPost]
    public async Task<ActionResult<VendaDto>> Registrar(CriarVendaDto dto)
    {
        var receita = await _receitaRepository.ObterPorIdAsync(dto.ReceitaId);
        if (receita is null)
            return NotFound(new { mensagem = "Receita não encontrada." });

        try
        {
            // O custo e "congelado" no momento da venda, usando o custo atual da receita
            var custoAtual = receita.CustoPorUnidade;

            var venda = new Venda(dto.ReceitaId, dto.QuantidadeVendida, dto.PrecoUnitarioVenda, custoAtual, dto.DataVenda);
            await _vendaRepository.AdicionarAsync(venda);
            await _vendaRepository.SalvarAlteracoesAsync();

            var nomesPorId = new Dictionary<Guid, string> { [receita.Id] = receita.Nome };
            return Ok(ParaDto(venda, nomesPorId));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    private static VendaDto ParaDto(Venda v, Dictionary<Guid, string> nomesPorId)
    {
        var nome = nomesPorId.TryGetValue(v.ReceitaId, out var n) ? n : "(receita removida)";
        return new VendaDto(
            v.Id,
            v.ReceitaId,
            nome,
            v.QuantidadeVendida,
            v.PrecoUnitarioVenda,
            v.CustoUnitarioNoMomento,
            v.LucroTotal,
            v.DataVenda,
            v.CriadoEm
        );
    }
}
