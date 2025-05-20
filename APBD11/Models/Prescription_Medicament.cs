using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD11.Models;

public class Prescription_Medicament
{
    [Key]
    [ForeignKey("Medicament")]
    public int IdMedicament { get; set; }
        
    [Key]
    [ForeignKey("Prescription")]
    public int IdPrescription { get; set; }
        
    [Required]
    public int Dose { get; set; }
        
    [Required]
    [MaxLength(100)]
    public string Details { get; set; }
        
    public Medicament Medicament { get; set; }
    public Prescription Prescription { get; set; }
}