using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "LabID",
                table: "PHIAreas",
                type: "nvarchar(450)",
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

            migrationBuilder.CreateIndex(
                name: "IX_PHIAreas_LabID",
                table: "PHIAreas",
                column: "LabID");

            migrationBuilder.AddForeignKey(
                name: "FK_PHIAreas_Labs_LabID",
                table: "PHIAreas",
                column: "LabID",
                principalTable: "Labs",
                principalColumn: "LabID",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PHIAreas_Labs_LabID",
                table: "PHIAreas");

            migrationBuilder.DropIndex(
                name: "IX_PHIAreas_LabID",
                table: "PHIAreas");

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
                name: "LabID",
                table: "PHIAreas");

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
    }
}
