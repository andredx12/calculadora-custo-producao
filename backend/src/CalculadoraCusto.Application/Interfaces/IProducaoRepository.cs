namespace CalculadoraCusto.Application.Interfaces;

using CalculadoraCusto.Domain.Entities;

public interface IProducaoRepository
{
    Task<List<Producao>> ListarTodasAsync();
    Task<List<Producao>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim);
    Task<Producao?> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(Producao producao);
    Task<bool> SalvarAlteracoesAsync();
}
