﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Project.Repositories.Migrations
{
    public partial class AdvancePropertyDegisti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "StartDateOfWork",
                value: new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "StartDateOfWork",
                value: new DateTime(2023, 6, 13, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
