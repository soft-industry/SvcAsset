using SvcAsset.Core.Entities.Common;
using System;

namespace SvcAsset.Core.Entities
{
    public class AssetFeature : BaseEntity
    {
        public AssetFeature(int sequence, string featureDescription)
        {
            Sequence = sequence;
            FeatureDescription = featureDescription;
        }

        public Guid AssetId { get; private set; }

        public int Sequence { get; private set; }

        public string FeatureDescription { get; private set; }
    }
}
