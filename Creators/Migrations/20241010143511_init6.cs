using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Creators.Migrations
{
    /// <inheritdoc />
    public partial class init6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Account_Balance",
                table: "CreatorPage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Account_Balance",
                table: "CreatorPage",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
