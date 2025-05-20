using APBD11.DTOs;
using APBD11.Exceptions;
using APBD11.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD11.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IDbService _dbService;

        public PrescriptionController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] PrescriptionRequestDTO request)
        {
            try
            {
                var prescriptionId = await _dbService.AddPrescription(request);
                return Created("", new { IdPrescription = prescriptionId });
            }
            catch (InvalidPrescriptionDateException)
            {
                return BadRequest("Due date must be greater than or equal to the issue date.");
            }
            catch (TooManyMedicamentsException)
            {
                return BadRequest("Prescription cannot contain more than 10 medicaments.");
            }
            catch (MedicamentNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DoctorNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error while saving to databse: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request. Message: {ex.Message}");
            }
        }
    }
}
