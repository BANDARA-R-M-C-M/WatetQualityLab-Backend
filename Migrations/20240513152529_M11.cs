using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationOfInventory",
                table: "GeneralInventory");

            migrationBuilder.AddColumn<string>(
                name: "CategoryID",
                table: "GeneralInventory",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInventory_CategoryID",
                table: "GeneralInventory",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralInventory_Categories_CategoryID",
                table: "GeneralInventory",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralInventory_Categories_CategoryID",
                table: "GeneralInventory");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_GeneralInventory_CategoryID",
                table: "GeneralInventory");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "GeneralInventory");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DurationOfInventory",
                table: "GeneralInventory",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
