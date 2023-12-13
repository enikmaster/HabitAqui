using HabitAqui.Models;

namespace HabitAqui.Data.Mocks
{
    public static class LandlordMock
    {
        public static string locadorEmail = "locador@locador.com";
        public static Locador GenerateLandlordMockExample()
        {
            var funcionarioExemplo = new DetalhesUtilizador()
            {
                UserName = "funcionario@funcionario.com",
                Email = "funcionario@funcionario.com",
                Nome = "Funcionario",
                Apelido = "Marques",
                Nif = "987654321",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Localizacao = new Localizacao
                {
                    Morada = "Morada",
                    CodigoPostal = "1234-123",
                    Cidade = "Cidade",
                    Pais = "País"
                },
                Active = true,
            };

            var user = new Locador
            {
                UserName = "locador@locador.com",
                Email = locadorEmail,
                Nome = "Joao",
                Apelido = "Marques",
                Nif = "123456789",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Localizacao = new Localizacao
                {
                    Morada = "Morada",
                    CodigoPostal = "1234-123",
                    Cidade = "Cidade",
                    Pais = "País"
                },
                Active = true,
                DataInicioSubscricao = DateTime.Now,
                EstadoDaSubscricao = "Active",
                Administradores = new List<DetalhesUtilizador>() { funcionarioExemplo },
            };

            return user;
        }
    }
}
