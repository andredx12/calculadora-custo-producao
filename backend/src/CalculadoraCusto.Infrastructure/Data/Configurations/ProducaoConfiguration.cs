namespace CalculadoraCusto.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CalculadoraCusto.Domain.Entities;

public class ProducaoConfiguration : IEntityTypeConfiguration<Producao>
{
    public void Configure(EntityTypeBuilder<Producao> builder)
    {
        builder.ToTable("producoes");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("id");
        builder.Property(p => p.ReceitaId).HasColumnName("receita_id").IsRequired();
        builder.Property(p => p.QuantidadeProduzida).HasColumnName("quantidade_produzida").HasColumnType("numeric(10,3)").IsRequired();
        builder.Property(p => p.DataProducao).HasColumnName("data_producao").IsRequired();
        builder.Property(p => p.CriadoEm).HasColumnName("criado_em").IsRequired();

        builder.HasOne<Receita>()
            .WithMany()
            .HasForeignKey(p => p.ReceitaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.ReceitaId);
        builder.HasIndex(p => p.DataProducao);
    }
}
