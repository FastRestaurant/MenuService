using MenuService.Application.DTOs;
using MenuService.Application.DTOs.Common;
using MenuService.Application.DTOs.Dishes;
using MenuService.Application.UseCases.Dishes.Commands;
using MenuService.Application.UseCases.Dishes.Handlers;
using MenuService.Application.UseCases.Dishes.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuService.Api.Controllers;

[ApiController]
[Route("api/v1/dishes")]
[Authorize(Roles = "Admin,Waitress")]
public class DishesController : ControllerBase
{
    private readonly CreateDishHandler _createDishHandler;
    private readonly UpdateDishHandler _updateDishHandler;
    private readonly DeleteDishHandler _deleteDishHandler;
    private readonly GetAllDishesHandler _getAllDishesHandler;
    private readonly GetDishByIdHandler _getDishByIdHandler;
    private readonly GetDishesByCategoryHandler _getDishesByCategoryHandler;

    public DishesController(
        CreateDishHandler createDishHandler,
        UpdateDishHandler updateDishHandler,
        DeleteDishHandler deleteDishHandler,
        GetAllDishesHandler getAllDishesHandler,
        GetDishByIdHandler getDishByIdHandler,
        GetDishesByCategoryHandler getDishesByCategoryHandler)
    {
        _createDishHandler = createDishHandler;
        _updateDishHandler = updateDishHandler;
        _deleteDishHandler = deleteDishHandler;
        _getAllDishesHandler = getAllDishesHandler;
        _getDishByIdHandler = getDishByIdHandler;
        _getDishesByCategoryHandler = getDishesByCategoryHandler;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<DishDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PagedResultDto<DishDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] bool? available = null,
        [FromQuery] string? sort = null)
    {
        var result = await _getAllDishesHandler.HandleAsync(new GetAllDishesQuery
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
    [ProducesResponseType(typeof(DishDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getDishByIdHandler.HandleAsync(new GetDishByIdQuery { Id = id });

        return Ok(result);
    }

    [HttpGet("category/{categoryId:guid}")]
    [ProducesResponseType(typeof(PagedResultDto<DishDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PagedResultDto<DishDto>>> GetByCategory(
        Guid categoryId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _getDishesByCategoryHandler.HandleAsync(new GetDishesByCategoryQuery
        {
            CategoryId = categoryId,
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(DishDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateDishDto dto)
    {
        var result = await _createDishHandler.HandleAsync(new CreateDishCommand
        {
            Dish = dto
        });

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(DishDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDishDto dto)
    {
        var result = await _updateDishHandler.HandleAsync(new UpdateDishCommand
        {
            Id = id,
            Dish = dto
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
        await _deleteDishHandler.HandleAsync(new DeleteDishCommand { Id = id });
        return NoContent();
    }
}
