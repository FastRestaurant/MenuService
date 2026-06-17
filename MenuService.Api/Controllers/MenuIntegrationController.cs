using MenuService.Application.UseCases.Dishes.Handlers;
using MenuService.Application.UseCases.Dishes.Queries;
using MenuService.Application.UseCases.Drinks.Handlers;
using MenuService.Application.UseCases.Drinks.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuService.Api.Controllers;

[ApiController]
[Route("api/menu-integration")]
[Authorize(Roles = "Admin,Waitress,Kitchen")]
public class MenuIntegrationController : ControllerBase
{
    private readonly GetDishForOrderHandler _getDishForOrderHandler;
    private readonly GetDishExistsHandler _getDishExistsHandler;
    private readonly GetDishPreparationTimeHandler _getDishPreparationTimeHandler;
    private readonly GetDrinkForOrderHandler _getDrinkForOrderHandler;

    public MenuIntegrationController(
        GetDishForOrderHandler getDishForOrderHandler,
        GetDishExistsHandler getDishExistsHandler,
        GetDishPreparationTimeHandler getDishPreparationTimeHandler,
        GetDrinkForOrderHandler getDrinkForOrderHandler)
    {
        _getDishForOrderHandler = getDishForOrderHandler;
        _getDishExistsHandler = getDishExistsHandler;
        _getDishPreparationTimeHandler = getDishPreparationTimeHandler;
        _getDrinkForOrderHandler = getDrinkForOrderHandler;
    }

    [HttpGet("dishes/{id:guid}/for-order")]
    public async Task<IActionResult> GetDishForOrder(Guid id)
    {
        var result = await _getDishForOrderHandler.HandleAsync(
            new GetDishForOrderQuery { Id = id });

        return Ok(result);
    }

    [HttpGet("drinks/{id:guid}/for-order")]
    public async Task<IActionResult> GetDrinkForOrder(Guid id)
    {
        var result = await _getDrinkForOrderHandler.HandleAsync(
            new GetDrinkForOrderQuery { Id = id });

        return Ok(result);
    }

    [HttpGet("dishes/{id:guid}/exists")]
    public async Task<IActionResult> GetDishExists(Guid id)
    {
        var result = await _getDishExistsHandler.HandleAsync(
            new GetDishExistsQuery { Id = id });

        return Ok(result);
    }

    [HttpGet("dishes/{id:guid}/preparation-time")]
    public async Task<IActionResult> GetDishPreparationTime(Guid id)
    {
        var result = await _getDishPreparationTimeHandler.HandleAsync(
            new GetDishPreparationTimeQuery { Id = id });

        return Ok(result);
    }
}
