using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyRefNo",
                table: "Samples",
                newName: "YourRefNo");

            migrationBuilder.RenameColumn(
                name: "YourRefNo",
                table: "Reports",
                newName: "MyRefNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YourRefNo",
                table: "Samples",
                newName: "MyRefNo");

            migrationBuilder.RenameColumn(
                name: "MyRefNo",
                table: "Reports",
                newName: "YourRefNo");
        }
    }
}
