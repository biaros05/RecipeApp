using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "RecipeManager_Ratings",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeManager_Ratings_RecipeId",
                table: "RecipeManager_Ratings",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_Ratings_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Ratings",
                column: "RecipeId",
                principalTable: "RecipeManager_Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_Ratings_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Ratings");

            migrationBuilder.DropIndex(
                name: "IX_RecipeManager_Ratings_RecipeId",
                table: "RecipeManager_Ratings");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "RecipeManager_Ratings");
        }
    }
}
