using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Dtos;

public class HabitacaoDto
{
    [Required]
    [DisplayName("Nome da Habitação")]
    public string Nome { get; set; }

    [DisplayName("Descrição")] public string? Descricao { get; set; }
    [DisplayName("Área total")] public decimal? Area { get; set; }
    [DisplayName("Preço por noite")] public decimal PrecoPorNoite { get; set; }
    public string Morada { get; set; }

    [DisplayName("Código Postal")]
    [DataType(DataType.PostalCode)]
    [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "O código postal deve estar no formato 0000-000.")]
    public string CodigoPostal { get; set; }

    public string Cidade { get; set; }
    [DisplayName("País")] public string Pais { get; set; }

    [DisplayName("Categorias")] public List<int> CategoriasId { get; set; } = new();
    public List<string> Imagens { get; set; }
}