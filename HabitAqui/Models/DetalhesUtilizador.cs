using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class DetalhesUtilizador
{
    [Key]
    public int Id { get; set; }

    [Required] public string Nome { get; set; } //identity ja tem, tirar dps

    [Required] public string Apelido { get; set; } //identity ja tem, tirar dps
    [Required] public string Email { get; set; }//identity ja tem, tirar dps
    [Required] public string Telefone { get; set; }//identity ja tem, tirar dps

    public int LocalizacaoId { get; set; }
    public Localizacao Localizacao { get; set; }
}