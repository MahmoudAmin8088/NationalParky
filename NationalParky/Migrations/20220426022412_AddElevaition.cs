using Microsoft.EntityFrameworkCore.Migrations;

namespace NationalParky.Migrations
{
    public partial class AddElevaition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Elevaition",
                table: "Trails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Elevaition",
                table: "Trails");
        }
    }
}
