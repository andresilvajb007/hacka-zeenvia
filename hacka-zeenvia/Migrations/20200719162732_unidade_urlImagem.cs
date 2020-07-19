using Microsoft.EntityFrameworkCore.Migrations;

namespace hacka_zeenvia.Migrations
{
    public partial class unidade_urlImagem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Unidade",
                table: "Produto",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlImagem",
                table: "Produto",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unidade",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "UrlImagem",
                table: "Produto");
        }
    }
}
