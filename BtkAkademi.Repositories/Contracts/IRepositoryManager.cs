﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtkAkademi.Repositories.Contracts
{
    public interface IRepositoryManager
    {
        IBookRepository Book { get; }
        ICategoryRepository Category { get; }
        Task SaveAsync();
    }
}
