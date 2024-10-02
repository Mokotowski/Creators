using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Creators.Migrations
{
    /// <inheritdoc />
    public partial class init5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoComments_CreatorPhoto_CommentsGroup",
                table: "PhotoComments");

            migrationBuilder.AlterColumn<string>(
                name: "CommentsGroup",
                table: "CreatorPhoto",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CreatorPhoto_CommentsGroup",
                table: "CreatorPhoto",
                column: "CommentsGroup");

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoComments_CreatorPhoto_CommentsGroup",
                table: "PhotoComments",
                column: "CommentsGroup",
                principalTable: "CreatorPhoto",
                principalColumn: "CommentsGroup",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoComments_CreatorPhoto_CommentsGroup",
                table: "PhotoComments");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CreatorPhoto_CommentsGroup",
                table: "CreatorPhoto");

            migrationBuilder.AlterColumn<string>(
                name: "CommentsGroup",
                table: "CreatorPhoto",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoComments_CreatorPhoto_CommentsGroup",
                table: "PhotoComments",
                column: "CommentsGroup",
                principalTable: "CreatorPhoto",
                principalColumn: "HeartGroup",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
