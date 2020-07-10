using MediatR;
using NetTopologySuite.Geometries;
using SvcAsset.Core.Entities;
using SvcAsset.Core.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SvcAsset.Core.Commands.Asset
{
    public class CreateAssetCommand : IRequestHandler<CreateAssetRequest, CreateAssetResponse>
    {
        private readonly IEFContext _context;

        public CreateAssetCommand(IEFContext context)
        {
            _context = context;
        }

        public Task<CreateAssetResponse> Handle(CreateAssetRequest request, CancellationToken cancellationToken)
        {
            var newAsset = new Entities.Asset(request.CreateAsset)
            {
                DrawingNo = request.CreateAsset.DrawingNo,
                SerialNumber = request.CreateAsset.SerialNumber,
                OperationHours = request.CreateAsset.OperationHours
            };

            if (request.CreateAsset.Features.Any())
            {
                var features = request.CreateAsset.Features.ToList();
                for (var i = 0; i < features.Count; i++)
                {
                    var newFeature = new AssetFeature(i, features[i]);
                    newAsset.AddFeature(newFeature);
                }
            }

            if (!string.IsNullOrEmpty(request.CreateAsset.InventoryNo))
            {
                var newInventory = new Inventory(request.CreateAsset.InventoryNo);
                newAsset.AddInventory(newInventory);
            }

            if (request.CreateAsset.AssetLocation != null)
            {
                var location = request.CreateAsset.AssetLocation;
                var newLocation = new AssetLocation(newAsset.Id, location.LocatingTime, new Point(location.Longitude, location.Latitude), location.ClientOSInfo, location.ClientBrowserInfo, location.IsCreatedManually);
                newAsset.AddLocation(newLocation);
            }

            return HandleIntern(newAsset, cancellationToken);
        }

        private async Task<CreateAssetResponse> HandleIntern(Entities.Asset asset, CancellationToken cancellationToken)
        {
            _context.Assets.Add(asset);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateAssetResponse
            {
                Success = true,
                AssetCreated = asset
            };
        }
    }
}
