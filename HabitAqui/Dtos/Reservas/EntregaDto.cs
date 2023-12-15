using HabitAqui.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitAqui.Dtos.Reservas
{
    public class EntregaDto
    {
        public string FuncionarioId { get; set; }
        public DateTime DataEntrega {  get; set; }

        public TipoTransacao TipoTransacao { get; set; }

        public int ReservaId { get; set; }
        public bool Danos { get; set; }
        public string Observacoes { get; set; }
        public ICollection<Opcional>? Opcionais { get; set; }
        public ICollection<ImagemRegistoEntrega>? Imagens { get; set; }


    }
}
