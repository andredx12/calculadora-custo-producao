namespace CalculadoraCusto.Domain.Entities;

public class Venda
{
    public Guid Id { get; private set; }
    public Guid ReceitaId { get; private set; }
    public decimal QuantidadeVendida { get; private set; }
    public decimal PrecoUnitarioVenda { get; private set; }
    public decimal CustoUnitarioNoMomento { get; private set; }
    public DateTime DataVenda { get; private set; }
    public DateTime CriadoEm { get; private set; }

    private Venda() { }

    public Venda(Guid receitaId, decimal quantidadeVendida, decimal precoUnitarioVenda, decimal custoUnitarioNoMomento, DateTime dataVenda)
    {
        if (quantidadeVendida <= 0)
            throw new ArgumentException("Quantidade vendida deve ser maior que zero.");
        if (precoUnitarioVenda < 0)
            throw new ArgumentException("Preço de venda não pode ser negativo.");
        if (custoUnitarioNoMomento < 0)
            throw new ArgumentException("Custo não pode ser negativo.");

        Id = Guid.NewGuid();
        ReceitaId = receitaId;
        QuantidadeVendida = quantidadeVendida;
        PrecoUnitarioVenda = precoUnitarioVenda;
        CustoUnitarioNoMomento = custoUnitarioNoMomento;
        DataVenda = DateTime.SpecifyKind(dataVenda, DateTimeKind.Utc);
        CriadoEm = DateTime.UtcNow;
    }

    // Lucro dessa venda especifica (preco - custo) x quantidade
    public decimal LucroTotal => (PrecoUnitarioVenda - CustoUnitarioNoMomento) * QuantidadeVendida;
}
