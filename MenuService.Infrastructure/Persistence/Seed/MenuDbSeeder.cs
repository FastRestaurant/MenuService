using System.Security.Cryptography;
using System.Text;
using MenuService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Infrastructure.Persistence.Seed;

public static class MenuDbSeeder
{
    public static async Task SeedAsync(MenuDbContext context)
    {
        await RemoveTestDataAsync(context);

        var now = DateTime.UtcNow;
        var categories = BuildCategories(now);

        foreach (var category in categories)
            await EnsureCategoryAsync(context, category);

        await context.SaveChangesAsync();

        var categoryIdsByName = await context.Categories
            .ToDictionaryAsync(category => category.Name, category => category.Id);

        foreach (var dish in BuildDishes(categoryIdsByName, now))
            await EnsureDishAsync(context, dish);

        foreach (var drink in BuildDrinks(categoryIdsByName, now))
            await EnsureDrinkAsync(context, drink);

        await context.SaveChangesAsync();
    }

    private static async Task RemoveTestDataAsync(MenuDbContext context)
    {
        await context.Database.ExecuteSqlRawAsync("DELETE FROM [Dishes] WHERE [Name] LIKE 'E2E%'");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM [Drinks] WHERE [Name] LIKE 'E2E%'");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM [Categories] WHERE [Name] LIKE 'E2E%'");
    }

    private static async Task EnsureCategoryAsync(MenuDbContext context, Category category)
    {
        var existing = await context.Categories.FirstOrDefaultAsync(x => x.Name == category.Name);
        if (existing is null)
        {
            context.Categories.Add(category);
            return;
        }

        existing.Description = category.Description;
        existing.UpdatedAt = DateTime.UtcNow;
    }

    private static async Task EnsureDishAsync(MenuDbContext context, Dish dish)
    {
        var existing = await context.Dishes.AsNoTracking().FirstOrDefaultAsync(x => x.Name == dish.Name);
        if (existing is null)
        {
            context.Dishes.Add(dish);
            return;
        }

        if (existing.Id != dish.Id)
            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE [Dishes] SET [Id] = {dish.Id} WHERE [Id] = {existing.Id}");

        await context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE [Dishes]
            SET [CategoryId] = {dish.CategoryId},
                [Description] = {dish.Description},
                [Price] = {dish.Price},
                [EstimatedPreparationMinutes] = {dish.EstimatedPreparationMinutes},
                [Available] = {dish.Available},
                [ImageUrl] = {dish.ImageUrl},
                [UpdatedAt] = {DateTime.UtcNow}
            WHERE [Id] = {dish.Id}");
    }

    private static async Task EnsureDrinkAsync(MenuDbContext context, Drink drink)
    {
        var existing = await context.Drinks.AsNoTracking().FirstOrDefaultAsync(x => x.Name == drink.Name);
        if (existing is null)
        {
            context.Drinks.Add(drink);
            return;
        }

        if (existing.Id != drink.Id)
            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE [Drinks] SET [Id] = {drink.Id} WHERE [Id] = {existing.Id}");

        await context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE [Drinks]
            SET [CategoryId] = {drink.CategoryId},
                [Description] = {drink.Description},
                [Price] = {drink.Price},
                [Available] = {drink.Available},
                [ImageUrl] = {drink.ImageUrl},
                [UpdatedAt] = {DateTime.UtcNow}
            WHERE [Id] = {drink.Id}");
    }

    private static List<Category> BuildCategories(DateTime now) => new()
    {
        new() { Id = StableId("category:Entradas"), Name = "Entradas", Description = "Platos pequeños para comenzar", CreatedAt = now },
        new() { Id = StableId("category:Platos principales"), Name = "Platos principales", Description = "Comidas principales del restaurante", CreatedAt = now },
        new() { Id = StableId("category:Pastas"), Name = "Pastas", Description = "Pastas caseras y especialidades", CreatedAt = now },
        new() { Id = StableId("category:Postres"), Name = "Postres", Description = "Opciones dulces para finalizar la comida", CreatedAt = now },
        new() { Id = StableId("category:Bebidas"), Name = "Bebidas", Description = "Bebidas frías y calientes", CreatedAt = now }
    };

    private static List<Dish> BuildDishes(IReadOnlyDictionary<string, Guid> categories, DateTime now) => new()
    {
        Dish(categories, now, "Entradas", "Empanadas de carne", "Empanadas tradicionales de carne cortada a cuchillo", 1800, 10, "/images/dishes/empanadas-carne.jpg"),
        Dish(categories, now, "Entradas", "Empanadas de jamón y queso", "Empanadas rellenas de jamón y queso", 1700, 10, "/images/dishes/empanadas-jamon-queso.jpg"),
        Dish(categories, now, "Entradas", "Provoleta", "Queso provolone grillado con orégano", 4200, 12, "/images/dishes/provoleta.jpg"),
        Dish(categories, now, "Entradas", "Papas fritas", "Porción de papas fritas crocantes", 3500, 12, "/images/dishes/papas-fritas.jpg"),
        Dish(categories, now, "Entradas", "Rabas", "Rabas fritas con limón", 6500, 15, "/images/dishes/rabas.jpg"),
        Dish(categories, now, "Platos principales", "Milanesa napolitana", "Milanesa con salsa de tomate, jamón y queso", 7500, 20, "/images/dishes/milanesa-napolitana.jpg"),
        Dish(categories, now, "Platos principales", "Milanesa con papas", "Milanesa clásica acompañada con papas fritas", 6900, 18, "/images/dishes/milanesa-con-papas.jpg"),
        Dish(categories, now, "Platos principales", "Bife de chorizo", "Bife de chorizo grillado con guarnición", 10500, 25, "/images/dishes/bife-de-chorizo.jpg"),
        Dish(categories, now, "Platos principales", "Pollo grillado", "Pechuga de pollo grillada con ensalada", 6800, 20, "/images/dishes/pollo-grillado.jpg"),
        Dish(categories, now, "Platos principales", "Suprema a la suiza", "Suprema de pollo con salsa blanca y queso gratinado", 7900, 22, "/images/dishes/suprema-a-la-suiza.jpg"),
        Dish(categories, now, "Platos principales", "Hamburguesa completa", "Hamburguesa con lechuga, tomate, queso, jamón y huevo", 6200, 15, "/images/dishes/hamburguesa-completa.jpg"),
        Dish(categories, now, "Pastas", "Ravioles de ricota", "Ravioles caseros de ricota con salsa a elección", 6200, 18, "/images/dishes/ravioles-de-ricota.jpg"),
        Dish(categories, now, "Pastas", "Sorrentinos de jamón y queso", "Sorrentinos rellenos con jamón y queso", 6900, 20, "/images/dishes/sorrentinos-jamon-queso.jpg"),
        Dish(categories, now, "Pastas", "Tallarines caseros", "Tallarines caseros con salsa fileto", 5800, 16, "/images/dishes/tallarines-caseros.jpg"),
        Dish(categories, now, "Pastas", "Ñoquis de papa", "Ñoquis de papa con salsa mixta", 5600, 16, "/images/dishes/noquis-de-papa.jpg"),
        Dish(categories, now, "Postres", "Flan casero", "Flan tradicional con dulce de leche", 2800, 5, "/images/dishes/flan-casero.jpg"),
        Dish(categories, now, "Postres", "Budín de pan", "Budín de pan casero con caramelo", 2700, 5, "/images/dishes/budin-de-pan.jpg"),
        Dish(categories, now, "Postres", "Helado artesanal", "Dos bochas de helado artesanal", 3200, 3, "/images/dishes/helado-artesanal.jpg"),
        Dish(categories, now, "Postres", "Cheesecake", "Cheesecake con frutos rojos", 3900, 5, "/images/dishes/cheesecake.jpg"),
        Dish(categories, now, "Postres", "Tiramisú", "Postre italiano con café y cacao", 4100, 5, "/images/dishes/tiramisu.jpg")
    };

    private static List<Drink> BuildDrinks(IReadOnlyDictionary<string, Guid> categories, DateTime now) => new()
    {
        Drink(categories, now, "Coca-Cola 500ml", "Gaseosa cola individual", 1800, "/images/drinks/coca-cola-500ml.jpg"),
        Drink(categories, now, "Agua mineral 500ml", "Agua mineral sin gas", 1200, "/images/drinks/agua-mineral-500ml.jpg"),
        Drink(categories, now, "Sprite 500ml", "Gaseosa lima limón individual", 1800, "/images/drinks/sprite-500ml.jpg"),
        Drink(categories, now, "Fanta 500ml", "Gaseosa sabor naranja individual", 1800, "/images/drinks/fanta-500ml.jpg"),
        Drink(categories, now, "Agua con gas 500ml", "Agua mineral con gas", 1300, "/images/drinks/agua-con-gas-500ml.jpg"),
        Drink(categories, now, "Jugo de naranja", "Jugo natural de naranja", 2200, "/images/drinks/jugo-de-naranja.jpg"),
        Drink(categories, now, "Limonada", "Limonada casera con menta", 2400, "/images/drinks/limonada.jpg"),
        Drink(categories, now, "Cerveza Quilmes", "Cerveza rubia 473ml", 2600, "/images/drinks/cerveza-quilmes.jpg"),
        Drink(categories, now, "Cerveza Stella Artois", "Cerveza rubia 473ml", 3200, "/images/drinks/cerveza-stella-artois.jpg"),
        Drink(categories, now, "Café", "Café caliente", 1500, "/images/drinks/cafe.jpg")
    };

    private static Dish Dish(IReadOnlyDictionary<string, Guid> categories, DateTime now, string categoryName, string name, string description, decimal price, int minutes, string imageUrl)
        => new()
        {
            Id = StableId($"dish:{name}"),
            CategoryId = categories[categoryName],
            Name = name,
            Description = description,
            Price = price,
            EstimatedPreparationMinutes = minutes,
            Available = true,
            ImageUrl = imageUrl,
            CreatedAt = now
        };

    private static Drink Drink(IReadOnlyDictionary<string, Guid> categories, DateTime now, string name, string description, decimal price, string imageUrl)
        => new()
        {
            Id = StableId($"drink:{name}"),
            CategoryId = categories["Bebidas"],
            Name = name,
            Description = description,
            Price = price,
            Available = true,
            ImageUrl = imageUrl,
            CreatedAt = now
        };

    private static Guid StableId(string key)
    {
        var bytes = MD5.HashData(Encoding.UTF8.GetBytes(key));
        return new Guid(bytes);
    }
}
