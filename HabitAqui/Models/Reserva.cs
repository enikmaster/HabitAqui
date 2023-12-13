using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public enum EstadoReserva
{
    Pendente,
    Aceite,
    Rejeitado
}

public class Reserva
{
    [Key] public int Id { get; set; }

    public EstadoReserva Estado { get; set; } //gerir registos das reservas
    public DetalhesUtilizador Funcionario { get; set; }
    public DetalhesUtilizador Cliente { get; set; }
    public Habitacao Habitacao { get; set; }
    [Required] public DateTime DataInicio { get; set; }
    [Required] public DateTime DataFim { get; set; }
    public ICollection<RegistoEntrega>? RegistoEntregas { get; set; } //podemos ter uma reserva (pendente/rejeitada) sem ter registo de entrega
}