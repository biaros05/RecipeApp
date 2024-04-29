using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRatingAndDiff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DifficultyRating",
                table: "RecipeManager_Recipes");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "RecipeManager_Recipes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DifficultyRating",
                table: "RecipeManager_Recipes",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "RecipeManager_Recipes",
                type: "BINARY_DOUBLE",
                nullable: true);
        }
    }
}
