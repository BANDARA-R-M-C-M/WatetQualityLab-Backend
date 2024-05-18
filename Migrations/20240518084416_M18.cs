using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstrumentalQualityControl_Labs_LabId",
                table: "InstrumentalQualityControl");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaQualityControl_Labs_LabId",
                table: "MediaQualityControl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaQualityControl",
                table: "MediaQualityControl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstrumentalQualityControl",
                table: "InstrumentalQualityControl");

            migrationBuilder.RenameTable(
                name: "MediaQualityControl",
                newName: "MediaQualityControls");

            migrationBuilder.RenameTable(
                name: "InstrumentalQualityControl",
                newName: "InstrumentalQualityControls");

            migrationBuilder.RenameIndex(
                name: "IX_MediaQualityControl_LabId",
                table: "MediaQualityControls",
                newName: "IX_MediaQualityControls_LabId");

            migrationBuilder.RenameIndex(
                name: "IX_InstrumentalQualityControl_LabId",
                table: "InstrumentalQualityControls",
                newName: "IX_InstrumentalQualityControls_LabId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaQualityControls",
                table: "MediaQualityControls",
                column: "MediaQualityControlID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstrumentalQualityControls",
                table: "InstrumentalQualityControls",
                column: "InstrumentalQualityControlID");

            migrationBuilder.AddForeignKey(
                name: "FK_InstrumentalQualityControls_Labs_LabId",
                table: "InstrumentalQualityControls",
                column: "LabId",
                principalTable: "Labs",
                principalColumn: "LabID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaQualityControls_Labs_LabId",
                table: "MediaQualityControls",
                column: "LabId",
                principalTable: "Labs",
                principalColumn: "LabID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstrumentalQualityControls_Labs_LabId",
                table: "InstrumentalQualityControls");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaQualityControls_Labs_LabId",
                table: "MediaQualityControls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaQualityControls",
                table: "MediaQualityControls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstrumentalQualityControls",
                table: "InstrumentalQualityControls");

            migrationBuilder.RenameTable(
                name: "MediaQualityControls",
                newName: "MediaQualityControl");

            migrationBuilder.RenameTable(
                name: "InstrumentalQualityControls",
                newName: "InstrumentalQualityControl");

            migrationBuilder.RenameIndex(
                name: "IX_MediaQualityControls_LabId",
                table: "MediaQualityControl",
                newName: "IX_MediaQualityControl_LabId");

            migrationBuilder.RenameIndex(
                name: "IX_InstrumentalQualityControls_LabId",
                table: "InstrumentalQualityControl",
                newName: "IX_InstrumentalQualityControl_LabId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaQualityControl",
                table: "MediaQualityControl",
                column: "MediaQualityControlID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstrumentalQualityControl",
                table: "InstrumentalQualityControl",
                column: "InstrumentalQualityControlID");

            migrationBuilder.AddForeignKey(
                name: "FK_InstrumentalQualityControl_Labs_LabId",
                table: "InstrumentalQualityControl",
                column: "LabId",
                principalTable: "Labs",
                principalColumn: "LabID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaQualityControl_Labs_LabId",
                table: "MediaQualityControl",
                column: "LabId",
                principalTable: "Labs",
                principalColumn: "LabID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
