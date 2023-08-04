
using System.ComponentModel.DataAnnotations;

namespace BtkAkademi.Entities.Dtos
{
    public abstract record BookDtoForManipulation
    {
        [Required(ErrorMessage ="This must be filled")]
        [MinLength(3,ErrorMessage = "This field must have at least 3 characters")]
        [MaxLength(30, ErrorMessage = "This field must have at most 30 characters")]
        public string Title { get; init; }

        [Required(ErrorMessage = "This must be filled")]
        [Range(10,1000)]
        public decimal Price { get; init; }
    }
}
