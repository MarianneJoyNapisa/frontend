using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeownersMS.Migrations
{
    /// <inheritdoc />
    public partial class CompletelyTweakModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    FacilityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    PricePerHour = table.Column<float>(type: "REAL", nullable: true),
                    FacilityImage = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.FacilityId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    Privilege = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "FacilityReviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FacilityId = table.Column<int>(type: "INTEGER", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityReviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_FacilityReviews_Facility_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facility",
                        principalColumn: "FacilityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    LName = table.Column<string>(type: "TEXT", nullable: true),
                    FName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    ContactNo = table.Column<string>(type: "TEXT", nullable: true),
                    Job = table.Column<string>(type: "TEXT", nullable: true),
                    HireDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    ProfileImage = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Admin_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunityPost",
                columns: table => new
                {
                    CommunityPostId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedBy = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityPost", x => x.CommunityPostId);
                    table.ForeignKey(
                        name: "FK_CommunityPost_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resident",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    LName = table.Column<string>(type: "TEXT", nullable: true),
                    FName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    ContactNo = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    MoveInDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    ProfileImage = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resident", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Resident_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    LName = table.Column<string>(type: "TEXT", nullable: true),
                    FName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    ContactNo = table.Column<string>(type: "TEXT", nullable: true),
                    Job = table.Column<string>(type: "TEXT", nullable: true),
                    HireDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    ProfileImage = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Staff_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Announcement",
                columns: table => new
                {
                    AnnouncementId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    EventDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    BlocksAffected = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcement", x => x.AnnouncementId);
                    table.ForeignKey(
                        name: "FK_Announcement_Admin_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Admin",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    ResourceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.ResourceId);
                    table.ForeignKey(
                        name: "FK_Resource_Admin_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Admin",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunityComment",
                columns: table => new
                {
                    CommunityCommentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CommunityPostId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityComment", x => x.CommunityCommentId);
                    table.ForeignKey(
                        name: "FK_CommunityComment_CommunityPost_CommunityPostId",
                        column: x => x.CommunityPostId,
                        principalTable: "CommunityPost",
                        principalColumn: "CommunityPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacilityRequest",
                columns: table => new
                {
                    FacilityRequestId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReservationDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    ReservationTime = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    RequestCompletionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: true),
                    ResidentId = table.Column<int>(type: "INTEGER", nullable: true),
                    FacilityId = table.Column<int>(type: "INTEGER", nullable: true),
                    FullName = table.Column<string>(type: "TEXT", nullable: true),
                    EmailAddress = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityRequest", x => x.FacilityRequestId);
                    table.ForeignKey(
                        name: "FK_FacilityRequest_Facility_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facility",
                        principalColumn: "FacilityId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_FacilityRequest_Resident_ResidentId",
                        column: x => x.ResidentId,
                        principalTable: "Resident",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    StaffId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.ServiceId);
                    table.ForeignKey(
                        name: "FK_Service_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequest",
                columns: table => new
                {
                    ServiceRequestId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RequestedBy = table.Column<int>(type: "INTEGER", nullable: true),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequest", x => x.ServiceRequestId);
                    table.ForeignKey(
                        name: "FK_ServiceRequest_Resident_RequestedBy",
                        column: x => x.RequestedBy,
                        principalTable: "Resident",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRequest_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Announcement_CreatedBy",
                table: "Announcement",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityComment_CommunityPostId",
                table: "CommunityComment",
                column: "CommunityPostId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPost_CreatedBy",
                table: "CommunityPost",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityRequest_FacilityId",
                table: "FacilityRequest",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityRequest_ResidentId",
                table: "FacilityRequest",
                column: "ResidentId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityReviews_FacilityId",
                table: "FacilityReviews",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_CreatedBy",
                table: "Resource",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Service_StaffId",
                table: "Service",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequest_RequestedBy",
                table: "ServiceRequest",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequest_ServiceId",
                table: "ServiceRequest",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcement");

            migrationBuilder.DropTable(
                name: "CommunityComment");

            migrationBuilder.DropTable(
                name: "FacilityRequest");

            migrationBuilder.DropTable(
                name: "FacilityReviews");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "ServiceRequest");

            migrationBuilder.DropTable(
                name: "CommunityPost");

            migrationBuilder.DropTable(
                name: "Facility");

            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Resident");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
