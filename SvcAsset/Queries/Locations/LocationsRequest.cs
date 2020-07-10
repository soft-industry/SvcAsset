using MediatR;
using SvcAsset.Core.Entities;
using SvcAsset.Core.Models;
using SvcAsset.Core.Queries.Common;
using System;

namespace SvcAsset.Core.Queries.Locations
{
    public class LocationsRequest : PagedRequest, IRequest<PagedResponse<AssetLocation, LocationModel>>
    {
        public Guid AssetId { get; set; }

        public LocationsRequest(PaginationParameters paginationParameters)
        {
            PaginationParameters = paginationParameters;
        }

        public LocationsRequest(
            Guid assetId,
            PaginationParameters paginationParameters)
            : this(paginationParameters)
        {
            AssetId = assetId;
        }
    }
}
