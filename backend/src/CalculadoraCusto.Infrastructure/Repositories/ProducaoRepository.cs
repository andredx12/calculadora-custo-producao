namespace CalculadoraCusto.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using CalculadoraCusto.Application.Interfaces;
using CalculadoraCusto.Domain.Entities;
using CalculadoraCusto.Infrastructure.Data;

public class ProducaoRepository : IProducaoRepository
{
    private readonly AppDbContext _context;

    public ProducaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Producao>> ListarTodasAsync()
    {
        return await _context.Producoes
            .OrderByDescending(p => p.DataProducao)
            .ToListAsync();
    }

    public async Task<List<Producao>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        return await _context.Producoes
            .Where(p => p.DataProducao >= inicio && p.DataProducao <= fim)
            .OrderByDescending(p => p.DataProducao)
            .ToListAsync();
    }

    public async Task<Producao?> ObterPorIdAsync(Guid id)
    {
        return await _context.Producoes.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AdicionarAsync(Producao producao)
    {
        await _context.Producoes.AddAsync(producao);
    }

    public async Task<bool> SalvarAlteracoesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
