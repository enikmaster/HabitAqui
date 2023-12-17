using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitAqui.Models;

public enum EstadoReserva
{
    Pendente,
    Aceite,
    Rejeitado,
    Concluido
}

public class Reserva
{
    [Key] public int Id { get; set; }

    public EstadoReserva Estado { get; set; }
    public string FuncionarioId { get; set; }
    [ForeignKey("FuncionarioId")] public DetalhesUtilizador Funcionario { get; set; }
    public string ClienteId { get; set; }
    [ForeignKey("ClienteId")] public DetalhesUtilizador Cliente { get; set; }
    public Habitacao Habitacao { get; set; }
    [Required] public DateTime DataInicio { get; set; }
    [Required] public DateTime DataFim { get; set; }

    public ICollection<RegistoEntrega>?
        RegistoEntregas { get; set; }
}