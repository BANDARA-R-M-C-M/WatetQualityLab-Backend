using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0cf57763-09fd-43db-936e-8675e77304a9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "42d2dd0b-f793-4551-8e65-5b045e347689");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6d41fe7-3661-4695-b5d8-5bf079514820");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c55434f0-74fc-4a75-8cf1-a1fc953e2728");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "0cf57763-09fd-43db-936e-8675e77304a9", null, "Phi", "PHI" },
                    { "42d2dd0b-f793-4551-8e65-5b045e347689", null, "MohSupervisor", "MOH_Supervisor" },
                    { "a6d41fe7-3661-4695-b5d8-5bf079514820", null, "Mlt", "MLT" },
                    { "c55434f0-74fc-4a75-8cf1-a1fc953e2728", null, "Admin", "ADMIN" }
                });
        }
    }
}
