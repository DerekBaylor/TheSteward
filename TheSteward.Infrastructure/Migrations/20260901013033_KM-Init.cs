using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSteward.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class KMInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CookBook",
                columns: table => new
                {
                    CookBookId = table.Column<Guid>(type: "uuid", nullable: false),
                    CookBookName = table.Column<string>(type: "text", nullable: false),
                    CookBookDescription = table.Column<string>(type: "text", nullable: false),
                    UserHouseholdId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CookBook", x => x.CookBookId);
                    table.ForeignKey(
                        name: "FK_CookBook_UserHouseholds_UserHouseholdId",
                        column: x => x.UserHouseholdId,
                        principalTable: "UserHouseholds",
                        principalColumn: "UserHouseholdId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipeName = table.Column<string>(type: "text", nullable: false),
                    RecipeImgUrl = table.Column<string>(type: "text", nullable: false),
                    TotalServings = table.Column<int>(type: "integer", nullable: false),
                    ServingSize = table.Column<int>(type: "integer", nullable: false),
                    PrepTime = table.Column<int>(type: "integer", nullable: false),
                    CookTime = table.Column<int>(type: "integer", nullable: false),
                    TotalTime = table.Column<int>(type: "integer", nullable: false),
                    Cuisine = table.Column<int>(type: "integer", nullable: false),
                    RecipeCategory = table.Column<int>(type: "integer", nullable: false),
                    RecipePrivacy = table.Column<int>(type: "integer", nullable: false),
                    FoodAllergens = table.Column<int[]>(type: "integer[]", nullable: false),
                    UserHouseholdId = table.Column<Guid>(type: "uuid", nullable: false),
                    CookbookId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_Recipe_CookBook_CookbookId",
                        column: x => x.CookbookId,
                        principalTable: "CookBook",
                        principalColumn: "CookBookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recipe_UserHouseholds_UserHouseholdId",
                        column: x => x.UserHouseholdId,
                        principalTable: "UserHouseholds",
                        principalColumn: "UserHouseholdId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NutritionFact",
                columns: table => new
                {
                    NutritionId = table.Column<Guid>(type: "uuid", nullable: false),
                    NutritionName = table.Column<string>(type: "text", nullable: false),
                    NutritionDescription = table.Column<string>(type: "text", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionFact", x => x.NutritionId);
                    table.ForeignKey(
                        name: "FK_NutritionFact_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeText",
                columns: table => new
                {
                    RecipeTextId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeText", x => x.RecipeTextId);
                    table.ForeignKey(
                        name: "FK_RecipeText_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DryGood",
                columns: table => new
                {
                    DryGoodId = table.Column<Guid>(type: "uuid", nullable: false),
                    DryGoodName = table.Column<string>(type: "text", nullable: false),
                    PurchaseQuantity = table.Column<int>(type: "integer", nullable: false),
                    CurrentQuantity = table.Column<int>(type: "integer", nullable: false),
                    MinimumQuantity = table.Column<int>(type: "integer", nullable: false),
                    MaximumQuantity = table.Column<int>(type: "integer", nullable: false),
                    PantryId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: true),
                    ShoppingListId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DryGood", x => x.DryGoodId);
                    table.ForeignKey(
                        name: "FK_DryGood_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "RecipeId");
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentName = table.Column<string>(type: "text", nullable: false),
                    EquipmentDescription = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    KitchenId = table.Column<Guid>(type: "uuid", nullable: false),
                    PantryId = table.Column<Guid>(type: "uuid", nullable: true),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: true),
                    ShoppingListId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.EquipmentId);
                    table.ForeignKey(
                        name: "FK_Equipment_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "RecipeId");
                });

            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    IngredientId = table.Column<Guid>(type: "uuid", nullable: false),
                    IngredientName = table.Column<string>(type: "text", nullable: false),
                    MeasuringType = table.Column<int>(type: "integer", nullable: false),
                    BuyingAmountType = table.Column<int>(type: "integer", nullable: false),
                    PurchaseQuantity = table.Column<int>(type: "integer", nullable: false),
                    CurrentQuantity = table.Column<int>(type: "integer", nullable: false),
                    MinimumQuantity = table.Column<int>(type: "integer", nullable: false),
                    MaximumQuantity = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    PantryId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShoppingListId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.IngredientId);
                    table.ForeignKey(
                        name: "FK_Ingredient_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredient",
                columns: table => new
                {
                    RecipeIngredientId = table.Column<Guid>(type: "uuid", nullable: false),
                    IngredientId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipeQuantity = table.Column<int>(type: "integer", nullable: false),
                    MeasuringType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredient", x => x.RecipeIngredientId);
                    table.ForeignKey(
                        name: "FK_RecipeIngredient_Ingredient_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredient",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeIngredient_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kitchen",
                columns: table => new
                {
                    KitchenId = table.Column<Guid>(type: "uuid", nullable: false),
                    HouseholdId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShoppingListId = table.Column<Guid>(type: "uuid", nullable: false),
                    PantryId = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kitchen", x => x.KitchenId);
                    table.ForeignKey(
                        name: "FK_Kitchen_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "EquipmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kitchen_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "HouseholdId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pantry",
                columns: table => new
                {
                    PantryId = table.Column<Guid>(type: "uuid", nullable: false),
                    KitchenId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pantry", x => x.PantryId);
                    table.ForeignKey(
                        name: "FK_Pantry_Kitchen_KitchenId",
                        column: x => x.KitchenId,
                        principalTable: "Kitchen",
                        principalColumn: "KitchenId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingList",
                columns: table => new
                {
                    ShoppingListId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShoppingListName = table.Column<string>(type: "text", nullable: false),
                    ShoppingListStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ShoppingListEndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    KitchenId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingList", x => x.ShoppingListId);
                    table.ForeignKey(
                        name: "FK_ShoppingList_Kitchen_KitchenId",
                        column: x => x.KitchenId,
                        principalTable: "Kitchen",
                        principalColumn: "KitchenId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CookBook_UserHouseholdId",
                table: "CookBook",
                column: "UserHouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_DryGood_PantryId",
                table: "DryGood",
                column: "PantryId");

            migrationBuilder.CreateIndex(
                name: "IX_DryGood_RecipeId",
                table: "DryGood",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_DryGood_ShoppingListId",
                table: "DryGood",
                column: "ShoppingListId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_KitchenId",
                table: "Equipment",
                column: "KitchenId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_PantryId",
                table: "Equipment",
                column: "PantryId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_RecipeId",
                table: "Equipment",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_ShoppingListId",
                table: "Equipment",
                column: "ShoppingListId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_PantryId",
                table: "Ingredient",
                column: "PantryId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_RecipeId",
                table: "Ingredient",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_ShoppingListId",
                table: "Ingredient",
                column: "ShoppingListId");

            migrationBuilder.CreateIndex(
                name: "IX_Kitchen_EquipmentId",
                table: "Kitchen",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Kitchen_HouseholdId",
                table: "Kitchen",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Kitchen_PantryId",
                table: "Kitchen",
                column: "PantryId");

            migrationBuilder.CreateIndex(
                name: "IX_Kitchen_ShoppingListId",
                table: "Kitchen",
                column: "ShoppingListId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionFact_RecipeId",
                table: "NutritionFact",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Pantry_KitchenId",
                table: "Pantry",
                column: "KitchenId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_CookbookId",
                table: "Recipe",
                column: "CookbookId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_UserHouseholdId",
                table: "Recipe",
                column: "UserHouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredient_IngredientId",
                table: "RecipeIngredient",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredient_RecipeId",
                table: "RecipeIngredient",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeText_RecipeId",
                table: "RecipeText",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingList_KitchenId",
                table: "ShoppingList",
                column: "KitchenId");

            migrationBuilder.AddForeignKey(
                name: "FK_DryGood_Pantry_PantryId",
                table: "DryGood",
                column: "PantryId",
                principalTable: "Pantry",
                principalColumn: "PantryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DryGood_ShoppingList_ShoppingListId",
                table: "DryGood",
                column: "ShoppingListId",
                principalTable: "ShoppingList",
                principalColumn: "ShoppingListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_Kitchen_KitchenId",
                table: "Equipment",
                column: "KitchenId",
                principalTable: "Kitchen",
                principalColumn: "KitchenId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_Pantry_PantryId",
                table: "Equipment",
                column: "PantryId",
                principalTable: "Pantry",
                principalColumn: "PantryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_ShoppingList_ShoppingListId",
                table: "Equipment",
                column: "ShoppingListId",
                principalTable: "ShoppingList",
                principalColumn: "ShoppingListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_Pantry_PantryId",
                table: "Ingredient",
                column: "PantryId",
                principalTable: "Pantry",
                principalColumn: "PantryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_ShoppingList_ShoppingListId",
                table: "Ingredient",
                column: "ShoppingListId",
                principalTable: "ShoppingList",
                principalColumn: "ShoppingListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kitchen_Pantry_PantryId",
                table: "Kitchen",
                column: "PantryId",
                principalTable: "Pantry",
                principalColumn: "PantryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kitchen_ShoppingList_ShoppingListId",
                table: "Kitchen",
                column: "ShoppingListId",
                principalTable: "ShoppingList",
                principalColumn: "ShoppingListId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_Pantry_PantryId",
                table: "Equipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Kitchen_Pantry_PantryId",
                table: "Kitchen");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_Recipe_RecipeId",
                table: "Equipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_ShoppingList_ShoppingListId",
                table: "Equipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Kitchen_ShoppingList_ShoppingListId",
                table: "Kitchen");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_Kitchen_KitchenId",
                table: "Equipment");

            migrationBuilder.DropTable(
                name: "DryGood");

            migrationBuilder.DropTable(
                name: "NutritionFact");

            migrationBuilder.DropTable(
                name: "RecipeIngredient");

            migrationBuilder.DropTable(
                name: "RecipeText");

            migrationBuilder.DropTable(
                name: "Ingredient");

            migrationBuilder.DropTable(
                name: "Pantry");

            migrationBuilder.DropTable(
                name: "Recipe");

            migrationBuilder.DropTable(
                name: "CookBook");

            migrationBuilder.DropTable(
                name: "ShoppingList");

            migrationBuilder.DropTable(
                name: "Kitchen");

            migrationBuilder.DropTable(
                name: "Equipment");
        }
    }
}
