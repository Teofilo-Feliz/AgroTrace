using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroTrace.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEsquemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "usuarios");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                newName: "Usuarios",
                newSchema: "usuarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Usuarios",
                schema: "usuarios",
                newName: "Usuarios");
        }
    }
}
