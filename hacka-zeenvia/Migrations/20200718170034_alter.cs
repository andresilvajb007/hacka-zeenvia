using Microsoft.EntityFrameworkCore.Migrations;

namespace hacka_zeenvia.Migrations
{
    public partial class alter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Produto_Feirante_FeiranteId",
            //    table: "Produto");

            //migrationBuilder.AlterColumn<int>(
            //    name: "FeiranteId",
            //    table: "Produto",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "integer");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Produto_Feirante_FeiranteId",
            //    table: "Produto",
            //    column: "FeiranteId",
            //    principalTable: "Feirante",
            //    principalColumn: "FeiranteId",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produto_Feirante_FeiranteId",
                table: "Produto");

            migrationBuilder.AlterColumn<int>(
                name: "FeiranteId",
                table: "Produto",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Feirante_FeiranteId",
                table: "Produto",
                column: "FeiranteId",
                principalTable: "Feirante",
                principalColumn: "FeiranteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
