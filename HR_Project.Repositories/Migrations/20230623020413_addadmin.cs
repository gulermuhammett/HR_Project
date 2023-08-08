using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Project.Repositories.Migrations
{
    public partial class addadmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyTitle",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractFinishDate",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractStartDate",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MersisNo",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfEmployees",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxNumber",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxOffice",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "YearOfFoundation",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Address", "BirthPlace", "Birthdate", "CompanyID", "DepartmentID", "DismissalDate", "Email", "FirstName", "FirstName2", "Gender", "IsActive", "IsPasswordChange", "JobID", "LastName", "LastName2", "Password", "PhoneNumber", "Photo", "Role", "Salary", "StartDateOfWork", "TCIdentificationNumber" },
                values: new object[] { 101, "İzmir", "İzmir", new DateTime(1991, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, null, "muhammet.guler@bilgeadam.com", "Muhammet", null, 1, true, false, 1, "Güler", null, "123Abc.", "5386656649", "https://media.licdn.com/dms/image/D4D03AQHDyrxhHyFbzw/profile-displayphoto-shrink_800_800/0/1685523959801?e=2147483647&v=beta&t=8SZPlG5BfMKyNQmqBIBbRJ2C45EYBsfAaJAf-Rc5oV4", 1, 42000m, new DateTime(2023, 6, 23, 0, 0, 0, 0, DateTimeKind.Local), "12345678912" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 101);

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CompanyTitle",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ContractFinishDate",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ContractStartDate",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "MersisNo",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "NumberOfEmployees",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "TaxNumber",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "TaxOffice",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "YearOfFoundation",
                table: "Companies");
        }
    }
}
