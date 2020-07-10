using SvcAsset.Core.Entities;
using System;
using System.Collections.Generic;

namespace SvcAsset.Core.Models
{
    public class AssetCtorModel
    {
        public Guid Id { get; set; }

        public Guid ArticleId { get; set; }

        public string Lmid { get; set; } = string.Empty;

        public short ConstructionYear { get; set; }

        public bool IsSecondhand { get; set; }

        public Guid HolderId { get; set; }

        public DateTime? AvailabilityDate { get; set; }

        public IEnumerable<Purpose> Purposes { get; set; } = new List<Purpose>();
    }
}
