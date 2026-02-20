using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UNIVERSITY.Models;

public partial class UniversityContext : DbContext
{
    private static UniversityContext _context;
    public static UniversityContext GetContext()
    {
        if (_context == null)
            _context = new UniversityContext();

        return _context;
    }
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
        => optionsBuilder.UseSqlServer("Server=DESKTOP-67VCCKN\\SQLEXPRESS;Database=UNIVERSITY;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.Property(e => e.AuditoriumId).HasColumnName("auditorium_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.OfficeId).HasColumnName("office_id");

            entity.HasOne(d => d.Office).WithMany(p => p.Auditoria)
                .HasForeignKey(d => d.OfficeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Auditoriums_Offices");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasIndex(e => e.InventareNum, "NonClusteredIndex-20260121-201852").IsUnique();

            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.AuditoriumId).HasColumnName("auditorium_id");
            entity.Property(e => e.DateBalance).HasColumnName("date_balance");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.InventareNum)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("inventare_num");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.OfficeId).HasColumnName("office_id");
            entity.Property(e => e.Photo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("photo");
            entity.Property(e => e.ServiceLife).HasColumnName("service_life");
            entity.Property(e => e.Weigth).HasColumnName("weigth");

            entity.HasOne(d => d.Auditorium).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.AuditoriumId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Equipments_Auditoriums");

            entity.HasOne(d => d.Office).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.OfficeId)
                .HasConstraintName("FK_Equipments_Offices");
        });

        modelBuilder.Entity<Office>(entity =>
        {
            entity.Property(e => e.OfficeId).HasColumnName("office_id");
            entity.Property(e => e.FullNameOffice)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("full_name_office");
            entity.Property(e => e.Level)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("level");
            entity.Property(e => e.NameOffice)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name_office");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.PositionName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("position_name");
            entity.Property(e => e.Salary)
                .HasColumnType("money")
                .HasColumnName("salary");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.Property(e => e.WorkerId).HasColumnName("worker_id");
            entity.Property(e => e.BirthYear).HasColumnName("birth_year");
            entity.Property(e => e.Fathername)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fathername");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lastname");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("login");
            entity.Property(e => e.OfficeId).HasColumnName("office_id");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PositionId).HasColumnName("position_id");

            entity.HasOne(d => d.Office).WithMany(p => p.Workers)
                .HasForeignKey(d => d.OfficeId)
                .HasConstraintName("FK_Workers_Offices");

            entity.HasOne(d => d.Position).WithMany(p => p.Workers)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK_Workers_Positions");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
