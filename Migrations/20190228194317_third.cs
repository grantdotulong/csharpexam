using Microsoft.EntityFrameworkCore.Migrations;

namespace beltExamTwo.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "enthusiastCount",
                table: "Associations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "enthusiastCount",
                table: "Associations",
                nullable: false,
                defaultValue: 0);
        }
    }
}
