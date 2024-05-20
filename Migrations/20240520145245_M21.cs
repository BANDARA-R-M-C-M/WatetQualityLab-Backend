using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "SurgicalCategory",
                newName: "SurgicalCategoryName");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "GeneralCategory",
                newName: "GeneralCategoryName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SurgicalCategoryName",
                table: "SurgicalCategory",
                newName: "CategoryName");

            migrationBuilder.RenameColumn(
                name: "GeneralCategoryName",
                table: "GeneralCategory",
                newName: "CategoryName");
        }
    }
}
