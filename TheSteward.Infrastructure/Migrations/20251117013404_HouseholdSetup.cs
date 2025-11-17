using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSteward.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HouseholdSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Households_AspNetUsers_UserId",
                table: "Households");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Households",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "HasTaskManagerModule",
                table: "Households",
                newName: "HasTaskManagerAccess");

            migrationBuilder.RenameColumn(
                name: "HasKitchenManagerModule",
                table: "Households",
                newName: "HasMealManagerAccess");

            migrationBuilder.RenameColumn(
                name: "HasFinanceManagerModule",
                table: "Households",
                newName: "HasFinanceManagerAccess");

            migrationBuilder.RenameColumn(
                name: "HasFileManagerModule",
                table: "Households",
                newName: "HasFileManagerAccess");

            migrationBuilder.RenameIndex(
                name: "IX_Households_UserId",
                table: "Households",
                newName: "IX_Households_OwnerId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Households_AspNetUsers_OwnerId",
                table: "Households",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Households_HouseholdId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Households_AspNetUsers_OwnerId",
                table: "Households");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_HouseholdId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HouseholdId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Households",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "HasTaskManagerAccess",
                table: "Households",
                newName: "HasTaskManagerModule");

            migrationBuilder.RenameColumn(
                name: "HasMealManagerAccess",
                table: "Households",
                newName: "HasKitchenManagerModule");

            migrationBuilder.RenameColumn(
                name: "HasFinanceManagerAccess",
                table: "Households",
                newName: "HasFinanceManagerModule");

            migrationBuilder.RenameColumn(
                name: "HasFileManagerAccess",
                table: "Households",
                newName: "HasFileManagerModule");

            migrationBuilder.RenameIndex(
                name: "IX_Households_OwnerId",
                table: "Households",
                newName: "IX_Households_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Households_AspNetUsers_UserId",
                table: "Households",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
