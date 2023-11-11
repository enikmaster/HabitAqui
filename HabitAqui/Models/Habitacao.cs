using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Habitacao
{
    public int Id { get; set; }
    [Required] public Utilizador Funcionario { get; set; }
    [Required] public Locador Locador { get; set; }
    [Required] public DetalhesHabitacao DetalhesHabitacao { get; set; }
    [Required] public ICollection<HabitacaoCategoria> Categorias { get; set; }
    public ICollection<Avaliacao>? Avaliacoes { get; set; }
    public ICollection<Reserva>? Reservas { get; set; }
}