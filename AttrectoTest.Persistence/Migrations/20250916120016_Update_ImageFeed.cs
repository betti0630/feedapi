using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttrectoTest.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_ImageFeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "ImageFeeds");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ImageFeeds",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ImageFeeds");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "ImageFeeds",
                type: "longblob",
                nullable: false);
        }
    }
}
