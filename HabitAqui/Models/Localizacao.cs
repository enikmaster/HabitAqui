using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Localizacao
{
    [Key]
    public int Id { get; set; }

    [Required] [DisplayName("Morada")] public string Morada { get; set; }

    [DisplayName("Código Postal")]
    [DataType(DataType.PostalCode)]
    [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "O código postal deve estar no formato 0000-000.")]
    public string CodigoPostal { get; set; }

    [DisplayName("Cidade")] public string Cidade { get; set; }

    [DisplayName("País")] public string Pais { get; set; }
}