using Microsoft.EntityFrameworkCore.Storage;
using FeedbackBoard.Domain.Interfaces;
using FeedbackBoard.Infrastructure.Data;
using FeedbackBoard.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace FeedbackBoard.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Unit of Work implementation managing database transactions and repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FeedbackBoardDbContext _context;
        private IDbContextTransaction? _transaction;
        private IFeedbackRepository? _feedbackRepository;
        private bool _disposed;

        public UnitOfWork(FeedbackBoardDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IFeedbackRepository Feedbacks =>
            _feedbackRepository ??= new FeedbackRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _transaction?.Dispose();
                _context?.Dispose();
                _disposed = true;
            }
        }
    }
}
