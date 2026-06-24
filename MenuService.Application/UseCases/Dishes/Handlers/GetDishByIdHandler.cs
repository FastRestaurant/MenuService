using MenuService.Application.DTOs.Dishes;
using MenuService.Application.Interfaces;
using MenuService.Application.UseCases.Dishes.Queries;
using MenuService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuService.Application.UseCases.Dishes.Handlers;

public class GetDishByIdHandler
{
    private readonly IDishRepository _dishRepository;

    public GetDishByIdHandler(IDishRepository dishRepository)
    {
        _dishRepository = dishRepository;
    }

    public async Task<DishDto> HandleAsync(GetDishByIdQuery query)
    {
        var dish = await _dishRepository.GetByIdAsync(query.Id);

        if (dish is null)
            throw new NotFoundException("El plato no fue encontrado.");

        return new DishDto
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
        };
    }
}
