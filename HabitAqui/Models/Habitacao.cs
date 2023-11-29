using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Habitacao
{
    public int Id { get; set; }
    public DetalhesUtilizador Funcionario { get; set; }
    public Locador Locador { get; set; }
    public DetalhesHabitacao DetalhesHabitacao { get; set; }
    [Required] public ICollection<HabitacaoCategoria> Categorias { get; set; }
    public ICollection<ImagemHabitacao> Imagens { get; set; }
    public ICollection<Avaliacao>? Avaliacoes { get; set; }
    public ICollection<Reserva>? Reservas { get; set; }

    public virtual double MediaAvaliacoes
    {
        get
        {
            if (Avaliacoes != null && Avaliacoes.Count > 0)
                return Avaliacoes.Average(a => a.Nota);
            return 0;
        }
    }
}