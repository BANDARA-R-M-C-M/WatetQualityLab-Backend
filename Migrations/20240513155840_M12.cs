using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralInventory_Categories_CategoryID",
                table: "GeneralInventory");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "GeneralInventory",
                newName: "GeneralCategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralInventory_CategoryID",
                table: "GeneralInventory",
                newName: "IX_GeneralInventory_GeneralCategoryID");

            migrationBuilder.CreateTable(
                name: "GeneralCategory",
                columns: table => new
                {
                    GeneralCategoryID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralCategory", x => x.GeneralCategoryID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralInventory_GeneralCategory_GeneralCategoryID",
                table: "GeneralInventory",
                column: "GeneralCategoryID",
                principalTable: "GeneralCategory",
                principalColumn: "GeneralCategoryID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralInventory_GeneralCategory_GeneralCategoryID",
                table: "GeneralInventory");

            migrationBuilder.DropTable(
                name: "GeneralCategory");

            migrationBuilder.RenameColumn(
                name: "GeneralCategoryID",
                table: "GeneralInventory",
                newName: "CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralInventory_GeneralCategoryID",
                table: "GeneralInventory",
                newName: "IX_GeneralInventory_CategoryID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralInventory_Categories_CategoryID",
                table: "GeneralInventory",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
