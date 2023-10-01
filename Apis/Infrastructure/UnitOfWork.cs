using System.Transactions;
using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private IDbContextTransaction? _transaction;
    private bool _disposed;
    //
    private readonly AppDbContext _context;
    //
    private readonly IChemicalRepository _chemicalRepository;
    private readonly IUserRepository _userRepository;
    // repositories
    public IChemicalRepository ChemicalRepository => _chemicalRepository;
    public IUserRepository UserRepository => _userRepository;
    //
    public UnitOfWork(
        AppDbContext dbContext,
        IChemicalRepository chemicalRepository,
        IUserRepository userRepository)
    {
        _context = dbContext;
        // repositories
        _chemicalRepository = chemicalRepository;
        _userRepository = userRepository;
    }

    // state
    public EntityState State<TEntity>(TEntity entity) where TEntity : BaseEntity
    => _context.Entry<TEntity>(entity).State;

    #region save changes

    // public int SaveChanges() => _context.SaveChanges();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    => _context.SaveChangesAsync(cancellationToken);

    #endregion save changes

    #region transaction

    #region begin transaction

    public void BeginTransaction()
    {
        _transaction = _context.Database.BeginTransaction();
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    #endregion begin transaction

    #region commit

    public void Commit()
    {
        if (_transaction is null)
            throw new TransactionException("No transaction to commit");
        try
        {
            _context.SaveChanges();
            _transaction.Commit();
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            throw new TransactionException("No transaction to commit");
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    #endregion commit

    #region rollback

    public void Rollback()
    {
        if (_transaction is null)
            throw new TransactionException("No transaction to rollback");
        _transaction.Rollback();
        _transaction.Dispose();
        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            throw new TransactionException("No transaction to rollback");
        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    #endregion rollback

    #region execute transaction

    public void ExecuteTransaction(Action work)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            work.Invoke();
            _context.SaveChanges();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new TransactionException("Could not execute transaction", ex);
        }
    }

    public T ExecuteTransaction<T>(Func<T> work)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var result = work.Invoke();
            _context.SaveChanges();
            transaction.Commit();
            return result;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new TransactionException("Could not execute transaction", ex);
        }
    }

    // public async Task ExecuteTransactionAsync(Action work)
    // {
    //     using var transaction = await _context.Database.BeginTransactionAsync();
    //     try
    //     {
    //         work.Invoke();
    //         await _context.SaveChangesAsync();
    //         await transaction.CommitAsync();
    //     }
    //     catch (Exception ex)
    //     {
    //         await transaction.RollbackAsync();
    //         throw new TransactionException("Could not execute transaction", ex);
    //     }
    // }

    public async Task ExecuteTransactionAsync(
        Func<Task> work,
        CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await work.Invoke();
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new TransactionException("Could not execute transaction", ex);
        }
    }

    public async Task<T> ExecuteTransactionAsync<T>(
        Func<Task<T>> work,
        CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await work.Invoke();
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new TransactionException("Could not execute transaction", ex);
        }
    }

    #endregion execute transaction

    #endregion transaction

    #region dispose

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }

            _disposed = true;
        }
    }

    protected virtual async Task DisposeAsync(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                if (_transaction is not null)
                    await _transaction.DisposeAsync();
                await _context.DisposeAsync();
            }

            _disposed = true;
        }
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }

    #endregion dispose

    // test
    private async void Test()
    {
        // Func<Task<int>> work
        var result1 = await ExecuteTransactionAsync(async () =>
        {
            await Task.Delay(1000);
            return 1;
        });

        // Func<Task> work
        await ExecuteTransactionAsync(async () =>
        {
            var x = Task.Delay(1000);
            var y = Task.Delay(1000);
            await Task.WhenAll(x, y);
        });

        // Func<Task> work
        await ExecuteTransactionAsync(() =>
        {
            Console.WriteLine("hh");
            return Task.CompletedTask;
        });

        // Action work
        ExecuteTransaction(() =>
        {
            Console.WriteLine("hh");
        });

        // Func<int> work
        var result2 = ExecuteTransaction(() =>
        {
            Console.WriteLine("hh");
            return 1;
        });

        // Func<Task> work
        await ExecuteTransaction(() =>
        {
            Console.WriteLine("hh");
            return Task.CompletedTask;
        });
    }
}
