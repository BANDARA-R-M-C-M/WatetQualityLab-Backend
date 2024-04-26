using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3380c1f8-5cef-4f29-8b98-b74dca46f176");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ae37312-225a-4df5-a0b1-807b2c172cfe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a0d2a27-55f4-4c5b-bc4f-f8c9dae91281");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8d5c59d-af20-41af-bb83-c846cf9aacb3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "R1", null, "Admin", "ADMIN" },
                    { "R2", null, "Mlt", "MLT" },
                    { "R3", null, "MohSupervisor", "MOH_Supervisor" },
                    { "R4", null, "Phi", "PHI" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "R1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "R2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "R3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "R4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3380c1f8-5cef-4f29-8b98-b74dca46f176", null, "MohSupervisor", "MOH_Supervisor" },
                    { "4ae37312-225a-4df5-a0b1-807b2c172cfe", null, "Admin", "ADMIN" },
                    { "5a0d2a27-55f4-4c5b-bc4f-f8c9dae91281", null, "Phi", "PHI" },
                    { "a8d5c59d-af20-41af-bb83-c846cf9aacb3", null, "Mlt", "MLT" }
                });
        }
    }
}
