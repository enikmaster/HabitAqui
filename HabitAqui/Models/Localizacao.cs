namespace HabitAqui.Models;

public class Localizacao
{
    public int Id { get; set; }
    public Guid Guid { get; set; }

    public string Morada { get; set; }
    public string CodigoPostal { get; set; }
    public string Cidade { get; set; }
    public string Pais { get; set; }
}