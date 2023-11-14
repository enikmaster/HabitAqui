using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Migrations
{
    public partial class UpdateUtilizadorHabitacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FuncionarioId",
                table: "Habitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Habitacoes_FuncionarioId",
                table: "Habitacoes",
                column: "FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Utilizador_FuncionarioId",
                table: "Habitacoes",
                column: "FuncionarioId",
                principalTable: "Utilizador",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Utilizador_FuncionarioId",
                table: "Habitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Habitacoes_FuncionarioId",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "FuncionarioId",
                table: "Habitacoes");
        }
    }
}
