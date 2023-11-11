using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Localizacao> Localizacoes { get; set; }
    public DbSet<DetalhesHabitacao> DetalhesHabitacoes { get; set; }
    public DbSet<DetalhesUtilizador> DetalhesUtilizadores { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Habitacao> Habitacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Habitacao>().HasData(
            new Habitacao { Id = 1 },
            new Habitacao { Id = 2 }
        );
        builder.Entity<IdentityUserLogin<string>>()
            .HasKey(login => new { login.LoginProvider, login.ProviderKey });
    }
}