using MenuService.Application.DTOs;
using MenuService.Application.DTOs.Common;
using MenuService.Application.DTOs.Drinks;
using MenuService.Application.UseCases.Drinks.Commands;
using MenuService.Application.UseCases.Drinks.Handlers;
using MenuService.Application.UseCases.Drinks.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuService.Api.Controllers;

[ApiController]
[Route("api/v1/drinks")]
[Authorize(Roles = "Admin,Waitress")]
public class DrinksController : ControllerBase
{
    private readonly CreateDrinkHandler _createDrinkHandler;
    private readonly UpdateDrinkHandler _updateDrinkHandler;
    private readonly DeleteDrinkHandler _deleteDrinkHandler;
    private readonly GetAllDrinksHandler _getAllDrinksHandler;
    private readonly GetDrinkByIdHandler _getDrinkByIdHandler;
    private readonly GetDrinksByCategoryHandler _getDrinksByCategoryHandler;

    public DrinksController(
        CreateDrinkHandler createDrinkHandler,
        UpdateDrinkHandler updateDrinkHandler,
        DeleteDrinkHandler deleteDrinkHandler,
        GetAllDrinksHandler getAllDrinksHandler,
        GetDrinkByIdHandler getDrinkByIdHandler,
        GetDrinksByCategoryHandler getDrinksByCategoryHandler)
    {
        _createDrinkHandler = createDrinkHandler;
        _updateDrinkHandler = updateDrinkHandler;
        _deleteDrinkHandler = deleteDrinkHandler;
        _getAllDrinksHandler = getAllDrinksHandler;
        _getDrinkByIdHandler = getDrinkByIdHandler;
        _getDrinksByCategoryHandler = getDrinksByCategoryHandler;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<DrinkDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PagedResultDto<DrinkDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] bool? available = null,
        [FromQuery] string? sort = null)
    {
        var result = await _getAllDrinksHandler.HandleAsync(new GetAllDrinksQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Search = search,
            Available = available,
            Sort = sort
        });

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DrinkDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getDrinkByIdHandler.HandleAsync(new GetDrinkByIdQuery { Id = id });

        return Ok(result);
    }

    [HttpGet("category/{categoryId:guid}")]
    [ProducesResponseType(typeof(PagedResultDto<DrinkDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PagedResultDto<DrinkDto>>> GetByCategory(
        Guid categoryId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _getDrinksByCategoryHandler.HandleAsync(new GetDrinksByCategoryQuery
        {
            CategoryId = categoryId,
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(DrinkDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateDrinkDto dto)
    {
        var result = await _createDrinkHandler.HandleAsync(new CreateDrinkCommand
        {
            Drink = dto
        });

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(DrinkDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDrinkDto dto)
    {
        var result = await _updateDrinkHandler.HandleAsync(new UpdateDrinkCommand
        {
            Id = id,
            Drink = dto
        });

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deleteDrinkHandler.HandleAsync(new DeleteDrinkCommand { Id = id });
        return NoContent();
    }
}
