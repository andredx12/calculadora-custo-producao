namespace CalculadoraCusto.Domain.Entities;

public class Producao
{
    public Guid Id { get; private set; }
    public Guid ReceitaId { get; private set; }
    public decimal QuantidadeProduzida { get; private set; }
    public DateTime DataProducao { get; private set; }
    public DateTime CriadoEm { get; private set; }

    private Producao() { }

    public Producao(Guid receitaId, decimal quantidadeProduzida, DateTime dataProducao)
    {
        if (quantidadeProduzida <= 0)
            throw new ArgumentException("Quantidade produzida deve ser maior que zero.");

        Id = Guid.NewGuid();
        ReceitaId = receitaId;
        QuantidadeProduzida = quantidadeProduzida;
        DataProducao = dataProducao;
        CriadoEm = DateTime.UtcNow;
    }
}
