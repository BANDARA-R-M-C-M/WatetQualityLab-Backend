using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Results",
                table: "Reports",
                newName: "PCResults");

            migrationBuilder.RenameColumn(
                name: "MOHArea_name",
                table: "MOHAreas",
                newName: "MOHAreaName");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Samples",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhiId",
                table: "Samples",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ECResults",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MltId",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Samples");

            migrationBuilder.DropColumn(
                name: "PhiId",
                table: "Samples");

            migrationBuilder.DropColumn(
                name: "ECResults",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "MltId",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "PCResults",
                table: "Reports",
                newName: "Results");

            migrationBuilder.RenameColumn(
                name: "MOHAreaName",
                table: "MOHAreas",
                newName: "MOHArea_name");
        }
    }
}
