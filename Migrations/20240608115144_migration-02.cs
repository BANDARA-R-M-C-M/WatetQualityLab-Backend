using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class migration02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddedQuantity",
                table: "IssuedItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedQuantity",
                table: "IssuedItems");
        }
    }
}
