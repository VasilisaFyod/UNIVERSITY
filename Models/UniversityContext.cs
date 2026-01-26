using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UNIVERSITY.Models;

public partial class UniversityContext : DbContext
{
    public UniversityContext()
    {
    }

    public UniversityContext(DbContextOptions<UniversityContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditorium> Auditoriums { get; set; }

    public virtual DbSet<Equipment> Equipments { get; set; }

    public virtual DbSet<Office> Offices { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=UNIVERSITY;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.Property(e => e.AuditoriumId).ValueGeneratedNever();

            entity.HasOne(d => d.Office).WithMany(p => p.Auditoria)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Auditoriums_Offices");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.Property(e => e.EquipmentId).ValueGeneratedNever();

            entity.HasOne(d => d.Auditorium).WithMany(p => p.Equipment).HasConstraintName("FK_Equipments_Auditoriums");
        });

        modelBuilder.Entity<Office>(entity =>
        {
            entity.Property(e => e.OfficeId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.Property(e => e.PositionId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.Property(e => e.WorkerId).ValueGeneratedNever();

            entity.HasOne(d => d.Office).WithMany(p => p.Workers).HasConstraintName("FK_Workers_Offices");

            entity.HasOne(d => d.Position).WithMany(p => p.Workers).HasConstraintName("FK_Workers_Positions");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
