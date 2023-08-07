﻿using BtkAkademi.Entities.Models;
using BtkAkademi.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BtkAkademi.Repositories.EFCore
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateOneBook(Book book) => 
            Create(book);

        public void DeleteOneBook(Book book) => 
            Delete(book);

        public async Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges) => 
           await FindAll(trackChanges)
            .OrderBy(b=>b.Id)
            .ToListAsync();

        public async Task<Book> GetOneBookByIdAsync(int id, bool trackChanges) =>
            await FindByCondition(item => item.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();

        public void UpdateOneBook(Book book) => 
            Update(book);
    }
}
