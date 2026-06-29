using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuService.Application.DTOs.Common;
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

    public async Task<PagedResultDto<DrinkDto>> HandleAsync(GetAllDrinksQuery query)
    {
        if (query.PageNumber < 1) query.PageNumber = 1;
        if (query.PageSize < 1) query.PageSize = 10;
        if (query.PageSize > 50) query.PageSize = 50;

        var totalItems = await _drinkRepository.CountAsync(query.Search, query.Available);
        var totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize);

        if (totalPages > 0 && query.PageNumber > totalPages)
            query.PageNumber = totalPages;

        var drinks = await _drinkRepository.GetAllAsync(
            query.PageNumber,
            query.PageSize,
            query.Search,
            query.Available,
            query.Sort);

        var items = drinks.Select(drink => new DrinkDto
        {
            Id = drink.Id,
            CategoryId = drink.CategoryId,
            CategoryName = drink.Category?.Name ?? string.Empty,
            Name = drink.Name,
            Brand = drink.Brand,
            Description = drink.Description,
            Price = drink.Price,
            Available = drink.Available,
            ImageUrl = drink.ImageUrl,
            CreatedAt = drink.CreatedAt,
            UpdatedAt = drink.UpdatedAt
        });

        return new PagedResultDto<DrinkDto>
        {
            Items = items,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }
}
