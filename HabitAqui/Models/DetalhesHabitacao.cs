namespace HabitAqui.Models;

public class DetalhesHabitacao
{
    private Guid Guid { get; set; }
    private string Nome { get; set; } = string.Empty;
    private string Descricao {  get; set; } = string.Empty;
    private Decimal Area { get; set; }
    private Decimal PrecoPorNoite {  get; set; }
    private Localizacao localizacao { get; set; }
}