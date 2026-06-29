using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace MenuService.Application.DTOs.Drinks;

public class CreateDrinkDto
{
    [Required(ErrorMessage = "La categoría es obligatoria.")]
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100, ErrorMessage = "La marca no puede superar los 100 caracteres.")]
    public string? Brand { get; set; }

    [MaxLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
    public string? Description { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
    public decimal Price { get; set; }

    public bool Available { get; set; } = true;

    [MaxLength(500, ErrorMessage = "La URL de la imagen no puede superar los 500 caracteres.")]
    public string? ImageUrl { get; set; }
}
