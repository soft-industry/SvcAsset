using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SvcAsset.Core.Entities.Common
{
    public class BaseAggregateRoot : BaseEntity
    {
        [NotMapped]
        public List<INotification> Events { get; } = new List<INotification>();
    }
}
