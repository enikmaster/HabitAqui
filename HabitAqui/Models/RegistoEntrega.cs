using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class RegistoEntrega
{
    public int Id { get; set; }
    [Required] public int FuncionarioID { get; set; }
    [Required] public bool Danos { get; set; }
    public string Observacoes { get; set; }
    public ICollection<EquipamentoOpcional> EquipamentosOpcionais { get; set; }
    public ICollection<ImagemRegistoEntrega>? Imagens { get; set; }
}