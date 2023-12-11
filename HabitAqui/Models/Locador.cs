using System.ComponentModel;

namespace HabitAqui.Models;

public class Locador : DetalhesUtilizador
{
    [DisplayName("Data de início da subscrição")]
    public DateTime DataInicioSubscricao { get; set; }

    public string EstadoDaSubscricao { get; set; }

    public ICollection<Habitacao>? Habitacoes { get; set; } // pode ter 0 ou mais habitacoes

    [DisplayName("Funcionários")]
    public ICollection<DetalhesUtilizador> Administradores { get; set; } // tem de ter pelo menos 1 administrador
}