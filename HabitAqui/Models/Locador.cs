using System.ComponentModel;

namespace HabitAqui.Models;

public class Locador : DetalhesUtilizador
{
    //[Key] public int Id { get; set; }
    //[Required(ErrorMessage = "O nome é obrigatório")] public string Nome { get; set; }
    //public bool Ativo { get; set; }

    [DisplayName("Data de início da subscrição")]
    public DateTime DataInicioSubscricao { get; set; }

    public string EstadoDaSubscricao { get; set; }
    public ICollection<Habitacao>? Habitacoes { get; set; } // pode ter 0 ou mais habitacoes

    [DisplayName("Funcionário(s) responsável(eis)")]
    public ICollection<DetalhesUtilizador> Administradores { get; set; } // tem de ter pelo menos 1 administrador
}