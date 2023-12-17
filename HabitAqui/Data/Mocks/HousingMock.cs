using HabitAqui.Models;
using System;

namespace HabitAqui.Data.Mocks;

public static class HousingMock
{
    public static List<Habitacao> GenerateOneHabitacaoMock(List<Locador> locadores)
    {
        List<Habitacao> habitacoes = new List<Habitacao>();
        
       
        // Sample data arrays for variability
        string[] descriptions = {
        "Confortável apartamento com vista para o mar", "Moderno estúdio no centro da cidade",
        "Casa espaçosa em bairro tranquilo", "Charmoso loft em zona histórica",
        "Vivenda luxuosa com jardim", "Apartamento minimalista e moderno",
        "Moradia tradicional com terraço", "Estúdio elegante e funcional",
        "Casa de campo com vista para a natureza", "Penthouse com vista panorâmica",
        "Apartamento acolhedor e central", "Vila espaçosa com piscina"
    };
        string[] names = {
        "Refúgio do Mar", "Estúdio Central", "Casa da Paz",
        "Loft da Cidade", "Vivenda do Jardim", "Apartamento Zen",
        "Casa Terracotta", "Estúdio Moderno", "Casa do Campo",
        "Penthouse Panorâmica", "Lar Aconchegante", "Vila das Águas"
    };
        string[] cities = {
        "Lisboa", "Porto", "Coimbra", "Aveiro", "Funchal", "Braga",
        "Faro", "Évora", "Guarda", "Viseu", "Leiria", "Setúbal"
    };
        string[] addresses = {
        "Rua das Flores", "Avenida dos Aliados", "Praça da República",
        "Largo da Sé", "Rua do Sol", "Avenida da Liberdade",
        "Rua da Esperança", "Praça do Comércio", "Travessa do Fado",
        "Caminho do Mar", "Alameda dos Oceanos", "Rua dos Navegantes"
    };
        string[] postCodes = {
        "1250-096", "4000-123", "3000-213", "3800-001", "9000-020", "4700-030",
        "8000-123", "7000-555", "6300-853", "3500-001", "2400-123", "2900-001"
    };
        decimal[] areas = { 45.0m, 60.0m, 75.0m, 90.0m, 110.0m, 150.0m, 200.0m, 250.0m, 300.0m, 350.0m, 400.0m, 450.0m };

        decimal[] pricesPerNight = { 45.0m, 55.0m, 65.0m, 75.0m, 90.0m, 110.0m, 130.0m, 150.0m, 170.0m, 190.0m, 210.0m, 230.0m };

        int aux = 0;

        for (int h = 0; h < 12; h++) // 12 Habitacao objects
        {
            if(aux == 3) { aux = 0; }
            // Create new Habitacao with varied properties
            var habitacao = new Habitacao
            {
                
            LocadorId = locadores[aux++].Id,
                DetalhesHabitacao = new DetalhesHabitacao
                {
                    Area = areas[h], // Random area between 0 and 500
                    Descricao = descriptions[h % descriptions.Length],
                    Nome = names[h % names.Length],
                    Localizacao = new Localizacao
                    {
                        Cidade = cities[h % cities.Length],
                        CodigoPostal = postCodes[h % postCodes.Length],
                        Morada = addresses[h % addresses.Length],
                        Pais = "Portugal"
                    },
                    PrecoPorNoite = pricesPerNight[h] // Random price between 40 and 200
                },
                Active = true,
                Avaliacoes = null,
                Reservas = null,
                Categorias = null,
                Imagens = new List<Imagem>()
            };

            // Adding mock images to the habitacao with unique paths
            int startImageNumber = h * 5 + 1; // Start from 001 for the first habitacao
            for (int i = startImageNumber; i < startImageNumber + 5; i++)
            {
                string imagePath = $"habitacao0{i:00}.jpg"; // Ensuring two digits
                habitacao.Imagens.Add(new Imagem { Path = imagePath });
            }

            // Add the habitacao to the list of habitacoes
            habitacoes.Add(habitacao);
        }

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

}