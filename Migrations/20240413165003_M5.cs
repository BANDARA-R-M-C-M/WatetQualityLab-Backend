using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "33ac6f4c-73c9-4032-8add-3ad2c7e496b8", null, "Mlt", "MLT" },
                    { "5d0d5bb6-77e1-45e1-8ff4-c1fb2d3342db", null, "MohSupervisor", "MOH_Supervisor" },
                    { "96d6a490-0c3d-4f7c-8d86-004b7cef5804", null, "Phi", "PHI" },
                    { "973dd9ff-c60d-45c1-961f-53e68c4ac1ef", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33ac6f4c-73c9-4032-8add-3ad2c7e496b8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5d0d5bb6-77e1-45e1-8ff4-c1fb2d3342db");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "96d6a490-0c3d-4f7c-8d86-004b7cef5804");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "973dd9ff-c60d-45c1-961f-53e68c4ac1ef");
        }
    }
}
