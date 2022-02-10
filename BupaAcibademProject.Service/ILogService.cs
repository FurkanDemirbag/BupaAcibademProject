using BupaAcibademProject.Domain.Entities;
using BupaAcibademProject.Service.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Service
{
    public interface ILogService : IScopedService
    {
        Task<string> LogException(Exception ex);
        Task SaveServiceHistory(ServiceHistory history);
        Task LogEntityHistory<T>(T data, T old, bool isTransactional) where T : Entity;
        Task LogEntityHistory(EntityLog log);
        Task LogDeleteHistory<T>(int id, bool isTransactional) where T : Entity;
        Task WriteEntityHistories();
        Task ClearEntityHistories();
    }
}
