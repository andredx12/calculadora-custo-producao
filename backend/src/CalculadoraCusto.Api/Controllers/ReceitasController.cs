namespace CalculadoraCusto.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using CalculadoraCusto.Application.DTOs;
using CalculadoraCusto.Application.Interfaces;
using CalculadoraCusto.Domain.Entities;
using CalculadoraCusto.Domain.Enums;

[ApiController]
[Route("api/[controller]")]
public class ReceitasController : ControllerBase
{
    private readonly IReceitaRepository _repository;

    public ReceitasController(IReceitaRepository repository)
    {
        _repository = repository;
    }

    // GET api/receitas
    [HttpGet]
    public async Task<ActionResult<List<ReceitaDto>>> Listar([FromQuery] bool apenasAtivas = false)
    {
        var receitas = await _repository.ListarTodasAsync(apenasAtivas);
        return Ok(receitas.Select(r => ParaDto(r)).ToList());
    }

    // GET api/receitas/busca?termo=bolo
    [HttpGet("busca")]
    public async Task<ActionResult<List<ReceitaDto>>> Buscar([FromQuery] string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return Ok(new List<ReceitaDto>());

        var receitas = await _repository.BuscarPorNomeAsync(termo);
        return Ok(receitas.Select(r => ParaDto(r)).ToList());
    }

    // GET api/receitas/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ReceitaDto>> ObterPorId(Guid id)
    {
        var receita = await _repository.ObterPorIdAsync(id);
        if (receita is null)
            return NotFound(new { mensagem = "Receita não encontrada." });

        return Ok(ParaDto(receita));
    }

