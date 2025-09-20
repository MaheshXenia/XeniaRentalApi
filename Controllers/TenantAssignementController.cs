using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Account;
using XeniaRentalApi.Repositories.TenantAssignment;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TenantAssignementController : ControllerBase
    {
        private readonly ITenantAssignmentRepository _tenantAssignmentRepository;


        public TenantAssignementController(ITenantAssignmentRepository tenantAssignmentRepository)
        {
            _tenantAssignmentRepository = tenantAssignmentRepository;
        }


        [HttpGet("all/tenantassignments")]
        public async Task<ActionResult<IEnumerable<TenantAssignemnt>>> Get()
        {
            var assignments = await _tenantAssignmentRepository.GetTenantAssignments();
            if (assignments == null || !assignments.Any())
            {
                return NotFound(new { Status = "Error", Message = "No tenantAssignments found." });
            }
            return Ok(new { Status = "Success", Data = assignments });
        }

        [HttpGet("all/closeagreements")]
        public async Task<ActionResult<IEnumerable<TenantAssignemnt>>> GetCloseAgreements()
        {
            var assignments = await _tenantAssignmentRepository.GetAllCloseAgreemnts();
            if (assignments == null || !assignments.Any())
            {
                return NotFound(new { Status = "Error", Message = "No closeagreements found." });
            }
            return Ok(new { Status = "Success", Data = assignments });
        }

        [HttpGet("all/closeagreementByParams")]
        public async Task<ActionResult<IEnumerable<TenantAssignemnt>>> GetCloseAgreementsByParams(DateTime startdate,DateTime endDate,int propId,int unitId)
        {
            var assignments = await _tenantAssignmentRepository.GetAllCloseAgreemntsByParams(startdate,endDate,propId,unitId);
            if (assignments == null || !assignments.Any())
            {
                return NotFound(new { Status = "Error", Message = "No closeagreements found." });
            }
            return Ok(new { Status = "Success", Data = assignments });
        }

        [HttpPost("CreateCloseAgreement")]
        public async Task<IActionResult> CreateCloseAgreement([FromBody] DTOs.TenantAssignment assignment)
        {
            if (assignment == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid tenantAssignments." });
            }

            var createdAssignment = await _tenantAssignmentRepository.CreateCloseAgreements(assignment);
            return CreatedAtAction(nameof(GetTenantAssignment), new { id = createdAssignment }, new { Status = "Success", Data = createdAssignment });
        }

        // GET api/<AccountGroupController>/5
        [HttpGet("tenantassignments/{companyId}")]
        public async Task<ActionResult<PagedResultDto<TenantAssignemnt>>> GetTenantAssignmentsByCompanyId(int companyId, int pageNumber = 1, int pageSize = 10)
        {

            var tenantAssignemnts = await _tenantAssignmentRepository.GetTenantAssignmentsByCompanyId(companyId, pageNumber, pageSize);
            if (tenantAssignemnts == null)
            {
                return NotFound(new { Status = "Error", Message = "No tenantAssignments found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = tenantAssignemnts });
        }

        [HttpGet("closeagreements/{companyId}")]
        public async Task<ActionResult<PagedResultDto<TenantAssignemnt>>> GetCloseAgreementsByCompanyId(int companyId, int pageNumber = 1, int pageSize = 10)
        {

            var tenantAssignemnts = await _tenantAssignmentRepository.GetCloseAgreementsByCompanyId(companyId, pageNumber, pageSize);
            if (tenantAssignemnts == null)
            {
                return NotFound(new { Status = "Error", Message = "No tenantAssignments found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = tenantAssignemnts });
        }


        [HttpPost]
        public async Task<IActionResult> CreateTenantAssignments([FromBody] DTOs.TenantAssignment assignment)
        {
            if (assignment == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid tenantAssignments." });
            }

            var createdAssignment = await _tenantAssignmentRepository.CreateTenantAssignments(assignment);
            return CreatedAtAction(nameof(GetTenantAssignment), new { id = createdAssignment }, new { Status = "Success", Data = createdAssignment });
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTOs.TenantAssignmentDocumentGetDTO>> GetTenantAssignment(int id)
        {
            var assignment = await _tenantAssignmentRepository.GetTenantAssignemntsbyId(id);
            if (assignment == null)
            {
                return NotFound(new { Status = "Error", Message = "TenantAssignments not found." });
            }
            return Ok(new { Status = "Success", Data = assignment });
        }

        [HttpGet("GetTenantAssignemntDocumentsbyId/{assignmentid}")]
        public async Task<ActionResult<TenantAssignemnt>> GetTenantAssignemntDocumentsbyId(int assignmentid,int doctype)
        {
            var assignment = await _tenantAssignmentRepository.GetTenantAssignemntDocumentsbyId(assignmentid,doctype);
            if (assignment == null)
            {
                return NotFound(new { Status = "Error", Message = "TenantAssignment not found." });
            }
            return Ok(new { Status = "Success", Data = assignment });
        }

        [HttpPost("create-tenant-assignment-document")]
        public async Task<IActionResult> CreateTenantAssignmentWithDocument([FromBody] TenantAssignmentDocumentUploadDTO dto)
        {
            if (dto == null || dto.Assignment == null || dto.Documents == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid tenant." });
            }

            var createdTenant = await _tenantAssignmentRepository.AddTenantAssignmentWithDocumentsAsync(dto);
            return CreatedAtAction(nameof(GetTenantAssignment), new { id = createdTenant }, new { Status = "Success", Data = createdTenant });

        }


        [HttpPut("UpdateTenantAssignment")]
        public async Task<IActionResult> UpdateTenantAssignment(TenantAssignmentUpdateDTO assignemnt)
        {
            if (assignemnt == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid tenant assignemnt data" });
            }

            var updated = await _tenantAssignmentRepository.UpDateTenantAssignment(assignemnt);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "tenant not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Tenant Assignment updated successfully." });
        }

        [HttpPut("UpdateCloseAgreement/{id}")]
        public async Task<IActionResult> UpdateCloseAgreement(int id, [FromBody] Models.TenantAssignemnt assignments)
        {
            if (assignments == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid tenant assignemnt data" });
            }

            var updated = await _tenantAssignmentRepository.UpDateCloseAssignment(id, assignments);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "tenant not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Tenant Assignment updated successfully." });
        }

        // DELETE api/<AccountGroupController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var deleted = await _tenantAssignmentRepository.DeleteTenantAssignment(id);
            if (!deleted)
            {
                return NotFound(new { Status = "Error", Message = "Tenant Assignment not found or delete failed." });
            }

            return Ok(new { Status = "Success", Message = "Tenant Assignment deleted successfully." });
        }

        [HttpGet("TenantAssignment/search")]
        public async Task<ActionResult<PagedResultDto<TenantAssignemnt>>> Get(
          string? search,
          int pageNumber = 1,
          int pageSize = 10)
        {
            var result = await _tenantAssignmentRepository.GetTenantAssignmentAsync(search, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
