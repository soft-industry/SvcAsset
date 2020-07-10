using System;
using System.Collections.Generic;

namespace SvcAsset.Core.Models
{
    public class AvailableArticlesModel
    {
        public List<Guid> ArticleIds { get; set; } = new List<Guid>();

        public List<Guid> SkipEventIds { get; set; } = new List<Guid>();
    }
}
