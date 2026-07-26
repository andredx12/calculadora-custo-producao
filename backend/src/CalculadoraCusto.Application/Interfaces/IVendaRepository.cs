namespace CalculadoraCusto.Application.Interfaces;

using CalculadoraCusto.Domain.Entities;

public interface IVendaRepository
{
    Task<List<Venda>> ListarTodasAsync();
    Task<List<Venda>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim);
    Task<Venda?> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(Venda venda);
    Task<bool> SalvarAlteracoesAsync();
}
