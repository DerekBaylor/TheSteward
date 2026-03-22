using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSteward.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IsActiveToUserHousehold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserHouseholds",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserHouseholds");
        }
    }
}