    // POST api/receitas
    [HttpPost]
    public async Task<ActionResult<ReceitaDto>> Criar(CriarReceitaDto dto)
    {
        try
        {
            var receita = new Receita(
                dto.Nome,
                dto.QuantidadeProduzida,
                dto.UnidadeProduzida,
                dto.Descricao,
                dto.MargemLucroPadrao
            );

            foreach (var ing in dto.Ingredientes)
            {
                if (!Enum.TryParse<UnidadeMedida>(ing.UnidadeCompra, ignoreCase: true, out var unidadeCompra))
                    return BadRequest(new { mensagem = $"Unidade de compra inválida: {ing.UnidadeCompra}" });

                if (!Enum.TryParse<UnidadeMedida>(ing.UnidadeUtilizada, ignoreCase: true, out var unidadeUtilizada))
                    return BadRequest(new { mensagem = $"Unidade utilizada inválida: {ing.UnidadeUtilizada}" });

                var receitaIngrediente = new ReceitaIngrediente(
                    receita.Id,
                    ing.NomeIngrediente,
                    ing.QuantidadeComprada,
                    unidadeCompra,
                    ing.ValorPago,
                    ing.QuantidadeUtilizada,
                    unidadeUtilizada,
                    ing.IngredienteId,
                    ing.Ordem
                );

                receita.AdicionarIngrediente(receitaIngrediente);
            }

            await _repository.AdicionarAsync(receita);
            await _repository.SalvarAlteracoesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = receita.Id }, ParaDto(receita));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    // PUT api/receitas/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<ReceitaDto>> Atualizar(Guid id, AtualizarReceitaDto dto)
    {
        var receita = await _repository.ObterPorIdAsync(id);
        if (receita is null)
            return NotFound(new { mensagem = "Receita não encontrada." });

        try
        {
            receita.AtualizarDados(dto.Nome, dto.QuantidadeProduzida, dto.UnidadeProduzida, dto.Descricao, dto.MargemLucroPadrao);
            await _repository.AtualizarAsync(receita);
            await _repository.SalvarAlteracoesAsync();

            return Ok(ParaDto(receita));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    // DELETE api/receitas/{id} (soft delete)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Desativar(Guid id)
    {
        var receita = await _repository.ObterPorIdAsync(id);
        if (receita is null)
            return NotFound(new { mensagem = "Receita não encontrada." });

        receita.Desativar();
        await _repository.AtualizarAsync(receita);
        await _repository.SalvarAlteracoesAsync();

        return NoContent();
    }

    // POST api/receitas/{id}/ingredientes
    [HttpPost("{id}/ingredientes")]
    public async Task<ActionResult<ReceitaDto>> AdicionarIngrediente(Guid id, CriarReceitaIngredienteDto dto)
    {
        var receita = await _repository.ObterPorIdAsync(id);
        if (receita is null)
            return NotFound(new { mensagem = "Receita não encontrada." });

        if (!Enum.TryParse<UnidadeMedida>(dto.UnidadeCompra, ignoreCase: true, out var unidadeCompra))
            return BadRequest(new { mensagem = $"Unidade de compra inválida: {dto.UnidadeCompra}" });

        if (!Enum.TryParse<UnidadeMedida>(dto.UnidadeUtilizada, ignoreCase: true, out var unidadeUtilizada))
            return BadRequest(new { mensagem = $"Unidade utilizada inválida: {dto.UnidadeUtilizada}" });

        try
        {
            var novoOrdem = dto.Ordem == 0 ? receita.Ingredientes.Count : dto.Ordem;

            var receitaIngrediente = new ReceitaIngrediente(
                receita.Id,
                dto.NomeIngrediente,
                dto.QuantidadeComprada,
                unidadeCompra,
                dto.ValorPago,
                dto.QuantidadeUtilizada,
                unidadeUtilizada,
                dto.IngredienteId,
                novoOrdem
            );

            receita.AdicionarIngrediente(receitaIngrediente);
            await _repository.AtualizarAsync(receita);
            await _repository.SalvarAlteracoesAsync();

            return Ok(ParaDto(receita));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    // DELETE api/receitas/{id}/ingredientes/{receitaIngredienteId}
    [HttpDelete("{id}/ingredientes/{receitaIngredienteId}")]
    public async Task<ActionResult<ReceitaDto>> RemoverIngrediente(Guid id, Guid receitaIngredienteId)
    {
        var receita = await _repository.ObterPorIdAsync(id);
        if (receita is null)
            return NotFound(new { mensagem = "Receita não encontrada." });

        receita.RemoverIngrediente(receitaIngredienteId);
        await _repository.AtualizarAsync(receita);
        await _repository.SalvarAlteracoesAsync();

        return Ok(ParaDto(receita));
    }

    // POST api/receitas/{id}/duplicar
    [HttpPost("{id}/duplicar")]
    public async Task<ActionResult<ReceitaDto>> Duplicar(Guid id)
    {
        var original = await _repository.ObterPorIdAsync(id);
        if (original is null)
            return NotFound(new { mensagem = "Receita não encontrada." });

        var copia = new Receita(
            $"{original.Nome} (cópia)",
            original.QuantidadeProduzida,
            original.UnidadeProduzida,
            original.Descricao,
            original.MargemLucroPadrao
        );

        foreach (var ing in original.Ingredientes.OrderBy(i => i.Ordem))
        {
            var novoIngrediente = new ReceitaIngrediente(
                copia.Id,
                ing.NomeIngrediente,
                ing.QuantidadeComprada,
                ing.UnidadeCompra,
                ing.ValorPago,
                ing.QuantidadeUtilizada,
                ing.UnidadeUtilizada,
                ing.IngredienteId,
                ing.Ordem
            );
            copia.AdicionarIngrediente(novoIngrediente);
        }

        await _repository.AdicionarAsync(copia);
        await _repository.SalvarAlteracoesAsync();

        return CreatedAtAction(nameof(ObterPorId), new { id = copia.Id }, ParaDto(copia));
    }

    // POST api/receitas/{id}/simular-margem
    [HttpPost("{id}/simular-margem")]
    public async Task<ActionResult<ResumoFinanceiroDto>> SimularMargem(Guid id, SimularMargemDto dto)
    {
        var receita = await _repository.ObterPorIdAsync(id);
        if (receita is null)
            return NotFound(new { mensagem = "Receita não encontrada." });

        var resumo = new ResumoFinanceiroDto(
            receita.CustoTotal,
            receita.CustoPorUnidade,
            dto.MargemLucro,
            receita.CalcularLucroPorUnidade(dto.MargemLucro),
            receita.CalcularPrecoVenda(dto.MargemLucro)
        );

        return Ok(resumo);
    }

    private static ReceitaDto ParaDto(Receita r)
    {
        var ingredientesDto = r.Ingredientes.Select(i => new ReceitaIngredienteDto(
            i.Id,
            i.IngredienteId,
            i.NomeIngrediente,
            i.QuantidadeComprada,
            i.UnidadeCompra.ToString(),
            i.ValorPago,
            i.QuantidadeUtilizada,
            i.UnidadeUtilizada.ToString(),
            i.Ordem,
            i.CustoUnitario,
            i.CustoUtilizado
        )).OrderBy(i => i.Ordem).ToList();

        var margem = r.MargemLucroPadrao ?? 0;
        var resumo = new ResumoFinanceiroDto(
            r.CustoTotal,
            r.CustoPorUnidade,
            margem,
            r.CalcularLucroPorUnidade(),
            r.CalcularPrecoVenda()
        );

        return new ReceitaDto(
            r.Id,
            r.Nome,
            r.Descricao,
            r.QuantidadeProduzida,
            r.UnidadeProduzida,
            r.MargemLucroPadrao,
            r.Ativo,
            r.CriadoEm,
            ingredientesDto,
            resumo
        );
    }
}
