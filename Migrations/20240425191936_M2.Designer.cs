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
    [Migration("20240425191936_M2")]
    partial class M2
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
                        .ValueGeneratedOnAdd()
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
                            Id = "4ae37312-225a-4df5-a0b1-807b2c172cfe",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "a8d5c59d-af20-41af-bb83-c846cf9aacb3",
                            Name = "Mlt",
                            NormalizedName = "MLT"
                        },
                        new
                        {
                            Id = "3380c1f8-5cef-4f29-8b98-b74dca46f176",
                            Name = "MohSupervisor",
                            NormalizedName = "MOH_Supervisor"
                        },
                        new
                        {
                            Id = "5a0d2a27-55f4-4c5b-bc4f-f8c9dae91281",
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

            modelBuilder.Entity("Project_v1.Models.Lab", b =>
                {
                    b.Property<string>("LabID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Lab_location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lab_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lab_telephone")
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

                    b.Property<string>("MOHArea_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MOHAreaID");

                    b.HasIndex("LabID");

                    b.ToTable("MOHAreas");
                });

            modelBuilder.Entity("Project_v1.Models.PHIArea", b =>
                {
                    b.Property<string>("PHIAreaID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MOHAreaId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PHIArea_name")
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

                    b.Property<string>("EcoliCount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("IssuedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LabId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PresumptiveColiformCount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Results")
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

                    b.Property<DateOnly>("DateOfCollection")
                        .HasColumnType("date");

                    b.Property<string>("PHIAreaId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Phi_Area")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StateOfChlorination")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SampleId");

                    b.HasIndex("PHIAreaId");

                    b.ToTable("Samples");
                });

            modelBuilder.Entity("Project_v1.Models.Users.SystemUser", b =>
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
                    b.HasOne("Project_v1.Models.Users.SystemUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Project_v1.Models.Users.SystemUser", null)
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

                    b.HasOne("Project_v1.Models.Users.SystemUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Project_v1.Models.Users.SystemUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("Project_v1.Models.Users.SystemUser", b =>
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

            modelBuilder.Entity("Project_v1.Models.Lab", b =>
                {
                    b.Navigation("MOHAreas");

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
#pragma warning restore 612, 618
        }
    }
}
