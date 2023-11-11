using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class ImagemHabitacao
{
    [Key]
    public int Id { get; set; }
    [Required] public string Nome { get; set; }
    public string FileType { get; set; }
    public string Extension { get; set; }
    public DateTime? CreatedOn { get; set; }

    public int DetalhesHabitacaoId { get; set; }
    [Required] public DetalhesHabitacao DetalhesHabitacao { get; set; }
}