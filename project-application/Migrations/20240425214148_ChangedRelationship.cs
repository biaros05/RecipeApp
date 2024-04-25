using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class ChangedRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeManager_Recipes_RecipeManager_Users_UserFavouriteId",
                table: "RecipeManager_Recipes");

            migrationBuilder.DropIndex(
                name: "IX_RecipeManager_Recipes_UserFavouriteId",
                table: "RecipeManager_Recipes");

            migrationBuilder.DropColumn(
                name: "UserFavouriteId",
                table: "RecipeManager_Recipes");

            migrationBuilder.CreateTable(
                name: "RecipeUser",
                columns: table => new
                {
                    UserFavoriteRecipiesId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    UserFavouriteUserId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeUser", x => new { x.UserFavoriteRecipiesId, x.UserFavouriteUserId });
                    table.ForeignKey(
                        name: "FK_RecipeUser_RecipeManager_Recipes_UserFavoriteRecipiesId",
                        column: x => x.UserFavoriteRecipiesId,
                        principalTable: "RecipeManager_Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeUser_RecipeManager_Users_UserFavouriteUserId",
                        column: x => x.UserFavouriteUserId,
                        principalTable: "RecipeManager_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeUser_UserFavouriteUserId",
                table: "RecipeUser",
                column: "UserFavouriteUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeUser");

            migrationBuilder.AddColumn<int>(
                name: "UserFavouriteId",
                table: "RecipeManager_Recipes",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeManager_Recipes_UserFavouriteId",
                table: "RecipeManager_Recipes",
                column: "UserFavouriteId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeManager_Recipes_RecipeManager_Users_UserFavouriteId",
                table: "RecipeManager_Recipes",
                column: "UserFavouriteId",
                principalTable: "RecipeManager_Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
