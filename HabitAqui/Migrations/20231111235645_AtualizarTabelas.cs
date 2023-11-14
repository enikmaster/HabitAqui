using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Migrations
{
    public partial class AtualizarTabelas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImagensHabitacao_DetalhesHabitacoes_DetalhesHabitacaoId",
                table: "ImagensHabitacao");

            migrationBuilder.DropForeignKey(
                name: "FK_ImagensRegistoEntrega_RegistosEntrega_RegistoEntregaId",
                table: "ImagensRegistoEntrega");

            migrationBuilder.AlterColumn<int>(
                name: "RegistoEntregaId",
                table: "ImagensRegistoEntrega",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DetalhesHabitacaoId",
                table: "ImagensHabitacao",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Utilizador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DetalhesUtilizadorId = table.Column<int>(type: "int", nullable: false),
                    LocadorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Utilizador_DetalhesUtilizadores_DetalhesUtilizadorId",
                        column: x => x.DetalhesUtilizadorId,
                        principalTable: "DetalhesUtilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Utilizador_Locadores_LocadorId",
                        column: x => x.LocadorId,
                        principalTable: "Locadores",
                        principalColumn: "LocadorId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Utilizador_DetalhesUtilizadorId",
                table: "Utilizador",
                column: "DetalhesUtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizador_LocadorId",
                table: "Utilizador",
                column: "LocadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImagensHabitacao_DetalhesHabitacoes_DetalhesHabitacaoId",
                table: "ImagensHabitacao",
                column: "DetalhesHabitacaoId",
                principalTable: "DetalhesHabitacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImagensRegistoEntrega_RegistosEntrega_RegistoEntregaId",
                table: "ImagensRegistoEntrega",
                column: "RegistoEntregaId",
                principalTable: "RegistosEntrega",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImagensHabitacao_DetalhesHabitacoes_DetalhesHabitacaoId",
                table: "ImagensHabitacao");

            migrationBuilder.DropForeignKey(
                name: "FK_ImagensRegistoEntrega_RegistosEntrega_RegistoEntregaId",
                table: "ImagensRegistoEntrega");

            migrationBuilder.DropTable(
                name: "Utilizador");

            migrationBuilder.AlterColumn<int>(
                name: "RegistoEntregaId",
                table: "ImagensRegistoEntrega",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DetalhesHabitacaoId",
                table: "ImagensHabitacao",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ImagensHabitacao_DetalhesHabitacoes_DetalhesHabitacaoId",
                table: "ImagensHabitacao",
                column: "DetalhesHabitacaoId",
                principalTable: "DetalhesHabitacoes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImagensRegistoEntrega_RegistosEntrega_RegistoEntregaId",
                table: "ImagensRegistoEntrega",
                column: "RegistoEntregaId",
                principalTable: "RegistosEntrega",
                principalColumn: "Id");
        }
    }
}
