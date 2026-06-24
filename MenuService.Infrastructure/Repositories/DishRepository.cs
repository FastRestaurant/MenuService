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
}
