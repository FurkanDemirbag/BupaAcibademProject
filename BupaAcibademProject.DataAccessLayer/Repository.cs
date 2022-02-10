using BupaAcibademProject.Domain.Entities;
using BupaAcibademProject.Domain.Exceptions;
using BupaAcibademProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace BupaAcibademProject.DataAccessLayer
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected DbContext _dbContext;
        protected DbSet<T> _dbSet;
        protected IUnitOfWork _unitOfWork;

        private bool _disposed = false;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public virtual async Task Add(T entity)
        {
            if (entity.Id > 0)
            {
                throw new InvalidOperationException(typeof(T).Name + " - Id sıfır olmalıdır");
            }
            entity.Id = 0;
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(int id)
        {
            var entity = await Get(id);
            if (entity == null)
            {
                throw new BusException("Silinecek kayıt bulunamadı");
            }
            await Delete(entity);
        }

        public virtual async Task Delete(T entity)
        {
            entity.Deleted = true;
            await Update(entity);
        }

        public virtual async Task Update(T entity)
        {
            entity.UpdateDate = DateTime.Now;
            _dbSet.Update(entity);

            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<T> Get(int id, params string[] includes)
        {
            return await Get(a => a.Id == id, null, includes);
        }

        public virtual async Task<T> Get(Expression<Func<T, bool>> filter, Expression<Func<T, T>> select = null, params string[] includes)
        {
            IQueryable<T> data = _dbSet;
            if (filter != null)
            {
                data = data.Where(filter);
            }
            data = data.Where(a => !a.Deleted);
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    data = data.Include(include);
                }
            }
            if (select != null)
            {
                data = data.Select(select);
            }
            return await data.FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> Query(Expression<Func<T, bool>> filter, Expression<Func<T, T>> select = null, params string[] includes)
        {
            IQueryable<T> data = _dbSet;
            if (filter != null)
            {
                data = data.Where(filter);
            }
            data = data.Where(a => !a.Deleted);
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    data.Include(include);
                }
            }
            if (select != null)
            {
                data = data.Select(select);
            }
            return await data.ToListAsync();
        }

        public virtual async Task<int> Count(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> data = _dbSet;
            if (filter != null)
            {
                data = data.Where(filter);
            }
            data = data.Where(a => !a.Deleted);
            return await data.CountAsync();
        }

        public virtual async Task<bool> Any(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> data = _dbSet;
            if (filter != null)
            {
                data = data.Where(filter);
            }
            data = data.Where(a => !a.Deleted);
            return await data.AnyAsync();
        }

        public virtual async Task<Tuple<IEnumerable<T>, int>> Query(PagedQuery<T> query)
        {
            int totalCount = 0;
            IQueryable<T> data = _dbSet;
            if (query != null)
            {
                if (query.Filters != null)
                {
                    foreach (var filter in query.Filters)
                    {
                        data = data.Where(filter);
                    }
                }
                data = data.Where(a => !a.Deleted);
                totalCount = data.Count();

                data = _dbSet;
                if (query.Filters != null)
                {
                    foreach (var filter in query.Filters)
                    {
                        data = data.Where(filter);
                    }
                }
                data = data.Where(a => !a.Deleted);
                if (query.Includes != null)
                {
                    foreach (var include in query.Includes)
                    {
                        data = data.Include(include);
                    }
                }
                if (query.Orders != null && query.Orders.Any())
                {
                    var firsOrder = query.Orders.First();
                    if (firsOrder.Item2)
                    {
                        data = Queryable.OrderBy(data, (dynamic)firsOrder.Item1);
                    }
                    else
                    {
                        data = Queryable.OrderByDescending(data, (dynamic)firsOrder.Item1);
                    }
                    foreach (var orderBy in query.Orders.Skip(1))
                    {
                        if (orderBy.Item2)
                        {
                            data = Queryable.ThenBy((IOrderedQueryable<T>)data, (dynamic)orderBy.Item1);
                        }
                        else
                        {
                            data = Queryable.ThenByDescending((IOrderedQueryable<T>)data, (dynamic)orderBy.Item1);
                        }
                    }
                }
                if (query.Select != null)
                {
                    data = data.Select(query.Select);
                }
                if (query.PageSize > 0)
                {
                    data = data.Skip(query.PageSize * (query.Page - 1)).Take(query.PageSize);
                }
            }
            else
            {
                totalCount = data.Count();
            }
            return new Tuple<IEnumerable<T>, int>(await data.ToListAsync(), totalCount);
        }
      
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public virtual void Dispose()
        {
            Dispose(true);
        }

    }
}
