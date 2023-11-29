using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class ImagemRegistoEntrega
{
    [Key] public int Id { get; set; }
    public Imagem Imagem { get; set; }
    public RegistoEntrega RegistoEntrega { get; set; }
}