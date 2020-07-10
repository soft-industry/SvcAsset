using System;
using System.Collections.Generic;

namespace SvcAsset.Core.Models
{
    public class AvailableAssetsModel
    {
        public List<Guid> AssetIds { get; set; } = new List<Guid>();

        public List<Guid> SkipEventIds { get; set; } = new List<Guid>();
    }
}
