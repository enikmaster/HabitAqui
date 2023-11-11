using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class EquipamentoOpcional
{
    public int Id { get; set; }
    [Required] public string Nome { get; set; }
    [Required] public string Estado { get; set; }
}