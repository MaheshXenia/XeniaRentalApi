using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Account;
using XeniaRentalApi.Repositories.BedSpaceAssignemnt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class BedSpacsAssignement : ControllerBase
    {
        private readonly IBedSpaceAssignmentRepository _bedSpaceAssignmentRepository;


        public BedSpacsAssignement(IBedSpaceAssignmentRepository bedSpaceAssignmentRepository)
        {
            _bedSpaceAssignmentRepository = bedSpaceAssignmentRepository;
        }


        [HttpGet("all/bedspaceassignments")]
        public async Task<ActionResult<IEnumerable<BedSpaceAssignemnt>>> Get()
        {
            var bedSpaceAssignemnts = await _bedSpaceAssignmentRepository.GetBedSpaceAssigments();
            if (bedSpaceAssignemnts == null || !bedSpaceAssignemnts.Any())
            {
                return NotFound(new { Status = "Error", Message = "No bedspaceassigments found." });
            }
            return Ok(new { Status = "Success", Data = bedSpaceAssignemnts });
        }

        // GET api/<AccountGroupController>/5
        [HttpGet("bedspaceassignments/{companyId}")]
        public async Task<ActionResult<PagedResultDto<BedSpaceAssignemnt>>> GetBedSpaceAssignemntsByCompanyId(int companyId, string? srch, int pageNumber = 1,
            int pageSize = 10)
        {

            var bedSpaceAssignemnts = await _bedSpaceAssignmentRepository.GetBedSpaceAssignemntsByCompanyId(companyId,srch,pageNumber,pageSize);
            if (bedSpaceAssignemnts == null)
            {
                return NotFound(new { Status = "Error", Message = "No bedSpaceAssignemnts found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = bedSpaceAssignemnts });
        }


        [HttpPost]
        public async Task<IActionResult> CreateBedSpaceAssignemnt([FromBody] DTOs.BedSpaceAssignment assignment)
        {
            if (assignment == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid assignemnet." });
            }

            var createdAssignment = await _bedSpaceAssignmentRepository.CreateBedSpaceAssignemnt(assignment);
            return CreatedAtAction(nameof(GetBedSpaceAssigmentByID), new { id = createdAssignment }, new { Status = "Success", Data = createdAssignment });
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BedSpaceAssignemnt>> GetBedSpaceAssigmentByID(int id)
        {
            var assignment = await _bedSpaceAssignmentRepository.GetBedSpaceAssignmentbyId(id);
            if (assignment == null)
            {
                return NotFound(new { Status = "Error", Message = "BedSpaceAssignment not found." });
            }
            return Ok(new { Status = "Success", Data = assignment });
        }



        [HttpPut("UpdateBedSpaceAssignment/{id}")]
        public async Task<IActionResult> UpdateBedSpaceAssignment(int id, [FromBody] Models.BedSpaceAssignemnt assignemnt)
        {
            if (assignemnt == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid bed space assignment" });
            }

            var updated = await _bedSpaceAssignmentRepository.UpdateBedSpaceAssignment(id, assignemnt);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "bed space assignment not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "bed space assignment updated successfully." });
        }

        // DELETE api/<AccountGroupController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var deleted = await _bedSpaceAssignmentRepository.DeleteBedSpaceAssignment(id);
            if (!deleted)
            {
                return NotFound(new { Status = "Error", Message = "BedSpaceAssignment not found or delete failed." });
            }

            return Ok(new { Status = "Success", Message = "BedSpaceAssignment deleted successfully." });
        }

        [HttpGet("bedspaceassignment/search")]
        public async Task<ActionResult<PagedResultDto<BedSpaceAssignemnt>>> Get(
           string? propertyName,
           string? unitName,
           string? tenantName,
           string? contactNo,
           int pageNumber = 1,
           int pageSize = 10)
        {
            var result = await _bedSpaceAssignmentRepository.GetBedSpaceAssignmentAsync(propertyName,unitName,tenantName,contactNo, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
