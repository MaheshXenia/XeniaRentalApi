using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.Dictionnary;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Account;
using XeniaRentalApi.Repositories.Documents;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;


        public DocumentsController(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }


        [HttpGet("all/documents")]
        public async Task<ActionResult<IEnumerable<Documents>>> Get()
        {
            var documents = await _documentRepository.GetDocuments();
            if (documents == null || !documents.Any())
            {
                return NotFound(new { Status = "Error", Message = "No documents found." });
            }
            return Ok(new { Status = "Success", Data = documents });
        }

        // GET api/<AccountGroupController>/5
        [HttpGet("documents/{companyId}")]
        public async Task<ActionResult<PagedResultDto<Documents>>> GetDocumentsByCompanyId(int companyId, int pageNumber = 1,
            int pageSize = 10)
        {

            var documents = await _documentRepository.GetDocumentsCompanyId(companyId, pageNumber, pageSize);
            if (documents == null)
            {
                return NotFound(new { Status = "Error", Message = "No documents found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = documents });
        }


        [HttpPost]
        public async Task<IActionResult> CreateAccounts([FromBody] DTOs.CreateDocuments documents)
        {
            if (documents == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid documents group." });
            }

            var createdDocuments = await _documentRepository.CreateDocuments(documents);
            return CreatedAtAction(nameof(GetDocuments), new { id = createdDocuments }, new { Status = "Success", Data = createdDocuments });
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Documents>> GetDocuments(int id)
        {
            var createdDocuments = await _documentRepository.GetDocumentsbyId(id);
            if (createdDocuments == null)
            {
                return NotFound(new { Status = "Error", Message = "Documents not found." });
            }
            return Ok(new { Status = "Success", Data = createdDocuments });
        }




        [HttpPut("UpdateDocuments/{id}")]
        public async Task<IActionResult> UpdateDocuments(int id, [FromBody] Models.Documents documents)
        {
            if (documents == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid documents data" });
            }

            var updated = await _documentRepository.UpdateDocuments(id, documents);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "documents not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Documents updated successfully." });
        }

        // DELETE api/<AccountGroupController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var deleted = await _documentRepository.DeleteDocumentType(id);
            if (!deleted)
            {
                return NotFound(new { Status = "Error", Message = "Documents not found or delete failed." });
            }

            return Ok(new { Status = "Success", Message = "createdDocuments deleted successfully." });
        }

        [HttpGet("applicableTotypes")]
        public IActionResult GetApplicableToTypes()
        {
            return Ok(DocumentApplicableToTypeProvider.ApplicableTypes);
        }

        [HttpGet("UpdateDocuments/search")]
        public async Task<ActionResult<PagedResultDto<Documents>>> Get(
           string? search,
           int pageNumber = 1,
           int pageSize = 10)
        {
            var result = await _documentRepository.GetDocumentsAsync(search, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
