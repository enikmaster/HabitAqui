using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class ImagemHabitacao
{
    [Key] public int Id { get; set; }
    public Imagem Imagem { get; set; }
    public Habitacao Habitacao { get; set; }
}