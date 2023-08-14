using System.ComponentModel.DataAnnotations;

namespace BtkAkademi.Entities.Dtos;

public record UpdateCategoryDto
{
    [Required]
    public int Id { get; init; }

    [Required(ErrorMessage = "CategoryName is required")]
    [MaxLength(40)]
    [MinLength(3)]
    public string? CategoryName { get; set; }
};