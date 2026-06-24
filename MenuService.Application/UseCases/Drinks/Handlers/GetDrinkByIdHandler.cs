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

public class GetDrinkByIdHandler
{
    private readonly IDrinkRepository _drinkRepository;

    public GetDrinkByIdHandler(IDrinkRepository drinkRepository)
    {
        _drinkRepository = drinkRepository;
    }

    public async Task<DrinkDto> HandleAsync(GetDrinkByIdQuery query)
    {
        var drink = await _drinkRepository.GetByIdAsync(query.Id);

        if (drink is null)
            throw new NotFoundException("La bebida no fue encontrada.");

        return new DrinkDto
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
        };
    }
}
