using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSteward.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvestmentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investments_Expenses_ExpenseId",
                table: "Investments");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExpenseId",
                table: "Investments",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_Expenses_ExpenseId",
                table: "Investments",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "ExpenseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investments_Expenses_ExpenseId",
                table: "Investments");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExpenseId",
                table: "Investments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_Expenses_ExpenseId",
                table: "Investments",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "ExpenseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
