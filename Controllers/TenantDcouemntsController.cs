using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Account;
using XeniaRentalApi.Repositories.TenantDocuments;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TenantDcouemntsController : ControllerBase
    {
        private readonly ITenantDocumentRepository _tenantDocumentRepository;


        public TenantDcouemntsController(ITenantDocumentRepository tenantDocumentRepository)
        {
            _tenantDocumentRepository = tenantDocumentRepository;
        }


        [HttpGet("all/tenantdocuments")]
        public async Task<ActionResult<IEnumerable<TenantDocuments>>> Get()
        {
            var documents = await _tenantDocumentRepository.GetTenantDocuments();
            if (documents == null || !documents.Any())
            {
                return NotFound(new { Status = "Error", Message = "No tenantDocuments found." });
            }
            return Ok(new { Status = "Success", Data = documents });
        }

        // GET api/<AccountGroupController>/5
        [HttpGet("tenantdocuments/{companyId}")]
        public async Task<ActionResult<PagedResultDto<TenantDocuments>>> GetTenantDocumentsByCompanyId(int companyId, int pageNumber = 1, int pageSize = 10)
        {

            var documents = await _tenantDocumentRepository.GetTenantDocumentsByCompanyId(companyId, pageNumber, pageSize);
            if (documents == null)
            {
                return NotFound(new { Status = "Error", Message = "No tenantDocuments found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = documents });
        }


        [HttpPost]
        public async Task<IActionResult> CreateTenantDocuments([FromBody] DTOs.CreateTenantDocuments account)
        {
            if (account == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid tenantDocuments." });
            }

            var createdAccount = await _tenantDocumentRepository.CreateTenantDocuments(account);
            return CreatedAtAction(nameof(GetTenantdocuments), new { id = createdAccount }, new { Status = "Success", Data = createdAccount });
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TenantDocuments>> GetTenantdocuments(int id)
        {
            var documents = await _tenantDocumentRepository.GetTenantDocumentsbyId(id);
            if (documents == null)
            {
                return NotFound(new { Status = "Error", Message = "tenantDocuments not found." });
            }
            return Ok(new { Status = "Success", Data = documents });
        }



        [HttpPut("UpdateTenantDocument/{id}")]
        public async Task<IActionResult> UpdateUpdateTenantDocument(int id, [FromBody] Models.TenantDocuments documents)
        {
            if (documents == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid tenant document data" });
            }

            var updated = await _tenantDocumentRepository.UpDateTenantDocument(id, documents);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "tenant not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Tenant document updated successfully." });
        }

        // DELETE api/<AccountGroupController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenantDocuments(int id)
        {
            var deleted = await _tenantDocumentRepository.DeleteTenantDocument(id);
            if (!deleted)
            {
                return NotFound(new { Status = "Error", Message = "Tenant Documents not found or delete failed." });
            }

            return Ok(new { Status = "Success", Message = "Tenant documents deleted successfully." });
        }

        [HttpGet("TenantDocuments/search")]
        public async Task<ActionResult<PagedResultDto<TenantDocuments>>> Get(
          string? search,
          int pageNumber = 1,
          int pageSize = 10)
        {
            var result = await _tenantDocumentRepository.GetTenantDocumentsAsync(search, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
