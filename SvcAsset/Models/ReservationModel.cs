using SvcAsset.Core.Entities;
using System;

namespace SvcAsset.Core.Models
{
    public class ReservationModel
    {
        public Guid Id { get; set; }

        public Guid? AssetId { get; set; }

        public Guid? ArticleId { get; set; }

        public Guid? TenantId { get; set; }

        public EventTimeModel EventTime { get; set; }

        public Purpose Purpose { get; set; }

        public EventLocationModel? EventLocation { get; set; }

        public AddressModel? EventAddress { get; set; }

        public string? EventLocationComment { get; set; }

        public CustomerModel? Customer { get; set; }

        public string? Comment { get; set; }

        public bool IsConfirmed { get; set; }

        public bool HasUserAssetAssignment { get; set; }

        #region Old
        public DateTime EventStart { get; set; }

        public DateTime? EventEnd { get; set; }
        #endregion
    }
}
