using APBD11.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD11.Data;

public class DatabaseContext  : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Prescription_Medicament> Prescription_Medicaments { get; set; }

    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

 protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(p =>
            {
                p.ToTable("Patient");
                
                p.HasKey(e => e.IdPatient);
                p.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                p.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                p.Property(e => e.BirthDate).IsRequired();
            });
            
            modelBuilder.Entity<Doctor>(d =>
            {
                d.ToTable("Doctor");
                
                d.HasKey(e => e.IdDoctor);
                d.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                d.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                d.Property(e => e.Email).IsRequired().HasMaxLength(100);
            });
            
            modelBuilder.Entity<Medicament>(m =>
            {
                m.ToTable("Medicament");
                
                m.HasKey(e => e.IdMedicament);
                m.Property(e => e.Name).IsRequired().HasMaxLength(100);
                m.Property(e => e.Description).IsRequired().HasMaxLength(100);
                m.Property(e => e.Type).IsRequired().HasMaxLength(100);
            });
            
            modelBuilder.Entity<Prescription>(p =>
            {
                p.ToTable("Prescription");
                
                p.HasKey(e => e.IdPrescription);
                p.Property(e => e.Date).IsRequired();
                p.Property(e => e.DueDate).IsRequired();
                
                p.HasOne(e => e.Patient)
                  .WithMany(pt => pt.Prescriptions)
                  .HasForeignKey(e => e.IdPatient)
                  .OnDelete(DeleteBehavior.Restrict);
                
                p.HasOne(e => e.Doctor)
                  .WithMany(d => d.Prescriptions)
                  .HasForeignKey(e => e.IdDoctor)
                  .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<Prescription_Medicament>(pm =>
            {
                pm.ToTable("Prescription_Medicament");
                
                pm.HasKey(e => new { e.IdMedicament, e.IdPrescription });
                pm.Property(e => e.Dose).IsRequired();
                pm.Property(e => e.Details).IsRequired().HasMaxLength(100);
                
                pm.HasOne(e => e.Medicament)
                  .WithMany(m => m.Prescription_Medicaments)
                  .HasForeignKey(e => e.IdMedicament)
                  .OnDelete(DeleteBehavior.Restrict);
                
                pm.HasOne(e => e.Prescription)
                  .WithMany(p => p.Prescription_Medicaments)
                  .HasForeignKey(e => e.IdPrescription)
                  .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<Patient>().HasData(new List<Patient>()
            {
                new Patient { IdPatient = 1, FirstName = "Jan", LastName = "Kowalski", BirthDate = new DateTime(1980, 1, 1) },
                new Patient { IdPatient = 2, FirstName = "Anna", LastName = "Nowak", BirthDate = new DateTime(1990, 5, 15) }
            });
            
            modelBuilder.Entity<Doctor>().HasData(new List<Doctor>()
            {
                new Doctor { IdDoctor = 1, FirstName = "Adam", LastName = "Lekarski", Email = "adam.lekarski@hospital.com" },
                new Doctor { IdDoctor = 2, FirstName = "Maria", LastName = "Doktorska", Email = "maria.doktorska@hospital.com" }
            });
            
            modelBuilder.Entity<Medicament>().HasData(new List<Medicament>()
            {
                new Medicament { IdMedicament = 1, Name = "Aspirin", Description = "Pain reliever", Type = "Tablet" },
                new Medicament { IdMedicament = 2, Name = "Ibuprofen", Description = "Anti-inflammatory", Type = "Capsule" },
                new Medicament { IdMedicament = 3, Name = "Paracetamol", Description = "Fever reducer", Type = "Tablet" }
            });
            
            modelBuilder.Entity<Prescription>().HasData(new List<Prescription>()
            {
                new Prescription { IdPrescription = 1, Date = new DateTime(2023, 1, 10), DueDate = new DateTime(2023, 2, 10), IdPatient = 1, IdDoctor = 1 },
                new Prescription { IdPrescription = 2, Date = new DateTime(2023, 2, 15), DueDate = new DateTime(2023, 3, 15), IdPatient = 2, IdDoctor = 2 }
            });
            
            modelBuilder.Entity<Prescription_Medicament>().HasData(new List<Prescription_Medicament>()
            {
                new Prescription_Medicament { IdPrescription = 1, IdMedicament = 1, Dose = 2, Details= "Take twice daily" },
                new Prescription_Medicament { IdPrescription = 1, IdMedicament = 2, Dose = 1, Details= "Take once daily" },
                new Prescription_Medicament { IdPrescription = 2, IdMedicament = 3, Dose = 3, Details= "Take three times daily" }
            });
        }
}