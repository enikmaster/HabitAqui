using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Avaliacao
{
    //public int IdAvaliacaoHabitacao { get; set; }
    private Guid Guid { get; set; }
    
    public DateTime DataAvaliacao { get; set; }

    [Required]
    public int Pontuacao { get; set; }

    [Required]
    public string Comentario { get; set; }

    public Habitacao Habitacao { get; set; }
    public Cliente Cliente { get; set; }
    
    
}