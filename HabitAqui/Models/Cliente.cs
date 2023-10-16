namespace HabitAqui.Models
{
    public class Cliente
    {
    public int IdCliente { get; set; }
    public string NomeCliente { get; set; }

    public ICollection<Reserva> Reservas { get; set; }
    }
}
