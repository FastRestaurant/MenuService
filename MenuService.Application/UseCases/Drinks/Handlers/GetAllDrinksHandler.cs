using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuService.Application.DTOs.Drinks;
using MenuService.Application.Interfaces;
using MenuService.Application.UseCases.Drinks.Queries;

namespace MenuService.Application.UseCases.Drinks.Handlers;

public class GetAllDrinksHandler
{
    private readonly IDrinkRepository _drinkRepository;

    public GetAllDrinksHandler(IDrinkRepository drinkRepository)
    {
        _drinkRepository = drinkRepository;
    }

    public async Task<IEnumerable<DrinkDto>> HandleAsync(GetAllDrinksQuery query)
    {
        var drinks = await _drinkRepository.GetAllAsync(
            query.PageNumber,
            query.PageSize);

        return drinks.Select(drink => new DrinkDto
        {
            Id = drink.Id,
            CategoryId = drink.CategoryId,
            CategoryName = drink.Category?.Name ?? string.Empty,
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
