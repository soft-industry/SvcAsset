using MediatR;
using NetTopologySuite.Geometries;
using SvcAsset.Core.Entities;
using SvcAsset.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SvcAsset.Core.Commands.Location
{
    public class CreateLocationCommand : IRequestHandler<CreateLocationRequest, CreateLocationResponse>
    {
        private readonly IEFContext _context;

        public CreateLocationCommand(IEFContext context)
        {
            _context = context;
        }

        public async Task<CreateLocationResponse> Handle(CreateLocationRequest request, CancellationToken cancellationToken)
        {
            var newLocation = new AssetLocation(request.AssetId, request.CreateLocation.LocatingTime, new Point(request.CreateLocation.Longitude, request.CreateLocation.Latitude), request.CreateLocation.ClientOSInfo, request.CreateLocation.ClientBrowserInfo, request.CreateLocation.IsCreatedManually);

            _context.AssetLocations.Add(newLocation);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateLocationResponse
            {
                Success = true,
                LocationCreated = newLocation
            };
        }
    }
}
