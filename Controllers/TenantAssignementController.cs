using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.Dtos;
using XeniaRentalApi.Repositories.TenantAssignment;

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


        [HttpGet("company/all/{companyId}")]
        public async Task<IActionResult> GetByCompanyAllId(int companyId)
        {
            var data = await _tenantAssignmentRepository.GetByCompanyAllId(companyId);
            return Ok(data);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompanyId( int companyId, bool isBedSpace = false, DateTime? startDate = null, DateTime? endDate = null,int? propertyId = null, int? unitId = null, string? search = null)
        {
            var data = await _tenantAssignmentRepository.GetByCompanyIdAsync( companyId,isBedSpace,startDate, endDate, propertyId,unitId, search );

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
            var updated = await _tenantAssignmentRepository.UpdateAsync(id,dto);
            if (updated == null) return NotFound(new { Message = "Tenant Assignment not found." });

            return Ok(new { Message = "Tenant Assignment updated successfully", Data = updated });
        }


        [HttpPut("closure/{id}")]
        public async Task<IActionResult> CloseUpdate(int id, [FromBody] TenantClosureCreateDto dto)
        {
            if (id != 1) return BadRequest("ID mismatch.");

            var updated = await _tenantAssignmentRepository.UpdateClosureAsync(id, dto);
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
