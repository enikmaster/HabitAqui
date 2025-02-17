﻿using HabitAqui.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Data;

public class ApplicationDbContext : IdentityDbContext<DetalhesUtilizador>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Avaliacao> Avaliacoes { set; get; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<DetalhesHabitacao> DetalhesHabitacoes { get; set; }
    public DbSet<DetalhesUtilizador> DetalhesUtilizadores { get; set; }
    public DbSet<Habitacao> Habitacoes { get; set; }
    public DbSet<HabitacaoCategoria> HabitacoesCategorias { get; set; }
    public DbSet<Imagem> Imagens { get; set; }
    public DbSet<ImagemRegistoEntrega> ImagensRegistoEntregas { get; set; }
    public DbSet<Locador> Locadores { get; set; }
    public DbSet<Localizacao> Localizacoes { get; set; }
    public DbSet<Opcional> Opcionais { get; set; }
    public DbSet<RegistoEntrega> RegistoEntregas { get; set; }
    public DbSet<Reserva> Reservas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<DetalhesHabitacao>()
            .Property(h => h.PrecoPorNoite)
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<DetalhesHabitacao>()
            .Property(h => h.Area)
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Habitacao>()
            .HasOne(h => h.Locador)
            .WithMany(l => l.Habitacoes)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Locador>()
            .HasMany(h => h.Habitacoes)
            .WithOne(l => l.Locador)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Habitacao>()
            .HasMany(r => r.Reservas)
            .WithOne(h => h.Habitacao)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Reserva>()
            .HasOne(c => c.Cliente)
            .WithMany()
            .HasForeignKey(c => c.ClienteId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Reserva>()
            .HasOne(f => f.Funcionario)
            .WithMany()
            .HasForeignKey(f => f.FuncionarioId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<DetalhesUtilizador>()
        .HasKey(d => d.Id);

        modelBuilder.Entity<DetalhesUtilizador>()
            .Property(d => d.Id)
            .ValueGeneratedOnAdd();
    }
}