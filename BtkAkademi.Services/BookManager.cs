using AutoMapper;
using BtkAkademi.Entities.Dtos;
using BtkAkademi.Entities.Exceptions;
using BtkAkademi.Entities.Models;
using BtkAkademi.Repositories.Contracts;
using BtkAkademi.Services.Contracts;

namespace BtkAkademi.Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
        }

        public BookDto CreateOneBook(InsertBookDto book)
        {
            var entity = _mapper.Map<Book>(book);
            _manager.Book.Create(entity);
            _manager.Save();
            return _mapper.Map<BookDto>(entity);
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {
            //check entity
            var entity = _manager.Book.GetOneBookById(id, trackChanges);

            if (entity is null)
                throw new BookNotFoundException(id);


            _manager.Book.DeleteOneBook(entity);
            _manager.Save();
        }

        public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
        {
            var books = _manager.Book.GetAllBooks(trackChanges);

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public BookDto GetOneBookById(int id, bool trackChanges)
        {
            var book = _manager.Book.GetOneBookById(id, trackChanges);

            if (book is null)
                throw new BookNotFoundException(id);

            return _mapper.Map<BookDto>(book);
        }

        public (UpdateBookDto updateBookDto, Book book) GetOneBookForPatch(int id, bool trackChanges)
        {
            var book = _manager.Book.GetOneBookById(id, trackChanges);

            if (book is null)
                throw new BookNotFoundException(id);

            var updateBookDto = _mapper.Map<UpdateBookDto>(book);

            return (updateBookDto, book);
        }

        public void SaveChangesForPatch(UpdateBookDto updateBookDto, Book book)
        {
            _mapper.Map(updateBookDto, book);
            _manager.Save();
        }

        public void UpdateOneBook(int id, UpdateBookDto bookDto, bool trackChanges)
        {
            //check params
            if (bookDto is null)
                throw new ArgumentNullException(nameof(bookDto));

            //check entity
            var entity = _manager.Book.GetOneBookById(id, trackChanges);

            if (entity is null)
                throw new BookNotFoundException(id);

            //entity.Title = book.Title;
            //entity.Price = book.Price;
            entity = _mapper.Map<Book>(bookDto);


            _manager.Book.Update(entity);
            _manager.Save();
        }
    }
}
