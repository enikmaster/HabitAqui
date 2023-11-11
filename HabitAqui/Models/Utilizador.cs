namespace HabitAqui.Models;

public class Utilizador
{
    public int Id { get; set; }

    public DetalhesUtilizador DetalhesUtilizador { get; set; }
    public Locador Locador { get; set; }
}