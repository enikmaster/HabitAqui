using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Migrations
{
    public partial class updateDetalhesUtilizador : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_DetalhesUtilizadores_DetalhesUtilizadorId",
                table: "Utilizadores");

            migrationBuilder.DropTable(
                name: "DetalhesUtilizadores");

            migrationBuilder.AlterColumn<string>(
                name: "DetalhesUtilizadorId",
                table: "Utilizadores",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Apelido",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LocalizacaoId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nif",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LocalizacaoId",
                table: "AspNetUsers",
                column: "LocalizacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Localizacoes_LocalizacaoId",
                table: "AspNetUsers",
                column: "LocalizacaoId",
                principalTable: "Localizacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_AspNetUsers_DetalhesUtilizadorId",
                table: "Utilizadores",
                column: "DetalhesUtilizadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Localizacoes_LocalizacaoId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_AspNetUsers_DetalhesUtilizadorId",
                table: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LocalizacaoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Apelido",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LocalizacaoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nif",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "DetalhesUtilizadorId",
                table: "Utilizadores",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "DetalhesUtilizadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocalizacaoId = table.Column<int>(type: "int", nullable: false),
                    Apelido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalhesUtilizadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalhesUtilizadores_Localizacoes_LocalizacaoId",
                        column: x => x.LocalizacaoId,
                        principalTable: "Localizacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalhesUtilizadores_LocalizacaoId",
                table: "DetalhesUtilizadores",
                column: "LocalizacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_DetalhesUtilizadores_DetalhesUtilizadorId",
                table: "Utilizadores",
                column: "DetalhesUtilizadorId",
                principalTable: "DetalhesUtilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
