using HabitAqui.Models;

public static class ReservaMock
{
    public static List<Reserva> GenerateReservasMocks(
        List<DetalhesUtilizador> users,
        List<Habitacao> habitacoes,
        List<Locador> locadores)
    {
        var reservas = new List<Reserva>();
        var random = new Random();

        var excludedKeywords = new List<string> { "admin", "gestor", "funcionario", "remax", "era", "kw" };

        // Helper function to check if the email contains any excluded keywords.
        Func<string, bool> emailContainsExcludedKeywords = email =>
            excludedKeywords.Any(keyword => email.ToLowerInvariant().Contains(keyword));

        // Find the first user whose email doesn't contain any excluded keywords.
        var user1 = users.FirstOrDefault(u => !emailContainsExcludedKeywords(u.Email));

        // Find the second user whose email doesn't contain any excluded keywords and is not the same user as user1.
        var user2 = users.FirstOrDefault(u => u.Id != user1?.Id && !emailContainsExcludedKeywords(u.Email));

        if (locadores.Count >= 2)
        {
            var habitacaoForLocador0 = habitacoes.FirstOrDefault(h => h.LocadorId == locadores[0].Id);
            var habitacaoForLocador1 = habitacoes.FirstOrDefault(h => h.LocadorId == locadores[1].Id);

            if (habitacaoForLocador0 != null && user1 != null)
                reservas.Add(new Reserva
                {
                    ClienteId = user1.Id,
                    Habitacao = habitacaoForLocador0,
                    FuncionarioId = locadores[0].Id,
                    DataInicio = DateTime.Now.AddDays(-random.Next(30, 30)),
                    DataFim = DateTime.Now.AddDays(-random.Next(23, 23)),
                    Estado = EstadoReserva.Aceite
                });

            if (habitacaoForLocador1 != null && user2 != null)
                reservas.Add(new Reserva
                {
                    ClienteId = user2.Id,
                    Habitacao = habitacaoForLocador1,
                    FuncionarioId = locadores[1].Id,
                    DataInicio = DateTime.Now.AddDays(-random.Next(60, 60)),
                    DataFim = DateTime.Now.AddDays(-random.Next(40, 40)),
                    Estado = EstadoReserva.Aceite
                });
        }

        return reservas;
    }

    public static List<RegistoEntrega> GenerateRegistoEntregaMocks(List<Reserva> reservas)
    {
        var allRegistoEntregas = new List<RegistoEntrega>();

        foreach (var reserva in reservas)
        {
            reserva.Estado = EstadoReserva.Concluido;

            var entrega = new RegistoEntrega
            {
                Funcionario = reserva.Funcionario,
                DataEntrega = reserva.DataInicio,
                TipoTransacao = TipoTransacao.Entrega,
                Danos = false,
                Observacoes = "Entrega sem problemas."
            };

            var devolucao = new RegistoEntrega
            {
                Funcionario = reserva.Funcionario,
                DataEntrega = reserva.DataFim,
                TipoTransacao = TipoTransacao.Devolucao,
                Danos = false,
                Observacoes = "Devolução realizada com sucesso."
            };
            allRegistoEntregas.Add(entrega);
            allRegistoEntregas.Add(devolucao);
        }

        return allRegistoEntregas;
    }
}