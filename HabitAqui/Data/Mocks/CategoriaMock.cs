using HabitAqui.Models;

namespace HabitAqui.Data.Mocks
{
    public static class CategoriaMock
    {
        public static List<Categoria> GenerateListCategorias()
        {
            List <Categoria> categorias = new List<Categoria>();
            var categoria = new Categoria
            {
                Nome = "Moradia",
                Active = true
            };
            categorias.Add(categoria);

            categoria = new Categoria
            {
                Nome = "Apartamento",
                Active = true
            };
            categorias.Add(categoria);

            categoria = new Categoria
            {
                Nome = "Estúdio",
                Active = true
            };
            categorias.Add(categoria);

            categoria = new Categoria
            {
                Nome = "Prédio",
                Active = true
            };
            categorias.Add(categoria);

            categoria = new Categoria
            {
                Nome = "Terreno",
                Active = true
            };

            categorias.Add(categoria);

            categoria = new Categoria
            {
                Nome = "Duplex",
                Active = true
            };

            categorias.Add(categoria);

            categoria = new Categoria
            {
                Nome = "Palacete",
                Active = true
            };
            categorias.Add(categoria);

            return categorias;
        }
    }
}