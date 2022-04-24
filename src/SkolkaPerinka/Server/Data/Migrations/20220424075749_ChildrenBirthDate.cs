using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkolkaPerinka.Server.Data.Migrations
{
    public partial class ChildrenBirthDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Old",
                table: "Childrens");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Childrens",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Childrens");

            migrationBuilder.AddColumn<int>(
                name: "Old",
                table: "Childrens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
