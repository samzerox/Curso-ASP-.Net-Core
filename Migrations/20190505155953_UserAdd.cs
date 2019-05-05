using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace contactos.Migrations
{
    public partial class UserAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    username = table.Column<string>(maxLength: 20, nullable: false),
                    fechaCreado = table.Column<DateTime>(nullable: false),
                    email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.username);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    username = table.Column<string>(maxLength: 20, nullable: false),
                    FechaCreado = table.Column<DateTime>(nullable: false),
                    email = table.Column<string>(nullable: true),
                    password = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.username);
                });
        }
    }
}
