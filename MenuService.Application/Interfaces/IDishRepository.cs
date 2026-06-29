using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuService.Domain.Entities;

namespace MenuService.Application.Interfaces;

public interface IDishRepository
{
    Task<IEnumerable<Dish>> GetAllAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Dish>> GetAllAsync(int pageNumber, int pageSize, string? search, bool? available, string? sort);
    Task<Dish?> GetByIdAsync(Guid id);
    Task<IEnumerable<Dish>> GetByCategoryIdAsync(Guid categoryId, int pageNumber, int pageSize);
    Task AddAsync(Dish dish);
    void Update(Dish dish);
    void Delete(Dish dish);

    Task<int> CountAsync();
    Task<int> CountAsync(string? search, bool? available);
    Task<int> CountByCategoryIdAsync(Guid categoryId);
}
