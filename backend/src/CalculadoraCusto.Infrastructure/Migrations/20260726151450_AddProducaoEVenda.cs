using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalculadoraCusto.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProducaoEVenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "producoes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    receita_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantidade_produzida = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    data_producao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    criado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_producoes", x => x.id);
                    table.ForeignKey(
                        name: "FK_producoes_receitas_receita_id",
                        column: x => x.receita_id,
                        principalTable: "receitas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vendas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    receita_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantidade_vendida = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    preco_unitario_venda = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    custo_unitario_no_momento = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    data_venda = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    criado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendas", x => x.id);
                    table.ForeignKey(
                        name: "FK_vendas_receitas_receita_id",
                        column: x => x.receita_id,
                        principalTable: "receitas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_producoes_data_producao",
                table: "producoes",
                column: "data_producao");

            migrationBuilder.CreateIndex(
                name: "IX_producoes_receita_id",
                table: "producoes",
                column: "receita_id");

            migrationBuilder.CreateIndex(
                name: "IX_vendas_data_venda",
                table: "vendas",
                column: "data_venda");

            migrationBuilder.CreateIndex(
                name: "IX_vendas_receita_id",
                table: "vendas",
                column: "receita_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "producoes");

            migrationBuilder.DropTable(
                name: "vendas");
        }
    }
}
