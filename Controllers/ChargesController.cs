using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Account;
using XeniaRentalApi.Repositories.Charges;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ChargesController : ControllerBase
    {
        private readonly IChargesRepository _chargesRepository;


        public ChargesController(IChargesRepository chargesRepository)
        {
            _chargesRepository = chargesRepository;
        }


        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ChargesDto>>> Get()
        {
            var charges = await _chargesRepository.GetCharges();
            if (charges == null || !charges.Any())
            {
                return NotFound(new { Status = "Error", Message = "No charges found." });
            }
            return Ok(new { Status = "Success", Data = charges });
        }


        [HttpGet("charges/{companyId}")]
        public async Task<ActionResult<PagedResultDto<XRS_Charges>>> GetChargesByCompanyId(int companyId, int pageNumber = 1,
            int pageSize = 10)
        {

            var charges = await _chargesRepository.GetChargesByCompanyId(companyId, pageNumber, pageSize);
            if (charges == null)
            {
                return NotFound(new { Status = "Error", Message = "No charges found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = charges });
        }


        [HttpPost]
        public async Task<IActionResult> CreateCharges([FromBody] DTOs.ChargesDto charges)
        {
            if (charges == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid charges." });
            }

            var createdAccount = await _chargesRepository.CreateCharges(charges);
            return CreatedAtAction(nameof(GetChargesById), new { id = createdAccount }, new { Status = "Success", Data = createdAccount });
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<XRS_Charges>> GetChargesById(int id)
        {
            var charges = await _chargesRepository.GetChargesbyId(id);
            if (charges == null)
            {
                return NotFound(new { Status = "Error", Message = "charges not found." });
            }
            return Ok(new { Status = "Success", Data = charges });
        }



        [HttpPut("UpdateCharges/{id}")]
        public async Task<IActionResult> UpdateCharges(int id, [FromBody] Models.XRS_Charges charges)
        {
            if (charges == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid charges data" });
            }

            var updated = await _chargesRepository.UpdateCharges(id, charges);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "charges not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "charges updated successfully." });
        }

        // DELETE api/<AccountGroupController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharges(int id)
        {
            var deleted = await _chargesRepository.DeleteCharges(id);
            if (!deleted)
            {
                return NotFound(new { Status = "Error", Message = "Charges not found or delete failed." });
            }

            return Ok(new { Status = "Success", Message = "Charges deleted successfully." });
        }

        [HttpGet("charges/search")]
        public async Task<ActionResult<PagedResultDto<XRS_Charges>>> Get(
           string? chargeName,
           string? PropertyName,
           int pageNumber = 1,
           int pageSize = 10)
        {
            var result = await _chargesRepository.GetChargesAsync(chargeName, PropertyName, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
