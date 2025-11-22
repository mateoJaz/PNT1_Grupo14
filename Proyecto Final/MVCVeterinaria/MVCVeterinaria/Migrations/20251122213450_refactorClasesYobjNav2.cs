using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCVeterinaria.Migrations
{
    /// <inheritdoc />
    public partial class refactorClasesYobjNav2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Veterinario",
                table: "Evento");

            migrationBuilder.CreateIndex(
                name: "IX_Evento_VeterinarioId",
                table: "Evento",
                column: "VeterinarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evento_Veterinario_VeterinarioId",
                table: "Evento",
                column: "VeterinarioId",
                principalTable: "Veterinario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evento_Veterinario_VeterinarioId",
                table: "Evento");

            migrationBuilder.DropIndex(
                name: "IX_Evento_VeterinarioId",
                table: "Evento");

            migrationBuilder.AddColumn<int>(
                name: "Veterinario",
                table: "Evento",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
