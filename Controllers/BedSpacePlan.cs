using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Account;
using XeniaRentalApi.Repositories.BedSpacePlan;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class BedSpacePlan : ControllerBase
    {
        private readonly IBedSpacePlanRepository _bedSpacePlanRepository;


        public BedSpacePlan(IBedSpacePlanRepository bedSpacePlanRepository)
        {
            _bedSpacePlanRepository = bedSpacePlanRepository;
        }


        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<BedSpacePlan>>> Get(int companyId)
        {
            var bedSpacePlans = await _bedSpacePlanRepository.GetBedSpacePlans(companyId);
            if (bedSpacePlans == null || !bedSpacePlans.Any())
            {
                return NotFound(new { Status = "Error", Message = "No BedpacePlans found." });
            }
            return Ok(new { Status = "Success", Data = bedSpacePlans });
        }



        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<PagedResultDto<BedSpacePlan>>> GetPlansByCompanyId(int companyId, string? search = null, int pageNumber = 1,int pageSize = 10)
        {

            var plans = await _bedSpacePlanRepository.GetBedSpacePlanByCompanyId(companyId, search,pageNumber, pageSize);
            if (plans == null)
            {
                return NotFound(new { Status = "Error", Message = "No plans found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = plans });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BedSpacePlan>> GetBedSpacePlanbyId(int id)
        {
            var bedSpacePlan = await _bedSpacePlanRepository.GetBedSpacePlanById(id);
            if (bedSpacePlan == null)
            {
                return NotFound(new { Status = "Error", Message = "Plan not found." });
            }
            return Ok(new { Status = "Success", Data = bedSpacePlan });
        }


        [HttpPost]
        public async Task<IActionResult> CreateBedSpacePlan([FromBody] XRS_BedSpacePlan bedSpacePlan)
        {
            if (bedSpacePlan == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid bedpaceplan." });
            }

            var createdPlan = await _bedSpacePlanRepository.CreateBedSpacePlan(bedSpacePlan);
            return CreatedAtAction(nameof(GetBedSpacePlanbyId), new { id = createdPlan }, new { Status = "Success", Data = createdPlan });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBedSpacePlan(int id, [FromBody] XRS_BedSpacePlan bedSpacePlan)
        {
            if (bedSpacePlan == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid bedspaceplan data" });
            }

            var updated = await _bedSpacePlanRepository.UpdateBedSpacePlan(id, bedSpacePlan);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "bedspaceplan not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "BedSpacePlan updated successfully." });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlan(int id)
        {
            var deleted = await _bedSpacePlanRepository.DeleteBedSpacePlan(id);
            if (!deleted)
            {
                return NotFound(new { Status = "Error", Message = "Plan not found or delete failed." });
            }

            return Ok(new { Status = "Success", Message = "Plan deleted successfully." });
        }

        
    }
}
