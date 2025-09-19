using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Account;
using XeniaRentalApi.Repositories.Ledger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class LedgerController : ControllerBase
    {
        private readonly ILedgerRepository _ledgerRepository;


        public LedgerController(ILedgerRepository ledgerRepository)
        {
            _ledgerRepository = ledgerRepository;
        }


        [HttpGet("all/ledgers")]
        public async Task<ActionResult<IEnumerable<Accounts>>> Get()
        {
            var ledgers = await _ledgerRepository.GetLedgers();
            if (ledgers == null || !ledgers.Any())
            {
                return NotFound(new { Status = "Error", Message = "No ledgers found." });
            }
            return Ok(new { Status = "Success", Data = ledgers });
        }

        // GET api/<AccountGroupController>/5
        [HttpGet("ledgers/{companyId}")]
        public async Task<ActionResult<IEnumerable<Ledger>>> GetLedgersByCompanyId(int companyId)
        {

            var ledgers = await _ledgerRepository.GetLedgerDetails(companyId);
            if (ledgers == null || !ledgers.Any())
            {
                return NotFound(new { Status = "Error", Message = "No ledgers found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = ledgers });
        }


        [HttpPost]
        public async Task<IActionResult> CreateLedgers([FromBody] Models.Ledger ledger)
        {
            if (ledger == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid ledger group." });
            }

            var createdLedger = await _ledgerRepository.CreateLedger(ledger);
            return CreatedAtAction(nameof(GetLedgerAccount), new { id = createdLedger }, new { Status = "Success", Data = createdLedger });
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Ledger>> GetLedgerAccount(int id)
        {
            var ledgers = await _ledgerRepository.GetLedgerbyId(id);
            if (ledgers == null)
            {
                return NotFound(new { Status = "Error", Message = "Ledger not found." });
            }
            return Ok(new { Status = "Success", Data = ledgers });
        }



        [HttpPut("UpdateLedger/{id}")]
        public async Task<IActionResult> UpdateLedger(int id, [FromBody] Models.Ledger ledger)
        {
            if (ledger == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid ledger data" });
            }

            var updated = await _ledgerRepository.UpdateLedger(id, ledger);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "ledger not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Ledger updated successfully." });
        }

        [HttpGet("UpdateLedger/search")]
        public async Task<ActionResult<PagedResultDto<Ledger>>> Get(
           string? search,
           int pageNumber = 1,
           int pageSize = 10)
        {
            var result = await _ledgerRepository.GetLedgerAsync(search, pageNumber, pageSize);
            return Ok(result);
        }


    }
}
