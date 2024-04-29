using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class FixingRecipeTableAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DifficultyRating",
                table: "RecipeManager_Recipes",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfServings",
                table: "RecipeManager_Recipes",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "RecipeManager_Recipes",
                type: "BINARY_DOUBLE",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DifficultyRating",
                table: "RecipeManager_Recipes");

            migrationBuilder.DropColumn(
                name: "NumberOfServings",
                table: "RecipeManager_Recipes");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "RecipeManager_Recipes");
        }
    }
}
