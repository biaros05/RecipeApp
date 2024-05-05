using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRatingDifficulty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Budget",
                table: "RecipeManager_Recipes",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "RecipeManager_Ratings",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "RecipeManager_DifficultyRatings",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeManager_Ratings_RecipeId",
                table: "RecipeManager_Ratings",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeManager_DifficultyRatings_RecipeId",
                table: "RecipeManager_DifficultyRatings",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_DifficultyRatings_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_DifficultyRatings",
                column: "RecipeId",
                principalTable: "RecipeManager_Recipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_Ratings_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Ratings",
                column: "RecipeId",
                principalTable: "RecipeManager_Recipes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_DifficultyRatings_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_DifficultyRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_Ratings_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Ratings");

            migrationBuilder.DropIndex(
                name: "IX_RecipeManager_Ratings_RecipeId",
                table: "RecipeManager_Ratings");

            migrationBuilder.DropIndex(
                name: "IX_RecipeManager_DifficultyRatings_RecipeId",
                table: "RecipeManager_DifficultyRatings");

            migrationBuilder.DropColumn(
                name: "Budget",
                table: "RecipeManager_Recipes");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "RecipeManager_Ratings");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "RecipeManager_DifficultyRatings");
        }
    }
}
