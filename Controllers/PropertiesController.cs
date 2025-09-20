﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.Dictionnary;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Account;
using XeniaRentalApi.Repositories.Properties;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertiesRepository _propertyRepository;


        public PropertiesController(IPropertiesRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }


        [HttpGet("all/properties/{companyId}")]
        public async Task<ActionResult<IEnumerable<XRS_Properties>>> Get(int companyId)
        {
            var properties = await _propertyRepository.GetProperties(companyId);
            if (properties == null || !properties.Any())
            {
                return NotFound(new { Status = "Error", Message = "No properties found." });
            }
            return Ok(new { Status = "Success", Data = properties });
        }

        
        [HttpGet("properties/company/{companyId}")]
        public async Task<ActionResult<PagedResultDto<XRS_Properties>>> GetPropertyByCompanyId(int companyId, string? search = null, int pageNumber = 1, int pageSize = 10)
        {

            var accounts = await _propertyRepository.GetPropertiesByCompanyId(companyId, search, pageNumber, pageSize);
            if (accounts == null)
            {
                return NotFound(new { Status = "Error", Message = "No properties found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = accounts });
        }


        [HttpPost]
        public async Task<IActionResult> CreateProperties([FromBody] XRS_Properties properties)
        {
            if (properties == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid properties group." });
            }

            var createdProperty = await _propertyRepository.CreateProperties(properties);
            return CreatedAtAction(nameof(GetPropertyById), new { id = createdProperty }, new { Status = "Success", Data = createdProperty });
        }

      
        [HttpGet("GetPropertyById/{id}")]
        public async Task<ActionResult<XRS_Properties>> GetPropertyById(int id)
        {
            var properties = await _propertyRepository.GetPrpoertiesbyId(id);
            if (properties == null)
            {
                return NotFound(new { Status = "Error", Message = "properties not found." });
            }
            return Ok(new { Status = "Success", Data = properties });
        }



        [HttpPut("UpdateProperties/{id}")]
        public async Task<IActionResult> UpdateProperties(int id, [FromBody] XRS_Properties property)
        {
            if (property == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid property data" });
            }

            var updated = await _propertyRepository.UpDateProperties(id, property);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "property not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Properties updated successfully." });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var deleted = await _propertyRepository.DeleteProperty(id);
            if (!deleted)
            {
                return NotFound(new { Status = "Error", Message = "property not found or delete failed." });
            }

            return Ok(new { Status = "Success", Message = "Property deleted successfully." });
        }
     
    }
}
