using System;

namespace SvcAsset.Core.Queries.Parameters
{
    public class ReservationQueryParameters
    {
        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public bool Compact { get; set; }

        public Guid? ArticleId { get; set; }
    }
}
