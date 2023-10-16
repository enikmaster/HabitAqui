namespace HabitAqui.Models;

public class Administrador
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public DetalhesUtilizador detalhesUtilizador { get; set; }
}