using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ms.MainApi.Migrations
{
    public partial class AddProductTableCorrections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order",
                table: "ProjectPictures");

            migrationBuilder.DropColumn(
                name: "order",
                table: "ProductPictures");

            migrationBuilder.AddColumn<bool>(
                name: "isMain",
                table: "ProjectPictures",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isMain",
                table: "ProductPictures",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isProcessed",
                table: "ProductPictures",
                type: "boolean",
                nullable: false,
                defaultValue: false);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isMain",
                table: "ProjectPictures");

            migrationBuilder.DropColumn(
                name: "isMain",
                table: "ProductPictures");

            migrationBuilder.DropColumn(
                name: "isProcessed",
                table: "ProductPictures");

            migrationBuilder.AddColumn<int>(
                name: "order",
                table: "ProjectPictures",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "order",
                table: "ProductPictures",
                type: "integer",
                nullable: false,
                defaultValue: 0);

        }
    }
}
