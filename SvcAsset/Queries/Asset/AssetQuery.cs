using MediatR;
using Microsoft.EntityFrameworkCore;
using SvcAsset.Core.Helpers;
using SvcAsset.Core.Interfaces;
using SvcAsset.Core.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SvcAsset.Core.Queries.Asset
{
    public class AssetQuery : IRequestHandler<AssetRequest, AssetModel?>
    {
        private readonly Interfaces.IEFContext _ctx;

        public AssetQuery(IEFContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<AssetModel?> Handle(AssetRequest request, CancellationToken cancellationToken)
        {
            return await _ctx.Assets
                .Where(x => x.Id == request.AssetId)
                .GetAssetModels()
                .SingleOrDefaultAsync();
        }
    }
}
