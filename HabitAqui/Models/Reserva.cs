using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Reserva
{
    public int Id { get; set; }
    [Required] public int ClienteId { get; set; }
    [Required] public int HabitacaoId { get; set; }
    [Required] public DateTime DataInicio { get; set; }
    [Required] public DateTime DataFim { get; set; }
    public ICollection<RegistoEntrega> RegistoEntregas { get; set; }
}