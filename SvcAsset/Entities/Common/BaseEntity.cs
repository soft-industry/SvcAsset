using System;

namespace SvcAsset.Core.Entities.Common
{
    public class BaseEntity : IEntity
    {
        public Guid Id { get; private set; }
    }
}
