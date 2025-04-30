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
            migrationBuilder.DropTable(
                name: "ServiceStaff");

            migrationBuilder.AddColumn<int>(
                name: "StaffAcceptedBy",
                table: "ServiceRequest",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequest_StaffAcceptedBy",
                table: "ServiceRequest",
                column: "StaffAcceptedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequest_Staff_StaffAcceptedBy",
                table: "ServiceRequest",
                column: "StaffAcceptedBy",
                principalTable: "Staff",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequest_Staff_StaffAcceptedBy",
                table: "ServiceRequest");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequest_StaffAcceptedBy",
                table: "ServiceRequest");

            migrationBuilder.DropColumn(
                name: "StaffAcceptedBy",
                table: "ServiceRequest");

            migrationBuilder.CreateTable(
                name: "ServiceStaff",
                columns: table => new
                {
                    ServiceStaffId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    StaffId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceStaff", x => x.ServiceStaffId);
                    table.ForeignKey(
                        name: "FK_ServiceStaff_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceStaff_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStaff_ServiceId_StaffId",
                table: "ServiceStaff",
                columns: new[] { "ServiceId", "StaffId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStaff_StaffId",
                table: "ServiceStaff",
                column: "StaffId");
        }
    }
}
