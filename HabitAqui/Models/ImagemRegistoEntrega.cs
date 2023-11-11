using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitAqui.Models;

public class ImagemRegistoEntrega
{
    [Key]
    public int Id { get; set; }
    [Required] public string Nome { get; set; }
    public string FileType { get; set; }
    public string Extension { get; set; }
    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
    public byte[] Data { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    [ForeignKey("DetalhesHabitacao")] public int DetalhesHabitacaoId { get; set; }
    [Required] public DetalhesHabitacao DetalhesHabitacao { get; set; }
}