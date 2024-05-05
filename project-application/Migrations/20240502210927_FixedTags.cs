using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class FixedTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_Tags_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Tags");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "RecipeManager_Tags",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_Tags_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Tags",
                column: "RecipeId",
                principalTable: "RecipeManager_Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_Tags_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Tags");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "RecipeManager_Tags",
                type: "NUMBER(10)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_Tags_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Tags",
                column: "RecipeId",
                principalTable: "RecipeManager_Recipes",
                principalColumn: "Id");
        }
    }
}
