using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModernRecrut.Emplois.Infrastructure.Migrations
{
    public partial class SuppressionProprieteNom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nom",
                table: "Emplois");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "Emplois",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
