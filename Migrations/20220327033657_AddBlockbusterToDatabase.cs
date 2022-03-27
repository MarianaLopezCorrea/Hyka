using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hyka.Migrations
{
    public partial class AddBlockbusterToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blockbusters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Names = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surnames = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blockbusters", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blockbusters");
        }
    }
}
