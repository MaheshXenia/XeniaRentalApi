using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.AccountGroups;
using XeniaRentalApi.Repositories.Auth;
using XeniaRentalApi.Service.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountGroupController : ControllerBase
    {
        private readonly IAccountGroupRepository _accountGroupRepository;

   
        public AccountGroupController(IAccountGroupRepository accountGroupRepository)
        {
            _accountGroupRepository = accountGroupRepository;
        }


        [HttpGet("all/accountGroups")]
        public async Task<ActionResult<IEnumerable<Models.AccountGroups>>> Get()
        {
            var accountGroups = await _accountGroupRepository.GetAccountGroups();
            if (accountGroups == null || !accountGroups.Any())
            {
                return NotFound(new { Status = "Error", Message = "No accountGroups found." });
            }
            return Ok(new { Status = "Success", Data = accountGroups });
        }

        // GET api/<AccountGroupController>/5
        [HttpGet("accountGroups/{companyId}")]
        public async Task<ActionResult<IEnumerable<Models.AccountGroups>>> GetAccountGroupsByCompanyId(int companyId)
        {
            
            var accountGroups = await _accountGroupRepository.GetAccountGroupsByCompanyId(companyId);
            if (accountGroups == null || !accountGroups.Any())
            {
                return NotFound(new { Status = "Error", Message = "No accountGroups found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = accountGroups });
        }

    
        [HttpPost]
        public async Task<IActionResult> CreateAccountGroups([FromBody] Models.AccountGroups accountGroup)
        {
            if (accountGroup == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid account group." });
            }

            var createdAccountGroup = await _accountGroupRepository.CreateAccountGroups(accountGroup);
            return CreatedAtAction(nameof(GetaccountGroup), new { id = createdAccountGroup }, new { Status = "Success", Data = createdAccountGroup });
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.AccountGroups>> GetaccountGroup(int id)
        {
            var accountGroup = await _accountGroupRepository.GetAccountGroupById(id);
            if (accountGroup == null)
            {
                return NotFound(new { Status = "Error", Message = "AccountGroup not found." });
            }
            return Ok(new { Status = "Success", Data = accountGroup });
        }



        [HttpPut("UpdateAccountGroup/{id}")]
        public async Task<IActionResult> UpdateAccountGroup(int id, [FromBody] Models.AccountGroups account)
        {
            if (account == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid account data" });
            }

            var updated = await _accountGroupRepository.UpdateAccountGroup(id, account);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "account not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Account Group updated successfully." });
        }

        // DELETE api/<AccountGroupController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var deleted = await _accountGroupRepository.DeleteAccountGroup(id);
            if (!deleted)
            {
                return NotFound(new { Status = "Error", Message = "AccountGroup not found or delete failed." });
            }

            return Ok(new { Status = "Success", Message = "Account Group deleted successfully." });
        }

        [HttpGet("accountGroups/search")]
        public async Task<ActionResult<PagedResultDto<XRS_Units>>> Get(
            string? search,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var result = await _accountGroupRepository.GetAccountGroupsAsync(search, pageNumber, pageSize);
            return Ok(result);
        }



    }
}
