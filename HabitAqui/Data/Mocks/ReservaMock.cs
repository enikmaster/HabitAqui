using HabitAqui.Models;

namespace HabitAqui.Data.Mocks
{
    public static class ReservaMock
    {
        public static Reserva GenerateReservaMock(DetalhesUtilizador funcionario, DetalhesUtilizador Cliente, Habitacao habitacao)
        {
            return new Reserva
            {
                Funcionario = funcionario,
                Cliente = Cliente,
                Habitacao = habitacao,
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(7), 
                Estado = EstadoReserva.Aceite
                // RegistoEntregas pode ser inicializado conforme necessário
            };
        }
    }
}