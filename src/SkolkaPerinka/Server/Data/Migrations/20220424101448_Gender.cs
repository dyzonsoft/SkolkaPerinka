using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkolkaPerinka.Server.Data.Migrations
{
    public partial class Gender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Childrens",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Childrens");
        }
    }
}
