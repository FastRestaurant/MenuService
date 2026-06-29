using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuService.Application.DTOs.Common;
using MenuService.Application.DTOs.Drinks;
using MenuService.Application.Interfaces;
using MenuService.Application.UseCases.Drinks.Queries;
using MenuService.Domain.Exceptions;

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

    public async Task<PagedResultDto<DrinkDto>> HandleAsync(GetDrinksByCategoryQuery query)
    {
        var category = await _categoryRepository.GetByIdAsync(query.CategoryId);

        if (category is null)
            throw new NotFoundException("La categoría no fue encontrada.");

        var totalItems = await _drinkRepository.CountByCategoryIdAsync(query.CategoryId);

        var drinks = await _drinkRepository.GetByCategoryIdAsync(
            query.CategoryId,
            query.PageNumber,
            query.PageSize);

        var items = drinks.Select(drink => new DrinkDto
        {
            Id = drink.Id,
            CategoryId = drink.CategoryId,
            CategoryName = drink.Category?.Name ?? category.Name,
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
            TotalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize)
        };
    }
}
