using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSteward.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseRelationshipColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Credit",
                table: "Expenses",
                newName: "CreditId");

            migrationBuilder.AlterColumn<decimal>(
                name: "ContributionAmount",
                table: "Investments",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetSubCategoryId",
                table: "Expenses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_BudgetSubCategoryId",
                table: "Expenses",
                column: "BudgetSubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_BudgetSubCategories_BudgetSubCategoryId",
                table: "Expenses",
                column: "BudgetSubCategoryId",
                principalTable: "BudgetSubCategories",
                principalColumn: "BudgetSubCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_BudgetSubCategories_BudgetSubCategoryId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_BudgetSubCategoryId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "BudgetSubCategoryId",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "CreditId",
                table: "Expenses",
                newName: "Credit");

            migrationBuilder.AlterColumn<decimal>(
                name: "ContributionAmount",
                table: "Investments",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");
        }
    }
}
