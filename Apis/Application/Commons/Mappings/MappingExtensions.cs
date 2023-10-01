using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Commons.Models;
using Microsoft.EntityFrameworkCore;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Application.Commons.Mappings;

public static class MappingExtensions
{
    public static PagedList<TDestination> ToPaginatedList<TDestination>(
        this IQueryable<TDestination> queryable,
        int pageNumber,
        int pageSize) where TDestination : class
        => PagedList<TDestination>.Create(queryable, pageNumber, pageSize);

    public static Task<PagedList<TDestination>> ToPaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default) where TDestination : class
        => PagedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize, cancellationToken);

    public static List<TDestination> ProjectToList<TDestination>(
        this IQueryable queryable,
        IConfigurationProvider configuration,
        CancellationToken? cancellationToken = null) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).ToList();

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(
        this IQueryable queryable,
        IConfigurationProvider configuration,
        CancellationToken cancellationToken = default) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).ToListAsync(cancellationToken);
}
