using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Reserva
{
    [Key] public int Id { get; set; }
    public DetalhesUtilizador Funcionario { get; set; }
    public Cliente Cliente { get; set; }
    public Habitacao Habitacao { get; set; }
    [Required] public DateTime DataInicio { get; set; }
    [Required] public DateTime DataFim { get; set; }
    public ICollection<RegistoEntrega> RegistoEntregas { get; set; }
}