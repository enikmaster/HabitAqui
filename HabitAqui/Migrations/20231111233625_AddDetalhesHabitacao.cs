using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Migrations
{
    public partial class AddDetalhesHabitacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DetalhesHabitacaoId",
                table: "ImagensHabitacao",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DetalhesHabitacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Area = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoPorNoite = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    localizacaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalhesHabitacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalhesHabitacoes_Localizacoes_localizacaoId",
                        column: x => x.localizacaoId,
                        principalTable: "Localizacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagensHabitacao_DetalhesHabitacaoId",
                table: "ImagensHabitacao",
                column: "DetalhesHabitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalhesHabitacoes_localizacaoId",
                table: "DetalhesHabitacoes",
                column: "localizacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImagensHabitacao_DetalhesHabitacoes_DetalhesHabitacaoId",
                table: "ImagensHabitacao",
                column: "DetalhesHabitacaoId",
                principalTable: "DetalhesHabitacoes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImagensHabitacao_DetalhesHabitacoes_DetalhesHabitacaoId",
                table: "ImagensHabitacao");

            migrationBuilder.DropTable(
                name: "DetalhesHabitacoes");

            migrationBuilder.DropIndex(
                name: "IX_ImagensHabitacao_DetalhesHabitacaoId",
                table: "ImagensHabitacao");

            migrationBuilder.DropColumn(
                name: "DetalhesHabitacaoId",
                table: "ImagensHabitacao");
        }
    }
}
