using BupaAcibademProject.Domain.Entities;
using BupaAcibademProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Service
{
    public interface IServiceBase
    {
        string StoreKey { get; }
        Task<Result<T>> Get<T>(Expression<Func<T, bool>> filter, Expression<Func<T, T>> select = null, params string[] includes) where T : Entity;
        Task<Result<IEnumerable<T>>> Query<T>(Expression<Func<T, bool>> filter, Expression<Func<T, T>> select = null, params string[] includes) where T : Entity;
        Task<Result<Tuple<IEnumerable<T>, int>>> Query<T>(PagedQuery<T> query) where T : Entity;

    }
}
