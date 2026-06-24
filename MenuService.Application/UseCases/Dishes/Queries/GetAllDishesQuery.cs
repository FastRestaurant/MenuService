using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuService.Application.UseCases.Dishes.Queries;

public class GetAllDishesQuery
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}