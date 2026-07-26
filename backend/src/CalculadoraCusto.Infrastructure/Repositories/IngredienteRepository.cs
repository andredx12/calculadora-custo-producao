namespace CalculadoraCusto.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using CalculadoraCusto.Application.Interfaces;
using CalculadoraCusto.Domain.Entities;
using CalculadoraCusto.Infrastructure.Data;

public class IngredienteRepository : IIngredienteRepository
{
    private readonly AppDbContext _context;

    public IngredienteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ingrediente>> ListarTodosAsync(bool apenasAtivos = false)
    {
        var query = _context.Ingredientes.AsQueryable();

        if (apenasAtivos)
            query = query.Where(i => i.Ativo);

        return await query.OrderBy(i => i.Nome).ToListAsync();
    }

    public async Task<Ingrediente?> ObterPorIdAsync(Guid id)
    {
        return await _context.Ingredientes.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<Ingrediente>> BuscarPorNomeAsync(string termo)
    {
        return await _context.Ingredientes
            .Where(i => EF.Functions.ILike(i.Nome, $"%{termo}%"))
            .OrderBy(i => i.Nome)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Ingrediente ingrediente)
    {
        await _context.Ingredientes.AddAsync(ingrediente);
    }

    public Task AtualizarAsync(Ingrediente ingrediente)
    {
        _context.Ingredientes.Update(ingrediente);
        return Task.CompletedTask;
    }

    public async Task<bool> SalvarAlteracoesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
