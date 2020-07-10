using System;
using System.Collections.Generic;

namespace SvcAsset.Core.Models
{
    public class AvailableItemsModel
    {
        public List<Guid> AvailableIds { get; set; } = new List<Guid>();
    }
}
