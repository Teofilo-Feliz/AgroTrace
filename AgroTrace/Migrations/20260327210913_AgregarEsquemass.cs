using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroTrace.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEsquemass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Usuarios_UsuarioId",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens");

            migrationBuilder.EnsureSchema(
                name: "Ganaderia");

            migrationBuilder.EnsureSchema(
                name: "Finanzas");

            migrationBuilder.EnsureSchema(
                name: "Produccion");

            migrationBuilder.EnsureSchema(
                name: "Seguridad");

            migrationBuilder.EnsureSchema(
                name: "Sanidad");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                schema: "usuarios",
                newName: "Usuarios",
                newSchema: "Seguridad");

            migrationBuilder.RenameTable(
                name: "Tratamientos",
                newName: "Tratamientos",
                newSchema: "Sanidad");

            migrationBuilder.RenameTable(
                name: "TiposProduciones",
                newName: "TiposProduciones",
                newSchema: "Produccion");

            migrationBuilder.RenameTable(
                name: "TiposGastos",
                newName: "TiposGastos",
                newSchema: "Finanzas");

            migrationBuilder.RenameTable(
                name: "TiposAnimales",
                newName: "TiposAnimales",
                newSchema: "Ganaderia");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "Seguridad");

            migrationBuilder.RenameTable(
                name: "Razas",
                newName: "Razas",
                newSchema: "Ganaderia");

            migrationBuilder.RenameTable(
                name: "Producciones",
                newName: "Producciones",
                newSchema: "Produccion");

            migrationBuilder.RenameTable(
                name: "ProduccionDetalles",
                newName: "ProduccionDetalles",
                newSchema: "Produccion");

            migrationBuilder.RenameTable(
                name: "Ingresos",
                newName: "Ingresos",
                newSchema: "Finanzas");

            migrationBuilder.RenameTable(
                name: "Gastos",
                newName: "Gastos",
                newSchema: "Finanzas");

            migrationBuilder.RenameTable(
                name: "Fincas",
                newName: "Fincas",
                newSchema: "Ganaderia");

            migrationBuilder.RenameTable(
                name: "EstadoAnimales",
                newName: "EstadoAnimales",
                newSchema: "Ganaderia");

            migrationBuilder.RenameTable(
                name: "Animales",
                newName: "Animales",
                newSchema: "Ganaderia");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "RefreshToken",
                newSchema: "Seguridad");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_UsuarioId",
                schema: "Seguridad",
                table: "RefreshToken",
                newName: "IX_RefreshToken_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshToken",
                schema: "Seguridad",
                table: "RefreshToken",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Usuarios_UsuarioId",
                schema: "Seguridad",
                table: "RefreshToken",
                column: "UsuarioId",
                principalSchema: "Seguridad",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Usuarios_UsuarioId",
                schema: "Seguridad",
                table: "RefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshToken",
                schema: "Seguridad",
                table: "RefreshToken");

            migrationBuilder.EnsureSchema(
                name: "usuarios");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                schema: "Seguridad",
                newName: "Usuarios",
                newSchema: "usuarios");

            migrationBuilder.RenameTable(
                name: "Tratamientos",
                schema: "Sanidad",
                newName: "Tratamientos");

            migrationBuilder.RenameTable(
                name: "TiposProduciones",
                schema: "Produccion",
                newName: "TiposProduciones");

            migrationBuilder.RenameTable(
                name: "TiposGastos",
                schema: "Finanzas",
                newName: "TiposGastos");

            migrationBuilder.RenameTable(
                name: "TiposAnimales",
                schema: "Ganaderia",
                newName: "TiposAnimales");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "Seguridad",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Razas",
                schema: "Ganaderia",
                newName: "Razas");

            migrationBuilder.RenameTable(
                name: "Producciones",
                schema: "Produccion",
                newName: "Producciones");

            migrationBuilder.RenameTable(
                name: "ProduccionDetalles",
                schema: "Produccion",
                newName: "ProduccionDetalles");

            migrationBuilder.RenameTable(
                name: "Ingresos",
                schema: "Finanzas",
                newName: "Ingresos");

            migrationBuilder.RenameTable(
                name: "Gastos",
                schema: "Finanzas",
                newName: "Gastos");

            migrationBuilder.RenameTable(
                name: "Fincas",
                schema: "Ganaderia",
                newName: "Fincas");

            migrationBuilder.RenameTable(
                name: "EstadoAnimales",
                schema: "Ganaderia",
                newName: "EstadoAnimales");

            migrationBuilder.RenameTable(
                name: "Animales",
                schema: "Ganaderia",
                newName: "Animales");

            migrationBuilder.RenameTable(
                name: "RefreshToken",
                schema: "Seguridad",
                newName: "RefreshTokens");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_UsuarioId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Usuarios_UsuarioId",
                table: "RefreshTokens",
                column: "UsuarioId",
                principalSchema: "usuarios",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
