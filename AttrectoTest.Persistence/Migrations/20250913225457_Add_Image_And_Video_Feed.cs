using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttrectoTest.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Image_And_Video_Feed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Feeds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Feeds",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AppUsers",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ImageFeeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ImageData = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageFeeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageFeeds_Feeds_Id",
                        column: x => x.Id,
                        principalTable: "Feeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VideoFeeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    VideoUrl = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoFeeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoFeeds_ImageFeeds_Id",
                        column: x => x.Id,
                        principalTable: "ImageFeeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Feeds_AuthorId",
                table: "Feeds",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feeds_AppUsers_AuthorId",
                table: "Feeds",
                column: "AuthorId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feeds_AppUsers_AuthorId",
                table: "Feeds");

            migrationBuilder.DropTable(
                name: "VideoFeeds");

            migrationBuilder.DropTable(
                name: "ImageFeeds");

            migrationBuilder.DropIndex(
                name: "IX_Feeds_AuthorId",
                table: "Feeds");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Feeds");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Feeds");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AppUsers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
