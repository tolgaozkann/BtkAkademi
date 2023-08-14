
namespace BtkAkademi.Entities.Models;

public class Category
{
    public int Id { get; set; }
    public string? CategoryName { get; set; }

    //ref: navigation property
    public ICollection<Book> Books { get; set; }
}

