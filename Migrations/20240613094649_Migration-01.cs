using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class Migration01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LabName",
                table: "Labs",
                type: "varchar(40)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)");

            migrationBuilder.AlterColumn<string>(
                name: "LabID",
                table: "Labs",
                type: "varchar(40)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LabName",
                table: "Labs",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(40)");

            migrationBuilder.AlterColumn<string>(
                name: "LabID",
                table: "Labs",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(40)");
        }
    }
}
