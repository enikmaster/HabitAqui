using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Migrations
{
    public partial class AddReserva : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservaId",
                table: "RegistosEntrega",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    HabitacaoId = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistosEntrega_ReservaId",
                table: "RegistosEntrega",
                column: "ReservaId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistosEntrega_Reservas_ReservaId",
                table: "RegistosEntrega",
                column: "ReservaId",
                principalTable: "Reservas",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistosEntrega_Reservas_ReservaId",
                table: "RegistosEntrega");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_RegistosEntrega_ReservaId",
                table: "RegistosEntrega");

            migrationBuilder.DropColumn(
                name: "ReservaId",
                table: "RegistosEntrega");
        }
    }
}
