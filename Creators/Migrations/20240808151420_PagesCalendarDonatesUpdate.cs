using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Creators.Migrations
{
    /// <inheritdoc />
    public partial class PagesCalendarDonatesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Donates");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Donates");

            migrationBuilder.AddColumn<DateTime>(
                name: "Since",
                table: "Followers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Donates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Since",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Donates");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Donates",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Time",
                table: "Donates",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}
