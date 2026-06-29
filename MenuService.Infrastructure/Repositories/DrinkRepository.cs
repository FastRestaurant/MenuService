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

public class DrinkRepository : IDrinkRepository
{
    private readonly MenuDbContext _context;

    public DrinkRepository(MenuDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Drink>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _context.Drinks
            .AsNoTracking()
            .Include(x => x.Category)
            .OrderBy(x => x.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Drink>> GetAllAsync(int pageNumber, int pageSize, string? search, bool? available, string? sort)
    {
        var query = ApplyFilters(_context.Drinks
            .AsNoTracking()
            .Include(x => x.Category), search, available);

        query = sort switch
        {
            "precio-asc" => query.OrderBy(x => x.Price).ThenBy(x => x.Name),
            "precio-desc" => query.OrderByDescending(x => x.Price).ThenBy(x => x.Name),
            _ => query.OrderBy(x => x.Name)
        };

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Drink?> GetByIdAsync(Guid id)
    {
        return await _context.Drinks
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Drink>> GetByCategoryIdAsync(
        Guid categoryId,
        int pageNumber,
        int pageSize)
    {
        return await _context.Drinks
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.CategoryId == categoryId)
            .OrderBy(x => x.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task AddAsync(Drink drink)
    {
        await _context.Drinks.AddAsync(drink);
    }

    public void Update(Drink drink)
    {
        _context.Drinks.Update(drink);
    }

    public void Delete(Drink drink)
    {
        _context.Drinks.Remove(drink);
    }

    public async Task<int> CountAsync()
    {
        return await _context.Drinks.CountAsync();
    }

    public async Task<int> CountAsync(string? search, bool? available)
    {
        return await ApplyFilters(_context.Drinks.AsNoTracking(), search, available).CountAsync();
    }

    public async Task<int> CountByCategoryIdAsync(Guid categoryId)
    {
        return await _context.Drinks
            .CountAsync(drink => drink.CategoryId == categoryId);
    }

    private static IQueryable<Drink> ApplyFilters(IQueryable<Drink> query, string? search, bool? available)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(drink =>
                drink.Name.Contains(term) ||
                (drink.Brand != null && drink.Brand.Contains(term)) ||
                (drink.Description != null && drink.Description.Contains(term)));
        }

        if (available.HasValue)
            query = query.Where(drink => drink.Available == available.Value);

        return query;
    }
}
