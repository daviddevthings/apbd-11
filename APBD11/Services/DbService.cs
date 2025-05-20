using APBD11.Data;
using APBD11.DTOs;
using APBD11.Exceptions;
using APBD11.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD11.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> AddPrescription(PrescriptionRequestDTO request)
    {
        var date = DateTime.Parse(request.Date);
        var dueDate = DateTime.Parse(request.DueDate);
        
        if (!(dueDate >= date))
        {
            throw new InvalidPrescriptionDateException();
        }

        if (request.medicaments.Count > 10)
        {
            throw new TooManyMedicamentsException();
        }

        foreach (var medicament in request.medicaments)
        {
            var exists = await _context.Medicaments.AnyAsync(m => m.IdMedicament == medicament.idMedicament);
            if (!exists)
            {
                throw new MedicamentNotFoundException(medicament.idMedicament);
            }
        }

        var doctorExists = await _context.Doctors.AnyAsync(d => d.IdDoctor == request.idDoctor);
        if (!doctorExists)
        {
            throw new DoctorNotFoundException(request.idDoctor);
        }

        
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
          
            Patient patient = await _context.Patients.FirstOrDefaultAsync(p => p.IdPatient == request.patient.IdPatient);
            if (patient == null)
            {
                patient = new Patient
                {
                    IdPatient = request.patient.IdPatient,
                    FirstName = request.patient.FirstName,
                    LastName = request.patient.LastName,
                    BirthDate = DateTime.Parse(request.patient.BirthDate),
                };
                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();
            }

          
            var prescription = new Prescription
            {
                Date = date,
                DueDate = dueDate,
                IdPatient = patient.IdPatient,
                IdDoctor = request.idDoctor,
                Prescription_Medicaments = new List<Prescription_Medicament>()
            };
            await _context.Prescriptions.AddAsync(prescription);
            await _context.SaveChangesAsync();

           
            foreach (var medicament in request.medicaments)
            {
                var prescriptionMedicament = new Prescription_Medicament
                {
                    IdPrescription = prescription.IdPrescription,
                    IdMedicament = medicament.idMedicament,
                    Dose = medicament.Dose,
                    Details = medicament.Details,
                };
                await _context.Prescription_Medicaments.AddAsync(prescriptionMedicament);
            }

            await _context.SaveChangesAsync();
            
          
            await transaction.CommitAsync();
            
            return prescription.IdPrescription;
        }
        catch (Exception)
        {

            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<PatientResponse> GetPatientDetailsAsync(int patientId)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.IdPatient == patientId);

        if (patient == null)
        {
            throw new PatientNotFoundException(patientId);
        }

        var prescriptions = await _context.Prescriptions
            .Where(p => p.IdPatient == patientId)
            .Include(p => p.Doctor)
            .Include(p => p.Prescription_Medicaments)
                .ThenInclude(pm => pm.Medicament)
            .OrderBy(p => p.DueDate)
            .ToListAsync();

        var response = new PatientResponse
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = prescriptions.Select(p => new PrescriptionDTO
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                Doctor = new DoctorDTO
                {
                    IdDoctor = p.Doctor.IdDoctor,
                    FirstName = p.Doctor.FirstName,
                    LastName = p.Doctor.LastName,
                    Email = p.Doctor.Email
                },
                Medicaments = p.Prescription_Medicaments.Select(pm => new MedicamentDTO
                {
                    IdMedicament = pm.Medicament.IdMedicament,
                    Name = pm.Medicament.Name,
                    Description = pm.Medicament.Description,
                    Type = pm.Medicament.Type,
                    Dose = pm.Dose,
                    Details = pm.Details
                }).ToList()
            }).ToList()
        };

        return response;
    }
}
