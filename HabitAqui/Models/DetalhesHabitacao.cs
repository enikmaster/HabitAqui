using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class DetalhesHabitacao
{
    [Key] public int Id { get; set; }

    [Required]
    [DisplayName("Nome da Habitação")]
    public string Nome { get; set; } = string.Empty;

    [DisplayName("Descrição")] public string Descricao { get; set; } = string.Empty;
    [DisplayName("Área total")] public decimal Area { get; set; }
    [DisplayName("Preço por noite")] public decimal PrecoPorNoite { get; set; }
    public Localizacao Localizacao { get; set; }
}