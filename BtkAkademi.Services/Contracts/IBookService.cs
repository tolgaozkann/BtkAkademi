using BtkAkademi.Entities.Dtos;
using BtkAkademi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtkAkademi.Services.Contracts
{
    public interface IBookService
    {
        IEnumerable<BookDto> GetAllBooks(bool trackChanges);
        BookDto GetOneBookById(int id, bool trackChanges);
        BookDto CreateOneBook(InsertBookDto book);
        void UpdateOneBook(int id, UpdateBookDto bookDto, bool trackChanges);
        void DeleteOneBook(int id, bool trackChanges);
        (UpdateBookDto updateBookDto, Book book) GetOneBookForPatch(int id, bool trackChanges);

        void SaveChangesForPatch(UpdateBookDto updateBookDto, Book book);
    }
}
