namespace CalculadoraCusto.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using CalculadoraCusto.Domain.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Receita> Receitas => Set<Receita>();
    public DbSet<Ingrediente> Ingredientes => Set<Ingrediente>();
    public DbSet<ReceitaIngrediente> ReceitaIngredientes => Set<ReceitaIngrediente>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
