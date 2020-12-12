using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectRainforest.Migrations
{
    public partial class UpdateUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendorID",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorID",
                table: "AspNetUsers");
        }
    }
}
