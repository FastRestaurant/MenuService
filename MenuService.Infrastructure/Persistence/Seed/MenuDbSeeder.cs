using System.Security.Cryptography;
using System.Text;
using MenuService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Infrastructure.Persistence.Seed;

public static class MenuDbSeeder
{
    public static async Task SeedAsync(MenuDbContext context)
    {
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

    private static async Task EnsureCategoryAsync(MenuDbContext context, Category category)
    {
        var existing = await context.Categories.FirstOrDefaultAsync(x => x.Name == category.Name);
        if (existing is null)
            context.Categories.Add(category);
    }

    private static async Task EnsureDishAsync(MenuDbContext context, Dish dish)
    {
        var existing = await context.Dishes.AsNoTracking().FirstOrDefaultAsync(x => x.Name == dish.Name);
        if (existing is null)
        {
            context.Dishes.Add(dish);
            return;
        }
    }

    private static async Task EnsureDrinkAsync(MenuDbContext context, Drink drink)
    {
        var existing = await context.Drinks.AsNoTracking().FirstOrDefaultAsync(x => x.Name == drink.Name);
        if (existing is null)
        {
            context.Drinks.Add(drink);
            return;
        }
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
        Dish(categories, now, "Entradas", "Empanadas de carne", "Empanadas tradicionales de carne cortada a cuchillo", 1800, 10, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358025/empanadas-carne_lcy2co.webp"),
        Dish(categories, now, "Entradas", "Empanadas de jamón y queso", "Empanadas rellenas de jamón y queso", 1700, 10, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358032/empanadas-jamon-queso_ldihju.webp"),
        Dish(categories, now, "Entradas", "Provoleta", "Queso provolone grillado con orégano", 4200, 12, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358103/provoleta_uayojx.jpg"),
        Dish(categories, now, "Entradas", "Papas fritas", "Porción de papas fritas crocantes", 3500, 12, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358089/papas-fritas_oqkwq2.jpg"),
        Dish(categories, now, "Entradas", "Rabas", "Rabas fritas con limón", 6500, 15, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358110/rabas_orochy.jpg"),

        Dish(categories, now, "Platos principales", "Milanesa napolitana", "Milanesa con salsa de tomate, jamón y queso", 7500, 20, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358076/milanesa-napolitana_lh5tei.webp"),
        Dish(categories, now, "Platos principales", "Milanesa con papas", "Milanesa clásica acompañada con papas fritas", 6900, 18, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358069/milanesa-con-papas_val9kl.jpg"),
        Dish(categories, now, "Platos principales", "Bife de chorizo", "Bife de chorizo grillado con guarnición", 10500, 25, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782357817/bife-de-chorizo_jizn3p.jpg"),
        Dish(categories, now, "Platos principales", "Pollo grillado", "Pechuga de pollo grillada con ensalada", 6800, 20, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358097/pollo-grillado_prfn4d.jpg"),
        Dish(categories, now, "Platos principales", "Suprema a la suiza", "Suprema de pollo con salsa blanca y queso gratinado", 7900, 22, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358136/suprema-a-la-suiza_mm9n3p.jpg"),
        Dish(categories, now, "Platos principales", "Hamburguesa completa", "Hamburguesa con lechuga, tomate, queso, jamón y huevo", 6200, 15, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358051/hamburguesa-completa_pwwylh.jpg"),

        Dish(categories, now, "Pastas", "Ravioles de ricota", "Ravioles caseros de ricota con salsa a elección", 6200, 18, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358119/ravioles-de-ricota_ohce2t.jpg"),
        Dish(categories, now, "Pastas", "Sorrentinos de jamón y queso", "Sorrentinos rellenos con jamón y queso", 6900, 20, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358126/sorrentinos-jamon-queso_h7zd5l.jpg"),
        Dish(categories, now, "Pastas", "Tallarines caseros", "Tallarines caseros con salsa fileto", 5800, 16, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358139/tallarines-caseros_s9kthm.jpg"),
        Dish(categories, now, "Pastas", "Ñoquis de papa", "Ñoquis de papa con salsa mixta", 5600, 16, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358083/noquis-de-papa_nktqv0.jpg"),

        Dish(categories, now, "Postres", "Flan casero", "Flan tradicional con dulce de leche", 2800, 5, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358043/flan-casero_tyfe0j.jpg"),
        Dish(categories, now, "Postres", "Budín de pan", "Budín de pan casero con caramelo", 2700, 5, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358005/budin-de-pan_wfawsg.jpg"),
        Dish(categories, now, "Postres", "Helado artesanal", "Dos bochas de helado artesanal", 3200, 3, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358061/helado-artesanal_ram3zv.jpg"),
        Dish(categories, now, "Postres", "Cheesecake", "Cheesecake con frutos rojos", 3900, 5, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358017/cheesecake_louraz.jpg"),
        Dish(categories, now, "Postres", "Tiramisú", "Postre italiano con café y cacao", 4100, 5, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358146/tiramisu_oq4puw.jpg")
    };

    private static List<Drink> BuildDrinks(IReadOnlyDictionary<string, Guid> categories, DateTime now) => new()
    {
        Drink(categories, now, "Coca-Cola 500ml", "Coca-Cola", "Gaseosa cola individual", 1800, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358287/coca-cola-500ml_rn4bhr.jpg"),
        Drink(categories, now, "Agua mineral 500ml", "Eco de los Andes", "Agua mineral sin gas", 1200, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358250/agua-mineral-500ml_vdkfzu.jpg"),
        Drink(categories, now, "Sprite 500ml", "Sprite", "Gaseosa lima limón individual", 1800, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358323/sprite-500ml_przhre.jpg"),
        Drink(categories, now, "Fanta 500ml", "Fanta", "Gaseosa sabor naranja individual", 1800, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358295/fanta-500ml_gp50z4.jpg"),
        Drink(categories, now, "Agua con gas 500ml", "Eco de los Andes", "Agua mineral con gas", 1300, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358241/agua-con-gas-500ml_r0jnme.jpg"),
        Drink(categories, now, "Jugo de naranja", "De la casa", "Jugo natural de naranja", 2200, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358307/jugo-de-naranja_otlza5.jpg"),
        Drink(categories, now, "Limonada", "De la casa", "Limonada casera con menta", 2400, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358313/limonada_pv8tpi.jpg"),
        Drink(categories, now, "Cerveza Quilmes", "Quilmes", "Cerveza rubia 473ml", 2600, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358265/cerveza-quilmes_l4gk4g.jpg"),
        Drink(categories, now, "Cerveza Stella Artois", "Stella Artois", "Cerveza rubia 473ml", 3200, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358274/cerveza-stella-artois_lpewpl.jpg"),
        Drink(categories, now, "Café", "De la casa", "Café caliente", 1500, "https://res.cloudinary.com/dez2fwxfe/image/upload/v1782358258/cafe_yvrl2v.jpg")
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

    private static Drink Drink(IReadOnlyDictionary<string, Guid> categories, DateTime now, string name, string brand, string description, decimal price, string imageUrl)
        => new()
        {
            Id = StableId($"drink:{name}"),
            CategoryId = categories["Bebidas"],
            Name = name,
            Brand = brand,
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
