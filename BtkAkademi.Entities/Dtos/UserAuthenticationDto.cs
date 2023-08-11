using System.ComponentModel.DataAnnotations;

namespace BtkAkademi.Entities.Dtos;

public record UserAuthenticationDto
{
    [Required(ErrorMessage = "UserName is required.")]
    public string? UserName { get; init; }

    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; init; }
}