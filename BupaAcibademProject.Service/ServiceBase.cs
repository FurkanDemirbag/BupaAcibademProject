using BupaAcibademProject.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using BupaAcibademProject.Domain.Models;
using System.Linq.Expressions;
using BupaAcibademProject.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace BupaAcibademProject.Service
{
    public class ServiceBase : IServiceBase
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ILogService _logService;
        protected readonly IUnitOfWork _unitOfWork;

        public ServiceBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logService = _serviceProvider.GetService<ILogService>();
            _unitOfWork = _serviceProvider.GetService<IUnitOfWork>();
        }

        public string StoreKey => "BupaAcibademSigorta!123";

        public async Task<Result<T>> Get<T>(Expression<Func<T, bool>> filter, Expression<Func<T, T>> select = null, params string[] includes) where T : Entity
        {
            try
            {
                var repository = _serviceProvider.GetService<IRepository<T>>();
                var data = await repository.Get(filter, select, includes);
                return new Result<T>() { Data = data };
            }
            catch (Exception ex)
            {
                return new Result<T>(StatusCodes.Status500InternalServerError.ToString(),await _logService.LogException(ex));
            }
        }

        public async Task<Result<IEnumerable<T>>> Query<T>(Expression<Func<T, bool>> filter, Expression<Func<T, T>> select = null, params string[] includes) where T : Entity
        {
            try
            {
                var repository = _serviceProvider.GetService<IRepository<T>>();
                var data = await repository.Query(filter, select, includes);
                return new Result<IEnumerable<T>>() { Data = data };
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<T>>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }

        public async Task<Result<Tuple<IEnumerable<T>, int>>> Query<T>(PagedQuery<T> query) where T : Entity
        {
            try
            {
                var repository = _serviceProvider.GetService<IRepository<T>>();
                var data = await repository.Query(query);
                return new Result<Tuple<IEnumerable<T>, int>>() { Data = data };
            }
            catch (Exception ex)
            {
                return new Result<Tuple<IEnumerable<T>, int>>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }
    }
}
