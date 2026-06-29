using MenuService.Application.DTOs.Drinks;
using MenuService.Application.Interfaces;
using MenuService.Application.UseCases.Drinks.Commands;
using MenuService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuService.Application.UseCases.Drinks.Handlers;

public class UpdateDrinkHandler
{
    private readonly IDrinkRepository _drinkRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDrinkHandler(
        IDrinkRepository drinkRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _drinkRepository = drinkRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DrinkDto> HandleAsync(UpdateDrinkCommand command)
    {
        var drink = await _drinkRepository.GetByIdAsync(command.Id);

        if (drink is null)
            throw new NotFoundException("La bebida no fue encontrada.");

        var category = await _categoryRepository.GetByIdAsync(command.Drink.CategoryId);

        if (category is null)
            throw new NotFoundException("La categoría no fue encontrada.");

        if (command.Drink.Price <= 0)
            throw new BusinessRuleException("El precio de la bebida debe ser mayor a cero.");

        drink.CategoryId = command.Drink.CategoryId;
        drink.Name = command.Drink.Name;
        drink.Brand = command.Drink.Brand;
        drink.Description = command.Drink.Description;
        drink.Price = command.Drink.Price;
        drink.Available = command.Drink.Available;
        drink.ImageUrl = command.Drink.ImageUrl;
        drink.UpdatedAt = DateTime.UtcNow;

        _drinkRepository.Update(drink);
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
