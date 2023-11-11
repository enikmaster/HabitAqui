using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Migrations
{
    public partial class PWEBTESTE4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locadores",
                columns: table => new
                {
                    LocadorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstadoDaSubscricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locadores", x => x.LocadorId);
                });

            migrationBuilder.CreateTable(
                name: "Localizacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Morada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localizacoes", x => x.Id);
                });

           

            migrationBuilder.CreateTable(
                name: "Habitacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocadorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habitacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Habitacoes_Locadores_LocadorId",
                        column: x => x.LocadorId,
                        principalTable: "Locadores",
                        principalColumn: "LocadorId");
                });

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

            migrationBuilder.CreateTable(
                name: "DetalhesUtilizadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apelido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalizacaoId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Avaliacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtilizadorId = table.Column<int>(type: "int", nullable: false),
                    Nota = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HabitacaoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_Habitacoes_HabitacaoId",
                        column: x => x.HabitacaoId,
                        principalTable: "Habitacoes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HabitacaoCaregorias",
                columns: table => new
                {
                    HabitacaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HabitacaoId1 = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitacaoCaregorias", x => x.HabitacaoId);
                    table.ForeignKey(
                        name: "FK_HabitacaoCaregorias_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HabitacaoCaregorias_Habitacoes_HabitacaoId1",
                        column: x => x.HabitacaoId1,
                        principalTable: "Habitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    table.ForeignKey(
                        name: "FK_Reservas_Habitacoes_HabitacaoId",
                        column: x => x.HabitacaoId,
                        principalTable: "Habitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Imagenshabitacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DetalhesHabitacaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagenshabitacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Imagenshabitacoes_DetalhesHabitacoes_DetalhesHabitacaoId",
                        column: x => x.DetalhesHabitacaoId,
                        principalTable: "DetalhesHabitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DetalhesUtilizadorId = table.Column<int>(type: "int", nullable: false),
                    LocadorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Utilizadores_DetalhesUtilizadores_DetalhesUtilizadorId",
                        column: x => x.DetalhesUtilizadorId,
                        principalTable: "DetalhesUtilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Utilizadores_Locadores_LocadorId",
                        column: x => x.LocadorId,
                        principalTable: "Locadores",
                        principalColumn: "LocadorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistosEntregas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FuncionarioID = table.Column<int>(type: "int", nullable: false),
                    Danos = table.Column<bool>(type: "bit", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReservaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistosEntregas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistosEntregas_Reservas_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reservas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipamentosOpcionais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistoEntregaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipamentosOpcionais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipamentosOpcionais_RegistosEntregas_RegistoEntregaId",
                        column: x => x.RegistoEntregaId,
                        principalTable: "RegistosEntregas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ImagemRegistoEntrega",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetalhesHabitacaoId = table.Column<int>(type: "int", nullable: false),
                    RegistoEntregaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagemRegistoEntrega", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagemRegistoEntrega_DetalhesHabitacoes_DetalhesHabitacaoId",
                        column: x => x.DetalhesHabitacaoId,
                        principalTable: "DetalhesHabitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImagemRegistoEntrega_RegistosEntregas_RegistoEntregaId",
                        column: x => x.RegistoEntregaId,
                        principalTable: "RegistosEntregas",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Habitacoes",
                columns: new[] { "Id", "LocadorId" },
                values: new object[] { 1, null });

            migrationBuilder.InsertData(
                table: "Habitacoes",
                columns: new[] { "Id", "LocadorId" },
                values: new object[] { 2, null });

            

            

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_HabitacaoId",
                table: "Avaliacoes",
                column: "HabitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalhesHabitacoes_localizacaoId",
                table: "DetalhesHabitacoes",
                column: "localizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalhesUtilizadores_LocalizacaoId",
                table: "DetalhesUtilizadores",
                column: "LocalizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipamentosOpcionais_RegistoEntregaId",
                table: "EquipamentosOpcionais",
                column: "RegistoEntregaId");

            migrationBuilder.CreateIndex(
                name: "IX_HabitacaoCaregorias_CategoriaId",
                table: "HabitacaoCaregorias",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_HabitacaoCaregorias_HabitacaoId1",
                table: "HabitacaoCaregorias",
                column: "HabitacaoId1");

            migrationBuilder.CreateIndex(
                name: "IX_Habitacoes_LocadorId",
                table: "Habitacoes",
                column: "LocadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagemRegistoEntrega_DetalhesHabitacaoId",
                table: "ImagemRegistoEntrega",
                column: "DetalhesHabitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagemRegistoEntrega_RegistoEntregaId",
                table: "ImagemRegistoEntrega",
                column: "RegistoEntregaId");

            migrationBuilder.CreateIndex(
                name: "IX_Imagenshabitacoes_DetalhesHabitacaoId",
                table: "Imagenshabitacoes",
                column: "DetalhesHabitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistosEntregas_ReservaId",
                table: "RegistosEntregas",
                column: "ReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_HabitacaoId",
                table: "Reservas",
                column: "HabitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_DetalhesUtilizadorId",
                table: "Utilizadores",
                column: "DetalhesUtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_LocadorId",
                table: "Utilizadores",
                column: "LocadorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Avaliacoes");

            migrationBuilder.DropTable(
                name: "EquipamentosOpcionais");

            migrationBuilder.DropTable(
                name: "HabitacaoCaregorias");

            migrationBuilder.DropTable(
                name: "ImagemRegistoEntrega");

            migrationBuilder.DropTable(
                name: "Imagenshabitacoes");

            migrationBuilder.DropTable(
                name: "Utilizadores");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "RegistosEntregas");

            migrationBuilder.DropTable(
                name: "DetalhesHabitacoes");

            migrationBuilder.DropTable(
                name: "DetalhesUtilizadores");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Localizacoes");

            migrationBuilder.DropTable(
                name: "Habitacoes");

            migrationBuilder.DropTable(
                name: "Locadores");
        }
    }
}
