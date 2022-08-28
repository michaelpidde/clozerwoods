using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClozerWoods.Migrations
{
    public partial class AddHomePageSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHome",
                table: "Pages",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHome",
                table: "Pages");
        }
    }
}
