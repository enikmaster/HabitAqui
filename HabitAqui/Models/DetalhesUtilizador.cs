using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HabitAqui.Models;

public class DetalhesUtilizador : IdentityUser
{
    [Required] public string Nome { get; set; }

    [Required] public string Apelido { get; set; }

    [Required] public string Nif { get; set; }
    [Required]public string Morada { get; set; }
    [Required]public string CodigoPostal { get; set; }

    [Required] public string Cidade { get; set; }

    [Required] public string Pais { get; set; }

    public bool Active { get; set; }

    //  public int LocalizacaoId { get; set; }
    //public Localizacao Localizacao { get; set; }
}