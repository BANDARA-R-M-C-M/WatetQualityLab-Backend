using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class M3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LabID",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MOHAreaId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PHIAreaId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Labs",
                columns: table => new
                {
                    LabID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Lab_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lab_location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lab_telephone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labs", x => x.LabID);
                });

            migrationBuilder.CreateTable(
                name: "MOHAreas",
                columns: table => new
                {
                    MOHAreaID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MOHArea_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOHAreas", x => x.MOHAreaID);
                });

            migrationBuilder.CreateTable(
                name: "PHIAreas",
                columns: table => new
                {
                    PHIAreaID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PHIArea_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MOHAreaId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHIAreas", x => x.PHIAreaID);
                    table.ForeignKey(
                        name: "FK_PHIAreas_MOHAreas_MOHAreaId",
                        column: x => x.MOHAreaId,
                        principalTable: "MOHAreas",
                        principalColumn: "MOHAreaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Samples",
                columns: table => new
                {
                    SampleRefId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StateOfChlorination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfCollection = table.Column<DateOnly>(type: "date", nullable: false),
                    CatagoryOfUse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollectingSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnalyzedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Phi_Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Acceptance = table.Column<bool>(type: "bit", nullable: false),
                    PHIAreaId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samples", x => x.SampleRefId);
                    table.ForeignKey(
                        name: "FK_Samples_PHIAreas_PHIAreaId",
                        column: x => x.PHIAreaId,
                        principalTable: "PHIAreas",
                        principalColumn: "PHIAreaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportRefId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PresumptiveColiformCount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EcoliCount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppearanceOfSample = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Results = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SampleRefId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LabId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SampleRefId1 = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportRefId);
                    table.ForeignKey(
                        name: "FK_Reports_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "LabID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Samples_SampleRefId1",
                        column: x => x.SampleRefId1,
                        principalTable: "Samples",
                        principalColumn: "SampleRefId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LabID",
                table: "AspNetUsers",
                column: "LabID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MOHAreaId",
                table: "AspNetUsers",
                column: "MOHAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PHIAreaId",
                table: "AspNetUsers",
                column: "PHIAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_PHIAreas_MOHAreaId",
                table: "PHIAreas",
                column: "MOHAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_LabId",
                table: "Reports",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SampleRefId1",
                table: "Reports",
                column: "SampleRefId1");

            migrationBuilder.CreateIndex(
                name: "IX_Samples_PHIAreaId",
                table: "Samples",
                column: "PHIAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Labs_LabID",
                table: "AspNetUsers",
                column: "LabID",
                principalTable: "Labs",
                principalColumn: "LabID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_MOHAreas_MOHAreaId",
                table: "AspNetUsers",
                column: "MOHAreaId",
                principalTable: "MOHAreas",
                principalColumn: "MOHAreaID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PHIAreas_PHIAreaId",
                table: "AspNetUsers",
                column: "PHIAreaId",
                principalTable: "PHIAreas",
                principalColumn: "PHIAreaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Labs_LabID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_MOHAreas_MOHAreaId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PHIAreas_PHIAreaId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Labs");

            migrationBuilder.DropTable(
                name: "Samples");

            migrationBuilder.DropTable(
                name: "PHIAreas");

            migrationBuilder.DropTable(
                name: "MOHAreas");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LabID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MOHAreaId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PHIAreaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LabID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MOHAreaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PHIAreaId",
                table: "AspNetUsers");
        }
    }
}
