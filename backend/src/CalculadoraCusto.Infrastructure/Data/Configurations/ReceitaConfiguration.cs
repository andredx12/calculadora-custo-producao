namespace CalculadoraCusto.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CalculadoraCusto.Domain.Entities;

public class ReceitaConfiguration : IEntityTypeConfiguration<Receita>
{
    public void Configure(EntityTypeBuilder<Receita> builder)
    {
        builder.ToTable("receitas");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id).HasColumnName("id");
        builder.Property(r => r.Nome).HasColumnName("nome").HasMaxLength(150).IsRequired();
        builder.Property(r => r.Descricao).HasColumnName("descricao").HasMaxLength(500);
        builder.Property(r => r.QuantidadeProduzida).HasColumnName("quantidade_produzida").HasColumnType("numeric(10,3)").IsRequired();
        builder.Property(r => r.UnidadeProduzida).HasColumnName("unidade_produzida").HasMaxLength(30).IsRequired();
        builder.Property(r => r.MargemLucroPadrao).HasColumnName("margem_lucro_padrao").HasColumnType("numeric(5,2)");
        builder.Property(r => r.Ativo).HasColumnName("ativo").IsRequired();
        builder.Property(r => r.CriadoEm).HasColumnName("criado_em").IsRequired();
        builder.Property(r => r.AtualizadoEm).HasColumnName("atualizado_em").IsRequired();

        builder.HasMany(r => r.Ingredientes)
            .WithOne()
            .HasForeignKey(ri => ri.ReceitaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(r => r.Ingredientes).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(r => r.Nome);
        builder.HasIndex(r => r.Ativo);
    }
}
