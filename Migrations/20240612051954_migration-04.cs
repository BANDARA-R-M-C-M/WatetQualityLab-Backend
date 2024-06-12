using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class migration04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemQR",
                table: "SurgicalInventory");

            migrationBuilder.DropColumn(
                name: "ReportUrl",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ItemQR",
                table: "GeneralInventory");

            migrationBuilder.AlterColumn<string>(
                name: "StateOfChlorination",
                table: "Samples",
                type: "varchar(25)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemQR",
                table: "SurgicalInventory",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "StateOfChlorination",
                table: "Samples",
                type: "varchar(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(25)");

            migrationBuilder.AddColumn<string>(
                name: "ReportUrl",
                table: "Reports",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ItemQR",
                table: "GeneralInventory",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");
        }
    }
}
