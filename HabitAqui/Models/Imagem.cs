using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Imagem
{
    [Key] public int Id { get; set; }
    [Required] public string Nome { get; set; }
    [Required] public string Path { get; set; }
    public string FileType { get; set; }
    public string Extension { get; set; }
    public byte[] Data { get; set; }

    public DateTime? CreatedOn { get; set; } = DateTime.Now;
    //public DateTime? ModifiedOn { get; set; }
    // public string CreatedBy { get; set; }
    // public string ModifiedBy { get; set; }
}