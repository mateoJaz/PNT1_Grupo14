using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCVeterinaria.Migrations
{
    /// <inheritdoc />
    public partial class FechaMascotas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Edad",
                table: "Mascota");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Mascota",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "Mascota");

            migrationBuilder.AddColumn<int>(
                name: "Edad",
                table: "Mascota",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
