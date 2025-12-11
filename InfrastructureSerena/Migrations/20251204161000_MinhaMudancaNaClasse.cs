using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureSerena.Migrations
{
    /// <inheritdoc />
    public partial class MinhaMudancaNaClasse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Valor",
                table: "Ativos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Valor",
                table: "Ativos");
        }
    }
}
