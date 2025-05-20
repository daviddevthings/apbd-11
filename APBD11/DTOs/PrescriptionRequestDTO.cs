namespace APBD11.DTOs;

public class PrescriptionRequestDTO
{
    public PatientRequestDTO patient { get; set; }
    public List<MedicamentRequestDTO> medicaments { get; set; }
    public int idDoctor { get; set; }
    public string Date { get; set; }
    public string DueDate { get; set; }
}

public class PatientRequestDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string BirthDate { get; set; }
}

public class MedicamentRequestDTO
{
    public int idMedicament { get; set; }
    public int Dose { get; set; }
    public string Details { get; set; }
}
