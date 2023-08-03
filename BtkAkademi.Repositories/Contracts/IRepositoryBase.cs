using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BtkAkademi.Repositories.Contracts
{
    public interface IRepositoryBase<T>
    {
        //Track Changes nesne üstündeki değişiklikleri izleyip izlememeyi belirtmek için alınmış bir parametre, Bazı durumlarda performans için yararlı olabilir.
        //Bu method sorgulanabilir bir liste döndürüyor
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression,bool trackChanges);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
