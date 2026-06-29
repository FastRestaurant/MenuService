using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuService.Application.Interfaces;
using MenuService.Domain.Entities;
using MenuService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Infrastructure.Repositories;

public class DishRepository : IDishRepository
{
    private readonly MenuDbContext _context;

    public DishRepository(MenuDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Dish>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _context.Dishes
            .AsNoTracking()
            .Include(x => x.Category)
            .OrderBy(x => x.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Dish>> GetAllAsync(int pageNumber, int pageSize, string? search, bool? available, string? sort)
    {
        var query = ApplyFilters(_context.Dishes
            .AsNoTracking()
            .Include(x => x.Category), search, available);

        query = sort switch
        {
            "precio-asc" => query.OrderBy(x => x.Price).ThenBy(x => x.Name),
            "precio-desc" => query.OrderByDescending(x => x.Price).ThenBy(x => x.Name),
            "tiempo-asc" => query.OrderBy(x => x.EstimatedPreparationMinutes).ThenBy(x => x.Name),
            "tiempo-desc" => query.OrderByDescending(x => x.EstimatedPreparationMinutes).ThenBy(x => x.Name),
            _ => query.OrderBy(x => x.Name)
        };

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Dish?> GetByIdAsync(Guid id)
    {
        return await _context.Dishes
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Dish>> GetByCategoryIdAsync(
        Guid categoryId,
        int pageNumber,
        int pageSize)
    {
        return await _context.Dishes
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.CategoryId == categoryId)
            .OrderBy(x => x.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task AddAsync(Dish dish)
    {
        await _context.Dishes.AddAsync(dish);
    }

    public void Update(Dish dish)
    {
        _context.Dishes.Update(dish);
    }

    public void Delete(Dish dish)
    {
        _context.Dishes.Remove(dish);
    }

    public async Task<int> CountAsync()
    {
        return await _context.Dishes.CountAsync();
    }

    public async Task<int> CountAsync(string? search, bool? available)
    {
        return await ApplyFilters(_context.Dishes.AsNoTracking(), search, available).CountAsync();
    }

    public async Task<int> CountByCategoryIdAsync(Guid categoryId)
    {
        return await _context.Dishes
            .CountAsync(dish => dish.CategoryId == categoryId);
    }

    private static IQueryable<Dish> ApplyFilters(IQueryable<Dish> query, string? search, bool? available)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(dish =>
                dish.Name.Contains(term) ||
                (dish.Description != null && dish.Description.Contains(term)) ||
                (dish.Category != null && dish.Category.Name.Contains(term)));
        }

        if (available.HasValue)
            query = query.Where(dish => dish.Available == available.Value);

        return query;
    }
}
