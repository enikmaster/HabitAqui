using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HabitAqui.Models;

public class DetalhesUtilizador : IdentityUser
{
    [Required] public string Nome { get; set; }

    [Required] public string Apelido { get; set; }

    [Required] public string Nif { get; set; }
    public bool Active { get; set; }

    // public int LocalizacaoId { get; set; }
    [Required] public Localizacao LocalizacaoX { get; set; }
}