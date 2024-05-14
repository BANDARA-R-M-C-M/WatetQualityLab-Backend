using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralInventory_Labs_LabId",
                table: "GeneralInventory");

            migrationBuilder.DropIndex(
                name: "IX_GeneralInventory_LabId",
                table: "GeneralInventory");

            migrationBuilder.DropColumn(
                name: "LabId",
                table: "GeneralInventory");

            migrationBuilder.AddColumn<string>(
                name: "LabId",
                table: "GeneralCategory",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralCategory_LabId",
                table: "GeneralCategory",
                column: "LabId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralCategory_Labs_LabId",
                table: "GeneralCategory",
                column: "LabId",
                principalTable: "Labs",
                principalColumn: "LabID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralCategory_Labs_LabId",
                table: "GeneralCategory");

            migrationBuilder.DropIndex(
                name: "IX_GeneralCategory_LabId",
                table: "GeneralCategory");

            migrationBuilder.DropColumn(
                name: "LabId",
                table: "GeneralCategory");

            migrationBuilder.AddColumn<string>(
                name: "LabId",
                table: "GeneralInventory",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInventory_LabId",
                table: "GeneralInventory",
                column: "LabId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralInventory_Labs_LabId",
                table: "GeneralInventory",
                column: "LabId",
                principalTable: "Labs",
                principalColumn: "LabID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
