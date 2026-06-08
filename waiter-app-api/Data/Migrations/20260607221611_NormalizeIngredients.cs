using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaiterApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Products_ProductId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_ProductId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Ingredients");

            migrationBuilder.CreateTable(
                name: "ProductIngredients",
                columns: table => new
                {
                    IngredientsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductIngredients", x => new { x.IngredientsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ProductIngredients_Ingredients_IngredientsId",
                        column: x => x.IngredientsId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductIngredients_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductIngredients_ProductsId",
                table: "ProductIngredients",
                column: "ProductsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductIngredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Ingredients",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_ProductId",
                table: "Ingredients",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Products_ProductId",
                table: "Ingredients",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
