using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Creators.Migrations
{
    /// <inheritdoc />
    public partial class PhotosFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HeartGroup",
                table: "PhotoHearts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CreatorPhotoId",
                table: "PhotoHearts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CommentsGroup",
                table: "PhotoComments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CreatorPhotoId",
                table: "PhotoComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Id_Photos",
                table: "CreatorPhoto",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "HeartGroup",
                table: "CreatorPhoto",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CommentsGroup",
                table: "CreatorPhoto",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CreatorPageId_Creator",
                table: "CreatorPhoto",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "CreatorPhoto",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CreatorPhoto_CommentsGroup",
                table: "CreatorPhoto",
                column: "CommentsGroup");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CreatorPhoto_HeartGroup",
                table: "CreatorPhoto",
                column: "HeartGroup");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoHearts_CreatorPhotoId",
                table: "PhotoHearts",
                column: "CreatorPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoHearts_HeartGroup",
                table: "PhotoHearts",
                column: "HeartGroup");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoComments_CommentsGroup",
                table: "PhotoComments",
                column: "CommentsGroup");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoComments_CreatorPhotoId",
                table: "PhotoComments",
                column: "CreatorPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorPhoto_CreatorPageId_Creator",
                table: "CreatorPhoto",
                column: "CreatorPageId_Creator");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorPhoto_Id_Photos",
                table: "CreatorPhoto",
                column: "Id_Photos");

            migrationBuilder.AddForeignKey(
                name: "FK_CreatorPhoto_CreatorPage_CreatorPageId_Creator",
                table: "CreatorPhoto",
                column: "CreatorPageId_Creator",
                principalTable: "CreatorPage",
                principalColumn: "Id_Creator",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatorPhoto_CreatorPage_Id_Photos",
                table: "CreatorPhoto",
                column: "Id_Photos",
                principalTable: "CreatorPage",
                principalColumn: "Id_Creator",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoComments_CreatorPhoto_CommentsGroup",
                table: "PhotoComments",
                column: "CommentsGroup",
                principalTable: "CreatorPhoto",
                principalColumn: "CommentsGroup",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoComments_CreatorPhoto_CreatorPhotoId",
                table: "PhotoComments",
                column: "CreatorPhotoId",
                principalTable: "CreatorPhoto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoHearts_CreatorPhoto_CreatorPhotoId",
                table: "PhotoHearts",
                column: "CreatorPhotoId",
                principalTable: "CreatorPhoto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoHearts_CreatorPhoto_HeartGroup",
                table: "PhotoHearts",
                column: "HeartGroup",
                principalTable: "CreatorPhoto",
                principalColumn: "HeartGroup",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreatorPhoto_CreatorPage_CreatorPageId_Creator",
                table: "CreatorPhoto");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatorPhoto_CreatorPage_Id_Photos",
                table: "CreatorPhoto");

            migrationBuilder.DropForeignKey(
                name: "FK_PhotoComments_CreatorPhoto_CommentsGroup",
                table: "PhotoComments");

            migrationBuilder.DropForeignKey(
                name: "FK_PhotoComments_CreatorPhoto_CreatorPhotoId",
                table: "PhotoComments");

            migrationBuilder.DropForeignKey(
                name: "FK_PhotoHearts_CreatorPhoto_CreatorPhotoId",
                table: "PhotoHearts");

            migrationBuilder.DropForeignKey(
                name: "FK_PhotoHearts_CreatorPhoto_HeartGroup",
                table: "PhotoHearts");

            migrationBuilder.DropIndex(
                name: "IX_PhotoHearts_CreatorPhotoId",
                table: "PhotoHearts");

            migrationBuilder.DropIndex(
                name: "IX_PhotoHearts_HeartGroup",
                table: "PhotoHearts");

            migrationBuilder.DropIndex(
                name: "IX_PhotoComments_CommentsGroup",
                table: "PhotoComments");

            migrationBuilder.DropIndex(
                name: "IX_PhotoComments_CreatorPhotoId",
                table: "PhotoComments");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CreatorPhoto_CommentsGroup",
                table: "CreatorPhoto");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CreatorPhoto_HeartGroup",
                table: "CreatorPhoto");

            migrationBuilder.DropIndex(
                name: "IX_CreatorPhoto_CreatorPageId_Creator",
                table: "CreatorPhoto");

            migrationBuilder.DropIndex(
                name: "IX_CreatorPhoto_Id_Photos",
                table: "CreatorPhoto");

            migrationBuilder.DropColumn(
                name: "CreatorPhotoId",
                table: "PhotoHearts");

            migrationBuilder.DropColumn(
                name: "CreatorPhotoId",
                table: "PhotoComments");

            migrationBuilder.DropColumn(
                name: "CreatorPageId_Creator",
                table: "CreatorPhoto");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "CreatorPhoto");

            migrationBuilder.AlterColumn<string>(
                name: "HeartGroup",
                table: "PhotoHearts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CommentsGroup",
                table: "PhotoComments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id_Photos",
                table: "CreatorPhoto",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "HeartGroup",
                table: "CreatorPhoto",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CommentsGroup",
                table: "CreatorPhoto",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
