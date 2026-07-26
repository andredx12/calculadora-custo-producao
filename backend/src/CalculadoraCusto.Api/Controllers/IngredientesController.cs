namespace CalculadoraCusto.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using CalculadoraCusto.Application.DTOs;
using CalculadoraCusto.Application.Interfaces;
using CalculadoraCusto.Domain.Entities;
using CalculadoraCusto.Domain.Enums;

[ApiController]
[Route("api/[controller]")]
public class IngredientesController : ControllerBase
{
    private readonly IIngredienteRepository _repository;

    public IngredientesController(IIngredienteRepository repository)
    {
        _repository = repository;
    }

    // GET api/ingredientes
    [HttpGet]
    public async Task<ActionResult<List<IngredienteDto>>> Listar([FromQuery] bool apenasAtivos = false)
    {
        var ingredientes = await _repository.ListarTodosAsync(apenasAtivos);
        return Ok(ingredientes.Select(ParaDto).ToList());
    }

    // GET api/ingredientes/busca?termo=creme
    [HttpGet("busca")]
    public async Task<ActionResult<List<IngredienteDto>>> Buscar([FromQuery] string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return Ok(new List<IngredienteDto>());

        var ingredientes = await _repository.BuscarPorNomeAsync(termo);
        return Ok(ingredientes.Select(ParaDto).ToList());
    }

    // GET api/ingredientes/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<IngredienteDto>> ObterPorId(Guid id)
    {
        var ingrediente = await _repository.ObterPorIdAsync(id);
        if (ingrediente is null)
            return NotFound(new { mensagem = "Ingrediente não encontrado." });

        return Ok(ParaDto(ingrediente));
    }

    // POST api/ingredientes
    [HttpPost]
    public async Task<ActionResult<IngredienteDto>> Criar(CriarIngredienteDto dto)
    {
        if (!Enum.TryParse<UnidadeMedida>(dto.UnidadePadrao, ignoreCase: true, out var unidade))
            return BadRequest(new { mensagem = $"Unidade de medida inválida: {dto.UnidadePadrao}" });

        try
        {
            var ingrediente = new Ingrediente(dto.Nome, unidade);
            await _repository.AdicionarAsync(ingrediente);
            await _repository.SalvarAlteracoesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = ingrediente.Id }, ParaDto(ingrediente));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    // PUT api/ingredientes/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<IngredienteDto>> Atualizar(Guid id, AtualizarIngredienteDto dto)
    {
        var ingrediente = await _repository.ObterPorIdAsync(id);
        if (ingrediente is null)
            return NotFound(new { mensagem = "Ingrediente não encontrado." });

        try
        {
            ingrediente.AtualizarNome(dto.Nome);
            await _repository.AtualizarAsync(ingrediente);
            await _repository.SalvarAlteracoesAsync();

            return Ok(ParaDto(ingrediente));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    // DELETE api/ingredientes/{id}  (soft delete - so desativa)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Desativar(Guid id)
    {
        var ingrediente = await _repository.ObterPorIdAsync(id);
        if (ingrediente is null)
            return NotFound(new { mensagem = "Ingrediente não encontrado." });

        ingrediente.Desativar();
        await _repository.AtualizarAsync(ingrediente);
        await _repository.SalvarAlteracoesAsync();

        return NoContent();
    }

    private static IngredienteDto ParaDto(Ingrediente i) => new(
        i.Id,
        i.Nome,
        i.UnidadePadrao.ToString(),
        i.Ativo,
        i.CriadoEm
    );
}
