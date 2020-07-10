namespace SvcAsset.Core.Commands.Asset
{
    public class CreateAssetResponse
    {
        public bool Success { get; internal set; }

        public Entities.Asset? AssetCreated { get; internal set; }
    }
}
