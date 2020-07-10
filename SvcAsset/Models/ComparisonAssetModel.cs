using System;

namespace SvcAsset.Core.Models
{
    public class ComparisonAssetModel
    {
        public Guid AssetId { get; set; }

        public double BeforeReservation { get; set; }

        public double AfterReservation { get; set; }
    }
}
