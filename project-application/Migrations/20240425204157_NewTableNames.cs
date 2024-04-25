using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class NewTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructions_Recipes_RecipeId",
                table: "Instructions");

            migrationBuilder.DropForeignKey(
                name: "FK_MeasuredIngredients_Ingredients_IngredientId",
                table: "MeasuredIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_MeasuredIngredients_Recipes_RecipeId",
                table: "MeasuredIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Users_OwnerId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Users_UserFavouriteId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Recipes_RecipeId",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeasuredIngredients",
                table: "MeasuredIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Instructions",
                table: "Instructions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "RecipeManager_Users");

            migrationBuilder.RenameTable(
                name: "Recipes",
                newName: "RecipeManager_Recipes");

            migrationBuilder.RenameTable(
                name: "MeasuredIngredients",
                newName: "RecipeManager_MeasuredIngredients");

            migrationBuilder.RenameTable(
                name: "Instructions",
                newName: "RecipeManager_Instructions");

            migrationBuilder.RenameTable(
                name: "Ingredients",
                newName: "RecipeManager_Ingredients");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_UserFavouriteId",
                table: "RecipeManager_Recipes",
                newName: "IX_RecipeManager_Recipes_UserFavouriteId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_OwnerId",
                table: "RecipeManager_Recipes",
                newName: "IX_RecipeManager_Recipes_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_MeasuredIngredients_RecipeId",
                table: "RecipeManager_MeasuredIngredients",
                newName: "IX_RecipeManager_MeasuredIngredients_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_MeasuredIngredients_IngredientId",
                table: "RecipeManager_MeasuredIngredients",
                newName: "IX_RecipeManager_MeasuredIngredients_IngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_Instructions_RecipeId",
                table: "RecipeManager_Instructions",
                newName: "IX_RecipeManager_Instructions_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeManager_Users",
                table: "RecipeManager_Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeManager_Recipes",
                table: "RecipeManager_Recipes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeManager_MeasuredIngredients",
                table: "RecipeManager_MeasuredIngredients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeManager_Instructions",
                table: "RecipeManager_Instructions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeManager_Ingredients",
                table: "RecipeManager_Ingredients",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_Instructions_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Instructions",
                column: "RecipeId",
                principalTable: "RecipeManager_Recipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_MeasuredIngredients_RecipeManager_Ingredients_IngredientId",
                table: "RecipeManager_MeasuredIngredients",
                column: "IngredientId",
                principalTable: "RecipeManager_Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_MeasuredIngredients_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_MeasuredIngredients",
                column: "RecipeId",
                principalTable: "RecipeManager_Recipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_Recipes_RecipeManager_Users_OwnerId",
                table: "RecipeManager_Recipes",
                column: "OwnerId",
                principalTable: "RecipeManager_Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_Recipes_RecipeManager_Users_UserFavouriteId",
                table: "RecipeManager_Recipes",
                column: "UserFavouriteId",
                principalTable: "RecipeManager_Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_RecipeManager_Recipes_RecipeId",
                table: "Tag",
                column: "RecipeId",
                principalTable: "RecipeManager_Recipes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_Instructions_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Instructions");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_MeasuredIngredients_RecipeManager_Ingredients_IngredientId",
                table: "RecipeManager_MeasuredIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_MeasuredIngredients_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_MeasuredIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_Recipes_RecipeManager_Users_OwnerId",
                table: "RecipeManager_Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_Recipes_RecipeManager_Users_UserFavouriteId",
                table: "RecipeManager_Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_RecipeManager_Recipes_RecipeId",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeManager_Users",
                table: "RecipeManager_Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeManager_Recipes",
                table: "RecipeManager_Recipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeManager_MeasuredIngredients",
                table: "RecipeManager_MeasuredIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeManager_Instructions",
                table: "RecipeManager_Instructions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeManager_Ingredients",
                table: "RecipeManager_Ingredients");

            migrationBuilder.RenameTable(
                name: "RecipeManager_Users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "RecipeManager_Recipes",
                newName: "Recipes");

            migrationBuilder.RenameTable(
                name: "RecipeManager_MeasuredIngredients",
                newName: "MeasuredIngredients");

            migrationBuilder.RenameTable(
                name: "RecipeManager_Instructions",
                newName: "Instructions");

            migrationBuilder.RenameTable(
                name: "RecipeManager_Ingredients",
                newName: "Ingredients");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeManager_Recipes_UserFavouriteId",
                table: "Recipes",
                newName: "IX_Recipes_UserFavouriteId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeManager_Recipes_OwnerId",
                table: "Recipes",
                newName: "IX_Recipes_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeManager_MeasuredIngredients_RecipeId",
                table: "MeasuredIngredients",
                newName: "IX_MeasuredIngredients_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeManager_MeasuredIngredients_IngredientId",
                table: "MeasuredIngredients",
                newName: "IX_MeasuredIngredients_IngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeManager_Instructions_RecipeId",
                table: "Instructions",
                newName: "IX_Instructions_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeasuredIngredients",
                table: "MeasuredIngredients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Instructions",
                table: "Instructions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructions_Recipes_RecipeId",
                table: "Instructions",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MeasuredIngredients_Ingredients_IngredientId",
                table: "MeasuredIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeasuredIngredients_Recipes_RecipeId",
                table: "MeasuredIngredients",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Users_OwnerId",
                table: "Recipes",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Users_UserFavouriteId",
                table: "Recipes",
                column: "UserFavouriteId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Recipes_RecipeId",
                table: "Tag",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }
    }
}
