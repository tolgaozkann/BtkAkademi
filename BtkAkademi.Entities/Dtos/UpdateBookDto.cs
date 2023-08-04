using System.ComponentModel.DataAnnotations;

namespace BtkAkademi.Entities.Dtos
{
    public record UpdateBookDto : BookDtoForManipulation
    {
        [Required]
        public int Id { get; init; }
    }
}
