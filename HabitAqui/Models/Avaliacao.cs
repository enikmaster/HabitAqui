using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Avaliacao
{
    [Key] public int Id { get; set; }
    public DetalhesUtilizador Cliente { get; set; }
    public int HabitacaoId { get; set; } //precisamos ter a referencia para Habitacao
    public Habitacao Habitacao { get; set; }
    [Required] public int Nota { get; set; }
    public string Comentario { get; set; }
}