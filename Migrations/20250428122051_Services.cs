using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeownersMS.Migrations
{
    /// <inheritdoc />
    public partial class Services : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvailableDateTimeStart",
                table: "Service",
                newName: "DayRange");

            migrationBuilder.RenameColumn(
                name: "AvailableDateTimeEnd",
                table: "Service",
                newName: "AvailableTimeStart");

            migrationBuilder.AlterColumn<int>(
                name: "Job",
                table: "Staff",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "AvailableTimeEnd",
                table: "Service",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableTimeEnd",
                table: "Service");

            migrationBuilder.RenameColumn(
                name: "DayRange",
                table: "Service",
                newName: "AvailableDateTimeStart");

            migrationBuilder.RenameColumn(
                name: "AvailableTimeStart",
                table: "Service",
                newName: "AvailableDateTimeEnd");

            migrationBuilder.AlterColumn<string>(
                name: "Job",
                table: "Staff",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
