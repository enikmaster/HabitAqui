using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Migrations
{
    public partial class updateLocadores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Utilizador_FuncionarioId",
                table: "Habitacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizador_DetalhesUtilizadores_DetalhesUtilizadorId",
                table: "Utilizador");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizador_Locadores_LocadorId",
                table: "Utilizador");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Utilizador",
                table: "Utilizador");

            migrationBuilder.RenameTable(
                name: "Utilizador",
                newName: "Utilizadores");

            migrationBuilder.RenameColumn(
                name: "LocadorId",
                table: "Locadores",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Utilizador_LocadorId",
                table: "Utilizadores",
                newName: "IX_Utilizadores_LocadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Utilizador_DetalhesUtilizadorId",
                table: "Utilizadores",
                newName: "IX_Utilizadores_DetalhesUtilizadorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Utilizadores",
                table: "Utilizadores",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Utilizadores_FuncionarioId",
                table: "Habitacoes",
                column: "FuncionarioId",
                principalTable: "Utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_DetalhesUtilizadores_DetalhesUtilizadorId",
                table: "Utilizadores",
                column: "DetalhesUtilizadorId",
                principalTable: "DetalhesUtilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_Locadores_LocadorId",
                table: "Utilizadores",
                column: "LocadorId",
                principalTable: "Locadores",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Utilizadores_FuncionarioId",
                table: "Habitacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_DetalhesUtilizadores_DetalhesUtilizadorId",
                table: "Utilizadores");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_Locadores_LocadorId",
                table: "Utilizadores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Utilizadores",
                table: "Utilizadores");

            migrationBuilder.RenameTable(
                name: "Utilizadores",
                newName: "Utilizador");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Locadores",
                newName: "LocadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Utilizadores_LocadorId",
                table: "Utilizador",
                newName: "IX_Utilizador_LocadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Utilizadores_DetalhesUtilizadorId",
                table: "Utilizador",
                newName: "IX_Utilizador_DetalhesUtilizadorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Utilizador",
                table: "Utilizador",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Utilizador_FuncionarioId",
                table: "Habitacoes",
                column: "FuncionarioId",
                principalTable: "Utilizador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizador_DetalhesUtilizadores_DetalhesUtilizadorId",
                table: "Utilizador",
                column: "DetalhesUtilizadorId",
                principalTable: "DetalhesUtilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizador_Locadores_LocadorId",
                table: "Utilizador",
                column: "LocadorId",
                principalTable: "Locadores",
                principalColumn: "LocadorId");
        }
    }
}
