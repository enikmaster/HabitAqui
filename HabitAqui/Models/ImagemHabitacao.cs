using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class ImagemHabitacao
{
    public int Id { get; set; }
    [Required] public string Nome { get; set; }
    public string FileType { get; set; }
    public string Extension { get; set; }
    public DateTime? CreatedOn { get; set; }

    public byte[] Data { get; set; }
    public int DetalhesHabitacaoId { get; set; }
    [Required] public DetalhesHabitacao DetalhesHabitacao { get; set; }
}