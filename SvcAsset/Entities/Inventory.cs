using SvcAsset.Core.Entities.Common;
using System;

namespace SvcAsset.Core.Entities
{
    public class Inventory : BaseEntity, ITenantEntity // to fix "The entity type requires a primary key to be defined"
    {
        public Inventory(string inventoryNo)
        {
            InventoryNo = inventoryNo;
        }

        public Guid AssetId { get; private set; }

        public Guid TenantId { get; private set; }

        public string InventoryNo { get; private set; }
    }
}
