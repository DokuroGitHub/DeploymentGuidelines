using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Commons.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> IncludeMultiple<TEntity>(
        this IQueryable<TEntity> query,
        params Expression<Func<TEntity, object>>[] includes) where TEntity : BaseEntity
    {
        if (includes is not null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        return query;
    }

    public static IIncludableQueryable<TEntity, TProperty> MyInclude<TEntity, TProperty>(
        this IQueryable<TEntity> source,
        Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : class
    {
        return EntityFrameworkQueryableExtensions.Include(source, navigationPropertyPath);
    }

    public static IIncludableQueryable<TEntity, TProperty> MyThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source,
        Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath) where TEntity : class
    {
        return EntityFrameworkQueryableExtensions.ThenInclude(source, navigationPropertyPath);
    }

    public static IQueryable<TEntity> Filter<TEntity>(
        this IQueryable<TEntity> query,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracked = false) where TEntity : BaseEntity
    {
        // Apply AsNoTracking if it is required
        if (tracked is false)
            query = query.AsNoTracking();

        // Apply where if it is provided
        if (where is not null)
            query = query.Where(where);

        // Apply include if it is provided
        if (include is not null)
            query = include(query);

        // Apply orderBy if it is provided
        if (orderBy is not null)
            query = orderBy(query);

        return query;
    }
}
