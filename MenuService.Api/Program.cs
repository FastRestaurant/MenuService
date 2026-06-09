using MenuService.Api.Middlewares;
using MenuService.Infrastructure.Extensions;
using MenuService.Infrastructure.Persistence;
using MenuService.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Services

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Migraciones y Seed inicial

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MenuDbContext>();

    await dbContext.Database.MigrateAsync();

    await MenuDbSeeder.SeedAsync(dbContext);
}

// Middleware global de excepciones

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // Todo lo que esté dentro de wwwroot puede ser servido públicamente.

app.UseAuthorization();

app.MapControllers();

app.Run();