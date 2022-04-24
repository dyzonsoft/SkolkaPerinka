using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkolkaPerinka.Server.Data.Migrations
{
    public partial class Email : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Childrens",
                newName: "ParentEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentEmail",
                table: "Childrens",
                newName: "ParentId");
        }
    }
}
