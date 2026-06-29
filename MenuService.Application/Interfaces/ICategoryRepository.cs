using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuService.Domain.Entities;

namespace MenuService.Application.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task<Category?> GetByNameAsync(string name);
    Task AddAsync(Category category);
    void Update(Category category);
    void Delete(Category category);
    Task<bool> ExistsByNameAsync(string name);
    Task<int> CountDishesAsync(Guid categoryId);
    Task<int> CountDrinksAsync(Guid categoryId);
}
