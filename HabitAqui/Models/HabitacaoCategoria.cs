using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitAqui.Models;

public class HabitacaoCategoria
{
    [Key] public int Id { get; set; }
    public int HabitacaoId { get; set; }

    [ForeignKey("HabitacaoId")]
    public Habitacao Habitacao { get; set; }

    public int CategoriaId { get; set; }
    [ForeignKey("CategoriaId")]
    public virtual Categoria Categoria { get; set; }
}