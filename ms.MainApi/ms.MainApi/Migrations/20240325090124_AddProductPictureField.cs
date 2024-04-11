using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ms.MainApi.Migrations
{
    public partial class AddProductPictureField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "order",
                table: "ProductPictures",
                type: "integer",
                nullable: false,
                defaultValue: 0);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order",
                table: "ProductPictures");

        }
    }
}
