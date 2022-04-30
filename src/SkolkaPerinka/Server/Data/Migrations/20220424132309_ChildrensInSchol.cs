using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkolkaPerinka.Server.Data.Migrations
{
    public partial class ChildrensInSchol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdChildrensInSchool",
                table: "Days",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdChildrensInSchool",
                table: "Days");
        }
    }
}
