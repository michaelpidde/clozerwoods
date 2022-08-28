using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClozerWoods.Migrations
{
    public partial class LinkMediaItemsToGalleries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_GalleryId",
                table: "MediaItems",
                column: "GalleryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaItems_Galleries_GalleryId",
                table: "MediaItems",
                column: "GalleryId",
                principalTable: "Galleries",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaItems_Galleries_GalleryId",
                table: "MediaItems");

            migrationBuilder.DropIndex(
                name: "IX_MediaItems_GalleryId",
                table: "MediaItems");
        }
    }
}
