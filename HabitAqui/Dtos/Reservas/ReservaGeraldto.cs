using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Dtos.Reservas
{
    public class ReservaGeraldto
    {
        [Required(ErrorMessage = "A data de check-in é obrigatória.")]
        [Display(Name = "Data de Check-In")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de check-out é obrigatória.")]
        [Display(Name = "Data de Check-Out")]
        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }

        public int HabitacaoId { get; set; }

        public string AnotacoesCliente { get; set; }

    }
}
