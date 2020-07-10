using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SvcAsset.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SvcAsset.Core.Interfaces
{
    public interface IEFContext
    {
        DbSet<Asset> Assets { get; }

        DbSet<AssetFeature> AssetFeatures { get; }

        DbSet<AssetLocation> AssetLocations { get; }

        DbSet<AssetPurpose> AssetPurposes { get; }

        DbSet<Inventory> Inventories { get; }

        DbSet<Event> Events { get; }

        DbSet<EventTime> EventTimes { get; }

        DbSet<EventGap> EventGaps { get; }

        DbSet<EventLock> EventLocks { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task<int> SaveChangesAsync();

        DatabaseFacade Database { get; }
    }
}
