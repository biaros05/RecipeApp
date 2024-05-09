using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class FKInstructionsMeasuredIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_Instructions_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Instructions");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_MeasuredIngredients_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_MeasuredIngredients");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "RecipeManager_MeasuredIngredients",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "RecipeManager_Instructions",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_Instructions_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Instructions",
                column: "RecipeId",
                principalTable: "RecipeManager_Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_MeasuredIngredients_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_MeasuredIngredients",
                column: "RecipeId",
                principalTable: "RecipeManager_Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_Instructions_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Instructions");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_MeasuredIngredients_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_MeasuredIngredients");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "RecipeManager_MeasuredIngredients",
                type: "NUMBER(10)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "RecipeManager_Instructions",
                type: "NUMBER(10)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_Instructions_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_Instructions",
                column: "RecipeId",
                principalTable: "RecipeManager_Recipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_MeasuredIngredients_RecipeManager_Recipes_RecipeId",
                table: "RecipeManager_MeasuredIngredients",
                column: "RecipeId",
                principalTable: "RecipeManager_Recipes",
                principalColumn: "Id");
        }
    }
}
