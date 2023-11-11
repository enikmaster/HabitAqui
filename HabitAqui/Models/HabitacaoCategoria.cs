using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class HabitacaoCategoria
{
    [Key]
    public int HabitacaoId { get; set; }
    public Habitacao Habitacao { get; set; }
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; }
}