using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class DetalhesUtilizador
{
    [Key]
    public int Id { get; set; }

    [Required] public string Nome { get; set; }

    [Required] public string Apelido { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Telefone { get; set; }

    public int LocalizacaoId { get; set; }
    public Localizacao Localizacao { get; set; }
}