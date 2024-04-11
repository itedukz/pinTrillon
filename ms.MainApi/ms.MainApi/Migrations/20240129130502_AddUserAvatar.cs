using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ms.MainApi.Migrations
{
    public partial class AddUserAvatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fileName",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "filePath",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fileName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "filePath",
                table: "Users");

        }
    }
}
