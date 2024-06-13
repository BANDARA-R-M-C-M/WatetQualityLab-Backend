using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project_v1.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<string>(type: "varchar(10)", nullable: false),
                    Feedback = table.Column<string>(type: "varchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                });

            migrationBuilder.CreateTable(
                name: "Labs",
                columns: table => new
                {
                    LabID = table.Column<string>(type: "varchar(40)", nullable: false),
                    LabName = table.Column<string>(type: "varchar(30)", nullable: false),
                    LabLocation = table.Column<string>(type: "varchar(50)", nullable: false),
                    LabTelephone = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labs", x => x.LabID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralCategory",
                columns: table => new
                {
                    GeneralCategoryID = table.Column<string>(type: "varchar(40)", nullable: false),
                    GeneralCategoryName = table.Column<string>(type: "varchar(30)", nullable: false),
                    LabId = table.Column<string>(type: "varchar(40)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralCategory", x => x.GeneralCategoryID);
                    table.ForeignKey(
                        name: "FK_GeneralCategory_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "LabID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentalQualityControls",
                columns: table => new
                {
                    InstrumentalQualityControlID = table.Column<string>(type: "varchar(40)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InstrumentId = table.Column<string>(type: "varchar(40)", nullable: false),
                    TemperatureFluctuation = table.Column<double>(type: "float", nullable: false),
                    PressureGradient = table.Column<double>(type: "float", nullable: false),
                    Timer = table.Column<string>(type: "varchar(40)", nullable: false),
                    Sterility = table.Column<string>(type: "varchar(40)", nullable: false),
                    Stability = table.Column<string>(type: "varchar(40)", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(150)", nullable: false),
                    MltId = table.Column<string>(type: "varchar(40)", nullable: false),
                    LabId = table.Column<string>(type: "varchar(40)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentalQualityControls", x => x.InstrumentalQualityControlID);
                    table.ForeignKey(
                        name: "FK_InstrumentalQualityControls_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "LabID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaQualityControls",
                columns: table => new
                {
                    MediaQualityControlID = table.Column<string>(type: "varchar(40)", nullable: false),
                    MediaId = table.Column<string>(type: "varchar(40)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sterility = table.Column<string>(type: "varchar(40)", nullable: false),
                    Stability = table.Column<string>(type: "varchar(40)", nullable: false),
                    Sensitivity = table.Column<string>(type: "varchar(40)", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(150)", nullable: false),
                    MltId = table.Column<string>(type: "varchar(40)", nullable: false),
                    LabId = table.Column<string>(type: "varchar(40)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaQualityControls", x => x.MediaQualityControlID);
                    table.ForeignKey(
                        name: "FK_MediaQualityControls_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "LabID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MOHAreas",
                columns: table => new
                {
                    MOHAreaID = table.Column<string>(type: "varchar(40)", nullable: false),
                    MOHAreaName = table.Column<string>(type: "varchar(30)", nullable: false),
                    LabID = table.Column<string>(type: "varchar(40)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOHAreas", x => x.MOHAreaID);
                    table.ForeignKey(
                        name: "FK_MOHAreas_Labs_LabID",
                        column: x => x.LabID,
                        principalTable: "Labs",
                        principalColumn: "LabID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurgicalCategory",
                columns: table => new
                {
                    SurgicalCategoryID = table.Column<string>(type: "varchar(40)", nullable: false),
                    SurgicalCategoryName = table.Column<string>(type: "varchar(30)", nullable: false),
                    LabId = table.Column<string>(type: "varchar(40)", nullable: false)
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
                name: "GeneralInventory",
                columns: table => new
                {
                    GeneralInventoryID = table.Column<string>(type: "varchar(40)", nullable: false),
                    ItemName = table.Column<string>(type: "varchar(30)", nullable: false),
                    IssuedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IssuedBy = table.Column<string>(type: "varchar(40)", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(150)", nullable: false),
                    GeneralCategoryID = table.Column<string>(type: "varchar(40)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralInventory", x => x.GeneralInventoryID);
                    table.ForeignKey(
                        name: "FK_GeneralInventory_GeneralCategory_GeneralCategoryID",
                        column: x => x.GeneralCategoryID,
                        principalTable: "GeneralCategory",
                        principalColumn: "GeneralCategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PHIAreas",
                columns: table => new
                {
                    PHIAreaID = table.Column<string>(type: "varchar(40)", nullable: false),
                    PHIAreaName = table.Column<string>(type: "varchar(30)", nullable: false),
                    MOHAreaId = table.Column<string>(type: "varchar(40)", nullable: false)
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
                name: "SurgicalInventory",
                columns: table => new
                {
                    SurgicalInventoryID = table.Column<string>(type: "varchar(40)", nullable: false),
                    ItemName = table.Column<string>(type: "varchar(40)", nullable: false),
                    IssuedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IssuedBy = table.Column<string>(type: "varchar(40)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(150)", nullable: false),
                    SurgicalCategoryID = table.Column<string>(type: "varchar(40)", nullable: false)
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
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageUrl = table.Column<string>(type: "varchar(250)", nullable: true),
                    LabID = table.Column<string>(type: "varchar(40)", nullable: true),
                    MOHAreaId = table.Column<string>(type: "varchar(40)", nullable: true),
                    PHIAreaId = table.Column<string>(type: "varchar(40)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Labs_LabID",
                        column: x => x.LabID,
                        principalTable: "Labs",
                        principalColumn: "LabID");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_MOHAreas_MOHAreaId",
                        column: x => x.MOHAreaId,
                        principalTable: "MOHAreas",
                        principalColumn: "MOHAreaID");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_PHIAreas_PHIAreaId",
                        column: x => x.PHIAreaId,
                        principalTable: "PHIAreas",
                        principalColumn: "PHIAreaID");
                });

            migrationBuilder.CreateTable(
                name: "Samples",
                columns: table => new
                {
                    SampleId = table.Column<string>(type: "varchar(40)", nullable: false),
                    YourRefNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    StateOfChlorination = table.Column<string>(type: "varchar(25)", nullable: false),
                    DateOfCollection = table.Column<DateOnly>(type: "date", nullable: false),
                    CatagoryOfSource = table.Column<string>(type: "varchar(40)", nullable: false),
                    CollectingSource = table.Column<string>(type: "varchar(40)", nullable: false),
                    AnalyzedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    phiAreaName = table.Column<string>(type: "varchar(40)", nullable: false),
                    Acceptance = table.Column<string>(type: "varchar(8)", nullable: false),
                    Comments = table.Column<string>(type: "varchar(70)", nullable: false),
                    PhiId = table.Column<string>(type: "varchar(40)", nullable: false),
                    PHIAreaId = table.Column<string>(type: "varchar(40)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samples", x => x.SampleId);
                    table.ForeignKey(
                        name: "FK_Samples_PHIAreas_PHIAreaId",
                        column: x => x.PHIAreaId,
                        principalTable: "PHIAreas",
                        principalColumn: "PHIAreaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IssuedItems",
                columns: table => new
                {
                    IssuedItemID = table.Column<string>(type: "varchar(40)", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    AddedQuantity = table.Column<int>(type: "int", nullable: false),
                    IssuedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IssuedBy = table.Column<string>(type: "varchar(40)", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(150)", nullable: false),
                    SurgicalInventoryID = table.Column<string>(type: "varchar(40)", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportRefId = table.Column<string>(type: "varchar(40)", nullable: false),
                    MyRefNo = table.Column<string>(type: "varchar(10)", nullable: false),
                    PresumptiveColiformCount = table.Column<int>(type: "int", nullable: false),
                    IssuedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EcoliCount = table.Column<int>(type: "int", nullable: false),
                    AppearanceOfSample = table.Column<string>(type: "varchar(40)", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(200)", nullable: false),
                    Contaminated = table.Column<bool>(type: "bit", nullable: false),
                    MltId = table.Column<string>(type: "varchar(40)", nullable: false),
                    LabId = table.Column<string>(type: "varchar(40)", nullable: false),
                    SampleId = table.Column<string>(type: "varchar(40)", nullable: false)
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
                        name: "FK_Reports_Samples_SampleId",
                        column: x => x.SampleId,
                        principalTable: "Samples",
                        principalColumn: "SampleId",
                        onDelete: ReferentialAction.NoAction);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

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
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralCategory_LabId",
                table: "GeneralCategory",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInventory_GeneralCategoryID",
                table: "GeneralInventory",
                column: "GeneralCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentalQualityControls_LabId",
                table: "InstrumentalQualityControls",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuedItems_SurgicalInventoryID",
                table: "IssuedItems",
                column: "SurgicalInventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_MediaQualityControls_LabId",
                table: "MediaQualityControls",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_MOHAreas_LabID",
                table: "MOHAreas",
                column: "LabID");

            migrationBuilder.CreateIndex(
                name: "IX_PHIAreas_MOHAreaId",
                table: "PHIAreas",
                column: "MOHAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_LabId",
                table: "Reports",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SampleId",
                table: "Reports",
                column: "SampleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Samples_PHIAreaId",
                table: "Samples",
                column: "PHIAreaId");

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "GeneralInventory");

            migrationBuilder.DropTable(
                name: "InstrumentalQualityControls");

            migrationBuilder.DropTable(
                name: "IssuedItems");

            migrationBuilder.DropTable(
                name: "MediaQualityControls");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "GeneralCategory");

            migrationBuilder.DropTable(
                name: "SurgicalInventory");

            migrationBuilder.DropTable(
                name: "Samples");

            migrationBuilder.DropTable(
                name: "SurgicalCategory");

            migrationBuilder.DropTable(
                name: "PHIAreas");

            migrationBuilder.DropTable(
                name: "MOHAreas");

            migrationBuilder.DropTable(
                name: "Labs");
        }
    }
}
