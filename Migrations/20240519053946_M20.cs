using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Media",
                table: "MediaQualityControls",
                newName: "MediaId");

            migrationBuilder.RenameColumn(
                name: "Instrument",
                table: "InstrumentalQualityControls",
                newName: "InstrumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MediaId",
                table: "MediaQualityControls",
                newName: "Media");

            migrationBuilder.RenameColumn(
                name: "InstrumentId",
                table: "InstrumentalQualityControls",
                newName: "Instrument");
        }
    }
}
