using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Locador
{
    [Key] public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; }

    public string EstadoDaSubscricao { get; set; }
    public bool Ativo { get; set; }

    public ICollection<Habitacao>? Habitacoes { get; set; } // pode ter 0 ou mais habitacoes

    [DisplayName("Funcionário(s) responsável(eis)")]
    public ICollection<DetalhesUtilizador> Administradores { get; set; } // tem de ter pelo menos 1 administrador
}