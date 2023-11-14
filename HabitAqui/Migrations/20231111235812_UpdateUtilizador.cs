using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Migrations
{
    public partial class UpdateUtilizador : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilizador_Locadores_LocadorId",
                table: "Utilizador");

            migrationBuilder.AlterColumn<int>(
                name: "LocadorId",
                table: "Utilizador",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizador_Locadores_LocadorId",
                table: "Utilizador",
                column: "LocadorId",
                principalTable: "Locadores",
                principalColumn: "LocadorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilizador_Locadores_LocadorId",
                table: "Utilizador");

            migrationBuilder.AlterColumn<int>(
                name: "LocadorId",
                table: "Utilizador",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizador_Locadores_LocadorId",
                table: "Utilizador",
                column: "LocadorId",
                principalTable: "Locadores",
                principalColumn: "LocadorId",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
