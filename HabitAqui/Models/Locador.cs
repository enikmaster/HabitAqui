using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Locador
{
    [Key]
    public int LocadorId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; }

    public string EstadoDaSubscricao { get; set; }
    public bool Ativo { get; set; }
    public ICollection<Habitacao> Habitacoes { get; set; }
    public ICollection<Utilizador> Funcionarios { get; set; }
}