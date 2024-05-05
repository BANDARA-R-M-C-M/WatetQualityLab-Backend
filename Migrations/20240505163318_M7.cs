using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lab_telephone",
                table: "Labs",
                newName: "LabTelephone");

            migrationBuilder.RenameColumn(
                name: "Lab_name",
                table: "Labs",
                newName: "LabName");

            migrationBuilder.RenameColumn(
                name: "Lab_location",
                table: "Labs",
                newName: "LabLocation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LabTelephone",
                table: "Labs",
                newName: "Lab_telephone");

            migrationBuilder.RenameColumn(
                name: "LabName",
                table: "Labs",
                newName: "Lab_name");

            migrationBuilder.RenameColumn(
                name: "LabLocation",
                table: "Labs",
                newName: "Lab_location");
        }
    }
}
