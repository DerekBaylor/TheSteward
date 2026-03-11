using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSteward.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFilingStatusToIncome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FilingStatus",
                table: "Incomes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilingStatus",
                table: "Incomes");
        }
    }
}
