namespace CalculadoraCusto.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CalculadoraCusto.Domain.Entities;

public class ReceitaIngredienteConfiguration : IEntityTypeConfiguration<ReceitaIngrediente>
{
    public void Configure(EntityTypeBuilder<ReceitaIngrediente> builder)
    {
        builder.ToTable("receita_ingredientes");
        builder.HasKey(ri => ri.Id);

        builder.Property(ri => ri.Id).HasColumnName("id");
        builder.Property(ri => ri.ReceitaId).HasColumnName("receita_id").IsRequired();
        builder.Property(ri => ri.IngredienteId).HasColumnName("ingrediente_id");
        builder.Property(ri => ri.NomeIngrediente).HasColumnName("nome_ingrediente").HasMaxLength(150).IsRequired();
        builder.Property(ri => ri.QuantidadeComprada).HasColumnName("quantidade_comprada").HasColumnType("numeric(10,3)").IsRequired();
        builder.Property(ri => ri.UnidadeCompra).HasColumnName("unidade_compra").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(ri => ri.ValorPago).HasColumnName("valor_pago").HasColumnType("numeric(10,2)").IsRequired();
        builder.Property(ri => ri.QuantidadeUtilizada).HasColumnName("quantidade_utilizada").HasColumnType("numeric(10,3)").IsRequired();
        builder.Property(ri => ri.UnidadeUtilizada).HasColumnName("unidade_utilizada").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(ri => ri.Ordem).HasColumnName("ordem").IsRequired();
        builder.Property(ri => ri.CriadoEm).HasColumnName("criado_em").IsRequired();
        builder.Property(ri => ri.AtualizadoEm).HasColumnName("atualizado_em").IsRequired();

        builder.HasOne<Ingrediente>()
            .WithMany()
            .HasForeignKey(ri => ri.IngredienteId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Ignore(ri => ri.CustoUnitario);
        builder.Ignore(ri => ri.CustoUtilizado);

        builder.HasIndex(ri => ri.ReceitaId);
        builder.HasIndex(ri => ri.IngredienteId);
    }
}
