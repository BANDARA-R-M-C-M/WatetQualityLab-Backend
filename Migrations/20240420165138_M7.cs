using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "21bb4472-26b3-470f-888f-bcd07f29b954");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2faf9438-fd45-4e3c-83ee-16b78ab651a0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac3b3ce2-fb20-4ad1-9df2-16866953b537");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea736e3a-8556-41ef-bb6b-bbe90a58aa19");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "11150bdc-3fe1-439d-a478-ee98992635ed", null, "Mlt", "MLT" },
                    { "d36aab81-0e3c-4b19-82e1-3161a0ba36a5", null, "Admin", "ADMIN" },
                    { "d9516ade-10e3-4fa0-9835-bfa9b4e6c13d", null, "Phi", "PHI" },
                    { "e29cca6d-390c-4ab1-9ad6-0021d456f2bb", null, "MohSupervisor", "MOH_Supervisor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "11150bdc-3fe1-439d-a478-ee98992635ed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d36aab81-0e3c-4b19-82e1-3161a0ba36a5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9516ade-10e3-4fa0-9835-bfa9b4e6c13d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e29cca6d-390c-4ab1-9ad6-0021d456f2bb");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "21bb4472-26b3-470f-888f-bcd07f29b954", null, "Admin", "ADMIN" },
                    { "2faf9438-fd45-4e3c-83ee-16b78ab651a0", null, "MohSupervisor", "MOH_Supervisor" },
                    { "ac3b3ce2-fb20-4ad1-9df2-16866953b537", null, "Mlt", "MLT" },
                    { "ea736e3a-8556-41ef-bb6b-bbe90a58aa19", null, "Phi", "PHI" }
                });
        }
    }
}
