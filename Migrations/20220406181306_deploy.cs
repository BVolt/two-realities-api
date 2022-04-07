using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace two_realities.Migrations
{
    public partial class deploy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoritePairs",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    TitleOne = table.Column<string>(type: "TEXT", nullable: true),
                    YearOne = table.Column<int>(type: "INTEGER", nullable: true),
                    TitleTwo = table.Column<string>(type: "TEXT", nullable: true),
                    YearTwo = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritePairs", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "BLOB", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoritePairs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
