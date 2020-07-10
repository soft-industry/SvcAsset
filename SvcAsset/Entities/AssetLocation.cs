using NetTopologySuite.Geometries;
using SvcAsset.Core.Entities.Common;
using System;

namespace SvcAsset.Core.Entities
{
    public class AssetLocation : BaseEntity // to fix "The entity type requires a primary key to be defined"
    {
        public AssetLocation(Guid assetId, DateTime locatingTime, Point location, string? operatingSystem, string? browser, bool isCreatedManually)
        : this(assetId, locatingTime, operatingSystem, browser, isCreatedManually)
        {
            Location = location;
            Location.SRID = 4326;
        }

        private AssetLocation(Guid assetId, DateTime locatingTime, string? operatingSystem, string? browser, bool isCreatedManually)
        {
            AssetId = assetId;
            LocatingTime = locatingTime;
            OperatingSystem = operatingSystem;
            Browser = browser;
            IsCreatedManually = isCreatedManually;
        }

        public Guid AssetId { get; private set; }

        public DateTime LocatingTime { get; private set; }

        public Point Location { get; private set; } = null!;

        public string? OperatingSystem { get; private set; }

        public string? Browser { get; private set; }

        public bool IsCreatedManually { get; private set; }
    }
}
