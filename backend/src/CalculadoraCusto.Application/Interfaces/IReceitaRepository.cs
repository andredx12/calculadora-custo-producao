namespace CalculadoraCusto.Application.Interfaces;

using CalculadoraCusto.Domain.Entities;

public interface IReceitaRepository
{
    Task<List<Receita>> ListarTodasAsync(bool apenasAtivas = false);
    Task<Receita?> ObterPorIdAsync(Guid id);
    Task<List<Receita>> BuscarPorNomeAsync(string termo);
    Task AdicionarAsync(Receita receita);
    Task AtualizarAsync(Receita receita);
    Task<bool> SalvarAlteracoesAsync();
}
