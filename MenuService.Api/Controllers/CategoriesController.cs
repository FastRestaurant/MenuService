using MenuService.Application.DTOs.Categories;
using MenuService.Application.UseCases.Categories.Commands;
using MenuService.Application.UseCases.Categories.Handlers;
using MenuService.Application.UseCases.Categories.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Waitress")]
public class CategoriesController : ControllerBase
{
    private readonly CreateCategoryHandler _createCategoryHandler;
    private readonly UpdateCategoryHandler _updateCategoryHandler;
    private readonly DeleteCategoryHandler _deleteCategoryHandler;
    private readonly GetAllCategoriesHandler _getAllCategoriesHandler;
    private readonly GetCategoryByIdHandler _getCategoryByIdHandler;

    public CategoriesController(
        CreateCategoryHandler createCategoryHandler,
        UpdateCategoryHandler updateCategoryHandler,
        DeleteCategoryHandler deleteCategoryHandler,
        GetAllCategoriesHandler getAllCategoriesHandler,
        GetCategoryByIdHandler getCategoryByIdHandler)
    {
        _createCategoryHandler = createCategoryHandler;
        _updateCategoryHandler = updateCategoryHandler;
        _deleteCategoryHandler = deleteCategoryHandler;
        _getAllCategoriesHandler = getAllCategoriesHandler;
        _getCategoryByIdHandler = getCategoryByIdHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getAllCategoriesHandler.HandleAsync(new GetAllCategoriesQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getCategoryByIdHandler.HandleAsync(
            new GetCategoryByIdQuery { Id = id });

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        var result = await _createCategoryHandler.HandleAsync(new CreateCategoryCommand
        {
            Category = dto
        });

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryDto dto)
    {
        var result = await _updateCategoryHandler.HandleAsync(new UpdateCategoryCommand
        {
            Id = id,
            Category = dto
        });

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deleteCategoryHandler.HandleAsync(new DeleteCategoryCommand { Id = id });
        return NoContent();
    }
}
