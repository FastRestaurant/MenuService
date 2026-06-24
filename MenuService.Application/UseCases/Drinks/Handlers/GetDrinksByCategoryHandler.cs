using MenuService.Application.DTOs.Drinks;
using MenuService.Application.Interfaces;
using MenuService.Application.UseCases.Drinks.Queries;
using MenuService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuService.Application.UseCases.Drinks.Handlers;

public class GetDrinksByCategoryHandler
{
    private readonly IDrinkRepository _drinkRepository;
    private readonly ICategoryRepository _categoryRepository;

    public GetDrinksByCategoryHandler(
        IDrinkRepository drinkRepository,
        ICategoryRepository categoryRepository)
    {
        _drinkRepository = drinkRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<DrinkDto>> HandleAsync(GetDrinksByCategoryQuery query)
    {
        var category = await _categoryRepository.GetByIdAsync(query.CategoryId);

        if (category is null)
            throw new NotFoundException("La categoría no fue encontrada.");

        var drinks = await _drinkRepository.GetByCategoryIdAsync(
            query.CategoryId,
            query.PageNumber,
            query.PageSize);

        return drinks.Select(drink => new DrinkDto
        {
            Id = drink.Id,
            CategoryId = drink.CategoryId,
            CategoryName = drink.Category?.Name ?? category.Name,
            Name = drink.Name,
            Description = drink.Description,
            Price = drink.Price,
            Available = drink.Available,
            ImageUrl = drink.ImageUrl,
            CreatedAt = drink.CreatedAt,
            UpdatedAt = drink.UpdatedAt
        });
    }
}
