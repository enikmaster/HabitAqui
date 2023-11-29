using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HabitAqui.Models;

public class DetalhesUtilizador : IdentityUser
{
    [Required] public string Nome { get; set; }
    [Required] public string Apelido { get; set; }
    [Required] public string Nif { get; set; }
    public bool Active { get; set; }
    [Required] public Localizacao Localizacao { get; set; }
}