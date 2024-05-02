using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class AddedTagTable : Migration
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
