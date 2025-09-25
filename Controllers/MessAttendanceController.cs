using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.Dtos;
using XeniaRentalApi.Repositories.MessDetails;


namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class MessAttendanceController : ControllerBase
    {
        private readonly IMessAttendancesRepository _messDetailsRepository;


        public MessAttendanceController(IMessAttendancesRepository messDetailsRepository)
        {
            _messDetailsRepository = messDetailsRepository;
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyAttendance(int companyId, int propertyId, int unitId, int month, int year)
        {
            var data = await _messDetailsRepository.GetMonthlyMessAttendanceAsync(companyId, propertyId, unitId, month, year);
            return Ok(data);
        }


        [HttpPost("mark")]
        public async Task<IActionResult> MarkAttendance([FromBody] MarkAttendanceDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid attendance record.");

            var success = await _messDetailsRepository.MarkAttendanceAsync(dto);

            if (success)
                return Ok(new { Message = "Attendance marked successfully." });

            return StatusCode(500, "Error marking attendance.");
        }


    }
}
