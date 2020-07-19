using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hacka_zeenvia.Migrations
{
    public partial class DataEnvioMensagem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Data",
                table: "MensagemZAP",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "MensagemZAP");
        }
    }
}
