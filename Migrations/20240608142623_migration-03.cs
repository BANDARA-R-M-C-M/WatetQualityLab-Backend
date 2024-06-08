using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations {
    /// <inheritdoc />
    public partial class migration03 : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_IssuedItems_SurgicalInventory_SurgicalInventoryID",
                table: "IssuedItems");

            migrationBuilder.AddForeignKey(
                name: "FK_IssuedItems_SurgicalInventory_SurgicalInventoryID",
                table: "IssuedItems",
                column: "SurgicalInventoryID",
                principalTable: "SurgicalInventory",
                principalColumn: "SurgicalInventoryID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_IssuedItems_SurgicalInventory_SurgicalInventoryID",
                table: "IssuedItems");

            migrationBuilder.AddForeignKey(
                name: "FK_IssuedItems_SurgicalInventory_SurgicalInventoryID",
                table: "IssuedItems",
                column: "SurgicalInventoryID",
                principalTable: "SurgicalInventory",
                principalColumn: "SurgicalInventoryID",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
