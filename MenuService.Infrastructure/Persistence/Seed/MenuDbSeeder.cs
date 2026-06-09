using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuService.Domain.Entities;

namespace MenuService.Infrastructure.Persistence.Seed;

public static class MenuDbSeeder
{
    public static async Task SeedAsync(MenuDbContext context)
    {
        if (context.Categories.Any())
            return;

        var entradasId = Guid.NewGuid();
        var principalesId = Guid.NewGuid();
        var pastasId = Guid.NewGuid();
        var postresId = Guid.NewGuid();
        var bebidasId = Guid.NewGuid();

        var now = DateTime.UtcNow;

        var categories = new List<Category>
        {
            new() { Id = entradasId, Name = "Entradas", Description = "Platos pequeños para comenzar", CreatedAt = now },
            new() { Id = principalesId, Name = "Platos principales", Description = "Comidas principales del restaurante", CreatedAt = now },
            new() { Id = pastasId, Name = "Pastas", Description = "Pastas caseras y especialidades", CreatedAt = now },
            new() { Id = postresId, Name = "Postres", Description = "Opciones dulces para finalizar la comida", CreatedAt = now },
            new() { Id = bebidasId, Name = "Bebidas", Description = "Bebidas frías y calientes", CreatedAt = now }
        };

        var dishes = new List<Dish>
        {
            new() { Id = Guid.NewGuid(), CategoryId = entradasId, Name = "Empanadas de carne", Description = "Empanadas tradicionales de carne cortada a cuchillo", Price = 1800, EstimatedPreparationMinutes = 10, Available = true, ImageUrl = "/images/dishes/empanadas-carne.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = entradasId, Name = "Empanadas de jamón y queso", Description = "Empanadas rellenas de jamón y queso", Price = 1700, EstimatedPreparationMinutes = 10, Available = true, ImageUrl = "/images/dishes/empanadas-jamon-queso.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = entradasId, Name = "Provoleta", Description = "Queso provolone grillado con orégano", Price = 4200, EstimatedPreparationMinutes = 12, Available = true, ImageUrl = "/images/dishes/provoleta.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = entradasId, Name = "Papas fritas", Description = "Porción de papas fritas crocantes", Price = 3500, EstimatedPreparationMinutes = 12, Available = true, ImageUrl = "/images/dishes/papas-fritas.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = entradasId, Name = "Rabas", Description = "Rabas fritas con limón", Price = 6500, EstimatedPreparationMinutes = 15, Available = true, ImageUrl = "/images/dishes/rabas.jpg", CreatedAt = now },

            new() { Id = Guid.NewGuid(), CategoryId = principalesId, Name = "Milanesa napolitana", Description = "Milanesa con salsa de tomate, jamón y queso", Price = 7500, EstimatedPreparationMinutes = 20, Available = true, ImageUrl = "/images/dishes/milanesa-napolitana.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = principalesId, Name = "Milanesa con papas", Description = "Milanesa clásica acompañada con papas fritas", Price = 6900, EstimatedPreparationMinutes = 18, Available = true, ImageUrl = "/images/dishes/milanesa-con-papas.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = principalesId, Name = "Bife de chorizo", Description = "Bife de chorizo grillado con guarnición", Price = 10500, EstimatedPreparationMinutes = 25, Available = true, ImageUrl = "/images/dishes/bife-de-chorizo.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = principalesId, Name = "Pollo grillado", Description = "Pechuga de pollo grillada con ensalada", Price = 6800, EstimatedPreparationMinutes = 20, Available = true, ImageUrl = "/images/dishes/pollo-grillado.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = principalesId, Name = "Suprema a la suiza", Description = "Suprema de pollo con salsa blanca y queso gratinado", Price = 7900, EstimatedPreparationMinutes = 22, Available = true, ImageUrl = "/images/dishes/suprema-a-la-suiza.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = principalesId, Name = "Hamburguesa completa", Description = "Hamburguesa con lechuga, tomate, queso, jamón y huevo", Price = 6200, EstimatedPreparationMinutes = 15, Available = true, ImageUrl = "/images/dishes/hamburguesa-completa.jpg", CreatedAt = now },

            new() { Id = Guid.NewGuid(), CategoryId = pastasId, Name = "Ravioles de ricota", Description = "Ravioles caseros de ricota con salsa a elección", Price = 6200, EstimatedPreparationMinutes = 18, Available = true, ImageUrl = "/images/dishes/ravioles-de-ricota.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = pastasId, Name = "Sorrentinos de jamón y queso", Description = "Sorrentinos rellenos con jamón y queso", Price = 6900, EstimatedPreparationMinutes = 20, Available = true, ImageUrl = "/images/dishes/sorrentinos-jamon-queso.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = pastasId, Name = "Tallarines caseros", Description = "Tallarines caseros con salsa fileto", Price = 5800, EstimatedPreparationMinutes = 16, Available = true, ImageUrl = "/images/dishes/tallarines-caseros.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = pastasId, Name = "Ñoquis de papa", Description = "Ñoquis de papa con salsa mixta", Price = 5600, EstimatedPreparationMinutes = 16, Available = true, ImageUrl = "/images/dishes/noquis-de-papa.jpg", CreatedAt = now },

            new() { Id = Guid.NewGuid(), CategoryId = postresId, Name = "Flan casero", Description = "Flan tradicional con dulce de leche", Price = 2800, EstimatedPreparationMinutes = 5, Available = true, ImageUrl = "/images/dishes/flan-casero.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = postresId, Name = "Budín de pan", Description = "Budín de pan casero con caramelo", Price = 2700, EstimatedPreparationMinutes = 5, Available = true, ImageUrl = "/images/dishes/budin-de-pan.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = postresId, Name = "Helado artesanal", Description = "Dos bochas de helado artesanal", Price = 3200, EstimatedPreparationMinutes = 3, Available = true, ImageUrl = "/images/dishes/helado-artesanal.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = postresId, Name = "Cheesecake", Description = "Cheesecake con frutos rojos", Price = 3900, EstimatedPreparationMinutes = 5, Available = true, ImageUrl = "/images/dishes/cheesecake.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = postresId, Name = "Tiramisú", Description = "Postre italiano con café y cacao", Price = 4100, EstimatedPreparationMinutes = 5, Available = true, ImageUrl = "/images/dishes/tiramisu.jpg", CreatedAt = now }
        };

        var drinks = new List<Drink>
        {
            new() { Id = Guid.NewGuid(), CategoryId = bebidasId, Name = "Coca-Cola 500ml", Description = "Gaseosa cola individual", Price = 1800, Available = true, ImageUrl = "/images/drinks/coca-cola-500ml.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = bebidasId, Name = "Agua mineral 500ml", Description = "Agua mineral sin gas", Price = 1200, Available = true, ImageUrl = "/images/drinks/agua-mineral-500ml.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = bebidasId, Name = "Sprite 500ml", Description = "Gaseosa lima limón individual", Price = 1800, Available = true, ImageUrl = "/images/drinks/sprite-500ml.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = bebidasId, Name = "Fanta 500ml", Description = "Gaseosa sabor naranja individual", Price = 1800, Available = true, ImageUrl = "/images/drinks/fanta-500ml.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = bebidasId, Name = "Agua con gas 500ml", Description = "Agua mineral con gas", Price = 1300, Available = true, ImageUrl = "/images/drinks/agua-con-gas-500ml.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = bebidasId, Name = "Jugo de naranja", Description = "Jugo natural de naranja", Price = 2200, Available = true, ImageUrl = "/images/drinks/jugo-de-naranja.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = bebidasId, Name = "Limonada", Description = "Limonada casera con menta", Price = 2400, Available = true, ImageUrl = "/images/drinks/limonada.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = bebidasId, Name = "Cerveza Quilmes", Description = "Cerveza rubia 473ml", Price = 2600, Available = true, ImageUrl = "/images/drinks/cerveza-quilmes.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = bebidasId, Name = "Cerveza Stella Artois", Description = "Cerveza rubia 473ml", Price = 3200, Available = true, ImageUrl = "/images/drinks/cerveza-stella-artois.jpg", CreatedAt = now },
            new() { Id = Guid.NewGuid(), CategoryId = bebidasId, Name = "Café", Description = "Café caliente", Price = 1500, Available = true, ImageUrl = "/images/drinks/cafe.jpg", CreatedAt = now }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.Dishes.AddRangeAsync(dishes);
        await context.Drinks.AddRangeAsync(drinks);

        await context.SaveChangesAsync();
    }
}