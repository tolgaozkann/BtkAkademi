

namespace BtkAkademi.Entities.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }

        //ref: navigation property
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
