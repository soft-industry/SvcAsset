using MediatR;
using SvcAsset.Core.Models;
using System;

namespace SvcAsset.Core.Commands.Location
{
    public class CreateLocationRequest : IRequest<CreateLocationResponse>
    {
        public LocationModel CreateLocation { get; set; }

        public Guid AssetId { get; set; }

        public CreateLocationRequest(Guid assetId, LocationModel createLocation)
        {
            CreateLocation = createLocation;
            AssetId = assetId;
        }
    }
}
