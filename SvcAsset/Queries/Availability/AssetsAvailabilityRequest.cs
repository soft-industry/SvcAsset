using MediatR;
using SvcAsset.Core.Models;
using SvcAsset.Core.Queries.Parameters;
using System;

namespace SvcAsset.Core.Queries.Availability
{
    public class AssetsAvailabilityRequest : IRequest<AvailableItemsModel>
    {
        public AvailableAssetsModel AvailableAssetsModel { get; set; }

        public AvailabilityQueryParamaters AvailabilityQueryParamaters { get; set; }

        public Guid TenantId { get; set; }

        public AssetsAvailabilityRequest(AvailableAssetsModel availableAssetsModel, AvailabilityQueryParamaters availabilityQueryParamaters, Guid tenatnId)
        {
            AvailableAssetsModel = availableAssetsModel;
            AvailabilityQueryParamaters = availabilityQueryParamaters;
            TenantId = tenatnId;
        }
    }
}
