using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRecipeAttempt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_RecipeManager_Recipes_RecipeId",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "RecipeManager_Tags");

            migrationBuilder.RenameIndex(
                name: "IX_Tag_RecipeId",
                table: "RecipeManager_Tags",
                newName: "IX_RecipeManager_Tags_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeManager_Tags",
                table: "RecipeManager_Tags",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RecipeManager_DifficultyRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ScaleRating = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeManager_DifficultyRatings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeManager_Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    StarRating = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeManager_Ratings", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_Tags_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Tags",
                column: "RecipeId",
                principalTable: "RecipeManager_Recipes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_Tags_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Tags");

            migrationBuilder.DropTable(
                name: "RecipeManager_DifficultyRatings");

            migrationBuilder.DropTable(
                name: "RecipeManager_Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeManager_Tags",
                table: "RecipeManager_Tags");

            migrationBuilder.RenameTable(
                name: "RecipeManager_Tags",
                newName: "Tag");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeManager_Tags_RecipeId",
                table: "Tag",
                newName: "IX_Tag_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_RecipeManager_Recipes_RecipeId",
                table: "Tag",
                column: "RecipeId",
                principalTable: "RecipeManager_Recipes",
                principalColumn: "Id");
        }
    }
}
