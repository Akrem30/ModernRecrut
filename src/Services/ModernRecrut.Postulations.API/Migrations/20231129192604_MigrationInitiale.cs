using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModernRecrut.Postulations.API.Migrations
{
    public partial class MigrationInitiale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Postulations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PretentionSalariale = table.Column<decimal>(type: "TEXT", nullable: false),
                    DateDisponibilite = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CandidatID = table.Column<Guid>(type: "TEXT", nullable: false),
                    OffreEmploiID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postulations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    NomEmetteur = table.Column<string>(type: "TEXT", nullable: true),
                    PostulationID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Postulations_PostulationID",
                        column: x => x.PostulationID,
                        principalTable: "Postulations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_PostulationID",
                table: "Notes",
                column: "PostulationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Postulations");
        }
    }
}
