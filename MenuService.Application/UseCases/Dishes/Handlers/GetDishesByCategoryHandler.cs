using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuService.Application.DTOs.Dishes;
using MenuService.Application.Interfaces;
using MenuService.Application.UseCases.Dishes.Queries;
using MenuService.Domain.Exceptions;

namespace MenuService.Application.UseCases.Dishes.Handlers;

public class GetDishesByCategoryHandler
{
    private readonly IDishRepository _dishRepository;
    private readonly ICategoryRepository _categoryRepository;

    public GetDishesByCategoryHandler(
        IDishRepository dishRepository,
        ICategoryRepository categoryRepository)
    {
        _dishRepository = dishRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<DishDto>> HandleAsync(GetDishesByCategoryQuery query)
    {
        var category = await _categoryRepository.GetByIdAsync(query.CategoryId);

        if (category is null)
            throw new NotFoundException("La categoría no fue encontrada.");

        var dishes = await _dishRepository.GetByCategoryIdAsync(
            query.CategoryId,
            query.PageNumber,
            query.PageSize);

        return dishes.Select(dish => new DishDto
        {
            Id = dish.Id,
            CategoryId = dish.CategoryId,
            CategoryName = dish.Category?.Name ?? category.Name,
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
