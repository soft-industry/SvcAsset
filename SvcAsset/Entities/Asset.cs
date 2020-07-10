using Ardalis.GuardClauses;
using SvcAsset.Core.Entities.Common;
using SvcAsset.Core.Helpers;
using SvcAsset.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SvcAsset.Core.Entities
{
    public class Asset : BaseEntity
    {
        public Guid ArticleId { get; private set; }

        public string LMID { get; private set; } = string.Empty;

        public string? DrawingNo { get; internal set; }

        public string? SerialNumber { get; internal set; }

        public short ConstructionYear { get; private set; }

        public int? OperationHours { get; internal set; }

        public bool IsSecondhand { get; private set; }

        public DateTime? AvailabilityDate { get; private set; }

        public Guid HolderTenantId { get; private set; }

        public ICollection<AssetFeature> Features { get; private set; } = null!;

        public ICollection<Inventory> Inventories { get; private set; } = null!;

        public ICollection<AssetPurpose> Purposes { get; private set; } = null!;

        public ICollection<AssetLocation> Locations { get; private set; } = null!;

        public ICollection<Event> Events { get; private set; } = null!;

        internal Asset(AssetCtorModel asset)
        {
            Guard.Against.GuidNullOrEmpty(asset.ArticleId, nameof(asset.ArticleId));
            Guard.Against.NullOrEmpty(asset.Lmid, nameof(asset.Lmid));
            Guard.Against.GuidNullOrEmpty(asset.HolderId, nameof(asset.HolderId));
            Guard.Against.OutOfRange(asset.ConstructionYear, nameof(asset.ConstructionYear), (short)1000, (short)9999);
            Guard.Against.Zero(asset.Purposes.Count(), nameof(asset.Purposes));

            ArticleId = asset.ArticleId;
            LMID = asset.Lmid;
            HolderTenantId = asset.HolderId;
            ConstructionYear = asset.ConstructionYear;
            IsSecondhand = asset.IsSecondhand;
            AvailabilityDate = asset.AvailabilityDate ?? DateTime.UtcNow;

            Purposes = new List<AssetPurpose>();

            foreach (var p in asset.Purposes)
            {
                if (Purposes.SingleOrDefault(x => x.PurposeId == p) == null)
                {
                    Purposes.Add(new AssetPurpose(p));
                }
            }

            Features = new List<AssetFeature>();
            Inventories = new List<Inventory>();
            Locations = new List<AssetLocation>();
            Events = new List<Event>();
        }

        //ef core
        private Asset() { }

        public void AddFeature(AssetFeature assetFeature)
        {
            Features.Add(assetFeature);
        }

        public void AddInventory(Inventory inventory)
        {
            Inventories.Add(inventory);
        }

        public void AddPurpose(AssetPurpose assetPurpose)
        {
            Purposes.Add(assetPurpose);
        }

        public void AddLocation(AssetLocation assetLocation)
        {
            Locations.Add(assetLocation);
        }

        public void AddEvent(Event evt)
        {
            Events.Add(evt);
        }
    }
}
