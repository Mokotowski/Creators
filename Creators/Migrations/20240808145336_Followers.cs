using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Creators.Migrations
{
    /// <inheritdoc />
    public partial class Followers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Calendar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOnly = table.Column<DateOnly>(type: "date", nullable: false),
                    Start = table.Column<TimeOnly>(type: "time", nullable: false),
                    End = table.Column<TimeOnly>(type: "time", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreatorPage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Calendar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Photos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Donates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Account_Numer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Account_Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Site_Commission = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatorPage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreatorPhoto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Photos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentsGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeartGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatorPhoto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Donates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Donates = table.Column<int>(type: "int", nullable: false),
                    Donator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false),
                    Count = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Followers",
                columns: table => new
                {
                    Id_Creator = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id_User = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followers", x => new { x.Id_Creator, x.Id_User });
                    table.ForeignKey(
                        name: "FK_Followers_AspNetUsers_Id_User",
                        column: x => x.Id_User,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilPicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailNotificationsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    BioLinks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhotoComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_User = table.Column<int>(type: "int", nullable: false),
                    CommentsGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false),
                    Hidden = table.Column<bool>(type: "bit", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoComments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhotoHearts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_User = table.Column<int>(type: "int", nullable: false),
                    HeartGroup = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoHearts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Followers_Id_User",
                table: "Followers",
                column: "Id_User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarEvents");

            migrationBuilder.DropTable(
                name: "CreatorPage");

            migrationBuilder.DropTable(
                name: "CreatorPhoto");

            migrationBuilder.DropTable(
                name: "Donates");

            migrationBuilder.DropTable(
                name: "Followers");

            migrationBuilder.DropTable(
                name: "PageData");

            migrationBuilder.DropTable(
                name: "PhotoComments");

            migrationBuilder.DropTable(
                name: "PhotoHearts");
        }
    }
}
