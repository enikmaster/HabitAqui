using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Migrations
{
    public partial class AddRegistoEntrega : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegistoEntregaId",
                table: "ImagensRegistoEntrega",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegistoEntregaId",
                table: "EquipamentosOpcionais",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RegistosEntrega",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FuncionarioID = table.Column<int>(type: "int", nullable: false),
                    Danos = table.Column<bool>(type: "bit", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistosEntrega", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagensRegistoEntrega_RegistoEntregaId",
                table: "ImagensRegistoEntrega",
                column: "RegistoEntregaId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipamentosOpcionais_RegistoEntregaId",
                table: "EquipamentosOpcionais",
                column: "RegistoEntregaId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipamentosOpcionais_RegistosEntrega_RegistoEntregaId",
                table: "EquipamentosOpcionais",
                column: "RegistoEntregaId",
                principalTable: "RegistosEntrega",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImagensRegistoEntrega_RegistosEntrega_RegistoEntregaId",
                table: "ImagensRegistoEntrega",
                column: "RegistoEntregaId",
                principalTable: "RegistosEntrega",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipamentosOpcionais_RegistosEntrega_RegistoEntregaId",
                table: "EquipamentosOpcionais");

            migrationBuilder.DropForeignKey(
                name: "FK_ImagensRegistoEntrega_RegistosEntrega_RegistoEntregaId",
                table: "ImagensRegistoEntrega");

            migrationBuilder.DropTable(
                name: "RegistosEntrega");

            migrationBuilder.DropIndex(
                name: "IX_ImagensRegistoEntrega_RegistoEntregaId",
                table: "ImagensRegistoEntrega");

            migrationBuilder.DropIndex(
                name: "IX_EquipamentosOpcionais_RegistoEntregaId",
                table: "EquipamentosOpcionais");

            migrationBuilder.DropColumn(
                name: "RegistoEntregaId",
                table: "ImagensRegistoEntrega");

            migrationBuilder.DropColumn(
                name: "RegistoEntregaId",
                table: "EquipamentosOpcionais");
        }
    }
}
