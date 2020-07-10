using Microsoft.EntityFrameworkCore;
using SvcAsset.Core.Entities;
using SvcAsset.Core.Models;
using System.Linq;

namespace SvcAsset.Core.Helpers
{
    public static class IQueryableAssetExtensions
    {
        public static IQueryable<AssetModel> GetAssetModels(this IQueryable<Asset> items)
        {
            return items.Include(i => i.Inventories)
                .Include(af => af.Features)
                .Include(ap => ap.Purposes)
                .Include(l => l.Locations)
                .Select(x => new AssetModel()
                {
                    Id = x.Id,
                    ArticleId = x.ArticleId,
                    Lmid = x.LMID,
                    ConstructionYear = x.ConstructionYear,
                    IsSecondhand = x.IsSecondhand,
                    HolderId = x.HolderTenantId,
                    DrawingNo = x.DrawingNo,
                    InventoryNo = x.Inventories.SingleOrDefault() != null
                        ? x.Inventories.Single().InventoryNo
                        : string.Empty,
                    SerialNumber = x.SerialNumber,
                    OperationHours = x.OperationHours,
                    AvailabilityDate = x.AvailabilityDate,
                    Purposes = x.Purposes.Select(p => p.PurposeId),
                    Features = x.Features.OrderBy(f => f.Sequence).Select(f => f.FeatureDescription),
                    AssetLocation = x.Locations.OrderBy(x => x.LocatingTime).LastOrDefault() != null
                        ? new LocationModel
                        {
                            Latitude = x.Locations.OrderBy(x => x.LocatingTime).Last().Location.Y,
                            Longitude = x.Locations.OrderBy(x => x.LocatingTime).Last().Location.X,
                            LocatingTime = x.Locations.OrderBy(x => x.LocatingTime).Last().LocatingTime,
                            IsCreatedManually = x.Locations.OrderBy(x => x.LocatingTime).Last().IsCreatedManually,
                            ClientBrowserInfo = x.Locations.OrderBy(x => x.LocatingTime).Last().Browser,
                            ClientOSInfo = x.Locations.OrderBy(x => x.LocatingTime).Last().OperatingSystem
                        }
                        : null
                });
        }
    }
}
