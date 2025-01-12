using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dash.Migrations
{
    public partial class AddedMoreToEventCalendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForegroundColor",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForegroundColor",
                table: "Events");
        }
    }
}
