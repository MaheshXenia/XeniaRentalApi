using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Account;
using XeniaRentalApi.Repositories.Tenant;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ITenantRepository _tenantRepository;


        public TenantController(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }


        [HttpGet("all/tenants")]
        public async Task<ActionResult<IEnumerable<Tenant>>> Get()
        {
            var tenants = await _tenantRepository.GetTenants();
            if (tenants == null || !tenants.Any())
            {
                return NotFound(new { Status = "Error", Message = "No tenants  found." });
            }
            return Ok(new { Status = "Success", Data = tenants });
        }

        // GET api/<AccountGroupController>/5
        [HttpGet("tenants/{companyId}")]
        public async Task<ActionResult<PagedResultDto<Tenant>>> GetTenantsByCompanyId(int companyId, int pageNumber = 1, int pageSize = 10)
        {

            var tenants = await _tenantRepository.GetTenantsByCompanyId(companyId,pageNumber,pageSize);
            if (tenants == null)
            {
                return NotFound(new { Status = "Error", Message = "No tenants found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = tenants });
        }


        [HttpPost]
        public async Task<IActionResult> CreateTenants([FromBody] DTOs.CreateTenant tenant)
        {
            if (tenant == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid tenant." });
            }

            var createdTenant = await _tenantRepository.CreateTenant(tenant);
            return CreatedAtAction(nameof(GetTenantById), new { id = createdTenant }, new { Status = "Success", Data = createdTenant });
        }

        [HttpPost("create-tenant-document")]
        public async Task<IActionResult> CreateTenantWithDocument([FromBody] TenantWithDocumentsDto dto)
        {
            if (dto == null || dto.Tenant == null || dto.Documents == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid tenant." });
            }

            var createdTenant = await _tenantRepository.AddTenantWithDocumentsAsync(dto);
            return CreatedAtAction(nameof(GetTenantById), new { id = createdTenant }, new { Status = "Success", Data = createdTenant });

        }


        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<DTOs.TenantDocumentGetDTO>> GetTenantById(int id)
        {
            var response = await _tenantRepository.GetTenantsbyId(id);
            if (response.Tenant == null)
            {
                return NotFound(new { Status = "Error", Message = "Tenant not found." });
            }
            return Ok(new { Status = "Success", Data = response });
        }



        [HttpPut("UpdateTenant/{id}")]
        public async Task<IActionResult> UpdateTenant(int id, [FromBody] Models.Tenant tenant)  
        {
            if (tenant == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid tenant data" });
            }

            var updated = await _tenantRepository.UpDateTenant(id, tenant);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "tenant not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Tenant updated successfully." });
        }

        [HttpPut("UpdateTenantDocuments")]
        public async Task<IActionResult> UpdateTenantDocuments([FromBody] DTOs.TenantDocumentDTO documents)
        {
            if (documents == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid tenant data" });
            }

            var updated = await _tenantRepository.UpDateTenantDocuments(documents);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "tenant not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Tenant updated successfully." });
        }

        // DELETE api/<AccountGroupController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenant(int id)
        {
            var deleted = await _tenantRepository.DeleteTenant(id);
            if (!deleted)
            {
                return NotFound(new { Status = "Error", Message = "Tenant not found or delete failed." });
            }

            return Ok(new { Status = "Success", Message = "Tenant deleted successfully." });
        }

        //[Authorize]
        [HttpPost("images")]
        [RequestSizeLimit(10_000_000)]
        public async Task<IActionResult> UploadTenantimages([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files selected.");
            }

            var result = await _tenantRepository.UploadFilesAsync(files);

            return Ok(result);
        }

        [HttpGet("Tenant/search")]
        public async Task<ActionResult<PagedResultDto<Tenant>>> Get(
          string? search,
          int pageNumber = 1,
          int pageSize = 10)
        {
            var result = await _tenantRepository.GetTenantAsync(search, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
