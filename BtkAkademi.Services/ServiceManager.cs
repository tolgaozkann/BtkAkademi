using AutoMapper;
using BtkAkademi.Entities.Dtos;
using BtkAkademi.Entities.Models;
using BtkAkademi.Repositories.Contracts;
using BtkAkademi.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BtkAkademi.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IBookService> _bookService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        public ServiceManager(IRepositoryManager repositoryManager, 
            ILoggerService loggerService, 
            IMapper mapper,
            UserManager<User> userManager,
            IConfiguration configuration,
            IBookLinks bookLinks)
        {
            _bookService = new Lazy<IBookService>(() => 
                new BookManager(repositoryManager, loggerService, mapper, bookLinks));

            _authenticationService = new Lazy<IAuthenticationService>(() => 
                new AuthenticationManager(loggerService,mapper,userManager,configuration));
        }

        public IBookService BookService => _bookService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
    }
}
