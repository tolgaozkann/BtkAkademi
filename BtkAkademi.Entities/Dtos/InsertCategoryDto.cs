using System.ComponentModel.DataAnnotations;

namespace BtkAkademi.Entities.Dtos;

public record InsertCategoryDto
{
    [Required(ErrorMessage = "CategoryName is required")]
    [MaxLength(40)]
    [MinLength(3)]
    public string? CategoryName { get; set; }
    
}