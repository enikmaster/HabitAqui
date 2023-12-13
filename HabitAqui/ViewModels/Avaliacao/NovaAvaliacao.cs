using HabitAqui.Models;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui.ViewModels.Avaliacao
{
    public class NovaAvaliacao
    {
        public int HabitacaoId { get; set; }

        //[Range(1, 5, ErrorMessage = "A nota deve estar entre 1 e 5.")]
        public int Nota { get; set; }
        public string Comentario { get; set; }
    }
}
