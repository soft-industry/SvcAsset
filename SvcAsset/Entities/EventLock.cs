using System;

namespace SvcAsset.Core.Entities
{
    public class EventLock
    {
        public EventLock(Guid eventId, Guid userId, DateTime lockDate)
        {
            EventId = eventId;
            UserId = userId;
            LockDate = lockDate;
        }

        public Guid Id { get; private set; }

        public Guid EventId { get; private set; }

        public Guid UserId { get; private set; }

        public DateTime LockDate { get; private set; }

        public Event Event { get; private set; } = null!;

        public void Update(Guid userId, DateTime lockDate)
        {
            UserId = userId;
            LockDate = lockDate;
        }
    }
}
