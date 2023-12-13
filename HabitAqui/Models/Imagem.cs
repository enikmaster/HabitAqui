using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitAqui.Models;

public class Imagem
{
    [Key] public int Id { get; set; }
    public string Path { get; set; }

    public int? HabitacaoId { get; set; }

    [ForeignKey("HabitacaoId")] public virtual Habitacao Habitacao { get; set; }
}