namespace CalculadoraCusto.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using CalculadoraCusto.Application.Interfaces;
using CalculadoraCusto.Domain.Entities;
using CalculadoraCusto.Infrastructure.Data;

public class VendaRepository : IVendaRepository
{
    private readonly AppDbContext _context;

    public VendaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Venda>> ListarTodasAsync()
    {
        return await _context.Vendas
            .OrderByDescending(v => v.DataVenda)
            .ToListAsync();
    }

    public async Task<List<Venda>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        return await _context.Vendas
            .Where(v => v.DataVenda >= inicio && v.DataVenda <= fim)
            .OrderByDescending(v => v.DataVenda)
            .ToListAsync();
    }

    public async Task<Venda?> ObterPorIdAsync(Guid id)
    {
        return await _context.Vendas.FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task AdicionarAsync(Venda venda)
    {
        await _context.Vendas.AddAsync(venda);
    }

    public async Task<bool> SalvarAlteracoesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
