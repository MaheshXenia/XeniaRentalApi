using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.BedSpace;
using XeniaRentalApi.Repositories.Category;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }


    [HttpGet("all/categories")]
    public async Task<ActionResult<IEnumerable<XRS_Categories>>> Get()
    {
        var categories = await _categoryRepository.GetCategories();
        if (categories == null || !categories.Any())
        {
            return NotFound(new { Status = "Error", Message = "No categories found." });
        }
        return Ok(new { Status = "Success", Data = categories });
    }

    // GET api/<AccountGroupController>/5
    [HttpGet("categories/{companyId}")]
    public async Task<ActionResult<PagedResultDto<XRS_Categories>>> GetCategoryByCompanyId(int companyId, int pageNumber = 1,
        int pageSize = 10)
    {

        var categories = await _categoryRepository.GetCategorybyCompanyId(companyId, pageNumber, pageSize);
        if (categories == null)
        {
            return NotFound(new { Status = "Error", Message = "No categories found the given Company ID." });
        }
        return Ok(new { Status = "Success", Data = categories });
    }


    [HttpPost]
    public async Task<IActionResult> CreateCategories([FromBody] XeniaRentalApi.DTOs.CreateCategory category)
    {
        if (category == null)
        {
            return BadRequest(new { Status = "Error", Message = "Invalid category." });
        }

        var createdCategory = await _categoryRepository.CreateCategory(category);
        return CreatedAtAction(nameof(GetCategory), new { id = createdCategory }, new { Status = "Success", Data = createdCategory });
    }

    //[Authorize(Roles = "Admin,SuperAdmin")]
    [HttpGet("{id}")]
    public async Task<ActionResult<XRS_Categories>> GetCategory(int id)
    {
        var category = await _categoryRepository.GetCategorybyId(id);
        if (category == null)
        {
            return NotFound(new { Status = "Error", Message = "Category not found." });
        }
        return Ok(new { Status = "Success", Data = category });
    }



    [HttpPut("UpdateCategory/{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] XeniaRentalApi.Models.XRS_Categories category)
    {
        if (category == null)
        {
            return BadRequest(new { Status = "Error", Message = "Invalid category data" });
        }

        var updated = await _categoryRepository.UpdateCategory(id, category);
        if (!updated)
        {
            return NotFound(new { Status = "Error", Message = "category not found or update failed." });
        }

        return Ok(new { Status = "Success", Message = "Category updated successfully." });
    }

    // DELETE api/<AccountGroupController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var deleted = await _categoryRepository.DeleteCategory(id);
        if (!deleted)
        {
            return NotFound(new { Status = "Error", Message = "Category not found or delete failed." });
        }

        return Ok(new { Status = "Success", Message = "Category deleted successfully." });
    }

    [HttpGet("category/search")]
    public async Task<ActionResult<PagedResultDto<XRS_Categories>>> Get(
        string? search,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var result = await _categoryRepository.GetCategoriesAsync(search, pageNumber, pageSize);
        return Ok(result);
    }
}