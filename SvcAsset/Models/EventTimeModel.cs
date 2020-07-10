using SvcAsset.Core.Entities;
using System;

namespace SvcAsset.Core.Models
{
    public class EventTimeModel
    {
        public TimeType TimeType { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public DurationType? DurationType { get; set; }

        public int? Duration { get; set; }
    }
}
