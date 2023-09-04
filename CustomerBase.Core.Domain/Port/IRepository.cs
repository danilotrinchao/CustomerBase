using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CustomerBase.Core.Domain
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(Guid id);
        Task<List<T>> GetAll();
        string ParseFilterExpression(Expression<Func<T, bool>> filter);
    }
}
