using MediatR;
using SvcAsset.Core.Models;

namespace SvcAsset.Core.Commands.Asset
{
    public class CreateAssetRequest : IRequest<CreateAssetResponse>
    {
        public AssetModel CreateAsset { get; set; }

        public CreateAssetRequest(AssetModel createAsset)
        {
            CreateAsset = createAsset;
        }
    }
}
