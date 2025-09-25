using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Company;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepsitory _companyRepository;


        public CompanyController(ICompanyRepsitory companyRepository)
        {
            _companyRepository = companyRepository;
        }


        [HttpGet("all/companies")]
        public async Task<ActionResult<IEnumerable<XRS_Company>>> Get(int pageNumber = 1, int pageSize = 10)
        {
            var companies = await _companyRepository.GetCompanies(pageNumber, pageSize);
            if (companies == null || !companies.Any())
            {
                return NotFound(new { Status = "Error", Message = "No companies found." });
            }
            return Ok(new { Status = "Success", Data = companies });
        }

       
        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<PagedResultDto<XRS_Company>>> GetCompanyByCompanyId(int companyId)
        {

            var company = await _companyRepository.GetCompanybyCompanyId(companyId, pageNumber, pageSize);
            if (company == null)
            {
                return NotFound(new { Status = "Error", Message = "No accounts found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = company });
        }


        [HttpPost]
        public async Task<IActionResult> CreateCompanies([FromBody] Models.XRS_Company company)
        {
            if (company == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid company." });
            }

            var createdCompany = await _companyRepository.CreateCompany(company);
            return CreatedAtAction(nameof(GetCompanyById), new { id = createdCompany }, new { Status = "Success", Data = createdCompany });
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<XRS_Company>> GetCompanyById(int id)
        {
            var company = await _companyRepository.GetCompanybyId(id);
            if (company == null)
            {
                return NotFound(new { Status = "Error", Message = "company not found." });
            }
            return Ok(new { Status = "Success", Data = company });
        }




        [HttpPut("UpdateCompany/{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] Models.XRS_Company company)
        {
            if (company == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid Company data" });
            }

            var updated = await _companyRepository.UpdateCompany(id, company);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "company not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Company updated successfully." });
        }

        // DELETE api/<AccountGroupController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var deleted = await _companyRepository.DeleteCompany(id);
            if (!deleted)
            {
                return NotFound(new { Status = "Error", Message = "company not found or delete failed." });
            }

            return Ok(new { Status = "Success", Message = "company deleted successfully." });
        }

        [HttpGet("company/search")]
        public async Task<ActionResult<PagedResultDto<XRS_Company>>> Get(
           string? search,
           int pageNumber = 1,
           int pageSize = 10)
        {
            var result = await _companyRepository.GetCompanyAsync(search, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
