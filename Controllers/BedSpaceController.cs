using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Account;
using XeniaRentalApi.Repositories.BedSpace;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class BedSpaceController : ControllerBase
    {
        private readonly IBedSpaceRepository _bedSpaceRepository;


        public BedSpaceController(IBedSpaceRepository bedSpaceRepository)
        {
            _bedSpaceRepository = bedSpaceRepository;
        }


        [HttpGet("all/bedspaces")]
        public async Task<ActionResult<IEnumerable<BedSpace>>> Get()
        {
            var bedSpaces = await _bedSpaceRepository.GetBedSpaces();
            if (bedSpaces == null || !bedSpaces.Any())
            {
                return NotFound(new { Status = "Error", Message = "No bespaces found." });
            }
            return Ok(new { Status = "Success", Data = bedSpaces });
        }

        // GET api/<AccountGroupController>/5
        [HttpGet("bedpaces/{companyId}")]
        public async Task<ActionResult<PagedResultDto<BedSpace>>> GetBedSpacesByCompanyId(int companyId, int pageNumber = 1,
            int pageSize = 10)
        {

            var bedSpaces = await _bedSpaceRepository.GetBedSpacebyCompanyId(companyId,pageNumber,pageSize);
            if (bedSpaces == null)
            {
                return NotFound(new { Status = "Error", Message = "No bedspaces found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = bedSpaces });
        }


        [HttpPost]
        public async Task<IActionResult> CreateBedSpaces([FromBody] DTOs.CreateBedSpace bedSpace)
        {
            if (bedSpace == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid bedpace group." });
            }

            var createdbedSpace = await _bedSpaceRepository.CreateBedSpaces(bedSpace);
            return CreatedAtAction(nameof(GetBedSpace), new { id = createdbedSpace }, new { Status = "Success", Data = createdbedSpace });
        }
        
        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BedSpace>> GetBedSpace(int id)
        {
            var accountGroup = await _bedSpaceRepository.GetBedSpacebyId(id);
            if (accountGroup == null)
            {
                return NotFound(new { Status = "Error", Message = "BedSpace not found." });
            }
            return Ok(new { Status = "Success", Data = accountGroup });
        }

        [HttpGet("GetBedSpaceBYUnitId/{unitId}")]
        public async Task<ActionResult<BedSpace>> GetBedSpaceByUnitId(int unitId)
        {
            var accountGroup = await _bedSpaceRepository.GetBedSpacebyUnitId(unitId);
            if (accountGroup == null)
            {
                return NotFound(new { Status = "Error", Message = "BedSpace not found." });
            }
            return Ok(new { Status = "Success", Data = accountGroup });
        }



        [HttpPut("UpdateBedSpace/{id}")]
        public async Task<IActionResult> UpdateBedSpace(int id, [FromBody] Models.BedSpace account)
        {
            if (account == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid bedspace data" });
            }

            var updated = await _bedSpaceRepository.UpdateBedSpace(id, account);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "bedspace not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Bedspace updated successfully." });
        }

        // DELETE api/<AccountGroupController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBedSpace(int id)
        {
            var deleted = await _bedSpaceRepository.DeleteBedSpace(id);
            if (!deleted)
            {
                return NotFound(new { Status = "Error", Message = "BedSpace not found or delete failed." });
            }

            return Ok(new { Status = "Success", Message = "BedSpace deleted successfully." });
        }

        [HttpGet("bedspaces/search")]
        public async Task<ActionResult<PagedResultDto<BedSpace>>> Get(
            string? search,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var result = await _bedSpaceRepository.GetBedSpaceAsync(search, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
