namespace HabitAqui.Models;

public class Habitacao
{
    public int Id { get; set; }
    public Guid Guid { get; set; }

    public DetalhesHabitacao DetalhesHabitacao { get; set; }

    public Gestor Gestor { get; set; }

    public Categoria Categoria { get; set; }
    public double Avaliacao { get; set; }
}