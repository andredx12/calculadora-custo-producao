namespace CalculadoraCusto.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CalculadoraCusto.Domain.Entities;

public class VendaConfiguration : IEntityTypeConfiguration<Venda>
{
    public void Configure(EntityTypeBuilder<Venda> builder)
    {
        builder.ToTable("vendas");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id).HasColumnName("id");
        builder.Property(v => v.ReceitaId).HasColumnName("receita_id").IsRequired();
        builder.Property(v => v.QuantidadeVendida).HasColumnName("quantidade_vendida").HasColumnType("numeric(10,3)").IsRequired();
        builder.Property(v => v.PrecoUnitarioVenda).HasColumnName("preco_unitario_venda").HasColumnType("numeric(10,2)").IsRequired();
        builder.Property(v => v.CustoUnitarioNoMomento).HasColumnName("custo_unitario_no_momento").HasColumnType("numeric(10,2)").IsRequired();
        builder.Property(v => v.DataVenda).HasColumnName("data_venda").IsRequired();
        builder.Property(v => v.CriadoEm).HasColumnName("criado_em").IsRequired();

        builder.Ignore(v => v.LucroTotal);

        builder.HasOne<Receita>()
            .WithMany()
            .HasForeignKey(v => v.ReceitaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(v => v.ReceitaId);
        builder.HasIndex(v => v.DataVenda);
    }
}
