namespace CalculadoraCusto.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CalculadoraCusto.Domain.Entities;

public class IngredienteConfiguration : IEntityTypeConfiguration<Ingrediente>
{
    public void Configure(EntityTypeBuilder<Ingrediente> builder)
    {
        builder.ToTable("ingredientes");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id).HasColumnName("id");
        builder.Property(i => i.Nome).HasColumnName("nome").HasMaxLength(150).IsRequired();
        builder.Property(i => i.UnidadePadrao)
            .HasColumnName("unidade_padrao")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(i => i.Ativo).HasColumnName("ativo").IsRequired();
        builder.Property(i => i.CriadoEm).HasColumnName("criado_em").IsRequired();
        builder.Property(i => i.AtualizadoEm).HasColumnName("atualizado_em").IsRequired();

        builder.HasIndex(i => i.Nome);
    }
}
