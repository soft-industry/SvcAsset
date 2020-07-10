using System;

namespace SvcAsset.Core.Models
{
    public class SearchReservationModel
    {
        public Guid Id { get; set; }

        public Guid AssetId { get; set; }

        public bool HasUserAssetAssignment { get; set; }

        public DateTime EventStart { get; set; }

        public DateTime? EventEnd { get; set; }

        public EventAddressModel? EventAddress { get; set; }

        public string Purpose { get; set; } = string.Empty;

        public EventCustomerModel? Customer { get; set; }
    }
}
