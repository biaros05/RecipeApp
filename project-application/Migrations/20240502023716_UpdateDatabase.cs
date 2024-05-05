using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_Ratings_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Ratings");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "RecipeManager_Ratings",
                newName: "OwnerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeManager_Ratings_RecipeId",
                table: "RecipeManager_Ratings",
                newName: "IX_RecipeManager_Ratings_OwnerUserId");

            migrationBuilder.AddColumn<int>(
                name: "OwnerUserId",
                table: "RecipeManager_DifficultyRatings",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeManager_DifficultyRatings_OwnerUserId",
                table: "RecipeManager_DifficultyRatings",
                column: "OwnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_DifficultyRatings_RecipeManager_Users_OwnerUserId",
                table: "RecipeManager_DifficultyRatings",
                column: "OwnerUserId",
                principalTable: "RecipeManager_Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_Ratings_RecipeManager_Users_OwnerUserId",
                table: "RecipeManager_Ratings",
                column: "OwnerUserId",
                principalTable: "RecipeManager_Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_DifficultyRatings_RecipeManager_Users_OwnerUserId",
                table: "RecipeManager_DifficultyRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_Ratings_RecipeManager_Users_OwnerUserId",
                table: "RecipeManager_Ratings");

            migrationBuilder.DropIndex(
                name: "IX_RecipeManager_DifficultyRatings_OwnerUserId",
                table: "RecipeManager_DifficultyRatings");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "RecipeManager_DifficultyRatings");

            migrationBuilder.RenameColumn(
                name: "OwnerUserId",
                table: "RecipeManager_Ratings",
                newName: "RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeManager_Ratings_OwnerUserId",
                table: "RecipeManager_Ratings",
                newName: "IX_RecipeManager_Ratings_RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_Ratings_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Ratings",
                column: "RecipeId",
                principalTable: "RecipeManager_Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
