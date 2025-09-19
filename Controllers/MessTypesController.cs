using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Account;
using XeniaRentalApi.Repositories.MessTypes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class MessTypesController : ControllerBase
    {
        private readonly IMessTypes _messTypesRepository;


        public MessTypesController(IMessTypes messTypesRepository)
        {
            _messTypesRepository = messTypesRepository;
        }


        [HttpGet("all/messtypes")]
        public async Task<ActionResult<IEnumerable<Models.MessTypes>>> GetMessTypes()
        {
            var messTypes = await _messTypesRepository.GetMessTypes();
            if (messTypes == null || !messTypes.Any())
            {
                return NotFound(new { Status = "Error", Message = "No messtypes found." });
            }
            return Ok(new { Status = "Success", Data = messTypes });
        }

        // GET api/<AccountGroupController>/5
        [HttpGet("messtypes/{companyId}")]
        public async Task<ActionResult<IEnumerable<Models.MessTypes>>> GetMesstypesByCompanyId(int companyId,int pageNumber = 1,int pageSize = 10)
        {

            var messtypes = await _messTypesRepository.GetMessTypesByCompanyId(companyId,pageNumber,pageSize);
            if (messtypes == null)
            {
                return NotFound(new { Status = "Error", Message = "No messtypes found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = messtypes });
        }


        [HttpPost]
        public async Task<IActionResult> CreateMesstypes([FromBody] DTOs.CreateMessTypes messtypes)
        {
            if (messtypes == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid mess types." });
            }

            var createdAccount = await _messTypesRepository.CreateMessTypes(messtypes);
            return CreatedAtAction(nameof(GetMessTypesById), new { id = createdAccount }, new { Status = "Success", Data = createdAccount });
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.MessTypes>> GetMessTypesById(int id)
        {
            var messTypes = await _messTypesRepository.GetMessTypesbyId(id);
            if (messTypes == null)
            {
                return NotFound(new { Status = "Error", Message = "messTypes not found." });
            }
            return Ok(new { Status = "Success", Data = messTypes });
        }



        [HttpPut("UpdateMessTypes/{id}")]
        public async Task<IActionResult> UpdateMessTypes(int id, [FromBody] Models.MessTypes messTypes)
        {
            if (messTypes == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid messtypes data" });
            }

            var updated = await _messTypesRepository.UpdatMessTypes(id, messTypes);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "messtypes not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "MessTypes updated successfully." });
        }

        // DELETE api/<AccountGroupController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMesstypes(int id)
        {
            var deleted = await _messTypesRepository.DeleteMessType(id);
            if (!deleted)
            {
                return NotFound(new { Status = "Error", Message = "MessTypes not found or delete failed." });
            }

            return Ok(new { Status = "Success", Message = "MessTypes deleted successfully." });
        }
    }
}
