using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Migrations
{
    public partial class AddHabitacaoCategoria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HabitacoesCategorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HabitacaoId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitacoesCategorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HabitacoesCategorias_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HabitacoesCategorias_Habitacoes_HabitacaoId",
                        column: x => x.HabitacaoId,
                        principalTable: "Habitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HabitacoesCategorias_CategoriaId",
                table: "HabitacoesCategorias",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_HabitacoesCategorias_HabitacaoId",
                table: "HabitacoesCategorias",
                column: "HabitacaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HabitacoesCategorias");
        }
    }
}
