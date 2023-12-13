using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public enum TipoTransacao
{
    Entrega,
    Devolucao
}

public class RegistoEntrega
{
    [Key] public int Id { get; set; }
    public DetalhesUtilizador Funcionario { get; set; }
    //public DetalhesUtilizador Cliente { get; set; }
    [Required] public DateTime DataEntrega { get; set; }
    public TipoTransacao TipoTransacao { get; set; }
    [Required] public bool Danos { get; set; }
    public string Observacoes { get; set; }
    public ICollection<Opcional>? Opcionais { get; set; }
    public ICollection<ImagemRegistoEntrega>? Imagens { get; set; }
}