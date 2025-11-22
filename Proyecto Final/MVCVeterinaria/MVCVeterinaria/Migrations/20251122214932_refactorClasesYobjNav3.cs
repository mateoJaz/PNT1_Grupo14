using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCVeterinaria.Migrations
{
    /// <inheritdoc />
    public partial class refactorClasesYobjNav3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "vivo",
                table: "Mascota",
                newName: "Vivo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Vivo",
                table: "Mascota",
                newName: "vivo");
        }
    }
}
