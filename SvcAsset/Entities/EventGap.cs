using SvcAsset.Core.Entities.Common;
using System;

namespace SvcAsset.Core.Entities
{
    public class EventGap : BaseEntity // to fix "The entity type requires a primary key to be defined"
    {
        public Guid EventId { get; private set; }

        public Guid ArticleId { get; private set; }

        public DateTime? AvailabilityDate { get; private set; }

        public Purpose PurposeId { get; private set; }

        public Guid AssetId { get; private set; }

        public Guid TenantId { get; private set; }

        public DateTime StartGap { get; set; }

        public DateTime EndGap { get; private set; }

        public long DateDiff { get; private set; }
    }
}
