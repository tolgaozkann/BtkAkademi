
using BtkAkademi.Entities.Dtos;
using BtkAkademi.Entities.LinkModels;
using Microsoft.AspNetCore.Http;

namespace BtkAkademi.Services.Contracts
{
    public interface IBookLinks
    {
        LinkResponse TryGenerateLinks(IEnumerable<BookDto> booksDto, string fields, HttpContext httpContext);
    }
}
