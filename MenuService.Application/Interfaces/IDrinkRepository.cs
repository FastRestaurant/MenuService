using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuService.Domain.Entities;

namespace MenuService.Application.Interfaces;

public interface IDrinkRepository
{
    Task<IEnumerable<Drink>> GetAllAsync(int pageNumber, int pageSize);
    Task<Drink?> GetByIdAsync(Guid id);
    Task<IEnumerable<Drink>> GetByCategoryIdAsync(Guid categoryId, int pageNumber, int pageSize);
    Task AddAsync(Drink drink);
    void Update(Drink drink);
    void Delete(Drink drink);
}
