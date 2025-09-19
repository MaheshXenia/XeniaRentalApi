using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Account;
using XeniaRentalApi.Repositories.Units;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitRepository _unitRepository;


        public UnitsController(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }


        [HttpGet("all/units")]
        public async Task<ActionResult<IEnumerable<Units>>> Get()
        {
            var units = await _unitRepository.GetUnits();
            if (units == null || !units.Any())
            {
                return NotFound(new { Status = "Error", Message = "No Units found." });
            }
            return Ok(new { Status = "Success", Data = units });
        }

        [HttpGet("all/unitChargesMapping")]
        public async Task<ActionResult<IEnumerable<UnitChargesMapping>>> GetUnitChargesMapping()
        {
            var units = await _unitRepository.GetUnitChargesMapping();
            if (units == null || !units.Any())
            {
                return NotFound(new { Status = "Error", Message = "No Unit Charges Mapping found." });
            }
            return Ok(new { Status = "Success", Data = units });
        }

        // GET api/<AccountGroupController>/5
        [HttpGet("units/{companyId}")]
        public async Task<ActionResult<PagedResultDto<Units>>> GetUnitsByCompanyId(int companyId, int pageNumber = 1, int pageSize = 10)
        {

            var units = await _unitRepository.GetUnitByCompanyId(companyId,pageNumber,pageSize);
            
            if (units == null)
            {
                return NotFound(new { Status = "Error", Message = "No units found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = units });
        }


        [HttpPost]
        public async Task<IActionResult> CreateUnits([FromBody] DTOs.CreateUnit units)
        {
            if (units == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid unit." });
            }

            var createdUnits = await _unitRepository.CreateUnit(units);
            return CreatedAtAction(nameof(GetUnitById), new { id = createdUnits }, new { Status = "Success", Data = createdUnits });
        }

        [HttpPost("UnitChargesMapping")]
        public async Task<IActionResult> CreateUnitCharges([FromBody] DTOs.UnitCharges units)
        {
            if (units == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid unit." });
            }

            var createdUnits = await _unitRepository.CreateUnitCharges(units);
            return CreatedAtAction(nameof(GetUnitById), new { id = createdUnits }, new { Status = "Success", Data = createdUnits });
        }

        [HttpPost("units/CreateUnitChargesMapping")]
        public async Task<IActionResult> CreateUnitChargesMapping([FromBody] DTOs.CreateUnitChargesMapping units)
        {
            if (units == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid unit charges group." });
            }

            var createdUnits = await _unitRepository.CreateUnitChargesMapping(units);
            return CreatedAtAction(nameof(GetUnitById), new { id = createdUnits }, new { Status = "Success", Data = createdUnits });
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTOs.UnitChargesDTO>> GetUnitById(int id)
        {
            var units = await _unitRepository.GetUnitsByUnitId(id);
            if (units == null)
            {
                return NotFound(new { Status = "Error", Message = "units not found." });
            }
            return Ok(new { Status = "Success", Data = units });
        }

        [HttpGet("GetUnitsByProprtyId/{id}")]
        public async Task<ActionResult<DTOs.UnitChargesDTO>> GetUnitBypropertyId(int id)
        {
            var units = await _unitRepository.GetUnitsByPropertyId(id);
            if (units == null)
            {
                return NotFound(new { Status = "Error", Message = "units not found." });
            }
            return Ok(new { Status = "Success", Data = units });
        }

        [HttpGet("unitcharges/{unitChargeId}")]
        public async Task<ActionResult<Units>> GetUnitChargeById(int unitChargeId)
        {
            var units = await _unitRepository.GetUnitChargesByUnitId(unitChargeId);
            if (units == null)
            {
                return NotFound(new { Status = "Error", Message = "units not found." });
            }
            return Ok(new { Status = "Success", Data = units });
        }



        [HttpPut("UpdateUnits")]
        public async Task<IActionResult> UpdateUnits([FromBody] DTOs.UnitChargesDTO unit)
        {
            if (unit == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid units  data" });
            }

            var updated = await _unitRepository.UpdateUnit(unit);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "units not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Units updated successfully." });
        }

        [HttpPut("UpdateUnitChargesMapping/{id}")]
        public async Task<IActionResult> UpdateUnitChargesMapping(int id, [FromBody] Models.UnitChargesMapping units)
        {
            if (units == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid units  data" });
            }

            var updated = await _unitRepository.UpdateUnitChargesMapping(id, units);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "units not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Units updated successfully." });
        }

        // DELETE api/<AccountGroupController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnits(int id)
        {
            var deleted = await _unitRepository.DeleteUnit(id);
            if (!deleted)
            {
                return NotFound(new { Status = "Error", Message = "Unit not found or delete failed." });
            }

            return Ok(new { Status = "Success", Message = "Unit deleted successfully." });
        }

        [HttpGet("Units/search")]
        public async Task<ActionResult<PagedResultDto<Units>>> Get(
          string? unitName,
          string? propertyName,
          int pageNumber = 1,
          int pageSize = 10)
        {
            var result = await _unitRepository.GetUnitsAsync(unitName, propertyName, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
