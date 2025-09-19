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


        [HttpGet("all/bedspacePlans")]
        public async Task<ActionResult<IEnumerable<BedSpacePlan>>> Get()
        {
            var bedSpacePlans = await _bedSpacePlanRepository.GetBedSpacePlans();
            if (bedSpacePlans == null || !bedSpacePlans.Any())
            {
                return NotFound(new { Status = "Error", Message = "No BedpacePlans found." });
            }
            return Ok(new { Status = "Success", Data = bedSpacePlans });
        }

        // GET api/<AccountGroupController>/5
        [HttpGet("bedpacePlans/{companyId}")]
        public async Task<ActionResult<PagedResultDto<BedSpacePlan>>> GetPlansByCompanyId(int companyId, int pageNumber = 1,
            int pageSize = 10)
        {

            var plans = await _bedSpacePlanRepository.GetBedSpacePlanbyCompanyId(companyId,pageNumber,pageSize);
            if (plans == null)
            {
                return NotFound(new { Status = "Error", Message = "No plans found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = plans });
        }


        [HttpPost]
        public async Task<IActionResult> CreateBedSpacePlan([FromBody] DTOs.CreateBedSpacePlan bedSpacePlan)
        {
            if (bedSpacePlan == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid bedpaceplan." });
            }

            var createdPlan = await _bedSpacePlanRepository.CreateBedSpacePlan(bedSpacePlan);
            return CreatedAtAction(nameof(GetBedSpacePlanbyId), new { id = createdPlan }, new { Status = "Success", Data = createdPlan });
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BedSpacePlan>> GetBedSpacePlanbyId(int id)
        {
            var bedSpacePlan = await _bedSpacePlanRepository.GetBedSpacePlanbyId(id);
            if (bedSpacePlan == null)
            {
                return NotFound(new { Status = "Error", Message = "Plan not found." });
            }
            return Ok(new { Status = "Success", Data = bedSpacePlan });
        }



        [HttpPut("UpdateBedSpacePlan/{id}")]
        public async Task<IActionResult> UpdateBedSpacePlan(int id, [FromBody] Models.BedSpacePlan account)
        {
            if (account == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid bedspaceplan data" });
            }

            var updated = await _bedSpacePlanRepository.UpdateBedSpacePlan(id, account);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "bedspaceplan not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "BedSpacePlan updated successfully." });
        }

        // DELETE api/<AccountGroupController>/5
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

        [HttpGet("bedspaceplan/search")]
        public async Task<ActionResult<PagedResultDto<BedSpacePlan>>> Get(
            string? search,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var result = await _bedSpacePlanRepository.GetBedSpacePlanAsync(search, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
