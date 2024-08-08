using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Creators.Migrations
{
    /// <inheritdoc />
    public partial class PagesCalendarDonates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PageData",
                table: "PageData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreatorPage",
                table: "CreatorPage");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PageData");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CreatorPage");

            migrationBuilder.AddColumn<string>(
                name: "Id_Creator",
                table: "PageData",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatorPageId_Creator",
                table: "PageData",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Id_Donates",
                table: "Donates",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "CreatorPageId_Creator",
                table: "Donates",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Id_Creator",
                table: "CreatorPage",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "UserModelId",
                table: "CreatorPage",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Id_Calendar",
                table: "CalendarEvents",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PageData",
                table: "PageData",
                column: "Id_Creator");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreatorPage",
                table: "CreatorPage",
                column: "Id_Creator");

            migrationBuilder.CreateIndex(
                name: "IX_PageData_CreatorPageId_Creator",
                table: "PageData",
                column: "CreatorPageId_Creator");

            migrationBuilder.CreateIndex(
                name: "IX_Donates_CreatorPageId_Creator",
                table: "Donates",
                column: "CreatorPageId_Creator");

            migrationBuilder.CreateIndex(
                name: "IX_Donates_Id_Donates",
                table: "Donates",
                column: "Id_Donates");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorPage_UserModelId",
                table: "CreatorPage",
                column: "UserModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_Id_Calendar",
                table: "CalendarEvents",
                column: "Id_Calendar");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvents_CreatorPage_Id_Calendar",
                table: "CalendarEvents",
                column: "Id_Calendar",
                principalTable: "CreatorPage",
                principalColumn: "Id_Creator",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatorPage_AspNetUsers_Id_Creator",
                table: "CreatorPage",
                column: "Id_Creator",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatorPage_AspNetUsers_UserModelId",
                table: "CreatorPage",
                column: "UserModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Donates_CreatorPage_CreatorPageId_Creator",
                table: "Donates",
                column: "CreatorPageId_Creator",
                principalTable: "CreatorPage",
                principalColumn: "Id_Creator",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Donates_CreatorPage_Id_Donates",
                table: "Donates",
                column: "Id_Donates",
                principalTable: "CreatorPage",
                principalColumn: "Id_Creator",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PageData_CreatorPage_CreatorPageId_Creator",
                table: "PageData",
                column: "CreatorPageId_Creator",
                principalTable: "CreatorPage",
                principalColumn: "Id_Creator",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PageData_CreatorPage_Id_Creator",
                table: "PageData",
                column: "Id_Creator",
                principalTable: "CreatorPage",
                principalColumn: "Id_Creator",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvents_CreatorPage_Id_Calendar",
                table: "CalendarEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatorPage_AspNetUsers_Id_Creator",
                table: "CreatorPage");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatorPage_AspNetUsers_UserModelId",
                table: "CreatorPage");

            migrationBuilder.DropForeignKey(
                name: "FK_Donates_CreatorPage_CreatorPageId_Creator",
                table: "Donates");

            migrationBuilder.DropForeignKey(
                name: "FK_Donates_CreatorPage_Id_Donates",
                table: "Donates");

            migrationBuilder.DropForeignKey(
                name: "FK_PageData_CreatorPage_CreatorPageId_Creator",
                table: "PageData");

            migrationBuilder.DropForeignKey(
                name: "FK_PageData_CreatorPage_Id_Creator",
                table: "PageData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PageData",
                table: "PageData");

            migrationBuilder.DropIndex(
                name: "IX_PageData_CreatorPageId_Creator",
                table: "PageData");

            migrationBuilder.DropIndex(
                name: "IX_Donates_CreatorPageId_Creator",
                table: "Donates");

            migrationBuilder.DropIndex(
                name: "IX_Donates_Id_Donates",
                table: "Donates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreatorPage",
                table: "CreatorPage");

            migrationBuilder.DropIndex(
                name: "IX_CreatorPage_UserModelId",
                table: "CreatorPage");

            migrationBuilder.DropIndex(
                name: "IX_CalendarEvents_Id_Calendar",
                table: "CalendarEvents");

            migrationBuilder.DropColumn(
                name: "Id_Creator",
                table: "PageData");

            migrationBuilder.DropColumn(
                name: "CreatorPageId_Creator",
                table: "PageData");

            migrationBuilder.DropColumn(
                name: "CreatorPageId_Creator",
                table: "Donates");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "CreatorPage");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PageData",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id_Donates",
                table: "Donates",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id_Creator",
                table: "CreatorPage",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CreatorPage",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Id_Calendar",
                table: "CalendarEvents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PageData",
                table: "PageData",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreatorPage",
                table: "CreatorPage",
                column: "Id");
        }
    }
}
