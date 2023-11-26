using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitAqui.Models;

public class Habitacao
{
    public int Id { get; set; }

    public int FuncionarioId { get; set; }
    public DetalhesUtilizador Funcionario { get; set; }
    [Required] public int LocadorId { get; set; }
    public Locador Locador { get; set; }

    [Required] public int DetalhesHabitacaoID { get; set; }

    [ForeignKey("DetalhesHabitacaoID")] public DetalhesHabitacao DetalhesHabitacao { get; set; }

    [Required] public ICollection<HabitacaoCategoria> Categorias { get; set; }
    public ICollection<Avaliacao>? Avaliacoes { get; set; }
    public ICollection<Reserva>? Reservas { get; set; }

    public double MediaAvaliacoes
    {
        get
        {
            if (Avaliacoes != null && Avaliacoes.Count > 0)
                return Avaliacoes.Average(a => a.Nota);
            return 0;
        }
    }
}