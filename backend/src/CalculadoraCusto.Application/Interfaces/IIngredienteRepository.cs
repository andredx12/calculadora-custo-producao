namespace CalculadoraCusto.Application.Interfaces;

using CalculadoraCusto.Domain.Entities;

public interface IIngredienteRepository
{
    Task<List<Ingrediente>> ListarTodosAsync(bool apenasAtivos = false);
    Task<Ingrediente?> ObterPorIdAsync(Guid id);
    Task<List<Ingrediente>> BuscarPorNomeAsync(string termo);
    Task AdicionarAsync(Ingrediente ingrediente);
    Task AtualizarAsync(Ingrediente ingrediente);
    Task<bool> SalvarAlteracoesAsync();
}
