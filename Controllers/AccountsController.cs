using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Account;
using XeniaRentalApi.Repositories.AccountGroups;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;


        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        [HttpGet("all/accounts")]
        public async Task<ActionResult<IEnumerable<Accounts>>> Get()
        {
            var accounts = await _accountRepository.GetAccounts();
            if (accounts == null || !accounts.Any())
            {
                return NotFound(new { Status = "Error", Message = "No accounts found." });
            }
            return Ok(new { Status = "Success", Data = accounts });
        }

        // GET api/<AccountGroupController>/5
        [HttpGet("accounts/{companyId}")]
        public async Task<ActionResult<IEnumerable<Accounts>>> GetAccountsByCompanyId(int companyId)
        {

            var accounts = await _accountRepository.GetAccountByCompanyId(companyId);
            if (accounts == null || !accounts.Any())
            {
                return NotFound(new { Status = "Error", Message = "No accounts found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = accounts });
        }


        [HttpPost]
        public async Task<IActionResult> CreateAccounts([FromBody] Models.Accounts account)
        {
            if (account == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid accounts." });
            }

            var createdAccount = await _accountRepository.CreateAccounts(account);
            return CreatedAtAction(nameof(Getaccount), new { id = createdAccount }, new { Status = "Success", Data = createdAccount });
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Accounts>> Getaccount(int id)
        {
            var accountGroup = await _accountRepository.GetAccountById(id);
            if (accountGroup == null)
            {
                return NotFound(new { Status = "Error", Message = "Accounts not found." });
            }
            return Ok(new { Status = "Success", Data = accountGroup });
        }



        [HttpPut("UpdateAccount/{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] Models.Accounts account)
        {
            if (account == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid account data" });
            }

            var updated = await _accountRepository.UpdateAccount(id, account);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "account not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Account updated successfully." });
        }

        // DELETE api/<AccountGroupController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var deleted = await _accountRepository.DeleteAccounts(id);
            if (!deleted)
            {
                return NotFound(new { Status = "Error", Message = "Account not found or delete failed." });
            }

            return Ok(new { Status = "Success", Message = "Account deleted successfully." });
        }

        [HttpGet("accounts/search")]
        public async Task<ActionResult<PagedResultDto<Accounts>>> Get(
            string? search,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var result = await _accountRepository.GetAccountsAsync(search, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
