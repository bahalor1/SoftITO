using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SinemaUygulaması.Models;

public partial class SinemaDbContext : DbContext
{
    public SinemaDbContext()
    {
    }

    public SinemaDbContext(DbContextOptions<SinemaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Biletler> Biletlers { get; set; }

    public virtual DbSet<Filmler> Filmlers { get; set; }

    public virtual DbSet<Seanslar> Seanslars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.;Database=SinemaDb;Trusted_Connection=True;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Biletler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Biletler__3214EC07C5EB7B7A");

            entity.ToTable("Biletler");

            entity.Property(e => e.KoltukNo).HasMaxLength(10);
            entity.Property(e => e.MusteriAdSoyad).HasMaxLength(100);

            entity.HasOne(d => d.Seans).WithMany(p => p.Biletlers)
                .HasForeignKey(d => d.SeansId)
                .HasConstraintName("FK__Biletler__SeansI__4E88ABD4");
        });

        modelBuilder.Entity<Filmler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Filmler__3214EC07FE45F8A1");

            entity.ToTable("Filmler");

            entity.Property(e => e.FilmAdi).HasMaxLength(100);
            entity.Property(e => e.Tur).HasMaxLength(50);
        });

        modelBuilder.Entity<Seanslar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Seanslar__3214EC07B5E3F40D");

            entity.ToTable("Seanslar");

            entity.Property(e => e.BaslangicSaati).HasColumnType("datetime");
            entity.Property(e => e.SalonAdi).HasMaxLength(50);

            entity.HasOne(d => d.Film).WithMany(p => p.Seanslars)
                .HasForeignKey(d => d.FilmId)
                .HasConstraintName("FK__Seanslar__FilmId__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
