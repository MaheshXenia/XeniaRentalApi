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


        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<XRS_Units>>> Get(int companyId, int? propertyId = null)
        {
            var units = await _unitRepository.GetUnits(companyId, propertyId);
            if (units == null || !units.Any())
            {
                return NotFound(new { Status = "Error", Message = "No Units found." });
            }
            return Ok(new { Status = "Success", Data = units });
        }


        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<PagedResultDto<XRS_Units>>> GetUnitByCompanyId(int companyId,[FromQuery] string? search = null,[FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10)
        {
            var result = await _unitRepository.GetUnitByCompanyId(companyId, search, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<XRS_Units>> GetUnit(int id)
        {
            var unit = await _unitRepository.GetUnitById(id);
            if (unit == null)
                return NotFound();
            return Ok(unit);
        }


        [HttpPost]
        public async Task<ActionResult<XRS_Units>> CreateUnit([FromBody] XRS_Units model)
        {
            if (model == null)
                return BadRequest();

            var createdUnit = await _unitRepository.CreateUnit(model);
            return CreatedAtAction(nameof(GetUnit), new { id = createdUnit.UnitId }, createdUnit);
        }

       
        [HttpPut("{id}")]
        public async Task<ActionResult<XRS_Units>> UpdateUnit(int id, [FromBody] XRS_Units model)
        {
            if (model == null || id != model.UnitId)
                return BadRequest();

            var updatedUnit = await _unitRepository.UpdateUnit(model);
            if (updatedUnit == null)
                return NotFound();

            return Ok(updatedUnit);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUnit(int id)
        {
            var result = await _unitRepository.DeleteUnit(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
