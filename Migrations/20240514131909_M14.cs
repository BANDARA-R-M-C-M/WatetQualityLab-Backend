using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurgicalCategory",
                columns: table => new
                {
                    SurgicalCategoryID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LabId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurgicalCategory", x => x.SurgicalCategoryID);
                    table.ForeignKey(
                        name: "FK_SurgicalCategory_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "LabID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurgicalInventory",
                columns: table => new
                {
                    SurgicalInventoryID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IssuedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurgicalCategoryID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurgicalInventory", x => x.SurgicalInventoryID);
                    table.ForeignKey(
                        name: "FK_SurgicalInventory_SurgicalCategory_SurgicalCategoryID",
                        column: x => x.SurgicalCategoryID,
                        principalTable: "SurgicalCategory",
                        principalColumn: "SurgicalCategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IssuedItems",
                columns: table => new
                {
                    IssuedItemID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IssuedQuantity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IssuedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurgicalInventoryID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuedItems", x => x.IssuedItemID);
                    table.ForeignKey(
                        name: "FK_IssuedItems_SurgicalInventory_SurgicalInventoryID",
                        column: x => x.SurgicalInventoryID,
                        principalTable: "SurgicalInventory",
                        principalColumn: "SurgicalInventoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssuedItems_SurgicalInventoryID",
                table: "IssuedItems",
                column: "SurgicalInventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_SurgicalCategory_LabId",
                table: "SurgicalCategory",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_SurgicalInventory_SurgicalCategoryID",
                table: "SurgicalInventory",
                column: "SurgicalCategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssuedItems");

            migrationBuilder.DropTable(
                name: "SurgicalInventory");

            migrationBuilder.DropTable(
                name: "SurgicalCategory");
        }
    }
}
