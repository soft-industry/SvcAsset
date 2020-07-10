namespace SvcAsset.Core.Commands.Location
{
    public class CreateLocationResponse
    {
        public bool Success { get; internal set; }

        public Entities.AssetLocation? LocationCreated { get; internal set; }
    }
}
