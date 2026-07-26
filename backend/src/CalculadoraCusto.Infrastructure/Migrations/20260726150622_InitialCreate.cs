using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalculadoraCusto.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ingredientes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    unidade_padrao = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ativo = table.Column<bool>(type: "boolean", nullable: false),
                    criado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    atualizado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredientes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "receitas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    quantidade_produzida = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    unidade_produzida = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    margem_lucro_padrao = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    ativo = table.Column<bool>(type: "boolean", nullable: false),
                    criado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    atualizado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receitas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "receita_ingredientes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    receita_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ingrediente_id = table.Column<Guid>(type: "uuid", nullable: true),
                    nome_ingrediente = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    quantidade_comprada = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    unidade_compra = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    valor_pago = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    quantidade_utilizada = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    unidade_utilizada = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ordem = table.Column<int>(type: "integer", nullable: false),
                    criado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    atualizado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receita_ingredientes", x => x.id);
                    table.ForeignKey(
                        name: "FK_receita_ingredientes_ingredientes_ingrediente_id",
                        column: x => x.ingrediente_id,
                        principalTable: "ingredientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_receita_ingredientes_receitas_receita_id",
                        column: x => x.receita_id,
                        principalTable: "receitas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ingredientes_nome",
                table: "ingredientes",
                column: "nome");

            migrationBuilder.CreateIndex(
                name: "IX_receita_ingredientes_ingrediente_id",
                table: "receita_ingredientes",
                column: "ingrediente_id");

            migrationBuilder.CreateIndex(
                name: "IX_receita_ingredientes_receita_id",
                table: "receita_ingredientes",
                column: "receita_id");

            migrationBuilder.CreateIndex(
                name: "IX_receitas_ativo",
                table: "receitas",
                column: "ativo");

            migrationBuilder.CreateIndex(
                name: "IX_receitas_nome",
                table: "receitas",
                column: "nome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "receita_ingredientes");

            migrationBuilder.DropTable(
                name: "ingredientes");

            migrationBuilder.DropTable(
                name: "receitas");
        }
    }
}
