﻿using BtkAkademi.Entities.Dtos;
using BtkAkademi.Entities.Models;
using BtkAkademi.Entities.RequestFeatures;
using System.Dynamic;

namespace BtkAkademi.Services.Contracts
{
    public interface IBookService
    {
        Task<(IEnumerable<ExpandoObject> books,MetaData metaData)> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges);
        Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges);
        Task<BookDto> CreateOneBookAsync(InsertBookDto book);
        Task UpdateOneBookAsync(int id, UpdateBookDto bookDto, bool trackChanges);
        Task DeleteOneBookAsync(int id, bool trackChanges);
        Task<(UpdateBookDto updateBookDto, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges);

        Task SaveChangesForPatchAsync(UpdateBookDto updateBookDto, Book book);
    }
}
