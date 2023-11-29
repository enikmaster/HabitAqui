using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Avaliacao
{
    [Key] public int Id { get; set; }
    public Cliente Cliente { get; set; }
    public Habitacao Habitacao { get; set; }
    [Required] public int Nota { get; set; }
    public string Comentario { get; set; }
}