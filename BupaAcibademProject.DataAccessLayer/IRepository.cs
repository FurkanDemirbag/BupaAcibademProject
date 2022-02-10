using BupaAcibademProject.Domain.Entities;
using BupaAcibademProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.DataAccessLayer
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        Task Add(T entity);
        Task Delete(int id);
        Task Delete(T entity);
        Task Update(T entity);

        Task<T> Get(int id, params string[] includes);
        Task<T> Get(Expression<Func<T, bool>> filter, Expression<Func<T, T>> select = null, params string[] includes);
        Task<IEnumerable<T>> Query(Expression<Func<T, bool>> filter, Expression<Func<T, T>> select = null, params string[] includes);

        Task<int> Count(Expression<Func<T, bool>> filter);
        Task<bool> Any(Expression<Func<T, bool>> filter);

        Task<Tuple<IEnumerable<T>, int>> Query(PagedQuery<T> query);
    }
}
