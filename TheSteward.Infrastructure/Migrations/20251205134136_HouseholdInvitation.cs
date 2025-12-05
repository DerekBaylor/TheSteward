using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSteward.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HouseholdInvitation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Households_HouseholdId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_HouseholdId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HouseholdId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "HouseholdInvitations",
                columns: table => new
                {
                    InvitationId = table.Column<Guid>(type: "uuid", nullable: false),
                    HouseholdId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvitedUserEmail = table.Column<string>(type: "text", nullable: false),
                    InvitedByUserId = table.Column<string>(type: "text", nullable: false),
                    InvitedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsAccepted = table.Column<bool>(type: "boolean", nullable: false),
                    AcceptedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseholdInvitations", x => x.InvitationId);
                    table.ForeignKey(
                        name: "FK_HouseholdInvitations_AspNetUsers_InvitedByUserId",
                        column: x => x.InvitedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseholdInvitations_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "HouseholdId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdInvitations_HouseholdId",
                table: "HouseholdInvitations",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdInvitations_InvitedByUserId",
                table: "HouseholdInvitations",
                column: "InvitedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseholdInvitations");

            migrationBuilder.AddColumn<Guid>(
                name: "HouseholdId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HouseholdId",
                table: "AspNetUsers",
                column: "HouseholdId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Households_HouseholdId",
                table: "AspNetUsers",
                column: "HouseholdId",
                principalTable: "Households",
                principalColumn: "HouseholdId");
        }
    }
}
