using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Dtos.Reservas;

public class ReservaDto
{
    public ReservaDto() // para que serve isto?
    {
        DataInicio = DateTime.Now;
        DataFim = DateTime.Now.AddDays(1);
        NomeHabitacao = string.Empty;
        DescricaoHabitacao = string.Empty;
        PrecoPorNoiteHabitacao = 0.0m;
    }

    [Required(ErrorMessage = "A data de check-in é obrigatória.")]
    [Display(Name = "Data de Check-In")]
    [DataType(DataType.Date)]
    public DateTime DataInicio { get; set; }

    [Required(ErrorMessage = "A data de check-out é obrigatória.")]
    [Display(Name = "Data de Check-Out")]
    [DataType(DataType.Date)]
    public DateTime DataFim { get; set; }

    public int FuncionarioId { get; set; } // TODO: explicar como funciona a reserva de uma habitação
    public int HabitacaoId { get; set; }

    [Display(Name = "Nome da Habitação")]
    public string NomeHabitacao { get; set; } // para que serve o nome da Habitação aqui?

    [Display(Name = "Descrição da Habitação")]
    public string DescricaoHabitacao { get; set; } // para que serve a descrição da Habitação aqui?

    [Display(Name = "Preço por Noite")]
    [DataType(DataType.Currency)]
    public decimal PrecoPorNoiteHabitacao { get; set; } // para que serve o preço por noite da Habitação aqui?

    public int NumeroNoites { get; set; }

    public int PrecoReserva { get; set; }

    public decimal precoTotal { get; set; }

    public string AnotacoesCliente { get; set; }
}