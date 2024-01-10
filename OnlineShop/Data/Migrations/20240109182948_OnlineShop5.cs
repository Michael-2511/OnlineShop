using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Data.Migrations
{
    public partial class OnlineShop5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Stars",
                table: "Reviews",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stars",
                table: "Reviews");
        }
    }
}
