using SvcAsset.Core.Entities.Common;
using System;

namespace SvcAsset.Core.Entities
{
    public class AssetPurpose : BaseEntity // to fix "The entity type requires a primary key to be defined"
    {
        public AssetPurpose(Purpose purposeId)
        {
            PurposeId = purposeId;
        }

        public Guid AssetId { get; private set; }

        public Purpose PurposeId { get; private set; }
    }
}
