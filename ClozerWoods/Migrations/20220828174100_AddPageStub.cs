using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClozerWoods.Migrations
{
    public partial class AddPageStub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Stub",
                table: "Pages",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stub",
                table: "Pages");
        }
    }
}
