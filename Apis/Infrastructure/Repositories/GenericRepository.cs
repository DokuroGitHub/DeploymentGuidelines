using System.Linq.Expressions;
using Application.Commons.Exceptions;
using Application.Commons.Extensions;
using Application.Commons.Mappings;
using Application.Commons.Models;
using Application.Repositories;
using Domain.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    protected DbSet<TEntity> _dbSet;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context; // for debugging only
    //
    public GenericRepository(AppDbContext context, IMapper mapper)
    {
        _dbSet = context.Set<TEntity>();
        _mapper = mapper;
        _context = context;
    }

    #region Read

    // untracked
    public PagedList<TResult> GetPageList<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int pageNumber = 1,
        int pageSize = 10) where TResult : class
    {
        var query = _dbSet.AsQueryable();

        // where
        query = query.Filter(
            where: where,
            include: include,
            orderBy: orderBy);

        // project to + paginated list
        var result = query.ProjectTo<TResult>(_mapper.ConfigurationProvider).ToPaginatedList(
            pageNumber: pageNumber,
            pageSize: pageSize);
        return result;
    }

    // untracked
    public async Task<PagedList<TResult>> GetPageListAsync<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default) where TResult : class
    {
        var query = _dbSet.AsQueryable();

        // where
        query = query.Filter(
            where: where,
            include: include,
            orderBy: orderBy);

        // project to + paginated list
        var result = await query.ProjectTo<TResult>(_mapper.ConfigurationProvider).ToPaginatedListAsync(
            pageNumber: pageNumber,
            pageSize: pageSize,
            cancellationToken: cancellationToken);
        return result;
    }

    // tracked/untracked
    public ICollection<TEntity> GetAll(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracked = false)
    {
        var query = _dbSet.AsQueryable();

        // where
        query = query.Filter(
            where: where,
            include: include,
            orderBy: orderBy,
            tracked: tracked);

        var items = query.ToList();
        if (items.Any())
            Console.WriteLine($"{typeof(TEntity)} State: {_context.Entry(items.First()).State}");
        return items;
    }

    // untracked
    public IReadOnlyCollection<TResult> GetAll<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null) where TResult : class
    {
        var query = _dbSet.AsQueryable();

        // where
        query = query.Filter(
            where: where,
            include: include,
            orderBy: orderBy);

        var items = query.ProjectToList<TResult>(_mapper.ConfigurationProvider);
        return items;
    }

    // tracked/untracked
    public async Task<ICollection<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default,
        bool tracked = false)
    {
        var query = _dbSet.AsQueryable();

        // where
        query = query.Filter(
            where: where,
            include: include,
            orderBy: orderBy,
            tracked: tracked);

        var items = await query.ToListAsync(cancellationToken);
        if (items.Any())
            Console.WriteLine($"{typeof(TEntity)} State: {_context.Entry(items.First()).State}");
        return items;
    }

    // untracked
    public async Task<IReadOnlyCollection<TResult>> GetAllAsync<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default) where TResult : class
    {
        var query = _dbSet.AsQueryable();

        // where
        query = query.Filter(
            where: where,
            include: include,
            orderBy: orderBy);

        var items = await query.ProjectToListAsync<TResult>(_mapper.ConfigurationProvider, cancellationToken);
        return items;
    }

    // tracked/untracked
    public TEntity? FirstOrdDefault(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracked = false)
    {
        var query = _dbSet.AsQueryable();

        // where
        query = query.Filter(
            where: where,
            include: include,
            orderBy: orderBy,
            tracked: tracked);

        var item = query.FirstOrDefault();
        if (item is not null)
            Console.WriteLine($"{typeof(TEntity)} State: {_context.Entry(item).State}");
        return item;
    }

    // untracked
    public TResult? FirstOrdDefault<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null) where TResult : class
    {
        var query = _dbSet.AsQueryable();

        // where
        query = query.Filter(
            where: where,
            include: include,
            orderBy: orderBy);

        var item = query.ProjectTo<TResult>(_mapper.ConfigurationProvider).FirstOrDefault();
        return item;
    }

    // tracked/untracked
    public async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default,
        bool tracked = false)
    {
        var query = _dbSet.AsQueryable();

        // where
        query = query.Filter(
            where: where,
            include: include,
            orderBy: orderBy,
            tracked: tracked);

        var item = await query.FirstOrDefaultAsync(cancellationToken);
        if (item is not null)
            Console.WriteLine($"{typeof(TEntity)} State: {_context.Entry(item).State}");
        return item;
    }

    // untracked
    public async Task<TResult?> FirstOrDefaultAsync<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default) where TResult : class
    {
        var query = _dbSet.AsQueryable();

        // where
        query = query.Filter(
            where: where,
            include: include,
            orderBy: orderBy);

        var item = await query.ProjectTo<TResult>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
        return item;
    }

    // tracked/untracked
    public TEntity Single(
        Expression<Func<TEntity, bool>>? where = null,
        bool tracked = false)
    {
        var query = _dbSet.AsQueryable();

        query = query.Filter(
            where: where,
            tracked: tracked);

        var item = query.SingleOrDefault();
        if (item is null)
            throw new NotFoundException($"{typeof(TEntity)} not found");
        Console.WriteLine($"{typeof(TEntity)} State: {_context.Entry(item).State}");
        return item;
    }

    // untracked
    public TResult Single<TResult>(
        Expression<Func<TEntity, bool>>? where = null) where TResult : class
    {
        var query = _dbSet.AsQueryable();

        query = query.Filter(
            where: where);

        var item = query.ProjectTo<TResult>(_mapper.ConfigurationProvider).SingleOrDefault();
        if (item is null)
            throw new NotFoundException($"{typeof(TEntity)} not found");
        return item;
    }

    // tracked/untracked
    public async Task<TEntity> SingleAsync(
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default,
        bool tracked = false)
    {
        var query = _dbSet.AsQueryable();

        query = query.Filter(
            where: where,
            tracked: tracked);

        var item = await query.SingleOrDefaultAsync(cancellationToken);
        if (item is null)
            throw new NotFoundException($"{typeof(TEntity)} not found");
        Console.WriteLine($"{typeof(TEntity)} State: {_context.Entry(item).State}");
        return item;
    }

    // untracked
    public async Task<TResult> SingleAsync<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default) where TResult : class
    {
        var query = _dbSet.AsQueryable();

        query = query.Filter(
            where: where);

        var item = await query.ProjectTo<TResult>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(cancellationToken);
        if (item is null)
            throw new NotFoundException($"{typeof(TEntity)} not found");
        return item;
    }

    // tracked/untracked
    public async Task<TEntity?> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default,
        bool tracked = false)
    {
        var query = _dbSet.AsQueryable();

        query = query.Filter(
            where: where,
            tracked: tracked);

        var item = await query.SingleOrDefaultAsync(cancellationToken);
        if (item is not null)
            Console.WriteLine($"{typeof(TEntity)} State: {_context.Entry(item).State}");
        return item;
    }

    // untracked
    public async Task<TResult?> SingleOrDefaultAsync<TResult>(
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default) where TResult : class
    {
        var query = _dbSet.AsQueryable();

        query = query.Filter(
            where: where);

        var item = await query.ProjectTo<TResult>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(cancellationToken);
        return item;
    }

    #region Find

    // tracked
    public TEntity? Find(params object?[]? keyValues)
    {
        var item = _dbSet.Find(keyValues: keyValues);
        if (item is null)
            return null;
        Console.WriteLine($"{typeof(TEntity)} State: {_context.Entry(item).State}");
        return item;
    }

    // untracked
    public TResult? Find<TResult>(params object?[]? keyValues) where TResult : class
    {
        var item = _dbSet.Find(keyValues: keyValues);
        if (item is null)
            return null;
        var result = _mapper.Map<TResult>(item);
        return result;
    }

    // tracked
    public async ValueTask<TEntity?> FindAsync(
        object?[]? keyValues,
        CancellationToken cancellationToken = default)
    {
        var item = await _dbSet.FindAsync(
            keyValues: keyValues,
            cancellationToken: cancellationToken);
        if (item is null)
            return null;
        Console.WriteLine($"{typeof(TEntity)} State: {_context.Entry(item).State}");
        return item;
    }

    // tracked
    public async ValueTask<TEntity?> FindAsync(
        CancellationToken cancellationToken = default,
        params object?[]? keyValues)
    {
        var item = await _dbSet.FindAsync(
            keyValues: keyValues,
            cancellationToken: cancellationToken);
        if (item is null)
            return null;
        Console.WriteLine($"{typeof(TEntity)} State: {_context.Entry(item).State}");
        return item;
    }

    // untracked
    public async ValueTask<TResult?> FindAsync<TResult>(
        object?[]? keyValues,
        CancellationToken cancellationToken = default) where TResult : class
    {
        var item = await _dbSet.FindAsync(
            keyValues: keyValues,
            cancellationToken: cancellationToken);
        if (item is null)
            return null;
        var result = _mapper.Map<TResult>(item);
        return result;
    }

    // untracked
    public async ValueTask<TResult?> FindAsync<TResult>(
        CancellationToken cancellationToken = default,
        params object?[]? keyValues) where TResult : class
    {
        var item = await _dbSet.FindAsync(
            keyValues: keyValues,
            cancellationToken: cancellationToken);
        if (item is null)
            return null;
        var result = _mapper.Map<TResult>(item);
        return result;
    }

    #endregion Find

    #endregion Read

    #region Create + Update + Delete

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public async Task AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public async Task AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void Remove(Expression<Func<TEntity, bool>> where)
    {
        var entity = _dbSet.FirstOrDefault(where);
        if (entity is null)
            throw new NotFoundException($"{typeof(TEntity)} not found");
        _dbSet.Remove(entity);
    }

    public async Task RemoveAsync(
        Expression<Func<TEntity, bool>> where,
        CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(where, cancellationToken);
        if (entity is null)
            throw new NotFoundException($"{typeof(TEntity)} not found");
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public void RemoveRange(Expression<Func<TEntity, bool>> where)
    {
        var entities = _dbSet.Where(where).ToList();
        if (entities.Count == 0)
            throw new NotFoundException($"{typeof(TEntity)}(s/es) not found");
        _dbSet.RemoveRange(entities);
    }

    public async Task RemoveRangeAsync(
        Expression<Func<TEntity, bool>> where,
        CancellationToken cancellationToken = default)
    {
        var entities = await _dbSet.Where(where).ToListAsync(cancellationToken);
        if (entities.Count == 0)
            throw new NotFoundException($"{typeof(TEntity)}(s/es) not found");
        _dbSet.RemoveRange(entities);
    }

    public void Purge()
    {
        _dbSet.RemoveRange(_dbSet);
    }

    #endregion Create + Update + Delete

    #region Any + Count + LongCount

    public bool Any(Expression<Func<TEntity, bool>>? where = null)
    {
        return where is null
            ? _dbSet.Any()
            : _dbSet.Any(where);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default)
    {
        return where is null
            ? await _dbSet.AnyAsync(cancellationToken)
            : await _dbSet.AnyAsync(where, cancellationToken);
    }

    public Task<bool> AllAsync(
        Expression<Func<TEntity, bool>> assert,
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default)
    {
        return where is null ?
             _dbSet.AllAsync(assert, cancellationToken) :
             _dbSet.Where(where).AllAsync(assert, cancellationToken);
    }

    public int Count(Expression<Func<TEntity, bool>>? where = null)
    {
        return where is null
            ? _dbSet.Count()
            : _dbSet.Count(where);
    }

    public async Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default)
    {
        return where is null
            ? await _dbSet.CountAsync(cancellationToken)
            : await _dbSet.CountAsync(where, cancellationToken);
    }

    public double LongCount(Expression<Func<TEntity, bool>>? where = null)
    {
        return where is null
            ? _dbSet.LongCount()
            : _dbSet.LongCount(where);
    }

    public async Task<double> LongCountAsync(
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken cancellationToken = default)
    {
        return where is null
            ? await _dbSet.LongCountAsync(cancellationToken)
            : await _dbSet.LongCountAsync(where, cancellationToken);
    }

    #endregion Any + Count + LongCount
}
