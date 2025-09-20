using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.Dtos;
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


        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompanyId(int companyId)
        {
            var data = await _tenantAssignmentRepository.GetByCompanyIdAsync(companyId);
            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _tenantAssignmentRepository.GetByIdAsync(id);
            if (item == null) return NotFound(new { Message = "Tenant Assignment not found." });
            return Ok(item);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TenantAssignmentCreateDto dto)
        {
            var created = await _tenantAssignmentRepository.CreateAsync(dto);
            return Ok(new { Message = "Tenant Assignment created successfully", Data = created });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TenantAssignmentCreateDto dto)
        {
            if (id != 1) return BadRequest("ID mismatch.");

            var updated = await _tenantAssignmentRepository.UpdateAsync(dto);
            if (updated == null) return NotFound(new { Message = "Tenant Assignment not found." });

            return Ok(new { Message = "Tenant Assignment updated successfully", Data = updated });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _tenantAssignmentRepository.DeleteAsync(id);
            if (!deleted) return NotFound(new { Message = "Tenant Assignment not found." });

            return Ok(new { Message = "Tenant Assignment deleted successfully" });
        }
    }
}
