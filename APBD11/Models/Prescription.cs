using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD11.Models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
        
    [Required]
    public DateTime Date { get; set; }
        
    [Required]
    public DateTime DueDate { get; set; }
        
    [Required]
    [ForeignKey("Patient")]
    public int IdPatient { get; set; }
        
    [Required]
    [ForeignKey("Doctor")]
    public int IdDoctor { get; set; }
        
    public Patient Patient { get; set; }
    public Doctor Doctor { get; set; }
    public ICollection<Prescription_Medicament> Prescription_Medicaments { get; set; } 
}