using BtkAkademi.Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace BtkAkademi.Entities.Dtos
{
    public record LinkParameters
    {
        public BookParameters BookParameters { get; init; }
        public HttpContext HttpContext { get; set; }
    }
}
