namespace CalculadoraCusto.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using CalculadoraCusto.Application.Interfaces;
using CalculadoraCusto.Domain.Entities;
using CalculadoraCusto.Infrastructure.Data;

public class ReceitaRepository : IReceitaRepository
{
    private readonly AppDbContext _context;

    public ReceitaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Receita>> ListarTodasAsync(bool apenasAtivas = false)
    {
        var query = _context.Receitas
            .Include(r => r.Ingredientes)
            .AsQueryable();

        if (apenasAtivas)
            query = query.Where(r => r.Ativo);

        return await query.OrderBy(r => r.Nome).ToListAsync();
    }

    public async Task<Receita?> ObterPorIdAsync(Guid id)
    {
        return await _context.Receitas
            .Include(r => r.Ingredientes)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Receita>> BuscarPorNomeAsync(string termo)
    {
        return await _context.Receitas
            .Include(r => r.Ingredientes)
            .Where(r => EF.Functions.ILike(r.Nome, $"%{termo}%"))
            .OrderBy(r => r.Nome)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Receita receita)
    {
        await _context.Receitas.AddAsync(receita);
    }

    public Task AtualizarAsync(Receita receita)
    {
        _context.Receitas.Update(receita);
        return Task.CompletedTask;
    }

    public async Task<bool> SalvarAlteracoesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
