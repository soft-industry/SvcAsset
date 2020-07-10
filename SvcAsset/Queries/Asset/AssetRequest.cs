using MediatR;
using SvcAsset.Core.Models;
using System;

namespace SvcAsset.Core.Queries.Asset
{
    public class AssetRequest : IRequest<AssetModel?>
    {
        public Guid AssetId { get; }

        public AssetRequest(Guid assetId)
        {
            AssetId = assetId;
        }
    }
}
