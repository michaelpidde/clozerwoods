using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClozerWoods.Migrations
{
    public partial class AddThumbnailToMediaItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "MediaItems",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "MediaItems");
        }
    }
}
