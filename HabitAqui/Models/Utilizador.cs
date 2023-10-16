namespace HabitAqui.Models;

public class Utilizador
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public DetalhesUtilizador detalhesUtilizador { get; set; }
}