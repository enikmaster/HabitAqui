namespace HabitAqui.Dtos.Habitacao;

public class EditarHabitacaoDto : HabitacaoDto
{
    public int Id { get; set; }
    public List<int> ImagensId { get; set; }
}