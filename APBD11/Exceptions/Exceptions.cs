namespace APBD11.Exceptions;

public class MedicamentNotFoundException : Exception
{
    public MedicamentNotFoundException(int idMedicament) 
        : base($"Medicament with ID {idMedicament} not found")
    {
    }
}
    
public class TooManyMedicamentsException : Exception
{
    public TooManyMedicamentsException() 
        : base("Prescription cannot contain more than 10 medicaments")
    {
    }
}
    
public class InvalidPrescriptionDateException : Exception
{
    public InvalidPrescriptionDateException() 
        : base("Prescription due date must be greater than or equal to the date")
    {
    }
}
    
public class DoctorNotFoundException : Exception
{
    public DoctorNotFoundException(int idDoctor) 
        : base($"Doctor with ID {idDoctor} not found")
    {
    }
}
    
public class PatientNotFoundException : Exception
{
    public PatientNotFoundException(int idPatient) 
        : base($"Patient with ID {idPatient} not found")
    {
    }
}