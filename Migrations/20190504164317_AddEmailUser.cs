using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace contactos.Migrations
{
    public partial class AddEmailUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreado",
                table: "Usuario",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Usuario",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaCreado",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "email",
                table: "Usuario");
        }
    }
}
