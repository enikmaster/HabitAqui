namespace HabitAqui.Models
{
    public class ImagemHabitacao
    {
        public int ImagemHabitacaoId { get; set; }
        public string Nome { get; set; }

        //está associada a uma habitação
        public int HabitacaoId { get; set; }

        public Habitacao Habitacao { get; set; } // Relacionar diretamente a ImagemHabitacao com a Habitação, para que seja possível aceder à habitação a partir da imagem
    }
}
