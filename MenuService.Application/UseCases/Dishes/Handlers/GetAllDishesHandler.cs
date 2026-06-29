using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuService.Application.DTOs.Common;
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

    public async Task<PagedResultDto<DishDto>> HandleAsync(GetAllDishesQuery query)
    {
        if (query.PageNumber < 1) query.PageNumber = 1;
        if (query.PageSize < 1) query.PageSize = 10;
        if (query.PageSize > 50) query.PageSize = 50;

        var totalItems = await _dishRepository.CountAsync(query.Search, query.Available);
        var totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize);

        if (totalPages > 0 && query.PageNumber > totalPages)
            query.PageNumber = totalPages;

        var dishes = await _dishRepository.GetAllAsync(
            query.PageNumber,
            query.PageSize,
            query.Search,
            query.Available,
            query.Sort);

        var items = dishes.Select(dish => new DishDto
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

        return new PagedResultDto<DishDto>
        {
            Items = items,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }
}
