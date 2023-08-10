using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtkAkademi.Services.Contracts
{
    public interface IServiceManager
    {
        public IBookService BookService { get; }
        public IAuthenticationService AuthenticationService { get; }
    }
}
