
using System.ComponentModel.DataAnnotations;

namespace BtkAkademi.Entities.Dtos
{
    public record InsertBookDto : BookDtoForManipulation
    {
        [Required(ErrorMessage = "CategoryId is Required")]
        public int CategoryId { get; init; }
    }
}
