using MenuService.Application.Interfaces;
using MenuService.Application.UseCases.Categories.Commands;
using MenuService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuService.Application.UseCases.Categories.Handlers;

public class DeleteCategoryHandler
{
    private const string FixedDrinksCategoryName = "Bebidas";

    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(DeleteCategoryCommand command)
    {
        var category = await _categoryRepository.GetByIdAsync(command.Id);

        if (category is null)
            throw new NotFoundException("La categoría no fue encontrada.");

        if (category.Name.Equals(FixedDrinksCategoryName, StringComparison.OrdinalIgnoreCase))
            throw new ConflictException("La categoría Bebidas es fija y no se puede eliminar.");

        var dishCount = await _categoryRepository.CountDishesAsync(command.Id);
        var drinkCount = await _categoryRepository.CountDrinksAsync(command.Id);

        if (dishCount > 0 || drinkCount > 0)
            throw new ConflictException("No se puede eliminar la categoría porque tiene productos asociados.");

        _categoryRepository.Delete(category);
        await _unitOfWork.SaveChangesAsync();
    }
}
