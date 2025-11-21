using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSteward.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserHouseholdSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserHouseholds",
                columns: table => new
                {
                    UserHouseholdId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDefaultUserHousehold = table.Column<bool>(type: "boolean", nullable: false),
                    IsHouseholdOwner = table.Column<bool>(type: "boolean", nullable: false),
                    HasAdminPermissions = table.Column<bool>(type: "boolean", nullable: false),
                    HasFinanceManagerWritePermission = table.Column<bool>(type: "boolean", nullable: false),
                    HasFinanceManagerReadPermission = table.Column<bool>(type: "boolean", nullable: false),
                    HasKitchenManagerWritePermission = table.Column<bool>(type: "boolean", nullable: false),
                    HasKitchenManagerReadPermission = table.Column<bool>(type: "boolean", nullable: false),
                    HasTaskManagerWritePermission = table.Column<bool>(type: "boolean", nullable: false),
                    HasTaskManagerReadPermission = table.Column<bool>(type: "boolean", nullable: false),
                    HasFileManagerWritePermission = table.Column<bool>(type: "boolean", nullable: false),
                    HasFileManagerReadPermission = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    HouseholdId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHouseholds", x => x.UserHouseholdId);
                    table.ForeignKey(
                        name: "FK_UserHouseholds_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserHouseholds_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "HouseholdId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserHouseholds_HouseholdId",
                table: "UserHouseholds",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHouseholds_UserId",
                table: "UserHouseholds",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserHouseholds");
        }
    }
}
