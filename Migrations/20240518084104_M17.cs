using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstrumentalQualityControl",
                columns: table => new
                {
                    InstrumentalQualityControlID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Instrument = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemperatureFluctuation = table.Column<double>(type: "float", nullable: false),
                    PressureGradient = table.Column<double>(type: "float", nullable: false),
                    Timer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sterility = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stability = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LabId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentalQualityControl", x => x.InstrumentalQualityControlID);
                    table.ForeignKey(
                        name: "FK_InstrumentalQualityControl_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "LabID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MediaQualityControl",
                columns: table => new
                {
                    MediaQualityControlID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Media = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sterility = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stability = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sensitivity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LabId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaQualityControl", x => x.MediaQualityControlID);
                    table.ForeignKey(
                        name: "FK_MediaQualityControl_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "LabID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentalQualityControl_LabId",
                table: "InstrumentalQualityControl",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaQualityControl_LabId",
                table: "MediaQualityControl",
                column: "LabId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstrumentalQualityControl");

            migrationBuilder.DropTable(
                name: "MediaQualityControl");
        }
    }
}
