using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;
        private DbContext _dbContext;
        private IDbContextTransaction _transaction;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool IsTransactional()
        {
            return _transaction != null;
        }

        public async Task BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = await _dbContext.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitTransaction()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                _transaction = null;

                _dbContext.ChangeTracker.Clear();
            }
        }

        public async Task RollBackTransaction()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                _transaction = null;

                _dbContext.ChangeTracker.Clear();
            }
        }

        public IDbContextTransaction GetTransaction()
        {
            return _transaction;
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _dbContext?.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
