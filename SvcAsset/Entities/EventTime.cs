using Ardalis.GuardClauses;
using SvcAsset.Core.Entities.Common;
using System;

namespace SvcAsset.Core.Entities
{
    public class EventTime : BaseEntity // to fix "The entity type requires a primary key to be defined"
    {
        public TimeType TimeType { get; private set; }

        public DateTime? Start { get; private set; }

        public DateTime? End { get; private set; }

        public DurationType? DurationType { get; private set; }

        public int? Duration { get; private set; }

        public EventTime(TimeType timeType, DurationType? durationType = null, DateTime? start = null, DateTime? end = null, int? duration = null)
        {
            TimeType = timeType;

            switch (timeType)
            {
                case TimeType.Date:
                    {
                        Guard.Against.Null(start, nameof(start));
                        Guard.Against.Null(end, nameof(end));
                        Start = start;
                        End = end;
                        DurationType = null;
                        Duration = null;

                        break;
                    }

                case TimeType.Duration:
                    {
                        Guard.Against.Null(durationType, nameof(durationType));
                        Guard.Against.Null(duration, nameof(duration));
                        Guard.Against.OutOfRange(duration.Value, nameof(duration), 1, int.MaxValue);
                        DurationType = durationType;
                        Duration = duration;
                        Start = null;
                        End = null;

                        break;
                    }
            }
        }
    }
}
