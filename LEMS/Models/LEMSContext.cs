using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LEMS.Models;

public partial class LEMSContext : DbContext
{
    public LEMSContext()
    {
    }

    public LEMSContext(DbContextOptions<LEMSContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branchs { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Equipment> Equipments { get; set; }

    public virtual DbSet<EquipmentEntry> EquipmentEntries { get; set; }

    public virtual DbSet<EquipmentSerial> EquipmentSerials { get; set; }

    public virtual DbSet<EquipmentUse> EquipmentUses { get; set; }

    public virtual DbSet<EquipmentsType> EquipmentsTypes { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<LaboratoryEquipment> LaboratoryEquipments { get; set; }

    public virtual DbSet<Labratory> Labratories { get; set; }

    public virtual DbSet<LabratoryAssignment> LabratoryAssignments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<WithdrawalSerial> WithdrawalSerials { get; set; }

    public virtual DbSet<Withdrawl> Withdrawls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=LEMSConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Branch).WithMany(p => p.Departments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Departments_Branchs");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.EquipmentsType).WithMany(p => p.Equipment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Equipments_EquipmentsTypes");
        });

        modelBuilder.Entity<EquipmentEntry>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.ActionByNavigation).WithMany(p => p.EquipmentEntries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EquipmentEntries_Users");

            entity.HasOne(d => d.Equipment).WithMany(p => p.EquipmentEntries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EquipmentEntries_Equipments");

            entity.HasOne(d => d.Labratory).WithMany(p => p.EquipmentEntries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EquipmentEntries_Labratories");
        });

        modelBuilder.Entity<EquipmentSerial>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.EquipmentEntry).WithMany(p => p.EquipmentSerials)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EquipmentSerials_EquipmentEntries");
        });

        modelBuilder.Entity<EquipmentUse>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Equipment).WithMany(p => p.EquipmentUses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EquipmentUses_Equipments");
        });

        modelBuilder.Entity<EquipmentsType>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<LaboratoryEquipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_DepartmentEquipments");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Department).WithMany(p => p.LaboratoryEquipments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DepartmentEquipments_Departments");

            entity.HasOne(d => d.Equipment).WithMany(p => p.LaboratoryEquipments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DepartmentEquipments_Equipments");

            entity.HasOne(d => d.Labratory).WithMany(p => p.LaboratoryEquipments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_laboratoryEquipments_Labratories");
        });

        modelBuilder.Entity<Labratory>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Department).WithMany(p => p.Labratories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Labratories_Departments");
        });

        modelBuilder.Entity<LabratoryAssignment>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.ActionByNavigation).WithMany(p => p.LabratoryAssignmentActionByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LabratoryAssignments_Users1");

            entity.HasOne(d => d.Labratory).WithMany(p => p.LabratoryAssignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LabratoryAssignments_Labratories");

            entity.HasOne(d => d.User).WithMany(p => p.LabratoryAssignmentUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LabratoryAssignments_Users");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Gender).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Gender");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Roles");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Users");
        });

        modelBuilder.Entity<WithdrawalSerial>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Serial).WithMany(p => p.WithdrawalSerials)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WithdrawalSerials_EquipmentSerials");

            entity.HasOne(d => d.Withdrawal).WithMany(p => p.WithdrawalSerials)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WithdrawalSerials_Withdrawls");
        });

        modelBuilder.Entity<Withdrawl>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.ActionByNavigation).WithMany(p => p.Withdrawls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Withdrawls_Users");

            entity.HasOne(d => d.Entry).WithMany(p => p.Withdrawls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Withdrawls_EquipmentEntries");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
