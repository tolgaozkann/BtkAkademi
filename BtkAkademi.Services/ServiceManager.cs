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
        private readonly IBookService _bookService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICategoryService _categoryService;


        public ServiceManager(IBookService bookService, 
            IAuthenticationService authenticationService, 
            ICategoryService categoryService)
        {
            _bookService = bookService;
            _authenticationService = authenticationService;
            _categoryService = categoryService;
        }

        public IBookService BookService => _bookService;
        public IAuthenticationService AuthenticationService => _authenticationService;
        public ICategoryService CategoryService  => _categoryService;
    }
}
