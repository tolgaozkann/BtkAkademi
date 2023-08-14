

namespace BtkAkademi.Services.Contracts
{
    public interface IServiceManager
    {
        public IBookService BookService { get; }
        public IAuthenticationService AuthenticationService { get; }
        public ICategoryService CategoryService { get; }
    }
}
