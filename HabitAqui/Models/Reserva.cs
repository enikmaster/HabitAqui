namespace HabitAqui.Models;

public class Reserva
{
    public int IdReserva { get; set; }

    public string EstadoReserva { get; set; }//aceite, pendente, recusada


    //Associada a uma habitação
    public int HabitacaoId { get; set; }
    public Habitacao Habitacao { get; set; }

    //Associado a um cliente
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    //private Guid Guid { get; set; }
}