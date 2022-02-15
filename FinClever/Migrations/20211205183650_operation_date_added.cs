using Microsoft.EntityFrameworkCore.Migrations;

namespace FinClever.Migrations
{
    public partial class operation_date_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Date",
                table: "Operations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Operations");
        }
    }
}
