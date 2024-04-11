using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ms.MainApi.Migrations
{
    public partial class AddProductTables_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<int>>(
                name: "materialsId",
                table: "Products",
                type: "integer[]",
                nullable: true,
                oldClrType: typeof(List<int>),
                oldType: "integer[]");

            migrationBuilder.AddColumn<int>(
                name: "catalogId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<List<int>>(
                name: "colorsId",
                table: "Products",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "catalogId",
                table: "ProductArticles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            
            migrationBuilder.CreateIndex(
                name: "IX_Products_catalogId",
                table: "Products",
                column: "catalogId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductArticles_catalogId",
                table: "ProductArticles",
                column: "catalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductArticles_Catalogs_catalogId",
                table: "ProductArticles",
                column: "catalogId",
                principalTable: "Catalogs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Catalogs_catalogId",
                table: "Products",
                column: "catalogId",
                principalTable: "Catalogs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductArticles_Catalogs_catalogId",
                table: "ProductArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Catalogs_catalogId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_catalogId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductArticles_catalogId",
                table: "ProductArticles");

            migrationBuilder.DropColumn(
                name: "catalogId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "colorsId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "catalogId",
                table: "ProductArticles");

            migrationBuilder.AlterColumn<List<int>>(
                name: "materialsId",
                table: "Products",
                type: "integer[]",
                nullable: false,
                oldClrType: typeof(List<int>),
                oldType: "integer[]",
                oldNullable: true);

        }
    }
}
