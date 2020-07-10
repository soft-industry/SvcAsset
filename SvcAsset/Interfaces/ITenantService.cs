using System;

namespace SvcAsset.Core.Interfaces
{
    public interface ITenantService
    {
        Guid TenantId { get; set; }
    }
}
