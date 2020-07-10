using System.Collections.Generic;

namespace SvcAsset.Core.Models
{
    public class AssetModel : AssetCtorModel
    {
        public string? DrawingNo { get; set; }

        public string? InventoryNo { get; set; }

        public string? SerialNumber { get; set; }

        public int? OperationHours { get; set; }

        public IEnumerable<string> Features { get; set; } = new List<string>();

        public LocationModel? AssetLocation { get; set; }
    }
}
