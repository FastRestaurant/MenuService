using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuService.Application.DTOs.Common;
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

    public async Task<PagedResultDto<DishDto>> HandleAsync(GetDishesByCategoryQuery query)
    {
        if (query.PageNumber < 1) query.PageNumber = 1;
        if (query.PageSize < 1) query.PageSize = 10;
        if (query.PageSize > 50) query.PageSize = 50;

        var category = await _categoryRepository.GetByIdAsync(query.CategoryId);

        if (category is null)
            throw new NotFoundException("La categoría no fue encontrada.");

        var totalItems = await _dishRepository.CountByCategoryIdAsync(query.CategoryId);
        var totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize);

        if (totalPages > 0 && query.PageNumber > totalPages)
            query.PageNumber = totalPages;

        var dishes = await _dishRepository.GetByCategoryIdAsync(
            query.CategoryId,
            query.PageNumber,
            query.PageSize);

        var items = dishes.Select(dish => new DishDto
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
