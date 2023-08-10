using System.ComponentModel.DataAnnotations;

namespace BtkAkademi.Entities.Dtos;

public record UserRegisterDto
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    [Required(ErrorMessage = "UserNamer is Required.")]
    public string? UserName { get; init; }

    [Required(ErrorMessage = "Password is Required.")]
    public string? Password { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }

    public ICollection<string>? Roles { get; init; }
}