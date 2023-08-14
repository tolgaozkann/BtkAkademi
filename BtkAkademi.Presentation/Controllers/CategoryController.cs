using BtkAkademi.Entities.Dtos;
using BtkAkademi.Presentation.ActionFilters;
using BtkAkademi.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BtkAkademi.Presentation.Controllers;

[ApiVersion("1.0")]
[ServiceFilter(typeof(LogFilterAttribute))]
[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class CategoryController : ControllerBase
{
    private readonly IServiceManager _services;

    public CategoryController(IServiceManager services)
    {
        _services = services;
    }

    [Authorize]
    [HttpGet(Name = "GetAllCategories")]
    public async Task<IActionResult> GetAllCategoriesAsync()
    {
        var categories = await _services.CategoryService.GetCategoriesAsync(false);

        return Ok(categories);
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOneCategoryByIdAsycn([FromRoute]int id)
    {
        var Category = await _services.CategoryService.GetOneCategoryByIdAsync(id, false);

        return Ok(Category);
    }

    [Authorize(Roles = "Admin, Editor")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [HttpPost(Name = "CreateOneCategory")]
    public async Task<IActionResult> CreateOneCategoryAsync([FromBody] InsertCategoryDto category)
    {
        var result = await _services.CategoryService.CreateOneCategory(category, true);

        return StatusCode(201, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOneBookAsync([FromRoute]int id)
    {
        await _services.CategoryService.DeleteOneCategoryAsync(id, true);

        return NoContent();
    }

    [Authorize(Roles = "Admin, Editor")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateOneBookAsync([FromRoute] int id, [FromBody] UpdateCategoryDto category)
    {
        if (category is null)
            return BadRequest("Request body is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
        await _services.CategoryService.UpdateOneCategoryAsync(id, category, false);

        return NoContent();
    }
}