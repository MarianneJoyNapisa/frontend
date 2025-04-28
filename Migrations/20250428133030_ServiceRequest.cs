using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeownersMS.Migrations
{
    /// <inheritdoc />
    public partial class ServiceRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestedAtStart",
                table: "ServiceRequest",
                newName: "RequestedTimeStart");

            migrationBuilder.RenameColumn(
                name: "RequestedAtEnd",
                table: "ServiceRequest",
                newName: "RequestedTimeEnd");

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestApprovedDateTime",
                table: "ServiceRequest",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestApprovedDateTime",
                table: "ServiceRequest");

            migrationBuilder.RenameColumn(
                name: "RequestedTimeStart",
                table: "ServiceRequest",
                newName: "RequestedAtStart");

            migrationBuilder.RenameColumn(
                name: "RequestedTimeEnd",
                table: "ServiceRequest",
                newName: "RequestedAtEnd");
        }
    }
}
