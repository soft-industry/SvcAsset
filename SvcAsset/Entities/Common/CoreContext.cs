using Microsoft.EntityFrameworkCore;
using SvcAsset.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SvcAsset.Core.Entities.Common
{
    public class CoreContext : DbContext, IEFContext
    {
        public DbSet<Asset> Assets { get; private set; }

        public DbSet<AssetFeature> AssetFeatures { get; private set; }

        public DbSet<AssetLocation> AssetLocations { get; private set; }

        public DbSet<AssetPurpose> AssetPurposes { get; private set; }

        public DbSet<Inventory> Inventories { get; private set; }

        public DbSet<Event> Events { get; private set; }

        public DbSet<EventTime> EventTimes { get; private set; }

        public DbSet<EventGap> EventGaps { get; private set; }

        public DbSet<EventLock> EventLocks { get; private set; }

        public Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }

        public CoreContext(DbContextOptions<CoreContext> options) : base(options)
        { }
    }
}
