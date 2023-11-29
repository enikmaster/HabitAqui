using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class RegistoEntrega
{
    [Key] public int Id { get; set; }
    public DetalhesUtilizador Utilizador { get; set; }
    [Required] public bool Danos { get; set; }
    public string Observacoes { get; set; }
    public ICollection<Opcional>? Opcionais { get; set; }
    public ICollection<ImagemRegistoEntrega>? Imagens { get; set; }
}