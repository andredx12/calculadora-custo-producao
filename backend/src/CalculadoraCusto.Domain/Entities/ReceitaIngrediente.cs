namespace CalculadoraCusto.Domain.Entities;

using CalculadoraCusto.Domain.Enums;

public class ReceitaIngrediente
{
    public Guid Id { get; private set; }
    public Guid ReceitaId { get; private set; }
    public Guid? IngredienteId { get; private set; }
    public string NomeIngrediente { get; private set; } = string.Empty;
    public decimal QuantidadeComprada { get; private set; }
    public UnidadeMedida UnidadeCompra { get; private set; }
    public decimal ValorPago { get; private set; }
    public decimal QuantidadeUtilizada { get; private set; }
    public UnidadeMedida UnidadeUtilizada { get; private set; }
    public int Ordem { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime AtualizadoEm { get; private set; }

    private ReceitaIngrediente() { }

    public ReceitaIngrediente(
        Guid receitaId,
        string nomeIngrediente,
        decimal quantidadeComprada,
        UnidadeMedida unidadeCompra,
        decimal valorPago,
        decimal quantidadeUtilizada,
        UnidadeMedida unidadeUtilizada,
        Guid? ingredienteId = null,
        int ordem = 0)
    {
        if (string.IsNullOrWhiteSpace(nomeIngrediente))
            throw new ArgumentException("Nome do ingrediente é obrigatório.");
        if (quantidadeComprada <= 0)
            throw new ArgumentException("Quantidade comprada deve ser maior que zero.");
        if (valorPago < 0)
            throw new ArgumentException("Valor pago não pode ser negativo.");
        if (quantidadeUtilizada <= 0)
            throw new ArgumentException("Quantidade utilizada deve ser maior que zero.");
        if (!SaoUnidadesCompativeis(unidadeCompra, unidadeUtilizada))
            throw new ArgumentException($"Não é possível converter de {unidadeCompra} para {unidadeUtilizada}.");

        Id = Guid.NewGuid();
        ReceitaId = receitaId;
        IngredienteId = ingredienteId;
        NomeIngrediente = nomeIngrediente.Trim();
        QuantidadeComprada = quantidadeComprada;
        UnidadeCompra = unidadeCompra;
        ValorPago = valorPago;
        QuantidadeUtilizada = quantidadeUtilizada;
        UnidadeUtilizada = unidadeUtilizada;
        Ordem = ordem;
        CriadoEm = DateTime.UtcNow;
        AtualizadoEm = DateTime.UtcNow;
    }

    public decimal CustoUnitario => QuantidadeComprada == 0 ? 0 : ValorPago / QuantidadeComprada;

    public decimal CustoUtilizado
    {
        get
        {
            var quantidadeConvertida = ConverterParaUnidadeCompra(QuantidadeUtilizada, UnidadeUtilizada, UnidadeCompra);
            return CustoUnitario * quantidadeConvertida;
        }
    }

    private enum Grandeza { Massa, Volume, Contavel }

    private static Grandeza GrandezaDe(UnidadeMedida u) => u switch
    {
        UnidadeMedida.Kg or UnidadeMedida.G => Grandeza.Massa,
        UnidadeMedida.Litro or UnidadeMedida.Ml => Grandeza.Volume,
        _ => Grandeza.Contavel
    };

    private static bool SaoUnidadesCompativeis(UnidadeMedida a, UnidadeMedida b)
    {
        if (a == b) return true;
        var grandeza = GrandezaDe(a);
        return grandeza == GrandezaDe(b) && grandeza != Grandeza.Contavel;
    }

    private static decimal FatorBase(UnidadeMedida u) => u switch
    {
        UnidadeMedida.Kg => 1000m,
        UnidadeMedida.G => 1m,
        UnidadeMedida.Litro => 1000m,
        UnidadeMedida.Ml => 1m,
        _ => 1m
    };

    private static decimal ConverterParaUnidadeCompra(decimal quantidade, UnidadeMedida de, UnidadeMedida para)
    {
        if (de == para) return quantidade;
        return quantidade * FatorBase(de) / FatorBase(para);
    }
}
