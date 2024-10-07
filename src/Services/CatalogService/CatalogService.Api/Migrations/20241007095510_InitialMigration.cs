using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CatalogService.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "catalog_brand_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "catalog_item_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "catalog_type_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "CatalogBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PictureFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailableStock = table.Column<int>(type: "int", nullable: false),
                    OnReorder = table.Column<bool>(type: "bit", nullable: false),
                    CatalogTypeId = table.Column<int>(type: "int", nullable: false),
                    CatalogBrandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogItems_CatalogBrands_CatalogBrandId",
                        column: x => x.CatalogBrandId,
                        principalTable: "CatalogBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogItems_CatalogTypes_CatalogTypeId",
                        column: x => x.CatalogTypeId,
                        principalTable: "CatalogTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CatalogBrands",
                columns: new[] { "Id", "Brand" },
                values: new object[,]
                {
                    { 1, "Azure" },
                    { 2, ".NET" },
                    { 3, "SQL Server" },
                    { 4, "Other" },
                    { 5, "CatalogBrandTestOne" },
                    { 6, "CatalogBrandTestTwo" }
                });

            migrationBuilder.InsertData(
                table: "CatalogTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Mug" },
                    { 2, "T-Shirt" },
                    { 3, "Sheet" },
                    { 4, "USB Memory" },
                    { 5, "CatalogTypeTestOne" },
                    { 6, "CatalogTypeTestTwo" }
                });

            migrationBuilder.InsertData(
                table: "CatalogItems",
                columns: new[] { "Id", "AvailableStock", "CatalogBrandId", "CatalogTypeId", "Description", "Name", "OnReorder", "PictureFileName", "Price" },
                values: new object[,]
                {
                    { 1, 100, 1, 1, ".NET Bot Black Hoodie and more", ".NET Bot Black Hoodie", false, "1.png", 19.5m },
                    { 2, 89, 1, 1, ".NET Black & White Mug", ".NET Black & White Mug", true, "2.png", 8.50m },
                    { 3, 56, 1, 2, "Prism White T-Shirt", "Prism White T-Shirt", false, "3.png", 12m },
                    { 4, 120, 1, 1, ".NET Foundation T-shirt", ".NET Foundation T-shirt", false, "4.png", 12m },
                    { 5, 55, 2, 3, "Roslyn Red Sheet", "Roslyn Red Sheet", false, "5.png", 8.5m },
                    { 6, 17, 1, 1, ".NET Blue Hoodie", ".NET Blue Hoodie", false, "6.png", 12m },
                    { 7, 8, 2, 2, "Roslyn Red T-Shirt", "Roslyn Red T-Shirt", false, "7.png", 12m },
                    { 8, 34, 2, 2, "Kudu Purple Hoodie", "Kudu Purple Hoodie", false, "8.png", 8.5m },
                    { 9, 76, 2, 2, "Cup<T> White Mug", "Cup<T> White Mug", false, "9.png", 12m },
                    { 10, 11, 1, 3, ".NET Foundation Sheet", ".NET Foundation Sheet", false, "10.png", 12m },
                    { 11, 3, 1, 3, "Cup<T> Sheet", "Cup<T> Sheet", false, "11.png", 8.5m },
                    { 12, 0, 2, 2, "Prism White TShirt", "Prism White TShirt", false, "12.png", 12m },
                    { 13, 0, 2, 2, "De los Palotes, pepito", "De los Palotes", false, "12.png", 12m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_CatalogBrandId",
                table: "CatalogItems",
                column: "CatalogBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_CatalogTypeId",
                table: "CatalogItems",
                column: "CatalogTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogItems");

            migrationBuilder.DropTable(
                name: "CatalogBrands");

            migrationBuilder.DropTable(
                name: "CatalogTypes");

            migrationBuilder.DropSequence(
                name: "catalog_brand_hilo");

            migrationBuilder.DropSequence(
                name: "catalog_item_hilo");

            migrationBuilder.DropSequence(
                name: "catalog_type_hilo");
        }
    }
}
