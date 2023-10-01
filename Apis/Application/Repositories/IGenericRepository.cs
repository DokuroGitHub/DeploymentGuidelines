using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Application.Commons.Models;
using Domain.Entities;

namespace Application.Repositories;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    #region Read
    ///!<summary> untracked </summary>
    PagedList<TResult> GetPageList<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int pageNumber = 1,
        int pageSize = 10) where TResult : class;
    ///!<summary> untracked </summary>
    Task<PagedList<TResult>> GetPageListAsync<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default) where TResult : class;
    ///?<summary> tracked/untracked </summary>
    ICollection<TEntity> GetAll(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracked = false);
    IReadOnlyCollection<TResult> GetAll<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null) where TResult : class;
    ///?<summary> tracked/untracked </summary>
    Task<ICollection<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default,
        bool tracked = false);
    ///!<summary> untracked </summary>
    Task<IReadOnlyCollection<TResult>> GetAllAsync<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default) where TResult : class;
    ///?<summary> tracked/untracked </summary>
    TEntity? FirstOrdDefault(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracked = false);
    ///!<summary> untracked </summary>
    TResult? FirstOrdDefault<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null) where TResult : class;
    ///?<summary> tracked/untracked </summary>
    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default,
        bool tracked = false);
    ///!<summary> untracked </summary>
    Task<TResult?> FirstOrDefaultAsync<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default) where TResult : class;
    ///?<summary> tracked/untracked </summary>
    TEntity Single(
        Expression<Func<TEntity, bool>>? where = null,
        bool tracked = false);
    ///!<summary> untracked </summary>
    TResult Single<TResult>(
        Expression<Func<TEntity, bool>>? where = null) where TResult : class;
    ///?<summary> tracked/untracked </summary>
    Task<TEntity> SingleAsync(
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default,
        bool tracked = false);
    ///!<summary> untracked </summary>
    Task<TResult> SingleAsync<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default) where TResult : class;
    ///?<summary> tracked/untracked </summary>
    Task<TEntity?> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default,
        bool tracked = false);
    ///!<summary> untracked </summary>
    Task<TResult?> SingleOrDefaultAsync<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default) where TResult : class;
    ///*<summary> tracked </summary>
    TEntity? Find(params object?[]? keyValues);
    ///!<summary> untracked </summary>
    TResult? Find<TResult>(params object?[]? keyValues) where TResult : class;
    ///*<summary> tracked </summary>
    ValueTask<TEntity?> FindAsync(
        object?[]? keyValues,
        CancellationToken cancellationToken = default);
    ///*<summary> tracked </summary>
    ValueTask<TEntity?> FindAsync(
        CancellationToken cancellationToken = default,
        params object?[]? keyValues);
    ///! <summary> untracked </summary>
    ValueTask<TResult?> FindAsync<TResult>(
        object?[]? keyValues,
        CancellationToken cancellationToken = default) where TResult : class;
    /// <summary> untracked </summary>
    ValueTask<TResult?> FindAsync<TResult>(
        CancellationToken cancellationToken = default,
        params object?[]? keyValues) where TResult : class;
    #endregion Read
    #region Create + Update + Delete
    void Add(TEntity entity);
    Task AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);
    void AddRange(IEnumerable<TEntity> entities);
    Task AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void Remove(Expression<Func<TEntity, bool>> where);
    Task RemoveAsync(
        Expression<Func<TEntity, bool>> where,
        CancellationToken cancellationToken = default);
    void RemoveRange(IEnumerable<TEntity> entities);
    void RemoveRange(Expression<Func<TEntity, bool>> where);
    Task RemoveRangeAsync(
        Expression<Func<TEntity, bool>> where,
        CancellationToken cancellationToken = default);
    ///<summary> = delete table </summary>
    void Purge();
    #endregion Create + Update + Delete
    #region Any + Count + LongCount
    bool Any(Expression<Func<TEntity, bool>>? where = null);
    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default);
    Task<bool> AllAsync(
        Expression<Func<TEntity, bool>> assert,
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default);
    int Count(Expression<Func<TEntity, bool>>? where = null);
    Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default);
    double LongCount(Expression<Func<TEntity, bool>>? where = null);
    Task<double> LongCountAsync(
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default);
    #endregion Any + Count + LongCount
}

