using System;
using System.Collections.Generic;

namespace SvcAsset.Core.Models
{
    public class AvailabilityModel
    {
        public Guid Id { get; set; }

        public IEnumerable<GapModel> Gaps { get; set; } = new List<GapModel>();
    }
}
