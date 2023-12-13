using HabitAqui.Models;

namespace HabitAqui.Data.Mocks
{
    public static class CategoriaMock
    {
        public static Categoria GenerateCategoria()
        {
            return new Categoria
            {
                Nome = "Categoria Exemplo",
                Active = true,
                Categorias = null
            };
        }
    }
}