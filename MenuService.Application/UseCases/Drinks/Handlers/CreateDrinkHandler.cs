using MenuService.Application.DTOs.Drinks;
using MenuService.Application.Interfaces;
using MenuService.Application.UseCases.Drinks.Commands;
using MenuService.Domain.Entities;
using MenuService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuService.Application.UseCases.Drinks.Handlers;

public class CreateDrinkHandler
{
    private readonly IDrinkRepository _drinkRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDrinkHandler(
        IDrinkRepository drinkRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _drinkRepository = drinkRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DrinkDto> HandleAsync(CreateDrinkCommand command)
    {
        var category = await _categoryRepository.GetByIdAsync(command.Drink.CategoryId);

        if (category is null)
            throw new NotFoundException("La categoría no fue encontrada."); 

        if (command.Drink.Price <= 0)
            throw new BusinessRuleException("El precio de la bebida debe ser mayor a cero.");

        var drink = new Drink
        {
            Id = Guid.NewGuid(),
            CategoryId = command.Drink.CategoryId,
            Name = command.Drink.Name,
            Brand = command.Drink.Brand,
            Description = command.Drink.Description,
            Price = command.Drink.Price,
            Available = command.Drink.Available,
            ImageUrl = command.Drink.ImageUrl,
            CreatedAt = DateTime.UtcNow
        };

        await _drinkRepository.AddAsync(drink);
        await _unitOfWork.SaveChangesAsync();

        return new DrinkDto
        {
            Id = drink.Id,
            CategoryId = drink.CategoryId,
            CategoryName = category.Name,
            Name = drink.Name,
            Brand = drink.Brand,
            Description = drink.Description,
            Price = drink.Price,
            Available = drink.Available,
            ImageUrl = drink.ImageUrl,
            CreatedAt = drink.CreatedAt,
            UpdatedAt = drink.UpdatedAt
        };
    }
}
