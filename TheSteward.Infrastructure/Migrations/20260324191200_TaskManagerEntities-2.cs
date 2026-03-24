using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSteward.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TaskManagerEntities2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecurrenceRules",
                columns: table => new
                {
                    RecurrenceRuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecurrenceFrequency = table.Column<int>(type: "integer", nullable: false),
                    RecurrenceDays = table.Column<int>(type: "integer", nullable: true),
                    IntervalDays = table.Column<int>(type: "integer", nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastGeneratedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurrenceRules", x => x.RecurrenceRuleId);
                });

            migrationBuilder.CreateTable(
                name: "TaskItemsCategories",
                columns: table => new
                {
                    TaskItemCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskItemCategoryName = table.Column<string>(type: "text", nullable: false),
                    IconName = table.Column<string>(type: "text", nullable: true),
                    ColorHex = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItemsCategories", x => x.TaskItemCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "TaskItems",
                columns: table => new
                {
                    TaskItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskItemName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserHouseholdId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedToUserHouseholdId = table.Column<Guid>(type: "uuid", nullable: true),
                    RecurrenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaskItemCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpenseId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItems", x => x.TaskItemId);
                    table.ForeignKey(
                        name: "FK_TaskItems_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "ExpenseId");
                    table.ForeignKey(
                        name: "FK_TaskItems_RecurrenceRules_RecurrenceId",
                        column: x => x.RecurrenceId,
                        principalTable: "RecurrenceRules",
                        principalColumn: "RecurrenceRuleId");
                    table.ForeignKey(
                        name: "FK_TaskItems_TaskItemsCategories_TaskItemCategoryId",
                        column: x => x.TaskItemCategoryId,
                        principalTable: "TaskItemsCategories",
                        principalColumn: "TaskItemCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskItems_UserHouseholds_AssignedToUserHouseholdId",
                        column: x => x.AssignedToUserHouseholdId,
                        principalTable: "UserHouseholds",
                        principalColumn: "UserHouseholdId");
                    table.ForeignKey(
                        name: "FK_TaskItems_UserHouseholds_CreatedByUserHouseholdId",
                        column: x => x.CreatedByUserHouseholdId,
                        principalTable: "UserHouseholds",
                        principalColumn: "UserHouseholdId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskItemOccurrences",
                columns: table => new
                {
                    TaskItemOccurrenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScheduledDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CompletedByUserHouseholdId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaskItemId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItemOccurrences", x => x.TaskItemOccurrenceId);
                    table.ForeignKey(
                        name: "FK_TaskItemOccurrences_TaskItems_TaskItemId",
                        column: x => x.TaskItemId,
                        principalTable: "TaskItems",
                        principalColumn: "TaskItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskItemOccurrences_TaskItemId",
                table: "TaskItemOccurrences",
                column: "TaskItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_AssignedToUserHouseholdId",
                table: "TaskItems",
                column: "AssignedToUserHouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_CreatedByUserHouseholdId",
                table: "TaskItems",
                column: "CreatedByUserHouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_ExpenseId",
                table: "TaskItems",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_RecurrenceId",
                table: "TaskItems",
                column: "RecurrenceId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_TaskItemCategoryId",
                table: "TaskItems",
                column: "TaskItemCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskItemOccurrences");

            migrationBuilder.DropTable(
                name: "TaskItems");

            migrationBuilder.DropTable(
                name: "RecurrenceRules");

            migrationBuilder.DropTable(
                name: "TaskItemsCategories");
        }
    }
}
