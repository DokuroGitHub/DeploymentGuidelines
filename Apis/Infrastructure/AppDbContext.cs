using System.Reflection;
using Application.Commons.Models;
using Domain.Entities;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext
{
    private readonly Appsettings _appsettings;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        Appsettings appsettings) : base(options)
    {
        _appsettings = appsettings;
    }

    public DbSet<Chemical> Chemicals { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // configurations using Fluent API
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);

        // global filters
        builder.Entity<Chemical>().HasQueryFilter(x => x.IsDeleted == false);
        builder.Entity<User>().HasQueryFilter(x => x.IsDeleted == false);

        // seed
        if (_appsettings.SeedOnInit)
            builder.Seed();
    }
}
