﻿using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models;

public class Categoria
{
    public int Id { get; set; }

    [Required] public string Nome { get; set; }
    public ICollection<HabitacaoCategoria>? Categorias { get; set; }
}