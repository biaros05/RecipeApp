using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class imageByteSizeFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecipeManager_Ingredients",
                columns: table => new
                {
                    IngredientId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Unit = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeManager_Ingredients", x => x.IngredientId);
                });

            migrationBuilder.CreateTable(
                name: "RecipeManager_Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    HashPass = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Salt = table.Column<byte[]>(type: "RAW(2000)", nullable: false),
                    Username = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Image = table.Column<byte[]>(type: "BLOB", maxLength: 3000, nullable: true),
                    Description = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeManager_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "RecipeManager_Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    OwnerId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PrepTimeMins = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CookTimeMins = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NumberOfServings = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Budget = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeManager_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeManager_Recipes_RecipeManager_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "RecipeManager_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeManager_DifficultyRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ScaleRating = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    OwnerUserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    RecipeId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeManager_DifficultyRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeManager_DifficultyRatings_RecipeManager_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "RecipeManager_Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeManager_DifficultyRatings_RecipeManager_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "RecipeManager_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeManager_Instructions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Index = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Text = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    RecipeId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeManager_Instructions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeManager_Instructions_RecipeManager_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "RecipeManager_Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeManager_MeasuredIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    IngredientId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Quantity = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    RecipeId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeManager_MeasuredIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeManager_MeasuredIngredients_RecipeManager_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "RecipeManager_Ingredients",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeManager_MeasuredIngredients_RecipeManager_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "RecipeManager_Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeManager_Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    StarRating = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    OwnerUserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    RecipeId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeManager_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeManager_Ratings_RecipeManager_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "RecipeManager_Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeManager_Ratings_RecipeManager_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "RecipeManager_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeManager_Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TagName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    RecipeId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeManager_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeManager_Tags_RecipeManager_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "RecipeManager_Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_RecipeManager_DifficultyRatings_OwnerUserId",
                table: "RecipeManager_DifficultyRatings",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeManager_DifficultyRatings_RecipeId",
                table: "RecipeManager_DifficultyRatings",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeManager_Instructions_RecipeId",
                table: "RecipeManager_Instructions",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeManager_MeasuredIngredients_IngredientId",
                table: "RecipeManager_MeasuredIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeManager_MeasuredIngredients_RecipeId",
                table: "RecipeManager_MeasuredIngredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeManager_Ratings_OwnerUserId",
                table: "RecipeManager_Ratings",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeManager_Ratings_RecipeId",
                table: "RecipeManager_Ratings",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeManager_Recipes_OwnerId",
                table: "RecipeManager_Recipes",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeManager_Tags_RecipeId",
                table: "RecipeManager_Tags",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeUser_UserFavouriteUserId",
                table: "RecipeUser",
                column: "UserFavouriteUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeManager_DifficultyRatings");

            migrationBuilder.DropTable(
                name: "RecipeManager_Instructions");

            migrationBuilder.DropTable(
                name: "RecipeManager_MeasuredIngredients");

            migrationBuilder.DropTable(
                name: "RecipeManager_Ratings");

            migrationBuilder.DropTable(
                name: "RecipeManager_Tags");

            migrationBuilder.DropTable(
                name: "RecipeUser");

            migrationBuilder.DropTable(
                name: "RecipeManager_Ingredients");

            migrationBuilder.DropTable(
                name: "RecipeManager_Recipes");

            migrationBuilder.DropTable(
                name: "RecipeManager_Users");
        }
    }
}
