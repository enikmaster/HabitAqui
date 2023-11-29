using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Opcional
{
    [Key] public int Id { get; set; }
    [Required] public string Nome { get; set; }
    [Required] public string Estado { get; set; }
}