namespace CalculadoraCusto.Domain.Entities;

public class Receita
{
    private readonly List<ReceitaIngrediente> _ingredientes = new();

    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public decimal QuantidadeProduzida { get; private set; }
    public string UnidadeProduzida { get; private set; } = "unidade";
    public decimal? MargemLucroPadrao { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime AtualizadoEm { get; private set; }

    public IReadOnlyCollection<ReceitaIngrediente> Ingredientes => _ingredientes.AsReadOnly();

    private Receita() { }

    public Receita(string nome, decimal quantidadeProduzida, string unidadeProduzida = "unidade", string? descricao = null, decimal? margemLucroPadrao = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome da receita é obrigatório.");
        if (quantidadeProduzida <= 0)
            throw new ArgumentException("Quantidade produzida deve ser maior que zero.");

        Id = Guid.NewGuid();
        Nome = nome.Trim();
        Descricao = descricao;
        QuantidadeProduzida = quantidadeProduzida;
        UnidadeProduzida = unidadeProduzida;
        MargemLucroPadrao = margemLucroPadrao;
        Ativo = true;
        CriadoEm = DateTime.UtcNow;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void AdicionarIngrediente(ReceitaIngrediente ingrediente)
    {
        _ingredientes.Add(ingrediente);
        AtualizadoEm = DateTime.UtcNow;
    }

    public void RemoverIngrediente(Guid receitaIngredienteId)
    {
        var item = _ingredientes.FirstOrDefault(i => i.Id == receitaIngredienteId);
        if (item is not null)
        {
            _ingredientes.Remove(item);
            AtualizadoEm = DateTime.UtcNow;
        }
    }

    public decimal CustoTotal => _ingredientes.Sum(i => i.CustoUtilizado);

    public decimal CustoPorUnidade => QuantidadeProduzida == 0 ? 0 : CustoTotal / QuantidadeProduzida;

    public decimal CalcularPrecoVenda(decimal? margemLucro = null)
    {
        var margem = margemLucro ?? MargemLucroPadrao ?? 0;
        return CustoPorUnidade * (1 + margem / 100);
    }

    public decimal CalcularLucroPorUnidade(decimal? margemLucro = null)
        => CalcularPrecoVenda(margemLucro) - CustoPorUnidade;
}
