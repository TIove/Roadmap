using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Roadmap.Data;
using Roadmap.Models.Db;

namespace Roadmap.DataProvider.MsSql.Ef;

public class RoadmapDbContext : DbContext, IDataProvider
{
    public DbSet<DbUser> Users { get; set; }

    public RoadmapDbContext(DbContextOptions<RoadmapDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("Roadmap.Models.Db"));
    }
    
    public object MakeEntityDetached(object obj)
    {
        Entry(obj).State = EntityState.Detached;
        return Entry(obj).State;
    }

    async Task IBaseDataProvider.SaveAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }

    void IBaseDataProvider.Save()
    {
        SaveChanges();
    }

    public void EnsureDeleted()
    {
        Database.EnsureDeleted();
    }

    public bool IsInMemory()
    {
        return Database.IsInMemory();
    }
}