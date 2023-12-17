namespace HabitAqui.Dtos.Habitacao;

public class EditarHabitacaoDto : HabitacaoDto
{
    public EditarHabitacaoDto()
    {
    }

    public EditarHabitacaoDto(Models.Habitacao habitacao)
    {
        Id = habitacao.Id;
        Nome = habitacao.DetalhesHabitacao.Nome;
        Descricao = habitacao.DetalhesHabitacao.Descricao;
        PrecoPorNoite = habitacao.DetalhesHabitacao.PrecoPorNoite;
        Area = habitacao.DetalhesHabitacao.Area;
        Morada = habitacao.DetalhesHabitacao.Localizacao.Morada;
        CodigoPostal = habitacao.DetalhesHabitacao.Localizacao.CodigoPostal;
        Cidade = habitacao.DetalhesHabitacao.Localizacao.Cidade;
        Pais = habitacao.DetalhesHabitacao.Localizacao.Pais;
        Imagens = habitacao.Imagens.Select(p => p.Path).ToList();
        ImagensId = habitacao.Imagens.Select(i => i.Id).ToList();
        CategoriasId = habitacao.Categorias.Select(c => c.CategoriaId).ToList();
    }

    public int Id { get; set; }
    public List<int> ImagensId { get; set; }
}