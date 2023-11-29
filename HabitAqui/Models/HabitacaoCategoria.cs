using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class HabitacaoCategoria
{
    [Key] public int Id { get; set; }
    public Habitacao Habitacao { get; set; }
    public Categoria Categoria { get; set; }
}