using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Cliente : DetalhesUtilizador
{
    public ICollection<Reserva> Reservas { get; set; }
    public ICollection<RegistoEntrega> RegistoEntregas { get; set; }
    public ICollection<Avaliacao> Avaliacoes { get; set; }
}