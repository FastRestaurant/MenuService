using MenuService.Application.DTOs.Dishes;
using MenuService.Application.UseCases.Dishes.Commands;
using MenuService.Application.UseCases.Dishes.Handlers;
using MenuService.Application.UseCases.Dishes.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _getAllDishesHandler.HandleAsync(new GetAllDishesQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getDishByIdHandler.HandleAsync(new GetDishByIdQuery { Id = id });
<<<<<<< HEAD
=======

>>>>>>> 34cbd93e06be99c72ca561340d10f29cf75cc0d3
        return Ok(result);
    }

    [HttpGet("category/{categoryId:guid}")]
    public async Task<IActionResult> GetByCategory(
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
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deleteDishHandler.HandleAsync(new DeleteDishCommand { Id = id });
        return NoContent();
    }
}
