using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuService.Application.DTOs.Dishes;
using MenuService.Application.Interfaces;
using MenuService.Application.UseCases.Dishes.Queries;

namespace MenuService.Application.UseCases.Dishes.Handlers;

public class GetAllDishesHandler
{
    private readonly IDishRepository _dishRepository;

    public GetAllDishesHandler(IDishRepository dishRepository)
    {
        _dishRepository = dishRepository;
    }

    public async Task<IEnumerable<DishDto>> HandleAsync(GetAllDishesQuery query)
    {
        var dishes = await _dishRepository.GetAllAsync(
            query.PageNumber,
            query.PageSize);

        return dishes.Select(dish => new DishDto
        {
            Id = dish.Id,
            CategoryId = dish.CategoryId,
            CategoryName = dish.Category?.Name ?? string.Empty,
            Name = dish.Name,
            Description = dish.Description,
            Price = dish.Price,
            EstimatedPreparationMinutes = dish.EstimatedPreparationMinutes,
            Available = dish.Available,
            ImageUrl = dish.ImageUrl,
            CreatedAt = dish.CreatedAt,
            UpdatedAt = dish.UpdatedAt
        });
    }
}
