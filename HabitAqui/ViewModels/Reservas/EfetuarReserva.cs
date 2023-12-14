using System;
using System.ComponentModel.DataAnnotations;
using HabitAqui.Models;

namespace HabitAqui.ViewModels.Reservas
{
    public class EfetuarReserva
    {
        [Required(ErrorMessage = "A data de check-in é obrigatória.")]
        [Display(Name = "Data de Check-In")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de check-out é obrigatória.")]
        [Display(Name = "Data de Check-Out")]
        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }

        public int FuncionarioId { get; set; }
        public int HabitacaoId { get; set; }

        [Display(Name = "Nome da Habitação")]
        public string NomeHabitacao { get; set; }

        [Display(Name = "Descrição da Habitação")]
        public string DescricaoHabitacao { get; set; }

        [Display(Name = "Preço por Noite")]
        [DataType(DataType.Currency)]
        public decimal PrecoPorNoiteHabitacao { get; set; }

        public int NumeroNoites { get; set; }

        public int PrecoReserva { get; set; }

        public decimal precoTotal { get; set; }

        public string AnotacoesCliente { get; set; }


        public EfetuarReserva()
        {
            DataInicio = DateTime.Now;
            DataFim = DateTime.Now.AddDays(1); 
            NomeHabitacao = string.Empty;
            DescricaoHabitacao = string.Empty;
            PrecoPorNoiteHabitacao = 0.0m;
            
            
        }
    }
}