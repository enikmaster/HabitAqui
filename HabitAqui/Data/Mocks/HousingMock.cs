using HabitAqui.Models;

namespace HabitAqui.Data.Mocks;

public static class HousingMock
{
    public static List<Habitacao> GenerateOneHabitacaoMock(string locadorId)
    {
        List<Habitacao> habitacoes = new List<Habitacao>();

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
        habitacoes.Add(habitacao);

         habitacao = new Habitacao
        {
            LocadorId = locadorId,
            DetalhesHabitacao = new DetalhesHabitacao
            {
                Area = 100,
                Descricao = "Habitação com 100m2",
                Nome = "Habitação Cruz de Celas",
                Localizacao = new Localizacao
                {
                    Cidade = "Coimbra",
                    CodigoPostal = "3000-319",
                    Morada = "Praceta Padre José Anchieta",
                    Pais = "Portugal"
                },
                PrecoPorNoite = 75
            },
            Active = true,
            Avaliacoes = null,
            Reservas = null,
            Categorias = null,
            Imagens = new List<Imagem>()
        };

        //
        basePath = $"imgs/habitacoes/{habitacao.Id}/";

        // Adding mock images to the habitacao with the Habitacao ID in the path
        habitacao.Imagens.Add(new Imagem { Path = "habitacao006.jpg" });
        habitacao.Imagens.Add(new Imagem { Path = "habitacao007.jpg" });
        habitacao.Imagens.Add(new Imagem { Path = "habitacao008.jpg" });
        habitacao.Imagens.Add(new Imagem { Path = "habitacao009.jpg" });
        habitacao.Imagens.Add(new Imagem { Path = "habitacao010.jpg" });
        habitacoes.Add(habitacao);
        ///////////////
        ///
         habitacao = new Habitacao
        {
            LocadorId = locadorId,
            DetalhesHabitacao = new DetalhesHabitacao
            {
                Area = 200,
                Descricao = "Habitação com 200m2",
                Nome = "Habitação Buarcos",
                Localizacao = new Localizacao
                {
                    Cidade = "Figueira da Foz",
                    CodigoPostal = "3400-400",
                    Morada = "Rua do Barco",
                    Pais = "Portugal"
                },
                PrecoPorNoite = 35
            },
            Active = true,
            Avaliacoes = null,
            Reservas = null,
            Categorias = null,
            Imagens = new List<Imagem>()
        };

        //
        basePath = $"imgs/habitacoes/{habitacao.Id}/";

        // Adding mock images to the habitacao with the Habitacao ID in the path
        habitacao.Imagens.Add(new Imagem { Path = "habitacao011.jpg" });
        habitacao.Imagens.Add(new Imagem { Path = "habitacao012.jpg" });
        habitacao.Imagens.Add(new Imagem { Path = "habitacao013.jpg" });
        habitacao.Imagens.Add(new Imagem { Path = "habitacao014.jpg" });
        habitacao.Imagens.Add(new Imagem { Path = "habitacao015.jpg" });
        habitacoes.Add(habitacao);


        return habitacoes;
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