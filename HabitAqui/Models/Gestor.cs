namespace HabitAqui.Models;

public class Gestor
{
    private DetalhesUtilizador detalhesUtilizador { get; set; }

    public ICollection<Habitacao> Habitacoes { get; set; }
    public ICollection<Reserva> Reservas { get; set; }
}