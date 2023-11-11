using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Avaliacao
{
    public int Id { get; set; }
    [Required] public int UtilizadorId { get; set; }
    [Required] public int Nota { get; set; }
    public string Comentario { get; set; }
}