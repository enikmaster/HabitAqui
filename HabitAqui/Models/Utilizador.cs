using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Utilizador
{
    [Key]
    public int Id { get; set; }

    public DetalhesUtilizador DetalhesUtilizador { get; set; }
    public Locador Locador { get; set; }
}