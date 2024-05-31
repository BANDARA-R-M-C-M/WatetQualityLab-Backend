﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Project_v1.Data;

#nullable disable

namespace Project_v1.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20240531172259_M24")]
    partial class M24
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "R1",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "R2",
                            Name = "Mlt",
                            NormalizedName = "MLT"
                        },
                        new
                        {
                            Id = "R3",
                            Name = "MohSupervisor",
                            NormalizedName = "MOH_Supervisor"
                        },
                        new
                        {
                            Id = "R4",
                            Name = "Phi",
                            NormalizedName = "PHI"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Project_v1.Models.GeneralCategory", b =>
                {
                    b.Property<string>("GeneralCategoryID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GeneralCategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LabId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("GeneralCategoryID");

                    b.HasIndex("LabId");

                    b.ToTable("GeneralCategory");
                });

            modelBuilder.Entity("Project_v1.Models.GeneralInventory", b =>
                {
                    b.Property<string>("GeneralInventoryID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GeneralCategoryID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IssuedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("IssuedDate")
                        .HasColumnType("date");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemQR")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GeneralInventoryID");

                    b.HasIndex("GeneralCategoryID");

                    b.ToTable("GeneralInventory");
                });

            modelBuilder.Entity("Project_v1.Models.InstrumentalQualityControl", b =>
                {
                    b.Property<string>("InstrumentalQualityControlID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("InstrumentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LabId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MltId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("PressureGradient")
                        .HasColumnType("float");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Stability")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sterility")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TemperatureFluctuation")
                        .HasColumnType("float");

                    b.Property<string>("Timer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("InstrumentalQualityControlID");

                    b.HasIndex("LabId");

                    b.ToTable("InstrumentalQualityControls");
                });

            modelBuilder.Entity("Project_v1.Models.IssuedItem", b =>
                {
                    b.Property<string>("IssuedItemID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IssuedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("IssuedDate")
                        .HasColumnType("date");

                    b.Property<int>("IssuedQuantity")
                        .HasColumnType("int");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SurgicalInventoryID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IssuedItemID");

                    b.HasIndex("SurgicalInventoryID");

                    b.ToTable("IssuedItems");
                });

            modelBuilder.Entity("Project_v1.Models.Lab", b =>
                {
                    b.Property<string>("LabID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LabLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LabName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LabTelephone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LabID");

                    b.ToTable("Labs");
                });

            modelBuilder.Entity("Project_v1.Models.MOHArea", b =>
                {
                    b.Property<string>("MOHAreaID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LabID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MOHAreaName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MOHAreaID");

                    b.HasIndex("LabID");

                    b.ToTable("MOHAreas");
                });

            modelBuilder.Entity("Project_v1.Models.MediaQualityControl", b =>
                {
                    b.Property<string>("MediaQualityControlID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("LabId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MediaId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MltId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sensitivity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Stability")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sterility")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MediaQualityControlID");

                    b.HasIndex("LabId");

                    b.ToTable("MediaQualityControls");
                });

            modelBuilder.Entity("Project_v1.Models.PHIArea", b =>
                {
                    b.Property<string>("PHIAreaID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MOHAreaId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PHIAreaName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PHIAreaID");

                    b.HasIndex("MOHAreaId");

                    b.ToTable("PHIAreas");
                });

            modelBuilder.Entity("Project_v1.Models.Report", b =>
                {
                    b.Property<string>("ReportRefId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AppearanceOfSample")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Contaminated")
                        .HasColumnType("bit");

                    b.Property<string>("EcoliCount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("IssuedDate")
                        .HasColumnType("date");

                    b.Property<string>("LabId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MltId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyRefNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PresumptiveColiformCount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReportUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SampleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ReportRefId");

                    b.HasIndex("LabId");

                    b.HasIndex("SampleId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Project_v1.Models.Sample", b =>
                {
                    b.Property<string>("SampleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Acceptance")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("AnalyzedDate")
                        .HasColumnType("date");

                    b.Property<string>("CatagoryOfSource")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CollectingSource")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("DateOfCollection")
                        .HasColumnType("date");

                    b.Property<string>("PHIAreaId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PhiId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StateOfChlorination")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("YourRefNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phiAreaName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SampleId");

                    b.HasIndex("PHIAreaId");

                    b.ToTable("Samples");
                });

            modelBuilder.Entity("Project_v1.Models.SurgicalCategory", b =>
                {
                    b.Property<string>("SurgicalCategoryID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LabId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SurgicalCategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SurgicalCategoryID");

                    b.HasIndex("LabId");

                    b.ToTable("SurgicalCategory");
                });

            modelBuilder.Entity("Project_v1.Models.SurgicalInventory", b =>
                {
                    b.Property<string>("SurgicalInventoryID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IssuedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("IssuedDate")
                        .HasColumnType("date");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemQR")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SurgicalCategoryID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SurgicalInventoryID");

                    b.HasIndex("SurgicalCategoryID");

                    b.ToTable("SurgicalInventory");
                });

            modelBuilder.Entity("Project_v1.Models.SystemUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("LabID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("MOHAreaId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PHIAreaId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("LabID");

                    b.HasIndex("MOHAreaId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("PHIAreaId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Project_v1.Models.SystemUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Project_v1.Models.SystemUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Project_v1.Models.SystemUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Project_v1.Models.SystemUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Project_v1.Models.GeneralCategory", b =>
                {
                    b.HasOne("Project_v1.Models.Lab", "Lab")
                        .WithMany("GeneralCategories")
                        .HasForeignKey("LabId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lab");
                });

            modelBuilder.Entity("Project_v1.Models.GeneralInventory", b =>
                {
                    b.HasOne("Project_v1.Models.GeneralCategory", "GeneralCategory")
                        .WithMany("GeneralInventories")
                        .HasForeignKey("GeneralCategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GeneralCategory");
                });

            modelBuilder.Entity("Project_v1.Models.InstrumentalQualityControl", b =>
                {
                    b.HasOne("Project_v1.Models.Lab", "Lab")
                        .WithMany("InstrumentalQualityControls")
                        .HasForeignKey("LabId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lab");
                });

            modelBuilder.Entity("Project_v1.Models.IssuedItem", b =>
                {
                    b.HasOne("Project_v1.Models.SurgicalInventory", "SurgicalInventory")
                        .WithMany("IssuedItems")
                        .HasForeignKey("SurgicalInventoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SurgicalInventory");
                });

            modelBuilder.Entity("Project_v1.Models.MOHArea", b =>
                {
                    b.HasOne("Project_v1.Models.Lab", "Lab")
                        .WithMany("MOHAreas")
                        .HasForeignKey("LabID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lab");
                });

            modelBuilder.Entity("Project_v1.Models.MediaQualityControl", b =>
                {
                    b.HasOne("Project_v1.Models.Lab", "Lab")
                        .WithMany("MediaQualityControls")
                        .HasForeignKey("LabId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lab");
                });

            modelBuilder.Entity("Project_v1.Models.PHIArea", b =>
                {
                    b.HasOne("Project_v1.Models.MOHArea", "MOHArea")
                        .WithMany("PHIAreas")
                        .HasForeignKey("MOHAreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MOHArea");
                });

            modelBuilder.Entity("Project_v1.Models.Report", b =>
                {
                    b.HasOne("Project_v1.Models.Lab", "Lab")
                        .WithMany("Reports")
                        .HasForeignKey("LabId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Project_v1.Models.Sample", "Sample")
                        .WithMany("Reports")
                        .HasForeignKey("SampleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lab");

                    b.Navigation("Sample");
                });

            modelBuilder.Entity("Project_v1.Models.Sample", b =>
                {
                    b.HasOne("Project_v1.Models.PHIArea", "PHIArea")
                        .WithMany("Samples")
                        .HasForeignKey("PHIAreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PHIArea");
                });

            modelBuilder.Entity("Project_v1.Models.SurgicalCategory", b =>
                {
                    b.HasOne("Project_v1.Models.Lab", "Lab")
                        .WithMany()
                        .HasForeignKey("LabId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lab");
                });

            modelBuilder.Entity("Project_v1.Models.SurgicalInventory", b =>
                {
                    b.HasOne("Project_v1.Models.SurgicalCategory", "SurgicalCategory")
                        .WithMany("SurgicalInventories")
                        .HasForeignKey("SurgicalCategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SurgicalCategory");
                });

            modelBuilder.Entity("Project_v1.Models.SystemUser", b =>
                {
                    b.HasOne("Project_v1.Models.Lab", "Lab")
                        .WithMany("Mlts")
                        .HasForeignKey("LabID");

                    b.HasOne("Project_v1.Models.MOHArea", "MOHArea")
                        .WithMany("Moh_supervisors")
                        .HasForeignKey("MOHAreaId");

                    b.HasOne("Project_v1.Models.PHIArea", "PHIArea")
                        .WithMany("Phis")
                        .HasForeignKey("PHIAreaId");

                    b.Navigation("Lab");

                    b.Navigation("MOHArea");

                    b.Navigation("PHIArea");
                });

            modelBuilder.Entity("Project_v1.Models.GeneralCategory", b =>
                {
                    b.Navigation("GeneralInventories");
                });

            modelBuilder.Entity("Project_v1.Models.Lab", b =>
                {
                    b.Navigation("GeneralCategories");

                    b.Navigation("InstrumentalQualityControls");

                    b.Navigation("MOHAreas");

                    b.Navigation("MediaQualityControls");

                    b.Navigation("Mlts");

                    b.Navigation("Reports");
                });

            modelBuilder.Entity("Project_v1.Models.MOHArea", b =>
                {
                    b.Navigation("Moh_supervisors");

                    b.Navigation("PHIAreas");
                });

            modelBuilder.Entity("Project_v1.Models.PHIArea", b =>
                {
                    b.Navigation("Phis");

                    b.Navigation("Samples");
                });

            modelBuilder.Entity("Project_v1.Models.Sample", b =>
                {
                    b.Navigation("Reports");
                });

            modelBuilder.Entity("Project_v1.Models.SurgicalCategory", b =>
                {
                    b.Navigation("SurgicalInventories");
                });

            modelBuilder.Entity("Project_v1.Models.SurgicalInventory", b =>
                {
                    b.Navigation("IssuedItems");
                });
#pragma warning restore 612, 618
        }
    }
}
