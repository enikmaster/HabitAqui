namespace HabitAqui.Models;

public class DetalhesHabitacao
{
    public int Id { get; set; }
    public Guid Guid { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Area { get; set; }
    public decimal PrecoPorNoite { get; set; }
    public Localizacao localizacao { get; set; }
}