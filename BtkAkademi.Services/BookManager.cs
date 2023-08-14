using AutoMapper;
using BtkAkademi.Entities.Dtos;
using BtkAkademi.Entities.Exceptions;
using BtkAkademi.Entities.LinkModels;
using BtkAkademi.Entities.Models;
using BtkAkademi.Entities.RequestFeatures;
using BtkAkademi.Repositories.Contracts;
using BtkAkademi.Services.Contracts;
using System.Dynamic;

namespace BtkAkademi.Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IBookLinks _bookLinks;

        public BookManager(IRepositoryManager manager, ILoggerService logger,
            IMapper mapper, IBookLinks bookLinks)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
            _bookLinks = bookLinks;
        }

        public async Task<BookDto> CreateOneBookAsync(InsertBookDto book)
        {
            var category = await _manager.Category.GetOneCategoryByCategoryId(book.CategoryId, false);

            if (category is null)
                throw new CategoryNotFoundException(book.CategoryId);
            var entity = _mapper.Map<Book>(book);
            _manager.Book.Create(entity);
            await _manager.SaveAsync();
            return _mapper.Map<BookDto>(entity);
        }

        public async Task DeleteOneBookAsync(int id, bool trackChanges)
        {
            //check entity
            var entity = await GetByIdAndCheck(id, trackChanges);
            //delete
            _manager.Book.DeleteOneBook(entity);
            await _manager.SaveAsync();
        }

        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetAllBooksAsync(LinkParameters linkParameters, bool trackChanges)
        {
            if (!linkParameters.BookParameters.ValidPriceRange)
                throw new PriceOutOfRangeBadRequestException();

            var booksWithMetaData = await _manager.Book.GetAllBooksAsync(linkParameters.BookParameters, trackChanges);

            var booksDto = _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);

            var links = _bookLinks.TryGenerateLinks(booksDto, linkParameters.BookParameters.Fields, linkParameters.HttpContext);

            return (linkResponse : links, metaData : booksWithMetaData.MetaData);
        }

        public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
        {
            var book = await GetByIdAndCheck(id, trackChanges);

            return _mapper.Map<BookDto>(book);
        }

        public async Task<(UpdateBookDto updateBookDto, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            var book = await GetByIdAndCheck(id, trackChanges);

            var updateBookDto = _mapper.Map<UpdateBookDto>(book);

            return (updateBookDto, book);
        }

        public async Task SaveChangesForPatchAsync(UpdateBookDto updateBookDto, Book book)
        {
            _mapper.Map(updateBookDto, book);
            await _manager.SaveAsync();
        }

        public async Task<List<Book>> GetAllBooksAsync(bool trackChanges) => 
           await _manager.Book.GetAllBooksAsync(trackChanges);

        public async Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges)
        {
            return await _manager
                .Book
                .GetAllBooksWithDetailsAsync(trackChanges);
        }


        public async Task UpdateOneBookAsync(int id, UpdateBookDto bookDto, bool trackChanges)
        {
            //check params
            if (bookDto is null)
                throw new ArgumentNullException(nameof(bookDto));

            //check entity
            var entity = await GetByIdAndCheck(id, trackChanges);

            //entity.Title = book.Title;
            //entity.Price = book.Price;
            entity = _mapper.Map<Book>(bookDto);


            _manager.Book.Update(entity);
            await _manager.SaveAsync();
        }

        public async Task<Book> GetByIdAndCheck(int id, bool trackChanges)
        {
            var entity = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);

            if (entity is null)
                throw new BookNotFoundException(id);

            return entity;
        }
    }
}
