using APBD11.DTOs;

namespace APBD11.Services;

public interface IDbService
{
    Task<int> AddPrescription(PrescriptionRequestDTO request);
    Task<PatientResponse> GetPatientDetailsAsync(int patientId);
}
