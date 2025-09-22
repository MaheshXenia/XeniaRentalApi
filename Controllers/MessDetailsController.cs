using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.MessDetails;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class MessDetailsController : ControllerBase
    {
        // GET: api/<MessDetailsController>
        private readonly IMessDetailsRepository _messDetailsRepository;


        public MessDetailsController(IMessDetailsRepository messDetailsRepository)
        {
            _messDetailsRepository = messDetailsRepository;
        }


        [HttpGet("all/messdetails")]
        public async Task<ActionResult<IEnumerable<XRS_MessDetails>>> Get()
        {
            var messDetails = await _messDetailsRepository.GetMessDetails();
            if (messDetails == null || !messDetails.Any())
            {
                return NotFound(new { Status = "Error", Message = "No messdetails found." });
            }
            return Ok(new { Status = "Success", Data = messDetails });
        }

        // GET api/<AccountGroupController>/5
        [HttpGet("messdetails/{companyId}")]
        public async Task<ActionResult<PagedResultDto<XRS_MessDetails>>> GetMessDetailsByCompanyId(int companyId, int pageNumber = 1, int pageSize = 10)
        {

            var messDetails = await _messDetailsRepository.GetMessDetailsByCompanyId(companyId,pageNumber,pageSize);
            if (messDetails == null )
            {
                return NotFound(new { Status = "Error", Message = "No messdetails found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = messDetails });
        }


        [HttpPost]
        public async Task<IActionResult> CreateAccounts([FromBody] Models.XRS_MessDetails messDetails)
        {
            if (messDetails == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid mess details." });
            }

            var createdMessDetails = await _messDetailsRepository.CreateMessDetails(messDetails);
            return CreatedAtAction(nameof(GetMessDetails), new { id = createdMessDetails }, new { Status = "Success", Data = createdMessDetails });
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<XRS_MessDetails>> GetMessDetails(int id)
        {
            var messdetails = await _messDetailsRepository.GetMessDetailsbyId(id);
            if (messdetails == null)
            {
                return NotFound(new { Status = "Error", Message = "Mess Details not found." });
            }
            return Ok(new { Status = "Success", Data = messdetails });
        }



        [HttpPut("UpdateMessDetails/{id}")]
        public async Task<IActionResult> UpdateMessDetails(int id, [FromBody] Models.XRS_MessDetails details)
        {
            if (details == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid details data" });
            }

            var updated = await _messDetailsRepository.UpdateMessDetails(id, details);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "details not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Mess Details updated successfully." });
        }

    }
}
