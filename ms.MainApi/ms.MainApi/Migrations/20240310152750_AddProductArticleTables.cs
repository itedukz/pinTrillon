using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ms.MainApi.Migrations
{
    public partial class AddProductArticleTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPictures_Pictures_pictureId",
                table: "ProductPictures");

            migrationBuilder.DropIndex(
                name: "IX_ProductPictures_pictureId",
                table: "ProductPictures");

            migrationBuilder.DropColumn(
                name: "pictureId",
                table: "ProductPictures");

            migrationBuilder.RenameColumn(
                name: "ProductPictures",
                table: "Products",
                newName: "properties");


            migrationBuilder.AddColumn<int>(
                name: "productArticleId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ProductPictures",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "ProductPictures",
                type: "text",
                nullable: false,
                defaultValue: "");
                        
            migrationBuilder.AddColumn<string>(
                name: "properties",
                table: "Catalogs",
                type: "jsonb",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductArticles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    createdBy = table.Column<int>(type: "integer", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updatedBy = table.Column<int>(type: "integer", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletedBy = table.Column<int>(type: "integer", nullable: true),
                    deletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductArticles", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_productArticleId",
                table: "Products",
                column: "productArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductArticles_productArticleId",
                table: "Products",
                column: "productArticleId",
                principalTable: "ProductArticles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductArticles_productArticleId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductArticles");

            migrationBuilder.DropIndex(
                name: "IX_Products_productArticleId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "productArticleId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ProductPictures");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "ProductPictures");

            migrationBuilder.DropColumn(
                name: "properties",
                table: "Catalogs");

            migrationBuilder.RenameColumn(
                name: "properties",
                table: "Products",
                newName: "ProductPictures");

            migrationBuilder.AddColumn<int>(
                name: "pictureId",
                table: "ProductPictures",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPictures_pictureId",
                table: "ProductPictures",
                column: "pictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPictures_Pictures_pictureId",
                table: "ProductPictures",
                column: "pictureId",
                principalTable: "Pictures",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
