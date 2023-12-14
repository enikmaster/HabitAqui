using HabitAqui.Models;

namespace HabitAqui.Data.Mocks;

public static class HousingMock
{
    public static Habitacao GenerateOneHabitacaoMock(string locadorId)
    {
            var habitacao = new Habitacao
            {
                LocadorId = locadorId,
                DetalhesHabitacao = new DetalhesHabitacao
                {
                    Area = 150,
                    Descricao = "Habitação com 150m2",
                    Nome = "Habitação Santa Clara",
                    Localizacao = new Localizacao
                    {
                        Cidade = "Coimbra",
                        CodigoPostal = "3030-300",
                        Morada = "Rua do Brasil 330",
                        Pais = "Portugal"
                    },
                    PrecoPorNoite = 80
                },
                Active = true,
                Avaliacoes = null,
                Reservas = null,
                Categorias = null,
                Imagens = new List<Imagem>()
            };

//
        string basePath = $"imgs/habitacoes/{habitacao.Id}/";

        // Adding mock images to the habitacao with the Habitacao ID in the path
        habitacao.Imagens.Add(new Imagem { Path = "habitacao001.jpg" });
        habitacao.Imagens.Add(new Imagem { Path = "habitacao002.jpg" });
        habitacao.Imagens.Add(new Imagem { Path = "habitacao003.jpg" });
        habitacao.Imagens.Add(new Imagem { Path = "habitacao004.jpg" });
        habitacao.Imagens.Add(new Imagem { Path = "habitacao005.jpg" });
        // Add more images as needed

        return habitacao;
    }

    public static HabitacaoCategoria GenerateHabitacaoCategoria(Habitacao habitacao, Categoria categoria)
    {
        return new HabitacaoCategoria
        {
            HabitacaoId = habitacao.Id,
            CategoriaId = categoria.Id
        };
    }


    public static Categoria generateCategoriaHabitacao()
    {
        return new Categoria
        {
            Active = true,
            Nome = "Categoria1",
            Categorias = null
        };
    }
}