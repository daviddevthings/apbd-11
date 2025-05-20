using APBD11.Exceptions;
using APBD11.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD11.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IDbService _dbService;

        public PatientController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{patientId:int}")]
        public async Task<IActionResult> GetPatientDetails(int patientId)
        {
            try
            {
                var patientDetails = await _dbService.GetPatientDetailsAsync(patientId);
                return Ok(patientDetails);
            }
            catch (PatientNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request. Message: {ex.Message}");
            }
        }
    }
}
