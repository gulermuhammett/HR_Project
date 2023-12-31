﻿// <auto-generated />
using System;
using HR_Project.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HR_Project.Repositories.Migrations
{
    [DbContext(typeof(HRProjectContext))]
    [Migration("20230613195605_AddAdvance2")]
    partial class AddAdvance2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("HR_Project.Entities.Entities.Advance", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Advances");
                });

            modelBuilder.Entity("HR_Project.Entities.Entities.Company", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            CompanyName = "KardeşlerYazılım",
                            IsActive = true
                        });
                });

            modelBuilder.Entity("HR_Project.Entities.Entities.Department", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int?>("CompanyID")
                        .HasColumnType("int");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID");

                    b.ToTable("Departments");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            CompanyID = 1,
                            DepartmentName = "Pazarlama",
                            IsActive = true
                        });
                });

            modelBuilder.Entity("HR_Project.Entities.Entities.Job", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int?>("DepartmentID")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("JobName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("DepartmentID");

                    b.ToTable("Jobs");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            DepartmentID = 1,
                            IsActive = true,
                            JobName = "Satış Müdürü"
                        });
                });

            modelBuilder.Entity("HR_Project.Entities.Entities.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BirthPlace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CompanyID")
                        .HasColumnType("int");

                    b.Property<int?>("DepartmentID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DismissalDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("JobID")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Role")
                        .HasColumnType("int");

                    b.Property<decimal?>("Salary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("StartDateOfWork")
                        .HasColumnType("datetime2");

                    b.Property<string>("TCIdentificationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("JobID");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Address = "Ankara Çankaya",
                            BirthPlace = "Ankara",
                            Birthdate = new DateTime(1991, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CompanyID = 1,
                            DepartmentID = 1,
                            FirstName = "Emre",
                            Gender = 1,
                            IsActive = true,
                            JobID = 1,
                            LastName = "Karaüzüm",
                            Password = "123Abc.",
                            PhoneNumber = "5386656649",
                            Photo = "https://media.licdn.com/dms/image/D4D03AQFKCXDB3b5hSA/profile-displayphoto-shrink_800_800/0/1668897343508?e=2147483647&v=beta&t=FMAUQ8X7dS4I6dL_FgCuWxpxXiwq8hiEIJXeh9K9cEQ",
                            Role = 2,
                            Salary = 42000m,
                            StartDateOfWork = new DateTime(2023, 6, 13, 0, 0, 0, 0, DateTimeKind.Local),
                            TCIdentificationNumber = "12345678912"
                        });
                });

            modelBuilder.Entity("HR_Project.Entities.Entities.Advance", b =>
                {
                    b.HasOne("HR_Project.Entities.Entities.User", "User")
                        .WithMany("Advances")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HR_Project.Entities.Entities.Department", b =>
                {
                    b.HasOne("HR_Project.Entities.Entities.Company", "Company")
                        .WithMany("Departments")
                        .HasForeignKey("CompanyID");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("HR_Project.Entities.Entities.Job", b =>
                {
                    b.HasOne("HR_Project.Entities.Entities.Department", "Department")
                        .WithMany("Jobs")
                        .HasForeignKey("DepartmentID");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("HR_Project.Entities.Entities.User", b =>
                {
                    b.HasOne("HR_Project.Entities.Entities.Company", "Company")
                        .WithMany("Users")
                        .HasForeignKey("CompanyID");

                    b.HasOne("HR_Project.Entities.Entities.Department", "Department")
                        .WithMany("Users")
                        .HasForeignKey("DepartmentID");

                    b.HasOne("HR_Project.Entities.Entities.Job", "Job")
                        .WithMany("Users")
                        .HasForeignKey("JobID");

                    b.Navigation("Company");

                    b.Navigation("Department");

                    b.Navigation("Job");
                });

            modelBuilder.Entity("HR_Project.Entities.Entities.Company", b =>
                {
                    b.Navigation("Departments");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("HR_Project.Entities.Entities.Department", b =>
                {
                    b.Navigation("Jobs");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("HR_Project.Entities.Entities.Job", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("HR_Project.Entities.Entities.User", b =>
                {
                    b.Navigation("Advances");
                });
#pragma warning restore 612, 618
        }
    }
}
