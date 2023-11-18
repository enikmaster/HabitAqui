using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace HabitAqui.Data;

public class ApplicationDbContext : IdentityDbContext<DetalhesUtilizador>
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
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<Avaliacao> Avaliacoes { get; set; }
    public DbSet<EquipamentoOpcional> EquipamentosOpcionais { get; set; }
    public DbSet<HabitacaoCategoria> HabitacaoCategorias { get; set; }
    public DbSet<ImagemHabitacao> Imagenshabitacoes { get; set; }
    public DbSet<Locador> Locadores { get; set; }
    public DbSet<RegistoEntrega> RegistosEntregas { get; set; }
    public DbSet<Utilizador> Utilizadores { get; set; } //talveztirar
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //modelBuilder.Entity<Habitacao>().HasData(
        //    new Habitacao { Id = 1 },
        //    new Habitacao { Id = 2 }
        //);
        //modelBuilder.Entity<IdentityUserLogin<string>>()
        //    .HasKey(login => new { login.LoginProvider, login.ProviderKey });

    }

}