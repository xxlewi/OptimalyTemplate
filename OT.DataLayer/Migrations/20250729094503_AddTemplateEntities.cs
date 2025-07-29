using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OT.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddTemplateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TemplateCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SalePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Sku = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateProducts", x => x.Id);
                    table.CheckConstraint("CK_TemplateProduct_Price_Positive", "[Price] > 0");
                    table.CheckConstraint("CK_TemplateProduct_SalePrice_LessThanPrice", "[SalePrice] IS NULL OR [SalePrice] < [Price]");
                    table.CheckConstraint("CK_TemplateProduct_SalePrice_Positive", "[SalePrice] IS NULL OR [SalePrice] > 0");
                    table.CheckConstraint("CK_TemplateProduct_StockQuantity_NonNegative", "[StockQuantity] >= 0");
                    table.ForeignKey(
                        name: "FK_TemplateProducts_TemplateCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TemplateCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "TemplateCategories",
                columns: new[] { "Id", "CreatedAt", "Description", "DisplayOrder", "IsActive", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Elektronické zařízení", 1, true, false, "Elektronika", null },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Módní oblečení", 2, true, false, "Oblečení", null },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Knihy a publikace", 3, true, false, "Knihy", null }
                });

            migrationBuilder.InsertData(
                table: "TemplateProducts",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "IsActive", "IsDeleted", "IsFeatured", "Name", "Price", "SalePrice", "Sku", "StockQuantity", "UpdatedAt" },
                values: new object[] { 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nejnovější iPhone s titanovým designem", true, false, true, "iPhone 15 Pro", 32990m, 29990m, "IPH15PRO", 15, null });

            migrationBuilder.InsertData(
                table: "TemplateProducts",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "IsActive", "IsDeleted", "Name", "Price", "SalePrice", "Sku", "StockQuantity", "UpdatedAt" },
                values: new object[,]
                {
                    { 2, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pokročilý Android smartphone", true, false, "Samsung Galaxy S24", 24990m, null, "SGS24", 8, null },
                    { 3, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Elegantní bavlněná košile", true, false, "Pánská košile", 1290m, null, "SHIRT001", 25, null }
                });

            migrationBuilder.InsertData(
                table: "TemplateProducts",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "IsActive", "IsDeleted", "Name", "Price", "SalePrice", "Sku", "UpdatedAt" },
                values: new object[] { 4, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kniha o psaní kvalitního kódu", true, false, "Čistý kód", 590m, null, "BOOK001", null });

            migrationBuilder.InsertData(
                table: "TemplateProducts",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "IsActive", "IsDeleted", "IsFeatured", "Name", "Price", "SalePrice", "Sku", "StockQuantity", "UpdatedAt" },
                values: new object[] { 5, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Letní šaty v moderním stylu", true, false, true, "Dámské šaty", 2490m, 1990m, "DRESS001", 3, null });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateCategories_DisplayOrder",
                table: "TemplateCategories",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateCategories_Name",
                table: "TemplateCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TemplateProducts_CategoryId",
                table: "TemplateProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateProducts_IsActive",
                table: "TemplateProducts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateProducts_IsFeatured",
                table: "TemplateProducts",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateProducts_Name",
                table: "TemplateProducts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateProducts_Sku",
                table: "TemplateProducts",
                column: "Sku",
                unique: true,
                filter: "[Sku] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateProducts");

            migrationBuilder.DropTable(
                name: "TemplateCategories");
        }
    }
}
