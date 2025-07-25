using Microsoft.EntityFrameworkCore;
using ClinicFlow.Domain.Entities;

namespace ClinicFlow.Infrastructure.Data;

public class ClinicFlowDbContext : DbContext
{
    public ClinicFlowDbContext(DbContextOptions<ClinicFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.User)
            .WithMany(u => u.Appointments)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .IsRequired();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<Appointment>()
            .HasIndex(a => a.ScheduledAt)
            .IsClustered(false);

        modelBuilder.Entity<Patient>()
            .Property(p => p.FirstName)
            .IsRequired();

        modelBuilder.Entity<Patient>()
            .Property(p => p.LastName)
            .IsRequired();
    }
}
