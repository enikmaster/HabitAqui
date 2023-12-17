using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Dtos.Reservas;

public class ReservaDto : ReservaGeraldto
{
    public ReservaDto()
    {
        DataInicio = DateTime.Now;
        DataFim = DateTime.Now.AddDays(1);
        NomeHabitacao = string.Empty;
        DescricaoHabitacao = string.Empty;
        PrecoPorNoiteHabitacao = 0.0m;
    }
    public int FuncionarioId { get; set; }

    [Display(Name = "Nome da Habitação")] public string NomeHabitacao { get; set; }

    [Display(Name = "Descrição da Habitação")]
    public string DescricaoHabitacao { get; set; }

    [Display(Name = "Preço por Noite")]
    [DataType(DataType.Currency)]
    public decimal PrecoPorNoiteHabitacao { get; set; }

    public int NumeroNoites { get; set; }

    public int PrecoReserva { get; set; }

    public decimal precoTotal { get; set; }


}