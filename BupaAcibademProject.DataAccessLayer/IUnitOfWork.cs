using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.DataAccessLayer
{
    public interface IUnitOfWork : IDisposable
    {
        bool IsTransactional();
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollBackTransaction();
        IDbContextTransaction GetTransaction();
    }
}
