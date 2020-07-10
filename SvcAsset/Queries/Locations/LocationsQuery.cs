using MediatR;
using Microsoft.EntityFrameworkCore;
using SvcAsset.Core.Entities;
using SvcAsset.Core.Interfaces;
using SvcAsset.Core.Models;
using SvcAsset.Core.Queries.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SvcAsset.Core.Queries.Locations
{
    public class LocationsQuery : IRequestHandler<LocationsRequest, PagedResponse<AssetLocation, LocationModel>>
    {
        private readonly IEFContext _ctx;

        public LocationsQuery(IEFContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<PagedResponse<AssetLocation, LocationModel>> Handle(LocationsRequest request, CancellationToken cancellationToken)
        {
            var query = _ctx.AssetLocations.Where(x => x.AssetId == request.AssetId).AsNoTracking();

            return await PagedResponse<AssetLocation, LocationModel>.Create(
                MapFrom,
                query,
                request.PaginationParameters.PageNumber,
                request.PaginationParameters.PageSize,
                cancellationToken);
        }

        public LocationModel MapFrom(AssetLocation assetLocation)
        {
            return new LocationModel
            {
                Latitude = assetLocation.Location.Y,
                Longitude = assetLocation.Location.X,
                LocatingTime = assetLocation.LocatingTime,
                IsCreatedManually = assetLocation.IsCreatedManually,
                ClientBrowserInfo = assetLocation.Browser,
                ClientOSInfo = assetLocation.OperatingSystem
            };
        }
    }
}
