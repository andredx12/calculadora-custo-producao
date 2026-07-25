namespace CalculadoraCusto.Domain.Entities;

using CalculadoraCusto.Domain.Enums;

public class Ingrediente
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public UnidadeMedida UnidadePadrao { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime AtualizadoEm { get; private set; }

    private Ingrediente() { } // uso exclusivo do EF Core

    public Ingrediente(string nome, UnidadeMedida unidadePadrao)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do ingrediente é obrigatório.", nameof(nome));

        Id = Guid.NewGuid();
        Nome = nome.Trim();
        UnidadePadrao = unidadePadrao;
        Ativo = true;
        CriadoEm = DateTime.UtcNow;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void AtualizarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do ingrediente é obrigatório.", nameof(nome));

        Nome = nome.Trim();
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Desativar()
    {
        Ativo = false;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Ativar()
    {
        Ativo = true;
        AtualizadoEm = DateTime.UtcNow;
    }
}
