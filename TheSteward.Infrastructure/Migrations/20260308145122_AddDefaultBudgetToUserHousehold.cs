using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSteward.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultBudgetToUserHousehold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DefaultBudgetId",
                table: "UserHouseholds",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserHouseholds_DefaultBudgetId",
                table: "UserHouseholds",
                column: "DefaultBudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHouseholds_Budgets_DefaultBudgetId",
                table: "UserHouseholds",
                column: "DefaultBudgetId",
                principalTable: "Budgets",
                principalColumn: "BudgetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHouseholds_Budgets_DefaultBudgetId",
                table: "UserHouseholds");

            migrationBuilder.DropIndex(
                name: "IX_UserHouseholds_DefaultBudgetId",
                table: "UserHouseholds");

            migrationBuilder.DropColumn(
                name: "DefaultBudgetId",
                table: "UserHouseholds");
        }
    }
}
