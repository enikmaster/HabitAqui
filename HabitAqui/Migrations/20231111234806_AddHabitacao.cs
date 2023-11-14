using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Migrations
{
    public partial class AddHabitacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HabitacaoId",
                table: "Avaliacoes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Habitacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocadorId = table.Column<int>(type: "int", nullable: false),
                    DetalhesHabitacaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habitacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Habitacoes_DetalhesHabitacoes_DetalhesHabitacaoId",
                        column: x => x.DetalhesHabitacaoId,
                        principalTable: "DetalhesHabitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Habitacoes_Locadores_LocadorId",
                        column: x => x.LocadorId,
                        principalTable: "Locadores",
                        principalColumn: "LocadorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_HabitacaoId",
                table: "Reservas",
                column: "HabitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_HabitacaoId",
                table: "Avaliacoes",
                column: "HabitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Habitacoes_DetalhesHabitacaoId",
                table: "Habitacoes",
                column: "DetalhesHabitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Habitacoes_LocadorId",
                table: "Habitacoes",
                column: "LocadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Habitacoes_HabitacaoId",
                table: "Avaliacoes",
                column: "HabitacaoId",
                principalTable: "Habitacoes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Habitacoes_HabitacaoId",
                table: "Reservas",
                column: "HabitacaoId",
                principalTable: "Habitacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Habitacoes_HabitacaoId",
                table: "Avaliacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Habitacoes_HabitacaoId",
                table: "Reservas");

            migrationBuilder.DropTable(
                name: "Habitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_HabitacaoId",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Avaliacoes_HabitacaoId",
                table: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "HabitacaoId",
                table: "Avaliacoes");
        }
    }
}
